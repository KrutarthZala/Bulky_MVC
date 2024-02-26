namespace BulkyBook_WebAPI.Models
{
    public class CompanyModel
    {
        public int CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyStreetAddress { get; set; }
        public string? CompanyCity { get; set; }
        public string? CompanyState { get; set; }
        public string? CompanyPostalCode { get; set; }
        public string? CompanyPhoneNumber { get; set; }
    }
}
