using System.Threading.Tasks;

namespace Xemio.Apps.Windows.Services.Commands
{
    public interface ICommandQueue
    {
        Task EnqueueCommandAsync(ICommand command);
    }
}