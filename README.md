# SwaggerGen.SignalR

.Net Core swagger ui Integrated signalr secondary library

## Nuget
``` 
Install-Package SwaggerGen.SignalR -Version
```

## Use
Introduce the nuget package to label the exclusive features on your own hub file [SignalRHub], attribute needs to pass a "Path" parameter to indicate the hub routing 

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
Integrate the swagger ui as usual 
```cshrap
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SignalRSample", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SignalRSample"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
```
Integrated signalr swagger ui 
```cshrap
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SignalRSample", Version = "v1" });
                c.SwaggerSignalR(); // This is the modification item
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SignalRSample"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>($"/{typeof(ChatHub).GetCustomAttribute<SignalRHubAttribute>().Path}"); // This is the modification item
                endpoints.MapControllers();
            });
        }
```

## Run
![image](https://user-images.githubusercontent.com/12271319/123726168-12775e80-d8c2-11eb-9447-dc3ebe02f795.png)
