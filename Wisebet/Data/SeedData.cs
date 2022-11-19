using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wisebet.Authorization;

namespace Wisebet.Data
{
    public class SeedData
    {
        //Add the Initiliaz function the "Program.cs" so it gets called when the application starts
        public static async Task Initialize(
            IServiceProvider serviceProvider,
            string password = "Test@1234")
        {

            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {


                // administrator
                var adminUid = await EnsureUser(serviceProvider, "admin@demo.com", "Test@1234");
                await EnsureRole(serviceProvider, adminUid, Constants.PredictionAdminRole); //Adding the Admin Role
            }
        }

        //Creating a new user if there is no users in the database
        private static async Task<string> EnsureUser(
            IServiceProvider serviceProvider,
            string userName, string initPassword)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, initPassword);
            }

            if (user == null)
                throw new Exception("User did not get created. Password policy problem?");

            return user.Id;
        }


        //Assignign a "Role" to a user a specific in the database
        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider, string userId, string role)
        {

            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult identityResult;

            //Checking if the role exits, if not we create it
            if (await roleManager.RoleExistsAsync(role) == false)
            {
                identityResult = await roleManager.CreateAsync(new IdentityRole(role));
            }


            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(userId);

            //Checking if the user exists in the database. If not => throw exception, if it does exists => add the role to the user
            if (user == null)
                throw new Exception("User not existing");


            // Assigning/Adding the role to the user.
            identityResult = await userManager.AddToRoleAsync(user, role);

            return identityResult;
        }

    }
}
