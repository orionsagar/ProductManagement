using Microsoft.AspNetCore.Identity;


namespace ProductManagement.Domain.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
