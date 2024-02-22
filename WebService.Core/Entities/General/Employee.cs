using WebService.Core.Entities.General;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebService.Core.Entities.General
{
    [Table("Employees")]
    public class Employee : Base<int>
    {
        [Required, StringLength(8, MinimumLength = 2)]
        public string EmployeeCode { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public double Salary { get; set; }
        public int DepartmentId { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }
        [StringLength(350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
