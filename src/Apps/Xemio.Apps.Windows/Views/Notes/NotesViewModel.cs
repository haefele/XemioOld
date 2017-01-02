using System.Collections.ObjectModel;
using System.Reactive;
using Caliburn.Micro;
using UwCore;
using Xemio.Apps.Windows.ViewParts.NoteFolderHierarchy;
using Xemio.Client.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.Views.Notes
{
    public class NotesViewModel : UwCoreScreen
    {
        private readonly IFoldersClient _foldersClient;
        public NoteFolderHierarchyViewModel NoteFolderHierarchyViewModel { get; }

        public UwCoreCommand<Unit> CreateFolder { get; }

        public NotesViewModel(IFoldersClient foldersClient)
        {
            this._foldersClient = foldersClient;
            this.NoteFolderHierarchyViewModel = IoC.Get<NoteFolderHierarchyViewModel>();
            this.NoteFolderHierarchyViewModel.ConductWith(this);
        }

        protected override async void OnActivate()
        {
            base.OnActivate();

            var res = await this._foldersClient.CreateFolderAsync(new CreateFolder
            {
                Name = "Test-Folder",
                ParentFolderId = null
            });
        }
    }
}