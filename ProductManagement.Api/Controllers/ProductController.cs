using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure;
using System.Security.Claims;
using System.Text.Json;


namespace ProductManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _db.Products.ToListAsync(); 
            return Ok(products);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,ProductManager")]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();


            await LogAuditAsync(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                nameof(ProductItem),
                product.Id.ToString(),
                "Create Product",
                product
            );

            return Ok(product);
            //return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            return product == null ? NotFound() : Ok(product);
        }


        [HttpGet("/api/summary/product/{id}")]
        public async Task<IActionResult> GetProductSummary(Guid id)
        {
            var projectCount = await _db.Projects.CountAsync(p => p.ProductId == id);
            var itemCount = await _db.ProductItems
                .CountAsync(i => i.Project.ProductId == id);

            return Ok(new { ProductId = id, Projects = projectCount, Items = itemCount });
        }

       
        private async Task LogAuditAsync(string userId, string userName, string entityName, string entityId, string action, object changes)
        {
            var audit = new AuditLog
            {
                UserId = userId,
                UserName = userName,
                EntityName = entityName,
                EntityId = entityId,
                Action = action,
                Changes = JsonSerializer.Serialize(changes),
                Timestamp = DateTime.UtcNow
            };

            _db.AuditLogs.Add(audit);
            await _db.SaveChangesAsync();
        }
    }
}
