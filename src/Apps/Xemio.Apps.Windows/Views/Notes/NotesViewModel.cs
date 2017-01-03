using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using UwCore;
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
        private readonly IFoldersClient _foldersClient;

        //public NoteFolderHierarchyViewModel NoteFolderHierarchyViewModel { get; }

        public UwCoreCommand<Unit> CreateFolder { get; }

        public NotesViewModel(IQueryExecutor queryExecutor, IFoldersClient foldersClient)
        {
            this._queryExecutor = queryExecutor;
            this._foldersClient = foldersClient;

            //this.NoteFolderHierarchyViewModel = IoC.Get<NoteFolderHierarchyViewModel>();
            //this.NoteFolderHierarchyViewModel.ConductWith(this);

            this.CreateFolder = UwCoreCommand.Create(this.CreateFolderImpl)
                .ShowLoadingOverlay("Oi")
                .HandleExceptions();
        }

        private async Task CreateFolderImpl(CancellationToken arg)
        {
            var folders = await this._queryExecutor.ExecuteAsync(new GetRootFoldersQuery());
        }

        protected override async void OnActivate()
        {
            base.OnActivate();
        }
    }
}