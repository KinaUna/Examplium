using Duende.IdentityServer.AspNetIdentity;
using Examplium.IdentityServer.Data;
using Examplium.IdentityServer.Services;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
var connectionString = builder.Configuration["DefaultDatabaseConnection"] ?? throw new InvalidOperationException("Configuration string 'DefaultDatabaseConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql =>
    {
        sql.MigrationsAssembly(migrationsAssembly);
        sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    }));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache();

var coreApiSecretString = builder.Configuration["CoreApiSecret"] ?? throw new InvalidOperationException("Configuration string 'CoreApiSecret' not found.");

builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.EmitStaticAudienceClaim = true;
    })
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder =>
            optionsBuilder.UseSqlServer(connectionString,
                sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                    sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder =>
            optionsBuilder.UseSqlServer(connectionString,
                sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                    sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
    })
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryCaching()
    .AddConfigurationStoreCache()
    .AddProfileService<ProfileService<ApplicationUser>>();

builder.Services.AddAuthentication();

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

IdentityServerInitialization.InitializeDatabase(app, coreApiSecretString);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
