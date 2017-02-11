using Core.Hubs;
using Core.Logics.Manage;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.StageService;
using System.Threading;
using System.Threading.Tasks;

namespace learnCoreMoltiThread.Hubs
{
    public class MonitorHub : Hub, IHubs
    {
        private const int messageTick = 100;
        private IHubsService hubService;
        private IEnveroment env;
        private static Dictionary<string, bool> requestPool = new Dictionary<string, bool>();
        public MonitorHub(IHubsService hubService, IEnveroment env)
        {
            this.hubService= hubService;
            this.env = env;
        }
        public void ConnectHub()
        {
            Command(new List<string>());
        }
        public void Command(List<string> Command)
        {
            var hubUnit = CheckConnection();
            hubUnit.DoCommand(Command);
        }
        private void PushMessage(string clientId)
        {
            if (!hubService.HubPool.ContainsKey(clientId))
                return;
            if (hubService.HubPool[clientId].ProcessIng)
                return;
            Thread hubProcess = new Thread(new ThreadStart(() =>
            {
                string id = clientId;
                if (!hubService.HubPool.ContainsKey(clientId))
                    return;
                var hs = hubService.HubPool[clientId];
                hs.StartProcess();
                while (hs.ProcessIng)
                {
                    if (!hubService.HubPool.ContainsKey(clientId))
                    {
                        hs.StopProcess();
                        break;
                    }
                    Clients.Client(clientId).state(hubService.HubPool[clientId].PushMessage());
                    System.Threading.Thread.Sleep(messageTick);
                }
            }));
            hubProcess.Start();
        }

        public override Task OnConnected()
        {
            Task endtask = new Task(() =>
            {
                Clients.All.state("123");
            });
            endtask.Start();
            return endtask;
        }

        public override Task OnDisconnected(bool disconnect)
        {
            Task endtask = new Task(() =>
            {
                var id = Context.ConnectionId;
                try
                {
                    var processUnit = hubService.HubPool[id];
                    processUnit.StopProcess();
                    try
                    {
                        hubService.HubPool.Remove(id);
                    }
                    catch { }
                }
                catch { }
            });
            endtask.Start();
            return endtask;
        }


        private IHubProcessUnit CheckConnection()
        {
            var clientId = Context.ConnectionId;
            var hubUnit = this.hubService.ConnectHub(clientId, env.NewMonitorHubProcessUnit());
            hubUnit.StopProcess();
            PushMessage(clientId);
            return hubUnit;
        }


    }
}
