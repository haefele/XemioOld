using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Formatter
{
    public class ProtobufInputFormatter : InputFormatter
    {
        private Dictionary<Type, MessageParser> _typeToParser;

        public ProtobufInputFormatter()
        {
            this._typeToParser = new Dictionary<Type, MessageParser>();

            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));
        }

        private MessageParser GetMessageParser(Type type)
        {
            if (this._typeToParser.TryGetValue(type, out MessageParser parser) == false)
            {
                lock (this._typeToParser)
                {
                    var parserProperty = type.GetTypeInfo().GetProperty(nameof(FolderDTO.Parser));

                    if (parserProperty == null)
                        throw new InvalidOperationException($"The type {type.FullName} has to be generated through a protobuf .proto file.");

                    parser = (MessageParser)parserProperty.GetValue(null);

                    this._typeToParser.Add(type, parser);
                }
            }

            return parser;
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var parser = this.GetMessageParser(context.ModelType);
            var result = parser.ParseFrom(context.HttpContext.Request.Body);

            return InputFormatterResult.SuccessAsync(result);
        }
    }
}
