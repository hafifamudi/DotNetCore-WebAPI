using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WebService.Core.Entities.General
{
    [Table("Departments")]
    public class Department : Base<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public ICollection<Employee> Employees { get; set; }
    }
}
