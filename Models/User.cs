using Microsoft.AspNetCore.Identity;

namespace Tech_Test_Backend.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt {  get; set; }
    }
}
