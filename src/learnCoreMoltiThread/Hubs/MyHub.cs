using Core.Hubs;
using Core.Logics.Control;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace learnCoreMoltiThread.Hubs
{
    public class MyHub : Hub, IHubs
    {
        private const int messageTick = 100;
        private IHubsService hubService;
        private IEnveroment env;

        public MyHub(IHubsService hubService, IEnveroment env)
        {
            this.hubService = hubService;
            this.env = env;
        }

        public void ConnectHub()
        {
            Command(new List<string>());
        }
        private IHubProcessUnit CheckConnection()
        {
            var clientId = Context.ConnectionId;
            var hubUnit = this.hubService.ConnectHub(clientId, env.NewHubProcessUnit());
            hubUnit.StopProcess();
            PushMessage(clientId);
            return hubUnit;
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
            Task hubProcess = new Task((String) =>
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
                    Clients.Client(clientId).actionLog(hubService.HubPool[clientId].PushMessageActionLog());
                    System.Threading.Thread.Sleep(messageTick);
                }
            }, clientId);
            hubProcess.Start();
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

        public override Task OnConnected()
        {
            Task endtask = new Task(() =>
            {
                Clients.All.state("123");
            });
            endtask.Start();
            return endtask;
        }

        public void Send(string messages)
        {
            var clientId = Context.ConnectionId;
            Clients.All.hello($"Hi dear {clientId} your message is {messages}");
        }
    }


}
