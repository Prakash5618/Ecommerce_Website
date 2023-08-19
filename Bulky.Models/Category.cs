using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Range(1,100,ErrorMessage ="Display number must be 1-100")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
