using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;
using System.Collections.ObjectModel;

namespace BilliardsClubManager.ViewModels
{
    class DashboardViewModel: ViewModelBase
    {
        ObservableCollection<GameModel> _games;

        #region properties

        public ObservableCollection<GameModel> Games
        {
            get
            {
                if (_games == null)
                    _games = new ObservableCollection<GameModel>();

                return _games;
            }
        }

        #endregion
    }
}
