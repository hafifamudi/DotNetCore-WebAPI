using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebService.Core.Entities.General;

namespace WebService.Core.Entities.Business
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }

    public class DepartmentCreateViewModel
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<EmployeeCreateViewModel> Employees { get; set; }
    }

    public class DepartmentUpdateViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
