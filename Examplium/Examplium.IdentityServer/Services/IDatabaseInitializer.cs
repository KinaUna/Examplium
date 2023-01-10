namespace Examplium.IdentityServer.Services
{
    public interface IDatabaseInitializer
    {
        void InitializeDatabase();
        void AddDefaultClients();
        void ResetClients();
        void AddDefaultIdentityResources();
        void ResetIdentityResources();
        void AddDefaultApiScopes();
        void ResetApiScopes();
    }
}
