namespace WatsonTracker.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WatsonTracker.Models;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole();

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                role = new IdentityRole { Name = "Developer" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                role = new IdentityRole { Name = "Submitter" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                role = new IdentityRole { Name = "ProjectManager" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Tester"))
            {
                role = new IdentityRole { Name = "Tester" };
                manager.Create(role);
            }
            //context.Roles.AddOrUpdate(n => n.Name == "Tester");

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];

            if (!context.Users.Any(u => u.Email == "kgycoder63@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "kgycoder63@gmail.com",
                    Email = "kgycoder63@gmail.com",
                    FirstName = "Kim",
                    LastName = "Yount",
                    DisplayName = "Kimmysan"
                };
                userManager.Create(user, "newKim2020");

                userManager.AddToRoles(user.Id, new string[]
                {
                    "Admin",
                    "ProjectManager",
                    "Developer",
                    "Submitter"
                });
            }
            //var user = new ApplicationUser
            //{
            //    UserName = "Admin1@mailanator.com",
            //    Email = "Admin1@mailanator.com",
            //    FirstName = "Kim",
            //    LastName = "Yount",
            //    DisplayName = "Kimmysan"
            //};


            if (!context.Users.Any(u => u.Email == "admin@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@demo.com",
                    Email = "admin@demo.com",
                    FirstName = "Administrator",
                    LastName = "Role",
                    DisplayName = "ADMIN"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id, new string[]
                {
                    "Admin"
                });
            }

            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoAdmin@mailinator.com",
                    Email = "DemoAdmin@mailinator.com",
                    FirstName = "Jane",
                    LastName = "Doe",
                    DisplayName = "DEMOADMIN"
                };
                userManager.Create(user, demoPassword);
                userManager.AddToRoles(user.Id, new string[] { "Admin" });

            }
            if (!context.Users.Any(u => u.Email == "DemoDev@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoDev@mailinator.com",
                    Email = "DemoDev@mailinator.com",
                    FirstName = "Jupiter",
                    LastName = "Doe",
                    DisplayName = "DEMODEV"
                };
                userManager.Create(user, demoPassword);
                userManager.AddToRoles(user.Id, new string[] { "Developer" });

            }
            if (!context.Users.Any(u => u.Email == "DemoSub@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoSub@mailinator.com",
                    Email = "DemoSub@mailinator.com",
                    FirstName = "January",
                    LastName = "Doe",
                    DisplayName = "DEMOSUB"
                };
                userManager.Create(user, demoPassword);
                userManager.AddToRoles(user.Id, new string[] { "Submitter" });

            }
            if (!context.Users.Any(u => u.Email == "DemoMgr@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoMgr@mailinator.com",
                    Email = "DemoMgr@mailinator.com",
                    FirstName = "Juniper",
                    LastName = "Doe",
                    DisplayName = "DEMOMGR"
                };
                userManager.Create(user, demoPassword);
                userManager.AddToRoles(user.Id, new string[] { "ProjectManager" });

            }
            if (!context.Users.Any(u => u.Email == "manager@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "manager@demo.com",
                    Email = "manager@demo.com",
                    FirstName = "Manager",
                    LastName = "Role",
                    //DisplayName = "MANGR"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "ProjectManager"
                    });
            }
            if (!context.Users.Any(u => u.Email == "developer@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "developer@demo.com",
                    Email = "developer@demo.com",
                    FirstName = "Developer",
                    LastName = "Role",
                    //DisplayName = "DEVPR"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Developer"
                    });
            }
            if (!context.Users.Any(u => u.Email == "submitter@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "submitter@demo.com",
                    Email = "submitter@demo.com",
                    FirstName = "Submitter",
                    LastName = "Role",
                    //DisplayName = "SUBMT"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Submitter"
                    });
            }
            if (!context.Users.Any(u => u.Email == "testuser1@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser1@btracker.com",
                        Email = "testuser1@btracker.com",
                        FirstName = "Test1",
                        LastName = "User1",
                        //DisplayName = "ImaTest1"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser2@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser2@btracker.com",
                        Email = "testuser2@btracker.com",
                        FirstName = "Test2",
                        LastName = "User2",
                        //DisplayName = "ImaTest2"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser3@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser3@btracker.com",
                        Email = "testuser3@btracker.com",
                        FirstName = "Test3",
                        LastName = "User3",
                        //DisplayName = "ImaTest3"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser4@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser4@btracker.com",
                        Email = "testuser4@btracker.com",
                        FirstName = "Test4",
                        LastName = "User4",
                        //DisplayName = "ImaTest4"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser5@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser5@btracker.com",
                        Email = "testuser5@btracker.com",
                        FirstName = "Test5",
                        LastName = "User5",
                        //DisplayName = "ImaTest5"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser6@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser@btracker.com",
                        Email = "testuser6@btracker.com",
                        FirstName = "Test6",
                        LastName = "User6",
                        //DisplayName = "ImaTest6"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser7@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser7@btracker.com",
                        Email = "testuser7@btracker.com",
                        FirstName = "Test7",
                        LastName = "User7",
                        //DisplayName = "ImaTest7"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser8@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser8@btracker.com",
                        Email = "testuser8@btracker.com",
                        FirstName = "Test8",
                        LastName = "User8",
                        //DisplayName = "ImaTest8"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser9@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser9@btracker.com",
                        Email = "testuser9@btracker.com",
                        FirstName = "Test9",
                        LastName = "User9",
                        //DisplayName = "ImaTest9"
                    }, "Abc&123!");
                }
            }
            if (!context.Users.Any(u => u.Email == "testuser10@btracker.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser10@btracker.com",
                        Email = "testuser10@btracker.com",
                        FirstName = "Test10",
                        LastName = "User10",
                        //DisplayName = "ImaTest10"
                    }, "Abc&123!");
                }
            }
            if (!context.TicketPriorities.Any(u => u.Name == "High"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "High" });
            }
            if (!context.TicketPriorities.Any(u => u.Name == "Medium"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Medium" });
            }
            if (!context.TicketPriorities.Any(u => u.Name == "Low"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Low" });
            }
            if (!context.TicketPriorities.Any(u => u.Name == "Urgent"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Urgent" });
            }
            if (!context.TicketPriorities.Any(u => u.Name == "High"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "high" });
            }
            if (!context.TicketTypes.Any(u => u.Name == "Production Fix"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Production Fix" });
            }
            if (!context.TicketTypes.Any(u => u.Name == "Project Task"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Project Task" });
            }
            if (!context.TicketTypes.Any(u => u.Name == "Software Update"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Software Update" });
            }
            if (!context.TicketStatus.Any(u => u.Name == "New"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "New" });
            }
            if (!context.TicketStatus.Any(u => u.Name == "In Development"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "In Development" });
            }
            if (!context.TicketStatus.Any(u => u.Name == "Completed"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "Completed" });
            }
        }
    }
}

