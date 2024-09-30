using Microsoft.AspNetCore.Identity;

namespace ComplexToDo.Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
    }
}
