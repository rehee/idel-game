using Core.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubServer
{
    public class HubsService : IHubsService
    {
        public Dictionary<string, IHubProcessUnit> HubPool { get; set; } = new Dictionary<string, IHubProcessUnit>();
        IEnveroment env;

        private IHubProcessUnit newUnit;
        public HubsService(IEnveroment env)
        {
            this.env = env;
        }

        public IHubProcessUnit ConnectHub(string clientId, IHubProcessUnit hubProcessUnit)
        {
            if (HubPool.ContainsKey(clientId))
                return HubPool[clientId];
            HubPool.Add(clientId, hubProcessUnit);
            return hubProcessUnit;
        }

        public void SetHubProcessUnit(IHubProcessUnit unit)
        {
            this.newUnit = unit;
        }
    }
}
