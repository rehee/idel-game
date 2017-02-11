using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Hubs
{
    public interface IHubsService
    {
        Dictionary<string, IHubProcessUnit> HubPool { get; set; }

        void SetHubProcessUnit(IHubProcessUnit unit);
        IHubProcessUnit ConnectHub(string clientId, IHubProcessUnit hubProcessUnit);
    }
}
