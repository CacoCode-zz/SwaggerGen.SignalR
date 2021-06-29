using System;
using System.Collections.Generic;
using System.Text;

namespace SwaggerGen.SignalR.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class SignalRHubAttribute : Attribute
    {
		public string Path { get; }
		public SignalRHubAttribute(string path)
		{
			if (string.IsNullOrEmpty(path)) throw new ArgumentException("Path is null or empty", nameof(path));
			Path = path;
		}
	}
}
