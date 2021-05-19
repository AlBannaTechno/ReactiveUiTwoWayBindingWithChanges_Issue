using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Documents;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveUiTwoWayBindingWithChangesIssue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.DatabaseServices.Name, v => v.Username.Text)
                    .DisposeWith(d);
                
                this.ViewModel.WhenPropertyChanged(vm => vm.DatabaseServices.Name, false)
                    .Subscribe(_ =>
                    {
                        
                    }).DisposeWith(d);
            });

        }
    }

    public class MainWindowViewModel : ReactiveObject
    {
        [Reactive] public IDatabaseServices DatabaseServices { get; private set; }

        public MainWindowViewModel()
        {
            DatabaseServices = new DatabaseServices();
        }
    }

    // make this IDatabaseServices : IReactiveObject, to make the system work
    public interface IDatabaseServices
    {
        string Name { get; set; }
    }
    
    public class DatabaseServices : ReactiveObject, IDatabaseServices
    {
        [Reactive] public string Name { get; set; }

        public DatabaseServices()
        {
        }
    }
 
}