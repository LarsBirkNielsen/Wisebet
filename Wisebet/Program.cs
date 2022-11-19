using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wisebet.Authorization;
using Wisebet.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>() //We can assign Roles to our identity User
        .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

//Custom figuration for creating a user
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true; //Password must have a digit
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Lockout.MaxFailedAccessAttempts = 3; //Can only try to type in password 3 times
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); //If fail to login within 3 times, you are Locked out for 3 minutes
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true; //User have to have an unique email
});

////Our fallback policy. You are only have Authorization if you are Authendicated
//builder.Services.AddAuthorization(options =>
//{

//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();

//});

//Adding our Authorization handlers
builder.Services.AddScoped<IAuthorizationHandler, PredictionCreatorAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, PredictionAdminAuthorizationHandler>();

var app = builder.Build();

//Calling the SeedData function
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //When we deploy our application a database will be crated when it starts for the first time if the requirements are met ( you have an sql database installed ex. )
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    //Getting the password from our user secrets.
    //Right click project folder and choose "Manage User Secrets" to create secret password so when you upload project people cant see your password
    var seedUserPass = builder.Configuration.GetValue<string>("SeedUserPassword");
    await SeedData.Initialize(services, seedUserPass);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
