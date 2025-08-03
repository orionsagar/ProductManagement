using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Entities
{
    public class ProjectDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        [Required]
        public Guid ProductId { get; set; }
    }
}
