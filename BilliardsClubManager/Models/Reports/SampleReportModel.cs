using BilliardsClubManager.Base;
using System;
using System.Data;

namespace BilliardsClubManager.Models.Reports
{
    class SampleReportModel : ReportBase
    {
        public SampleReportModel(): base("Sample Report") { }

        public override DataTable Generate()
        {
            throw new NotImplementedException();
        }
    }
}
