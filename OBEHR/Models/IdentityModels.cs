using Microsoft.AspNet.Identity.EntityFramework;
using OBEHR.Models.Base;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace OBEHR.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            PPUser = new PPUser();
        }

        public int PPUserId { get; set; }
        public virtual PPUser PPUser { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
            Role = new Role();
        }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }

    public class PPUser : BaseModel
    {
    }

    public class Role : BaseModel
    {
    }
}