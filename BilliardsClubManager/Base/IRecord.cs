using System.Collections.Generic;

namespace BilliardsClubManager.Base
{
    interface IRecord
    {
        void Clear();

        string Save();

        string Delete();

        IRecord Get(long id);

        IEnumerable<IRecord> Get(string searchKeywoard);
    }
}
