using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.Server.Contracts.Mapping
{
    public abstract class MapperBase<TIn, TOut> : IMapper<TIn, TOut>
    {
        public abstract Task<TOut> MapAsync(TIn input);

        public async Task<IList<TOut>> MapListAsync(IList<TIn> input)
        {
            var result = new List<TOut>();
            foreach (var i in input)
            {
                result.Add(await this.MapAsync(i));
            }
            return result;
        }
    }
}
