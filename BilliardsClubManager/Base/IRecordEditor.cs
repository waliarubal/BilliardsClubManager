using System.Windows.Controls;

namespace BilliardsClubManager.Base
{
    interface IRecordEditor
    {
        IRecord Record { get; set; }

        Control GetView();
    }
}
