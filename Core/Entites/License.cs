using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Core.Entities
{
    public enum LicenseStatus { Created = 0, Active = 1, Revoked = 2, Expired = 3 }
    public enum ProductType { Standard = 1, Pro = 2, Enterprise = 3 }

    [Index(nameof(LicenseKey), IsUnique = true)]
    public class License
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int UserId { get; set; }

        [Required] 
        [MaxLength(64)]
        public string LicenseKey { get; set; } = string.Empty; // Initialized

        public LicenseStatus Status { get; set; } = LicenseStatus.Active;
        public ProductType Type { get; set; } = ProductType.Standard;

        public DateTime ExpirationDate { get; set; }
        public int MaxActivations { get; set; } = 1;

        [Timestamp]
        public byte[] RowVersion { get; set; } = []; // Initialized

        // Initialize the collection to avoid null reference
        public ICollection<Activation> Activations { get; set; } = new List<Activation>();
    }

    [Index(nameof(LicenseId), nameof(HardwareId), IsUnique = true)]
    public class Activation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LicenseId { get; set; }
        
        [Required]
        [MaxLength(128)]
        public string HardwareId { get; set; } = string.Empty; // Initialized

        public DateTime ActivatedAt { get; set; } = DateTime.UtcNow;
    }
}