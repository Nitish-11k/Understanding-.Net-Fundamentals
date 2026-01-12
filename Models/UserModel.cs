using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class UserModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [Required]
    [MaxLength(20)]
    public string? Name{get; set;}

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string? Email{get; set;}

    [Required]
    [MinLength(8)]
    public string? Password{get; set;}
  }
}