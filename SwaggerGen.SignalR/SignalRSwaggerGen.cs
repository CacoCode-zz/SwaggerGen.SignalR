using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using SwaggerGen.SignalR.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SwaggerGen.SignalR
{
	public sealed class SignalRSwaggerGen : IDocumentFilter
	{
		private IEnumerable<Assembly> Assemblies { get; }

		public SignalRSwaggerGen(List<Assembly> assemblies)
		{
			if (assemblies == null || !assemblies.Any()) throw new ArgumentException("No assemblies provided", nameof(assemblies));
			Assemblies = assemblies;
		}

		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			var hubs = Assemblies.SelectMany(x => x.GetTypes().Where(t => t.BaseType == typeof(Hub)));
			foreach (var hub in hubs)
			{
				var path = hub.GetCustomAttribute<SignalRHubAttribute>().Path;
				if (string.IsNullOrEmpty(path))
				{
					path = hub.Name;
				}
				var methods = hub.GetMethods().Where(a => a.IsPublic && a.DeclaringType == hub);
                foreach (var method in methods)
                {
					var methodArgs = method.GetParameters();
					AddOpenApiPath(swaggerDoc, context, $"[Hub]{hub.Name}", $"{path}/{method.Name}",OperationType.Post, methodArgs);
				}
			}
		}

		private static void AddOpenApiPath(OpenApiDocument swaggerDoc, DocumentFilterContext context, string tag, string methodPath, OperationType operationType, IEnumerable<ParameterInfo> methodArgs)
		{
			swaggerDoc.Paths.Add(
				methodPath,
				new OpenApiPathItem
				{
					Operations = new Dictionary<OperationType, OpenApiOperation>
					{
						{
							operationType,
							new OpenApiOperation
							{
								Tags = new List<OpenApiTag> { new OpenApiTag { Name = tag } },
								Parameters = ToOpenApiParameters(context, methodArgs).ToList()
							}
						}
					}
				});
		}

		private static IEnumerable<OpenApiParameter> ToOpenApiParameters(DocumentFilterContext context, IEnumerable<ParameterInfo> args)
		{
			return args.Select(x =>
			{
				var param = new OpenApiParameter
				{
					Name = x.Name,
					In = ParameterLocation.Query
				};
				var schema = GetOpenApiSchema(context, x.ParameterType);
				param.Schema = schema.Reference == null
					? schema
					: new OpenApiSchema
					{
						Reference = new OpenApiReference
						{
							Id = schema.Reference.Id,
							Type = ReferenceType.Schema
						}
					};
				return param;
			});
		}

		private static OpenApiSchema GetOpenApiSchema(DocumentFilterContext context, Type type)
		{
			if (context.SchemaRepository.TryLookupByType(type, out OpenApiSchema schema)) return schema;
			return context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
		}
	}
}
