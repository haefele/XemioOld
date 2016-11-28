using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.Server.Contracts.Mapping
{
    public interface IMapper<TIn, TOut>
    {
        Task<TOut> MapAsync(TIn input);

        Task<IList<TOut>> MapListAsync(IList<TIn> input);
    }
}
