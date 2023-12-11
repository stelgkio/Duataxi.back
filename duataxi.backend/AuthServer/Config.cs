﻿using System.Collections.Generic;
using IdentityServer4.Models;

namespace AuthServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                 new IdentityResource("role", new[] { "role" })
              
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resourceapi", "Resource API")
                {
                    Scopes = {new Scope("api.read")}
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client {
                    RequireConsent = false,
                    ClientId = "angular_spa",
                    ClientName = "Angular SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api.read","role" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600 ,
                    AllowOfflineAccess = false,                      
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    
                //  RequireConsent = false,
                //     ClientId = "angular_spa",
                //     ClientName = "Angular SPA",
                //     AllowedGrantTypes = GrantTypes.Implicit,
                //     AllowedScopes = { "openid", "profile", "email", "api.read" },
                //     RedirectUris = {"https://webeasytravel.firebaseapp.com/"},
                //     PostLogoutRedirectUris = {"https://webeasytravel.firebaseapp.com/"},
                //     AllowedCorsOrigins = {"https://webeasytravel.firebaseapp.com/"},
                //     AllowAccessTokensViaBrowser = true,
                //     AccessTokenLifetime = 3600
                }  ,
                  new Client {
                    RequireConsent = false,
                    ClientId = "postman",
                    ClientName = "Postman",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "openid", "profile", "email", "api.read","role" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600 ,
                    AllowOfflineAccess = false,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },                    
               
                }

            };
        }
    }
}
