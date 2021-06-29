using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SwaggerGen.SignalR;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerGenOptionBuilderExtensions
    {
        public static void SwaggerSignalR(this SwaggerGenOptions swaggerGenOptions,List<Assembly> assemblies = null)
        {
            if (assemblies == null)
            {
                var assembly = Assembly.GetEntryAssembly();
                var types = assembly.GetTypes().Where(t => t.BaseType == typeof(Hub));
                assemblies = types.Select(a => a.Assembly).ToList();
            }
            swaggerGenOptions.DocumentFilter<SignalRSwaggerGen>(assemblies);
        }
    }
}
