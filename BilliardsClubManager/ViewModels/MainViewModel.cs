using BilliardsClubManager.Base;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.Windows.Controls;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class MainViewModel: ViewModelBase
    {
        ICommand _createEditorView, _createChildView, _initialize;

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

        public bool IsLicensed
        {
            get { return Shared.Instance.IsLicensed; }
        }


        #endregion

        void Initialize()
        {
            Shared.Instance.LoadSettings();
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
