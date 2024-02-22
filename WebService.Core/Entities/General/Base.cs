using System.ComponentModel.DataAnnotations;

namespace WebService.Core.Entities.General
{
    //Base class for entities common properties
    public class Base<T>
    {
        [Key]
        public T? Id { get; set; }
        public DateTimeOffset? EntryDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
