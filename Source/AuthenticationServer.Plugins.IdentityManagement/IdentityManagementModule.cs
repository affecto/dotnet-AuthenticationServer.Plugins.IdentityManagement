using System;
using System.Collections.Generic;
using System.Linq;
using Affecto.AuditTrail.Interfaces;
using Affecto.AuditTrail.Interfaces.Model;
using Affecto.AuthenticationServer.Plugins.IdentityManagement.Configuration;
using Affecto.AuthenticationServer.Plugins.Infrastructure;
using Autofac;
using IdentityServer3.Core.Services;

namespace Affecto.AuthenticationServer.Plugins.IdentityManagement
{
    public class IdentityManagementModule : FederatedAuthenticationPluginModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(CreateIdentityManagementConfiguration).SingleInstance().As<IIdentityManagementConfiguration>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterModule<Affecto.IdentityManagement.Autofac.ModuleRegistration>();
            builder.RegisterModule<Affecto.IdentityManagement.Store.PostgreSql.ModuleRegistration>();

            builder.RegisterType<AuditTrailMock>().As<IAuditTrailService>();
        }

        private static IIdentityManagementConfiguration CreateIdentityManagementConfiguration(IComponentContext arg)
        {
            return IdentityManagementConfiguration.Settings;
        }
    }

    internal class AuditTrailMock : IAuditTrailService
    {
        public IEnumerable<IAuditTrailEntry> GetEntries()
        {
            return Enumerable.Empty<IAuditTrailEntry>();
        }

        public IAuditTrailResult GetEntries(IAuditTrailFilter filter)
        {
            return null;
        }

        public IEnumerable<IAuditTrailEntry> GetEntriesForSubject(Guid subjectId)
        {
            return Enumerable.Empty<IAuditTrailEntry>();
        }

        public IAuditTrailEntry GetEntry(Guid auditTrailEntryId)
        {
            return null;
        }

        public IAuditTrailEntry CreateEntry(Guid subjectId, string summary, string subjectName, string userName)
        {
            return null;
        }

        public IAuditTrailEntry CreateEntry(Guid subjectId, Guid userId, string summary, string subjectName, string userName)
        {
            return null;
        }
    }
}