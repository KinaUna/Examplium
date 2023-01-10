using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Examplium.IdentityServer.Data;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Examplium.IdentityServer.Services
{
    public class DatabaseInitializer: IDatabaseInitializer
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseInitializer(IConfiguration configuration, ConfigurationDbContext configurationDbContext, PersistedGrantDbContext persistedGrantDbContext, ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _configurationDbContext = configurationDbContext;
            _applicationDbContext = applicationDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void InitializeDatabase()
        {
            MigrateDatabases();

            AddDefaultClients();

            AddDefaultIdentityResources();

            AddDefaultApiScopes();
        }

        private void MigrateDatabases()
        {
            _applicationDbContext.Database.Migrate();
            _configurationDbContext.Database.Migrate();
            _persistedGrantDbContext.Database.Migrate();
        }

        public void AddDefaultClients()
        {
            var coreApiSecretString = _configuration["CoreApiSecret"] ?? throw new InvalidOperationException("Configuration string 'CoreApiSecret' not found.");
            if (!_configurationDbContext.Clients.Any())
            {
                foreach (var client in IdentityServerConfiguration.Clients(coreApiSecretString))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }
        }

        public void ResetClients()
        {
            List<Client> existingClients = _configurationDbContext.Clients.ToList();
            foreach (Client client in existingClients)
            {
                _configurationDbContext.Clients.Remove(client);
            }

            _configurationDbContext.SaveChanges();

            AddDefaultClients();
        }

        public void AddDefaultIdentityResources()
        {
            if (!_configurationDbContext.IdentityResources.Any())
            {
                foreach (var identityResource in IdentityServerConfiguration.IdentityResources)
                {
                    _configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }
        }

        public void ResetIdentityResources()
        {
            List<IdentityResource> existingIdentityResources = _configurationDbContext.IdentityResources.ToList();
            foreach (IdentityResource identityResource in existingIdentityResources)
            {
                _configurationDbContext.IdentityResources.Remove(identityResource);
            }

            _configurationDbContext.SaveChanges();

            AddDefaultIdentityResources();
        }

        public void AddDefaultApiScopes()
        {
            if (!_configurationDbContext.ApiScopes.Any())
            {
                foreach (var apiScope in IdentityServerConfiguration.ApiScopes)
                {
                    _configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }
        }

        public void ResetApiScopes()
        {
            List<ApiScope> existingApiScopes = _configurationDbContext.ApiScopes.ToList();
            foreach (ApiScope apiScopeToDelete in existingApiScopes)
            {
                _configurationDbContext.ApiScopes.Remove(apiScopeToDelete);
            }

            _configurationDbContext.SaveChanges();

            AddDefaultApiScopes();
        }
    }
}
