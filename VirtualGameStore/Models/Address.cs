using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; } = string.Empty;
        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$", ErrorMessage = "Postal Code should be in the format L1L 1L1")]
        public string PostalCode { get; set; } = string.Empty;

        public Address Copy()
        {
            return new Address()
            {
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                Province = Province,
                Country = Country,
                PostalCode = PostalCode
            };
        }
    }
}
