using System.ComponentModel.DataAnnotations;

namespace ReenbitTest.Entities
{
    public class ChatMessage : BaseEntity
    {
        public ChatMessage() : base() { }

        [Required]
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        [Required]
        public string Text { get; set; }

        public bool IsDeletedForUser { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}


