using System;
using System.Data;
using System.Text;
using BilliardsClubManager.Base;
using Dapper;
using NullVoidCreations.WpfHelpers;

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
            var result = new DataTable();
            result.Columns.Add("Particulars", typeof(string));
            result.Columns.Add("Amount", typeof(decimal));

            using (var connection = Shared.Instance.GetConnection())
            {
                var sqlbuilder = new StringBuilder();
                sqlbuilder.AppendLine("SELECT");
                sqlbuilder.AppendLine("  SUM(Amount)");
                sqlbuilder.AppendLine("FROM [CreditNotes]");
                sqlbuilder.AppendLine("WHERE");
                sqlbuilder.AppendLine("  [Date] >= '{0:yyyy-MM-dd}' AND", Get<DateTime>("From Date"));
                sqlbuilder.AppendLine("  [Date] <= '{0:yyyy-MM-dd}';", Get<DateTime>("To Date"));

                
                var income = connection.ExecuteScalar<decimal>(sqlbuilder.ToString());
                result.Rows.Add("Total Revenue", income);

                sqlbuilder.Clear();
                sqlbuilder.AppendLine("SELECT");
                sqlbuilder.AppendLine("  SUM(ChargeTotal)");
                sqlbuilder.AppendLine("FROM [Games]");
                sqlbuilder.AppendLine("WHERE");
                sqlbuilder.AppendLine("  [Start] >= '{0:yyyy-MM-dd}' AND", Get<DateTime>("From Date"));
                sqlbuilder.AppendLine("  [End] <= '{0:yyyy-MM-dd}';", Get<DateTime>("To Date"));

                var expense = connection.ExecuteScalar<decimal>(sqlbuilder.ToString());
                result.Rows.Add("Cost of Gods Sold", expense);

                result.Rows.Add("Gross Profit", income - expense);
            }

            return result;
        }
    }
}
