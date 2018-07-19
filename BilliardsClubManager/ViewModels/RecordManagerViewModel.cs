using BilliardsClubManager.Base;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class RecordManagerViewModel: ViewModelBase
    {
        ICommand _createChildView, _save, _delete, _new, _refresh;

        Control _childView;
        string _errorMessage, _searchKeywoard;
        bool _isHavingError;
        IEnumerable<IRecord> _records;

        #region comstructor

        public RecordManagerViewModel(IRecordEditor editor)
        {
            Editor = editor;
        }

        public RecordManagerViewModel()
        {
            
        }

        #endregion

        #region properties

        public string SearchKeywoard
        {
            get => _searchKeywoard;
            set => Set(nameof(SearchKeywoard), ref _searchKeywoard, value);
        }

        public IRecordEditor Editor { get; }

        public Control ChildView
        {
            get => _childView;
            private set => Set(nameof(ChildView), ref _childView, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                Set(nameof(ErrorMessage), ref _errorMessage, value);
                IsHavingError = !string.IsNullOrEmpty(ErrorMessage);
            }
        }

        public bool IsHavingError
        {
            get => _isHavingError;
            private set => Set(nameof(IsHavingError), ref _isHavingError, value);
        }

        public IEnumerable<IRecord> Records
        {
            get => _records;
            private set => Set(nameof(Records), ref _records, value);
        }

        #endregion

        #region commands

        public ICommand CreateChildViewCommand
        {
            get
            {
                if (_createChildView == null)
                    _createChildView = new RelayCommand<IRecordEditor>(CreateChildView) { IsSynchronous = true };

                return _createChildView;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_save == null)
                    _save = new RelayCommand<object, string>(Save, CommandCallback);

                return _save;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_delete == null)
                    _delete = new RelayCommand<object, string>(Delete, CommandCallback);

                return _delete;
            }
        }

        public ICommand NewCommand
        {
            get
            {
                if (_new == null)
                    _new = new RelayCommand(New);

                return _new;
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (_refresh == null)
                    _refresh = new RelayCommand<string, IEnumerable<IRecord>>(Refresh, RefreshCallbnack);

                return _refresh;
            }
        }

        #endregion

        string Save(object argument)
        {
            return Editor.Record.Save();
        }

        string Delete(object argument)
        {
            return Editor.Record.Delete();
        }

        void New()
        {
            Editor.Record = Editor.Record.New();
        }

        void CommandCallback(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        IEnumerable<IRecord> Refresh(string searchKeywoard)
        {
            return Editor.Record.Get(searchKeywoard);
        }

        void RefreshCallbnack(IEnumerable<IRecord> records)
        {
            Records = records;
        }

        void CreateChildView(IRecordEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException("editor");

             ChildView = editor.GetView();
        }
    }
}
