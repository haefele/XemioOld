using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using UwCore;
using UwCore.Common;
using Xemio.Apps.Windows.Services.Queries;
using Xemio.Apps.Windows.Services.Queries.Folders;
using Xemio.Apps.Windows.Views.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.ViewParts.NoteFolderHierarchy
{
    public class NoteFolderHierarchyViewModel : UwCoreScreen
    {
        private readonly IQueryExecutor _queryExecutor;

        private readonly ObservableAsPropertyHelper<ObservableCollection<ItemViewModel>> _itemsHelper;

        public ObservableCollection<FolderDTO> ParentFolders { get; set; } = new ObservableCollection<FolderDTO>();
        public ObservableCollection<ItemViewModel> Items => this._itemsHelper.Value;

        public UwCoreCommand<ObservableCollection<ItemViewModel>> Refresh { get; }

        public NoteFolderHierarchyViewModel(IQueryExecutor queryExecutor)
        {
            Guard.NotNull(queryExecutor, nameof(queryExecutor));

            this._queryExecutor = queryExecutor;

            this.Refresh = UwCoreCommand
                .Create(this.RefreshImpl)
                .ShowLoadingOverlay("Loading")
                .HandleExceptions();
            this.Refresh.ToProperty(this, f => f.Items, out this._itemsHelper);
        }

        protected override async void OnActivate()
        {
            base.OnActivate();

            await this.Refresh.ExecuteAsync();
        }

        private async Task<ObservableCollection<ItemViewModel>> RefreshImpl()
        {
            var currentFolder = this.ParentFolders.LastOrDefault();

            var folders = currentFolder != null 
                ? await this._queryExecutor.ExecuteAsync(new GetSubFoldersQuery(currentFolder.Id)) 
                : await this._queryExecutor.ExecuteAsync(new GetRootFoldersQuery());

            return new ObservableCollection<ItemViewModel>(folders.Result.Select(f => new ItemViewModel(f)));
        }
    }


    public class ItemViewModel : UwCorePropertyChangedBase
    {
        public ItemViewModel(FolderDTO folder)
        {
            this.Name = folder.Name;
        }

        public string Name { get; set; }
    }
}