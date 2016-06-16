using System.Collections.Generic;

namespace Affecto.AuthenticationServer.Plugins.IdentityManagement.Configuration
{
    public interface IIdentityManagementConfiguration
    {
        bool AutoCreateUser { get; }
        IReadOnlyCollection<ICustomProperty> NewUserCustomProperties { get; }
    }
}