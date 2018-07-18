using System.Collections.Generic;

namespace BilliardsClubManager.Models
{
    interface IRecord
    {
        void Clear();

        void Save();

        void Delete();

        IRecord Get(long id);

        IEnumerable<IRecord> Get(string searchKeywoard);
    }
}
