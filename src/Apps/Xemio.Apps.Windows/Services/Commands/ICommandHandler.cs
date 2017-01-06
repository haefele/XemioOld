using System.Threading.Tasks;

namespace Xemio.Apps.Windows.Services.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}