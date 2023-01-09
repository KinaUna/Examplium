using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authorization;
// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Ciba
{
    [Authorize]
    [SecurityHeadersAttribute]
    public class ConsentModel : PageModel
    {
        private readonly IBackchannelAuthenticationInteractionService _interaction;
        private readonly IEventService _events;
        private readonly ILogger<Index> _logger;

        public ConsentModel(
            IBackchannelAuthenticationInteractionService interaction,
            IEventService events,
            ILogger<Index> logger)
        {
            _interaction = interaction;
            _events = events;
            _logger = logger;
        }

        public CibaViewModel? View { get; set; } = new CibaViewModel();

        [BindProperty]
        public CibaInputModel Input { get; set; } = new CibaInputModel();

        public async Task<IActionResult> OnGet(string id)
        {
            View = await BuildViewModelAsync(id);
            if (View == null)
            {
                return RedirectToPage("/Home/Error/Index");
            }

            Input = new CibaInputModel
            {
                Id = id
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // validate return url is still valid
            var request = await _interaction.GetLoginRequestByInternalIdAsync(Input?.Id);
            if (request == null || request.Subject.GetSubjectId() != User.GetSubjectId())
            {
                _logger.LogError("Invalid id {id}", Input?.Id);
                return RedirectToPage("/Home/Error/Index");
            }

            CompleteBackchannelLoginRequest? result = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (Input?.Button == "no")
            {
                result = new CompleteBackchannelLoginRequest(Input.Id);

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            }
            // user clicked 'yes' - validate the data
            else if (Input?.Button == "yes")
            {
                // if the user consented to some scope, build the response model
                if (Input.ScopesConsented != null && Input.ScopesConsented.Any())
                {
                    var scopes = Input.ScopesConsented;
                    if (CibaConsentOptions.EnableOfflineAccess == false)
                    {
                        scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                    }

                    result = new CompleteBackchannelLoginRequest(Input.Id)
                    {
                        ScopesValuesConsented = scopes.ToArray(),
                        Description = Input.Description
                    };

                    // emit event
                    await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, result.ScopesValuesConsented, false));
                }
                else
                {
                    ModelState.AddModelError("", CibaConsentOptions.MustChooseOneErrorMessage);
                }
            }
            else
            {
                ModelState.AddModelError("", CibaConsentOptions.InvalidSelectionErrorMessage);
            }

            if (result != null)
            {
                // communicate outcome of consent back to identityserver
                await _interaction.CompleteLoginRequestAsync(result);

                return RedirectToPage("/Ciba/All");
            }

            // we need to redisplay the consent UI
            if(Input != null && Input.Id != null )
            {            
                View = await BuildViewModelAsync(Input.Id, Input);
            }

            return Page();
        }

        private async Task<CibaViewModel?> BuildViewModelAsync(string id, CibaInputModel? model = null)
        {
            var request = await _interaction.GetLoginRequestByInternalIdAsync(id);
            if (request != null && request.Subject.GetSubjectId() == User.GetSubjectId() && model != null)
            {
                return CreateConsentViewModel(model, id, request);
            }
            else
            {
                _logger.LogError("No backchannel login request matching id: {id}", id);
            }
            return null;
        }

        private CibaViewModel CreateConsentViewModel(
            CibaInputModel model, string id,
            BackchannelUserLoginRequest request)
        {
            var vm = new CibaViewModel
            {
                ClientName = request.Client.ClientName ?? request.Client.ClientId,
                ClientUrl = request.Client.ClientUri,
                ClientLogoUrl = request.Client.LogoUri,
                BindingMessage = request.BindingMessage
            };

            vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources
                .Select(x => CreateScopeViewModel(x, model?.ScopesConsented == null || model.ScopesConsented?.Contains(x.Name) == true))
                .ToArray();

            var resourceIndicators = request.RequestedResourceIndicators ?? Enumerable.Empty<string>();
            var apiResources = request.ValidatedResources.Resources.ApiResources.Where(x => resourceIndicators.Contains(x.Name));

            var apiScopes = new List<ScopeViewModel>();
            foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
            {
                var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
                if (apiScope != null)
                {
                    var scopeVm = CreateScopeViewModel(parsedScope, apiScope, model == null || model.ScopesConsented?.Contains(parsedScope.RawValue) == true);
                    scopeVm.Resources = apiResources.Where(x => x.Scopes.Contains(parsedScope.ParsedName))
                        .Select(x => new ResourceViewModel
                        {
                            Name = x.Name,
                            DisplayName = x.DisplayName ?? x.Name,
                        }).ToArray();
                    apiScopes.Add(scopeVm);
                }
            }
            if (CibaConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
            {
                apiScopes.Add(GetOfflineAccessScope(model == null || model.ScopesConsented?.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess) == true));
            }
            vm.ApiScopes = apiScopes;

            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                Value = identity.Name,
                DisplayName = identity.DisplayName ?? identity.Name,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        public ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
        {
            var displayName = apiScope.DisplayName ?? apiScope.Name;
            if (!String.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
            {
                displayName += ":" + parsedScopeValue.ParsedParameter;
            }

            return new ScopeViewModel
            {
                Name = parsedScopeValue.ParsedName,
                Value = parsedScopeValue.RawValue,
                DisplayName = displayName,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = check || apiScope.Required
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = CibaConsentOptions.OfflineAccessDisplayName,
                Description = CibaConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }
    }
}
