using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xemio.Server.Contracts.Mapping
{
    public interface IMapper<TIn, TOut>
    {
        Task<TOut> MapAsync(TIn input, CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<TOut>> MapListAsync(IList<TIn> input, CancellationToken cancellationToken = default(CancellationToken));
    }
}
