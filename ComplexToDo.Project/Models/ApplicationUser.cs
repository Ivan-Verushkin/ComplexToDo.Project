using Microsoft.AspNetCore.Identity;

namespace ComplexToDo.Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
        public ICollection<ToDoList> ToDoLists { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
