using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Address1 { get; set; } = string.Empty;
        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;
    }
}
