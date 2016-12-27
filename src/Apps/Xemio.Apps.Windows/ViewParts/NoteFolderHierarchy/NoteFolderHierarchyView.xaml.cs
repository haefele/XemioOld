using Windows.UI.Xaml.Controls;

namespace Xemio.Apps.Windows.ViewParts.NoteFolderHierarchy
{
    public sealed partial class NoteFolderHierarchyView : UserControl
    {
        public NoteFolderHierarchyViewModel ViewModel => this.DataContext as NoteFolderHierarchyViewModel;

        public NoteFolderHierarchyView()
        {
            this.InitializeComponent();
        }
    }
}
