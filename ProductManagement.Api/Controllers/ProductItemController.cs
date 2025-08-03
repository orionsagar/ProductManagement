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
    [Route("api/items")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductItemController(AppDbContext db) => _db = db;


        // GET: /api/projects/{projectId}/items?status={status}
        [HttpGet("/api/projects/{projectId}/items")]
        public async Task<IActionResult> GetItems(Guid projectId, [FromQuery] string? status)
        {
            var query = _db.ProductItems
                .Where(i => i.ProjectId == projectId);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(i => i.Status.ToString() == status);
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateItemsStatus(Guid id, [FromBody] string newStatus)
        {
            var item = await _db.ProductItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            // Validate the new status
            if (Enum.TryParse<ProductItemStatus>(newStatus, true, out var status))
            {
                item.Status = status;
                await _db.SaveChangesAsync();

                await LogAuditAsync(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                nameof(ProductItem),
                item.Id.ToString(),
                "Updated status of Item",
                item
            );

                return Ok(item);
            }
            else
            {
                return BadRequest("Invalid status value.");
            }
        }


        [HttpGet("by-project/{projectId}")]
        public async Task<IActionResult> GetByProject(Guid projectId) =>
            Ok(await _db.ProductItems.Where(i => i.ProjectId == projectId).ToListAsync());

        [HttpPost]
        //[Authorize(Roles = "Admin,ProductManager,ProjectManager,ProductionEngineer")]
        public async Task<IActionResult> Create(ProductItemDto itemdto)
        {
            var item = new ProductItem
            {
                Name = itemdto.Name,
                Description = itemdto.Description,
                Status = itemdto.Status,
                ProjectId = itemdto.ProjectId
            };
            _db.ProductItems.Add(item);
            await _db.SaveChangesAsync();

            await LogAuditAsync(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                nameof(ProductItem),
                item.Id.ToString(),
                "Create Product Item",
                item
            );


            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductItem(Guid id, [FromBody] ProductItemDto dto)
        {
            var item = await _db.ProductItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Status = dto.Status;
            item.ProjectId = dto.ProjectId;

            await _db.SaveChangesAsync();

            await LogAuditAsync(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                nameof(ProductItem),
                item.Id.ToString(),
                "Edit Product Item",
                item
            );

            return Ok(item);
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
