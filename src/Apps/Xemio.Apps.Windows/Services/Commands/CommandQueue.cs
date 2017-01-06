using System.Reflection;
using System.Threading.Tasks;
using Autofac;

namespace Xemio.Apps.Windows.Services.Commands
{
    public class CommandQueue : ICommandQueue
    {
        private readonly IContainer _container;

        public CommandQueue(IContainer container)
        {
            this._container = container;
        }

        public async Task EnqueueCommandAsync(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = this._container.Resolve(handlerType);
            var method = handler.GetType().GetMethod(nameof(ICommandHandler<ICommand>.ExecuteAsync));

            await (Task)method.Invoke(handler, new object[] { command });
        }
    }
}