using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Xemio.Server.Infrastructure.Formatter
{

    public class ProtobufOutputFormatter : OutputFormatter
    {
        public ProtobufOutputFormatter()
        {
            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            IMessage message = (IMessage)context.Object;
            message.WriteTo(context.HttpContext.Response.Body);

            return Task.CompletedTask;            
        }
    }
}
