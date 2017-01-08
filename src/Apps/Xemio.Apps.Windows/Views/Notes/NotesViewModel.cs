using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using UwCore;
using Xemio.Apps.Windows.Services.Commands;
using Xemio.Apps.Windows.Services.Commands.Folders;
using Xemio.Apps.Windows.Services.Queries;
using Xemio.Apps.Windows.Services.Queries.Folders;
using Xemio.Apps.Windows.ViewParts.NoteFolderHierarchy;
using Xemio.Client.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.Views.Notes
{
    public class NotesViewModel : UwCoreScreen
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandQueue _commandQueue;

        public NoteFolderHierarchyViewModel NoteFolderHierarchyViewModel { get; }

        public UwCoreCommand<Unit> CreateFolder { get; }

        public NotesViewModel(IQueryExecutor queryExecutor, ICommandQueue commandQueue)
        {
            this._queryExecutor = queryExecutor;
            this._commandQueue = commandQueue;

            this.NoteFolderHierarchyViewModel = IoC.Get<NoteFolderHierarchyViewModel>();
            this.NoteFolderHierarchyViewModel.ConductWith(this);

            this.CreateFolder = UwCoreCommand.Create(this.CreateFolderImpl)
                .ShowLoadingOverlay("Oi")
                .HandleExceptions();
        }

        private async Task CreateFolderImpl(CancellationToken arg)
        {
            await this._commandQueue.EnqueueCommandAsync(new CreateFolderCommand("Neuer Ordner", this.NoteFolderHierarchyViewModel.CurrentFolder?.Id));
            await this.NoteFolderHierarchyViewModel.Reload.ExecuteAsync();
        }
    }
}