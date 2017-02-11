using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace learnCoreMoltiThread.Hubs
{
    public class SocketHandler
    {
        public const int BufferSize = 4096;
        WebSocket socket;
        IEnveroment env;
        private static List<SocketHandler> webSockets { get; set; } = new List<SocketHandler>();
        SocketHandler(WebSocket socket)
        {
            this.socket = socket;

        }


        public static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;
            //var name = hc.WebSockets.WebSocketRequestedProtocols.FirstOrDefault();

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket);
            webSockets.Add(h);
            if (webSockets.Count > 2)
            {
                if (webSockets[0] == webSockets[1])
                {
                    Console.WriteLine();
                }
            }

            await h.EchoLoop();
        }

        async Task EchoLoop()
        {
           
            var hubs = E.env.NewHubProcessUnit();
            if (!hubs.ProcessIng)
            {
                Task postData = new Task(() => {
                    hubs.StartProcess();
                    
                    while (hubs.ProcessIng)
                    {
                        try
                        {
                            var state = hubs.PushMessageC2().ToWebSocketOutPut();
                            this.socket.SendAsync(state, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        catch { }
                        System.Threading.Thread.Sleep(100);
                    }
                });
                postData.Start();
            }
            while (this.socket.State == WebSocketState.Open)
            {
                var buffer = new byte[BufferSize];
                var seg = new ArraySegment<byte>(buffer);
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);
                if(incoming.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                //var outgoing = new ArraySegment<byte>(buffer, 0, incoming.Count);
                //var str1 = System.Text.Encoding.UTF8.GetString(outgoing.Array).Split(' ');
                //byte[] toBytes = Encoding.UTF8.GetBytes(hubs.PushMessage());
                var str = buffer.WebSocketToString(incoming.Count).Replace("\0", "").Split(' ');
                Task processCommand = new Task(() =>
                {
                    
                    hubs.DoCommand(str.ToList());
                    var outPut = hubs.PushMessageC2().ToWebSocketOutPut();
                     this.socket.SendAsync(outPut, WebSocketMessageType.Text, true, CancellationToken.None);
                });
                processCommand.Start();
                //byte[] toBytes = Encoding.UTF8.GetBytes(new { c2dictionary=true, data = new { name="1" } }.ToJson());
                //var outPut = new ArraySegment<byte>(toBytes, 0, toBytes.Count());
                
                //await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            Console.WriteLine("closed");
            hubs.StopProcess();
        }
    }
}

namespace System
{
    public static class WebSocketExtend
    {
       
        public static string WebSocketToString(this byte[] buffer,int count)
        {
            var outgoing = new ArraySegment<byte>(buffer, 0, count);
            var str1 = System.Text.Encoding.UTF8.GetString(outgoing.Array);
            return str1;

        }

        public static ArraySegment<byte> ToWebSocketOutPut(this string input)
        {
            byte[] toBytes = Encoding.UTF8.GetBytes(input);
            var outPut = new ArraySegment<byte>(toBytes, 0, toBytes.Count());
            return outPut;
        }
    }
}
