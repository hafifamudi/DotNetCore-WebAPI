using System.ComponentModel.DataAnnotations;
using WebService.Core.Entities.General;

namespace WebService.Core.Entities.Business
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class EmployeeCreateViewModel
    {
        [Required, StringLength(8, MinimumLength = 2)]
        public string EmployeeCode { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public double Salary { get; set; }
        public int DepartmentId { get; set; }
        [StringLength(350)]
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }

    public class EmployeeUpdateViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(8, MinimumLength = 2)]
        public string EmployeeCode { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public double Salary { get; set; }
        [StringLength(350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
