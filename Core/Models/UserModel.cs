using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TodoApi.Core.Models
{
    public class UserModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [Required]
    [MaxLength(20)]
    public string? Name{get; set;} = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string? Email{get; set;} = string.Empty;

    [JsonIgnore]
    public string? PasswordHash{get; set;} = string.Empty;

    public string ? Role{get; set;} = "User";
  }
}