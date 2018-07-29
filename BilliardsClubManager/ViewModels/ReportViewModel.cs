using NullVoidCreations.WpfHelpers.Base;
using System.Data;

namespace BilliardsClubManager.ViewModels
{
    class ReportViewModel: ViewModelBase
    {
        DataTable _result;

        #region properties

        public DataTable Result
        {
            get => _result;
            private set => Set(nameof(Result), ref _result, value);
        }

        #endregion
    }
}
