using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Infrastructure;

namespace ProductManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/auditlogs")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AuditController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.AuditLogs.ToListAsync());
    }
}
