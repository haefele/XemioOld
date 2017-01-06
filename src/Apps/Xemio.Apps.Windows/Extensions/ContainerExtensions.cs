using Autofac;
using Xemio.Apps.Windows.Services.Auth;
using Xemio.Client.Notes;

namespace Xemio.Apps.Windows.Extensions
{
    public static class ContainerExtensions
    {
        public static void UpdateClients(this IContainer container, User currentUser)
        {
            var builder = new ContainerBuilder();
            void RegisterWebService<T>() => builder.RegisterType<T>().AsImplementedInterfaces().SingleInstance().WithParameter(new PositionalParameter(0, currentUser?.IdToken));
            
            RegisterWebService<FoldersClient>();
            RegisterWebService<NotesClient>();

            builder.Update(container);
        }
    }
}
