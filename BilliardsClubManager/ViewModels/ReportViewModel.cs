using BilliardsClubManager.Base;
using BilliardsClubManager.Models.Reports;
using Microsoft.Win32;
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

        public IEnumerable<Triplet<string, object, bool>> Parameters
        {
            get
            {
                var parameters = new List<Triplet<string, object, bool>>();
                foreach (var param in _report.Parameters)
                    parameters.Add(new Triplet<string, object, bool>(param.First, param.Second, param.Second is DateTime));
                return parameters;
            }
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
            if (Result == null)
                return;

            var file = new SaveFileDialog();
            file.Title = "Export as Spreadsheet";
            file.DefaultExt = "xlsx";
            file.Filter = "Excel Spreadsheet|*.xlsx";
            if (file.ShowDialog() == true)
                _report.ExportToExcel(Result, file.FileName);
        }
    }
}
