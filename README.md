# SwaggerGen.SignalR

.Net Core swagger ui Integrated signalr secondary library

## Nuget
``` 
Install-Package SwaggerGen.SignalR -Version
```

## Use
引入Nuget包，在自己的Hub文件上标注专属特性 **[SignalRHub]**，特性需要传一个Path参数，来标明Hub路由

```cshrap
    [SignalRHub("chatHub")]
    public class ChatHub : Hub
    {
        public Task SendMessage(ChatMessageInfo data)
        {
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
```
