using BilliardsClubManager.Base;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class MainViewModel: ViewModelBase
    {
        ICommand _createEditorView, _createChildView, _initialize, _uninitialize;

        Control _childView;

        #region properties

        public Control ChildView
        {
            get => _childView;
            private set => Set(nameof(ChildView), ref _childView, value);
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

        public ICommand InitializeCommand
        {
            get
            {
                if (_initialize == null)
                    _initialize = new RelayCommand(Initialize);

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

        public bool IsLicensed
        {
            get { return Shared.Instance.IsLicensed; }
        }


        #endregion

        void OnSharedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        void Initialize()
        {
            Shared.Instance.PropertyChanged += OnSharedPropertyChanged;

            Shared.Instance.LoadSettings();
            Shared.Instance.LoadLicense(Shared.Instance.LicenseFile);
        }

        void Uninitialize()
        {
            Shared.Instance.PropertyChanged -= OnSharedPropertyChanged;
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
    }
}
