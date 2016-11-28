using System;
using Xemio.Server.Contracts.Mapping;
using System.Threading.Tasks;

namespace Xemio.Server.Infrastructure.Mapping
{
    public class GuidToStringMapper : MapperBase<Guid?, string>
    {
        public override Task<string> MapAsync(Guid? input)
        {
            var result = input?.ToString("D");
            return Task.FromResult(result);
        }
    }
}
