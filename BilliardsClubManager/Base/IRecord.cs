using System.Collections.Generic;

namespace BilliardsClubManager.Base
{
    interface IRecord
    {
        IRecord New();

        string Save();

        string Delete();

        IRecord Get(long id);

        IEnumerable<IRecord> Get(string searchKeywoard);
    }
}
