using Examplium.IdentityServer.Services;

namespace Examplium.IdentityServer.Data
{
    public static class IdentityServerInitialization
    {

        public static void InitializeDatabase(WebApplication app)
        {
            using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
            
            var databaseInitializer = serviceScope?.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            
            databaseInitializer?.InitializeDatabase();
        }
    }
}
