using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Xemio.Server.Infrastructure.Formatter;

namespace Xemio.Hosts.AspNetCore.Setup
{

    public static class Protobuf
    {        
        public static void AddProtobufFormatters(this MvcOptions self)
        {
            self.InputFormatters.Add(new ProtobufInputFormatter());
            self.OutputFormatters.Add(new ProtobufOutputFormatter());
        }
    }
}
