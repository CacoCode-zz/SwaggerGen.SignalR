using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using SignalRSample.Models;
using SwaggerGen.SignalR.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRSample.Hubs
{
    [SignalRHub("chatHub")]
    public class ChatHub : Hub
    {
        /// <summary>
        /// 客户端调用方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task SendMessage(ChatMessageInfo data)
        {
            //服务端返回是调用方法
            return Clients.All.SendAsync("ReceiveMessage", data);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToGroup(string message)
        {
            return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", message);
        }
    }
}
