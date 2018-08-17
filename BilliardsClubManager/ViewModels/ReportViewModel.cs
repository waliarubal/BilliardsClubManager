using BilliardsClubManager.Base;
using BilliardsClubManager.Models.Reports;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using NullVoidCreations.WpfHelpers.DataStructures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class ReportViewModel: ViewModelBase
    {
        readonly ReportBase _report;
        DataTable _result;
        ICommand _generate, _export;

        #region constructor/destructor

        public ReportViewModel(ReportBase report)
        {
            if (report == null)
                throw new ArgumentNullException("report");

            _report = report;
        }

        public ReportViewModel()
        {
            _report = new SampleReportModel();
        }

        #endregion

        #region properties

        public IEnumerable<Doublet<string, object>> Parameters
        {
            get { return _report.Parameters; }
        }

        public DataTable Result
        {
            get { return _result; }
            private set { Set(nameof(Result), ref _result, value); }
        }

        #endregion

        #region commands

        public ICommand GenerateCommand
        {
            get
            {
                if (_generate == null)
                    _generate = new RelayCommand(Generate);

                return _generate;
            }
        }

        public ICommand ExportCommand
        {
            get
            {
                if (_export == null)
                    _export = new RelayCommand(Export);

                return _export;
            }
        }

        #endregion

        void Generate()
        {
            Result = _report.Generate();
        }

        void Export()
        {

        }
    }
}
