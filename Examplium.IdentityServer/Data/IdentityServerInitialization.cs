using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Examplium.IdentityServer.Data
{
    public static class IdentityServerInitialization
    {
        public static void InitializeDatabase(WebApplication app, string coreApiSecretString)
        {
            using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
            serviceScope?.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope?.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context?.Database.Migrate();
            if (context != null && !context.Clients.Any())
            {
                foreach (var client in IdentityServerConfiguration.Clients(coreApiSecretString))
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (context != null && !context.IdentityResources.Any())
            {
                foreach (var resource in IdentityServerConfiguration.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (context != null && !context.ApiScopes.Any())
            {
                foreach (var resource in IdentityServerConfiguration.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
