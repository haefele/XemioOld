using System.Collections.ObjectModel;
using System.Reactive;
using Caliburn.Micro;
using UwCore;
using Xemio.Apps.Windows.ViewParts.NoteFolderHierarchy;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.Views.Notes
{
    public class NotesViewModel : UwCoreScreen
    {
        public NoteFolderHierarchyViewModel NoteFolderHierarchyViewModel { get; }

        public UwCoreCommand<Unit> CreateFolder { get; }

        public NotesViewModel()
        {
            this.NoteFolderHierarchyViewModel = IoC.Get<NoteFolderHierarchyViewModel>();
            this.NoteFolderHierarchyViewModel.ConductWith(this);
        }
    }
}