using FrameLog;
using FrameLog.Contexts;
using FrameLog.History;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OBEHR.Models.FrameLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace OBEHR.Models.DAL
{
    public class OBEHRContext : IdentityDbContext<ApplicationUser>
    {
        public OBEHRContext()
            : base("OBEHR")
        {
            Logger = new FrameLogModule<ChangeSet, PPUser>(new ChangeSetFactory(), FrameLogContext);
        }

        public DbSet<PPUser> PPUser { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<City> City { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        #region logging
        public DbSet<ChangeSet> ChangeSets { get; set; }
        public DbSet<ObjectChange> ObjectChanges { get; set; }
        public DbSet<PropertyChange> PropertyChanges { get; set; }

        public readonly FrameLogModule<ChangeSet, PPUser> Logger;
        public IFrameLogContext<ChangeSet, PPUser> FrameLogContext
        {
            get { return new ExampleContextAdapter(this); }
        }
        public HistoryExplorer<ChangeSet, PPUser> HistoryExplorer
        {
            get { return new HistoryExplorer<ChangeSet, PPUser>(FrameLogContext); }
        }
        #endregion
    }

    public class OBEHRInitializer : DropCreateDatabaseIfModelChanges<OBEHRContext>
    {
        protected override void Seed(OBEHRContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Certificate(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON City(Name)");

            var UserManager = new UserManager<ApplicationUser>(new

                                               UserStore<ApplicationUser>(context));

            var RoleManager = new RoleManager<IdentityRole>(new
                                     RoleStore<IdentityRole>(context));

            string name = "Admin";
            string password = "123456";


            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(name))
            {
                var role = new ApplicationRole();
                role.Name = "Admin";
                role.Role.Name = "Admin";

                var roleresult = RoleManager.Create(role);
            }

            //Create User=Admin with password=123456
            var user = new ApplicationUser();
            user.UserName = name;
            user.PPUser.Name = name;

            var adminresult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, name);
            }
        }
    }
}
