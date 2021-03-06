﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Affecto.AuthenticationServer.Plugins.IdentityManagement.Configuration;
using Affecto.AuthenticationServer.Plugins.Infrastructure;
using Affecto.AuthenticationServer.Plugins.Infrastructure.Configuration;
using Affecto.IdentityManagement.Interfaces;
using Affecto.IdentityManagement.Interfaces.Model;
using IdentityServer3.Core.Models;
using Microsoft.AspNet.Identity;

namespace Affecto.AuthenticationServer.Plugins.IdentityManagement
{
    internal class UserService : FederatedAuthenticationUserServiceBase
    {
        private readonly IUserService userService;
        private readonly Lazy<IIdentityManagementConfiguration> identityManagementConfiguration;

        public UserService(IUserService userService, Lazy<IIdentityManagementConfiguration> identityManagementConfiguration, 
            Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration)
            : base(federatedAuthenticationConfiguration)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (identityManagementConfiguration == null)
            {
                throw new ArgumentNullException(nameof(identityManagementConfiguration));
            }

            this.userService = userService;
            this.identityManagementConfiguration = identityManagementConfiguration;
        }

        protected override void CreateOrUpdateExternallyAuthenticatedUser(string userAccountName, string userDisplayName, IReadOnlyCollection<string> groups)
        {
            if (!userService.IsExistingUserAccount(userAccountName, AccountType.Federated))
            {
                if (identityManagementConfiguration.Value.AutoCreateUser)
                {
                    IEnumerable<KeyValuePair<string, string>> customProperties = CreateCustomProperties();
                    userService.AddUser(userAccountName, AccountType.Federated, userDisplayName, groups, customProperties);
                }
            }
            else
            {
                userService.UpdateUserGroupsAndRoles(userAccountName, AccountType.Federated, groups);
            }
        }

        protected override AuthenticateResult CreateAuthenticateResult(string userName, string authenticationType, string identityProvider = "idsrv")
        {
            var identityBuilder = new ClaimsIdentityBuilder(userService);
            ClaimsIdentity identity;
            switch (authenticationType)
            {
                case AuthenticationTypes.Password:
                    identity = identityBuilder.Build(authenticationType, userName, AccountType.Password);
                    break;
                case AuthenticationTypes.Federation:
                    identity = identityBuilder.Build(authenticationType, userName, AccountType.Federated);
                    break;
                default:
                    throw new ArgumentException($"Authentication type '{authenticationType}' not supported.");

            }
            return new AuthenticateResult(identity.GetUserId(), identity.Name, identity.Claims, identityProvider);
        }

        protected override bool IsMatchingPassword(string userName, string password)
        {
            return userService.IsMatchingPassword(userName, password);
        }

        private IEnumerable<KeyValuePair<string, string>> CreateCustomProperties()
        {
            return identityManagementConfiguration.Value.NewUserCustomProperties.Select(customProperty => new KeyValuePair<string, string>(customProperty.Name, customProperty.Value));
        }
    }
}