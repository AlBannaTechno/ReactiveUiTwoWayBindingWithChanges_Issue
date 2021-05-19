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
                var regions = new List<string> {"US", "EG", "UK"};
                this.Regions.ItemsSource = regions;
                var nRegions = new List<int> { 16, 55, 86};
                
                this.Bind(ViewModel, vm => vm.DatabaseServices.DatabaseSettings.RegionCode, v => v.Regions.SelectedItem,
                    v => v < 0 ? "" : regions[v], v => regions.IndexOf((string)v)).DisposeWith(d);
                this.ViewModel.WhenPropertyChanged(vm => vm.DatabaseServices.DatabaseSettings.RegionCode, false)
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
        DatabaseSettings DatabaseSettings { get; set; }
    }
    
    public class DatabaseServices : ReactiveObject, IDatabaseServices
    {
        [Reactive] public DatabaseSettings DatabaseSettings { get; set; }

        public DatabaseServices()
        {
            DatabaseSettings = new DatabaseSettings();
        }
    }

    public class DatabaseSettings : ReactiveObject
    {
        [Reactive] public int RegionCode { get; set; }

        public DatabaseSettings()
        {
            Observable.Timer(TimeSpan.FromSeconds(2))
                .ObserveOnDispatcher()
                .Subscribe(_ =>
            {
                RegionCode = 2;
            });
        }
    }
}