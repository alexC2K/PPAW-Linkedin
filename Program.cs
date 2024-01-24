using Linkedin.DbModels.Database;
using Linkedin.Services.Applications;
using Linkedin.Services.Authentication;
using Linkedin.Services.Cache;
using Linkedin.Services.Companies;
using Linkedin.Services.Jobs;
using Linkedin.Services.Logger;
using Linkedin.Services.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    });

    // Setting up DB.
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddControllersWithViews();

    // Setting up services.
    builder.Services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
    builder.Services.AddTransient<ILoggerService, LoggerService>();
    builder.Services.AddTransient<ICacheService, CacheService>();
    builder.Services.AddTransient<IWebHostBuilder, WebHostBuilder>();
    builder.Services.AddTransient<IApplicationsService, ApplicationsService>();
    builder.Services.AddTransient<IUsersService, UsersService>();
    builder.Services.AddTransient<ICompaniesService, CompaniesService>();
    builder.Services.AddTransient<IJobsService, JobsService>();
    builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

    // Setting up Authentication.
    builder.Services
        .AddAuthentication(options => options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.SlidingExpiration = true;
            options.LoginPath = "/Account/LoginRegister";
        });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();   
    app.UseRouting();
    app.UseAuthorization();
    app.UseAuthentication();

    // Setting up routes.
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "Users",
            pattern: "{controller=Users}/{action=Home}/{id?}"
        );

        endpoints.MapControllerRoute(
            name: "LoginRegister",
            pattern: "{controller=Account}/{action=LoginRegister}/{id?}"
        );

        endpoints.MapControllerRoute(
            name: "Jobs",
            pattern: "{controller=Jobs}/{action=Home}/{id?}"
        );

        endpoints.MapControllerRoute(
            name: "Company",
            pattern: "{controller=Company}/{action=Home}/{id?}"
        );
    });

    app.Run();
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}