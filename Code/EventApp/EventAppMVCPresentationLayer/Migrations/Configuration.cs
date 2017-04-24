namespace EventAppMVCPresentationLayer.Migrations
{
    using EventAppMVCPresentationLayer.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;

    internal sealed class Configuration : DbMigrationsConfiguration<EventAppMVCPresentationLayer.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EventAppMVCPresentationLayer.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Manager" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Clerk" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Guest" });

            context.SaveChanges();

            // check for an admin, and if not there, create one
            if (!context.Users.Any(u => u.UserName == "admin@test.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                    //FirstName = "System",
                    //LastName = "Admin"
                };

                IdentityResult result = userManager.Create(user, "P@ssw0rd");

                if (result.Succeeded)
                {
                    // add claims if we want - don't do first one
                    //userManager.AddClaim(user.Id, new Claim(ClaimTypes.Role, "Administrator"));
                    userManager.AddClaim(user.Id, new Claim(ClaimTypes.GivenName, "System"));
                    userManager.AddClaim(user.Id, new Claim(ClaimTypes.Surname, "Administrator"));
                }

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Administrator" });
                context.SaveChanges();
                userManager.AddToRole(user.Id, "Administrator");
                userManager.AddToRole(user.Id, "Manager");
                userManager.AddToRole(user.Id, "Clerk");
                context.SaveChanges();


            }

            //if (!context.Users.Any(u => u.UserName == "Room100"))
            //{
            //    for (int i = 100; i <= 120; i++)
            //    {
            //        var user = new ApplicationUser
            //        {
            //            UserName = "Room" + i
            //        };

            //        IdentityResult result = userManager.Create(user, "0" + i);
            //        context.SaveChanges();
            //        userManager.AddToRole(user.Id, "Guest");
            //        context.SaveChanges();

            //    }
            //    for (int i = 200; i <= 220; i++)
            //    {
            //        var user = new ApplicationUser
            //        {
            //            UserName = "Room" + i
            //        };

            //        IdentityResult result = userManager.Create(user, "0" + i);
            //        context.SaveChanges();
            //        userManager.AddToRole(user.Id, "Guest");
            //        context.SaveChanges();

            //    }
            //    for (int i = 300; i <= 320; i++)
            //    {
            //        var user = new ApplicationUser
            //        {
            //            UserName = "Room" + i
            //        };

            //        IdentityResult result = userManager.Create(user, "0" + i);
            //        context.SaveChanges();
            //        userManager.AddToRole(user.Id, "Guest");
            //        context.SaveChanges();

            //    }
            //    context.SaveChanges();
            //}

            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
