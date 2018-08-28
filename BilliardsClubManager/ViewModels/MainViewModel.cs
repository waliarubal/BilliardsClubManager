using BilliardsClubManager.Base;
using NullVoidCreations.Licensing;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class MainViewModel: ViewModelBase
    {
        ICommand _createEditorView, _createChildView, _initialize, _uninitialize, _showReport;

        Control _childView;

        #region properties

        public Control ChildView
        {
            get => _childView;
            private set => Set(nameof(ChildView), ref _childView, value);
        }

        public bool IsLicensed
        {
            get { return Shared.Instance.IsLicensed; }
        }

        public StrongLicense License
        {
            get { return Shared.Instance.License; }
        }

        #endregion

        #region commands

        public ICommand CreateEditorViewCommand
        {
            get
            {
                if (_createEditorView == null)
                    _createEditorView = new RelayCommand<IRecordEditor>(CreateEditorView) { IsSynchronous = true };

                return _createEditorView;
            }
        }

        public ICommand CreateChildViewCommand
        {
            get
            {
                if (_createChildView == null)
                    _createChildView = new RelayCommand<ViewModelBase>(CreateChildView) { IsSynchronous = true };

                return _createChildView;
            }
        }

        public ICommand ShowReportCommand
        {
            get
            {
                if (_showReport == null)
                    _showReport = new RelayCommand<ReportBase>(ShowReport) { IsSynchronous = true };

                return _showReport;
            }
        }

        public ICommand InitializeCommand
        {
            get
            {
                if (_initialize == null)
                    _initialize = new RelayCommand<Window, Window>(Initialize, InitializeCallback) { IsCallbackSynchronous = true };

                return _initialize;
            }
        }

        public ICommand UninitializeCommand
        {
            get
            {
                if (_uninitialize == null)
                    _uninitialize = new RelayCommand(Uninitialize);

                return _uninitialize;
            }
        }

        #endregion

        void OnSharedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        Window Initialize(Window window)
        {
            Shared.Instance.PropertyChanged += OnSharedPropertyChanged;

            Shared.Instance.LoadLicense(Shared.Instance.LicenseFile);
            if (IsLicensed)
            {
                Shared.Instance.LoadSettings();
                Shared.Instance.Switch.Open();
            }

            return window;
        }

        void InitializeCallback(Window window)
        {
            if (IsLicensed)
                CreateChildViewCommand.Execute(new DashboardViewModel());
            else
                CreateChildViewCommand.Execute(new SettingViewModel());

            if (Shared.Instance.IsMaximizedOnStart)
                window.Maximize();
        }

        void Uninitialize()
        {
            Shared.Instance.PropertyChanged -= OnSharedPropertyChanged;

            if (IsLicensed)
            {
                Shared.Instance.Switch.Toggle(false);
                Shared.Instance.Switch.Close();
            }
        }

        void CreateChildView(ViewModelBase viewModel)
        {
            ChildView = viewModel.GetView();
        }

        void CreateEditorView(IRecordEditor editor)
        {
            var viewModel = new RecordManagerViewModel(editor);
            ChildView = viewModel.GetView();
        }

        void ShowReport(ReportBase report)
        {
            var viewModel = new ReportViewModel(report);
            ChildView = viewModel.GetView();
        }
    }
}
