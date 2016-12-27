using Windows.UI.Xaml.Controls;

namespace Xemio.Apps.Windows.Views.Notes
{
    public sealed partial class NotesView : Page
    { 
        public NotesViewModel ViewModel => this.DataContext as NotesViewModel;

        public NotesView()
        {
            this.InitializeComponent();
        }
    }
}
