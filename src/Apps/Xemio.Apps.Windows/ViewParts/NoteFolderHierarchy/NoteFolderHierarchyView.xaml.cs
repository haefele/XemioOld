using Windows.UI.Xaml.Controls;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.ViewParts.NoteFolderHierarchy
{
    public sealed partial class NoteFolderHierarchyView : UserControl
    {
        public NoteFolderHierarchyViewModel ViewModel => this.DataContext as NoteFolderHierarchyViewModel;

        public NoteFolderHierarchyView()
        {
            this.InitializeComponent();
        }

        private async void ItemsListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.ViewModel.SelectedItem = (ItemViewModel) e.ClickedItem;
            await this.ViewModel.ShowSelectedItem.ExecuteAsync();
        }
        
        private async void ParentFoldersListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.ViewModel.SelectedParentFolder = (FolderDTO) e.ClickedItem;
            await this.ViewModel.ShowSelectedParentFolder.ExecuteAsync();
        }
    }
}
