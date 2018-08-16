using System;
using System.Data;
using BilliardsClubManager.Base;

namespace BilliardsClubManager.Models.Reports
{
    class IncomeStatementModel : ReportBase
    {
        public IncomeStatementModel() : base("Income Statement")
        {
            Set("From Date", DateTime.Now);
            Set("To Date", DateTime.Now);
        }

        public override DataTable Generate()
        {
            return new DataTable();
        }
    }
}
