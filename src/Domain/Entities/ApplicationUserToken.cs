using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities
{
    public class ApplicationUserToken : IdentityUserToken<int>
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string IpAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ValidityDate { get; set; }
        public string UserId { get; set; }
    }
}
