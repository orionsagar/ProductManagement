using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure;
using System.Security.Claims;
using System.Text.Json;


namespace ProductManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProjectController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
       Ok(await _db.Projects.ToListAsync());



        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetByProduct(Guid productId) =>
            Ok(await _db.Projects.Where(p => p.ProductId == productId).ToListAsync());

        [HttpPost]
        //[Authorize(Roles = "Admin,ProductManager,ProjectManager")]
        public async Task<IActionResult> Create(ProjectDto projectdto)
        {
            var project = new Project
            {
                Name = projectdto.Name,
                Description = projectdto.Description,
                ProductId = projectdto.ProductId
            };

            _db.Projects.Add(project);
            await _db.SaveChangesAsync();


            await LogAuditAsync(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                nameof(ProductItem),
                project.Id.ToString(),
                "Create Project",
                project
            );




            return Ok(project);
            //return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectDto dto)
        {
            var project = await _db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.ProductId = dto.ProductId;

            await _db.SaveChangesAsync();

            await LogAuditAsync(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                nameof(Project),
                id.ToString(),
                "Edit Project",
                project
            );

            return Ok(project);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await _db.Projects.FindAsync(id);
            return project == null ? NotFound() : Ok(project);
        }



        [HttpGet("/api/summary/project/{id}")]
        public async Task<IActionResult> GetProjectSummary(Guid id)
        {
            var itemCount = await _db.ProductItems
                .CountAsync(i => i.ProjectId == id);

            var itemStats = await _db.ProductItems
                .Where(i => i.ProjectId == id)
                .GroupBy(i => i.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return Ok(new { ProjectId = id, TotalItems = itemCount, StatusBreakdown = itemStats });
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
