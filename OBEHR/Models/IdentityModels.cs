using Microsoft.AspNet.Identity.EntityFramework;
using OBEHR.Models.Base;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace OBEHR.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class PPUser : IdentityUser
    {

    }

    public class PPRole : IdentityRole
    {
  
    }
}