using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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

        private readonly ObservableCollection<FolderDTO> _parentFolders;
        private FolderDTO _selectedParentFolder;
        private readonly ObservableAsPropertyHelper<ObservableCollection<ItemViewModel>> _itemsHelper;
        private ItemViewModel _selectedItem;

        public ReadOnlyObservableCollection<FolderDTO> ParentFolders { get; }
        public FolderDTO SelectedParentFolder
        {
            get { return this._selectedParentFolder; }
            set { this.RaiseAndSetIfChanged(ref this._selectedParentFolder, value); }
        }
        public ObservableCollection<ItemViewModel> Items => this._itemsHelper.Value;
        public ItemViewModel SelectedItem
        {
            get { return this._selectedItem; }
            set { this.RaiseAndSetIfChanged(ref this._selectedItem, value); }
        }

        public UwCoreCommand<ObservableCollection<ItemViewModel>> Reload { get; }
        public UwCoreCommand<Unit> ShowSelectedItem { get; }
        public UwCoreCommand<Unit> ShowSelectedParentFolder { get; }

        public NoteFolderHierarchyViewModel(IQueryExecutor queryExecutor)
        {
            Guard.NotNull(queryExecutor, nameof(queryExecutor));

            this._queryExecutor = queryExecutor;
            
            this.ParentFolders = new ReadOnlyObservableCollection<FolderDTO>(this._parentFolders = new ObservableCollection<FolderDTO>());

            this.Reload = UwCoreCommand
                .Create(this.ReloadImpl)
                .ShowLoadingOverlay("Loading")
                .HandleExceptions();
            this.Reload.ToProperty(this, f => f.Items, out this._itemsHelper);

            this.ShowSelectedItem = UwCoreCommand
                .Create(this.ShowSelectedItemImpl)
                .ShowLoadingOverlay("Oi")
                .HandleExceptions();

            this.ShowSelectedParentFolder = UwCoreCommand
                .Create(this.ShowSelectedParentFolderImpl)
                .ShowLoadingOverlay("Hoi")
                .HandleExceptions();
        }

        protected override async void OnActivate()
        {
            base.OnActivate();

            await this.Reload.ExecuteAsync();
        }

        private async Task<ObservableCollection<ItemViewModel>> ReloadImpl()
        {
            var currentFolder = this.ParentFolders.LastOrDefault();

            var folders = currentFolder != null 
                ? await this._queryExecutor.ExecuteAsync(new GetSubFoldersQuery(currentFolder.Id)) 
                : await this._queryExecutor.ExecuteAsync(new GetRootFoldersQuery());

            return new ObservableCollection<ItemViewModel>(folders.Result.Select(f => new ItemViewModel(f)));
        }

        private async Task ShowSelectedItemImpl()
        {
            if (this.SelectedItem.IsFolder)
            {
                this._parentFolders.Add(this.SelectedItem.Folder);
                await this.Reload.ExecuteAsync();
            }
            else
            {

            }
        }

        private async Task ShowSelectedParentFolderImpl()
        {
            foreach (var folderAfterSelectedFolder in new List<FolderDTO>(this.ParentFolders.SkipWhile(f => f != this.SelectedParentFolder).Skip(1)))
            {
                this._parentFolders.Remove(folderAfterSelectedFolder);
            }

            this.SelectedParentFolder = null;
            await this.Reload.ExecuteAsync();
        }
    }

    public class ItemViewModel : UwCorePropertyChangedBase
    {
        public ItemViewModel(FolderDTO folder)
        {
            this.Folder = folder;
            this.Name = folder.Name;
        }

        public bool IsFolder => this.Folder != null;
        public FolderDTO Folder { get; }

        public bool IsNote => this.Note != null;
        public NoteDTO Note { get; }

        public string Name { get; }
    }
}