using System.ComponentModel.DataAnnotations;

namespace ReenbitTest.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
        }

        [Required]
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
