using System;
using Affecto.AuthenticationServer.Plugins.IdentityManagement.Configuration;
using Affecto.Testing.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Plugins.IdentityManagement.ConfigurationTests
{
    [DeploymentItem("ConfigFiles")]
    public abstract class ConfigurationTestsBase
    {
        private readonly ConfigSectionReader configSectionReader = new ConfigSectionReader(Environment.CurrentDirectory);
        protected IIdentityManagementConfiguration identityManagementConfiguration;

        protected void SetupIdentityManagementConfiguration(string configFileName)
        {
            identityManagementConfiguration = configSectionReader.GetConfigSection<IdentityManagementConfiguration>(configFileName, "identityManagement");
        }
    }
}