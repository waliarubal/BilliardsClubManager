using BilliardsClubManager.Base;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.DataStructures;
using System;
using System.Collections.Generic;

namespace BilliardsClubManager.ViewModels
{
    class ReportViewModel: ViewModelBase
    {
        readonly ReportBase _report;

        #region constructor/destructor

        public ReportViewModel()
        {

        }

        public ReportViewModel(ReportBase report)
        {
            if (report == null)
                throw new ArgumentNullException("report");

            _report = report;
        }

        #endregion

        #region properties

        public IEnumerable<Doublet<string, object>> Parameters
        {
            get { return _report.Parameters; }
        }

        #endregion
    }
}
