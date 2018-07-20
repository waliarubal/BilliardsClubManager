using System.Windows.Controls;

namespace BilliardsClubManager.Base
{
    interface IRecordEditor
    {
        IRecord Record { get; set; }

        Control GetView();

        bool IsNewAllowed { get; }

        bool IsSaveAllowed { get; }

        bool IsDeleteAllowed { get; }
    }
}
