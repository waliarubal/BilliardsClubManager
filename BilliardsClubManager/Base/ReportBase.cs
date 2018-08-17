using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.DataStructures;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace BilliardsClubManager.Base
{
    abstract class ReportBase: NotificationBase
    {
        readonly List<Doublet<string , object>> _parameters;
        string _name;

        protected ReportBase(string name)
        {
            _parameters = new List<Doublet<string, object>>();
            Name = name;
        }

        #region properties

        public string Name
        {
            get { return _name; }
            private set { Set(nameof(Name), ref _name, value); }
        }

        public IEnumerable<Doublet<string, object>> Parameters
        {
            get { return _parameters; }
        }

        #endregion

        protected T Get<T>(string name)
        {
            foreach(var parameter in _parameters)
                if (parameter.First.Equals(name))
                    return (T)parameter.Second;

            return default(T);
        }

        protected void Set<T>(string name, T value)
        {
            foreach (var parameter in _parameters)
            {
                if (parameter.First.Equals(name))
                {
                    parameter.Second = value;
                    return;
                }
            }

            _parameters.Add(new Doublet<string, object>(name, value));
        }

        protected void AddParameter(string name, object value)
        {
            _parameters.Add(new Doublet<string, object>(name, value));
        }

        protected bool RemoveParameter(string name)
        {
            for(var index = 0; index < _parameters.Count; index++)
            {
                if (_parameters[index].First.Equals(name))
                {
                    _parameters.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }

        public abstract DataTable Generate();

        public void ExportToExcel(DataTable data, string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            using (var package = new ExcelPackage())
            using(var worksheet = package.Workbook.Worksheets.Add(Name))
            {
                worksheet.Cells.LoadFromDataTable(data, true);
                package.SaveAs(fileInfo);
            }
        }
    }
}
