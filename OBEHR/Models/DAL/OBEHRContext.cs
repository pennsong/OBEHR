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
        public DbSet<Assurance> Assurance { get; set; }
        public DbSet<Client> Client { get; set; }
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

    public class OBEHRInitializer : DropCreateDatabaseAlways<OBEHRContext>
    {
        protected override void Seed(OBEHRContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Certificate(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON City(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Client(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_ClientName ON Assurance(ClientId,Name)");

            var UserManager = new UserManager<ApplicationUser>(new

                                               UserStore<ApplicationUser>(context));

            var RoleManager = new RoleManager<IdentityRole>(new
                                     RoleStore<IdentityRole>(context));

            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists("Admin"))
            {
                var role = new ApplicationRole();
                role.Name = "Admin";
                role.Role.Name = "Admin";

                var roleresult = RoleManager.Create(role);
            }
            if (!RoleManager.RoleExists("HRAdmin"))
            {
                var role = new ApplicationRole();
                role.Name = "HRAdmin";
                role.Role.Name = "HRAdmin";

                var roleresult = RoleManager.Create(role);
            }
            if (!RoleManager.RoleExists("HR"))
            {
                var role = new ApplicationRole();
                role.Name = "HR";
                role.Role.Name = "HR";

                var roleresult = RoleManager.Create(role);
            }
            if (!RoleManager.RoleExists("Candidate"))
            {
                var role = new ApplicationRole();
                role.Name = "Candidate";
                role.Role.Name = "Candidate";

                var roleresult = RoleManager.Create(role);
            }

            //Create User=Admin with password=123456
            var name = "Admin";
            var password = "123456";
            var userRole = "Admin";

            var user = new ApplicationUser();
            user.UserName = name;
            user.PPUser.Name = name;

            var userResult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (userResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, userRole);
            }

            name = "hra";
            password = "123456";
            userRole = "HRAdmin";

            user = new ApplicationUser();
            user.UserName = name;
            user.PPUser.Name = name;

            userResult = UserManager.Create(user, password);

            if (userResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, userRole);
            }

            name = "hr";
            password = "123456";
            userRole = "HR";

            user = new ApplicationUser();
            user.UserName = name;
            user.PPUser.Name = name;

            userResult = UserManager.Create(user, password);

            if (userResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, userRole);
            }

            name = "ca";
            password = "123456";
            userRole = "Candidate";

            user = new ApplicationUser();
            user.UserName = name;
            user.PPUser.Name = name;

            userResult = UserManager.Create(user, password);

            if (userResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, userRole);
            }
        }
    }
}
