using ShababTrade.Data.Models;
using ShababTrade.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShababTrade.ViewModels.Base
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public static AppUser AppUser { get; set; }

        #region Exchange Users

        private List<UserLoginInfo> _exchangeUsers = new List<UserLoginInfo>();

        public List<UserLoginInfo> ExchangeUsers
        {
            get => _exchangeUsers;
            set => Set(ref _exchangeUsers, value);
        }

        #endregion

        #region Available Exchanges

        private ObservableCollection<string> _availableExchanges = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableExchanges
        {
            get => _availableExchanges;
            set => Set(ref _availableExchanges, value);
        }

        #endregion

        #region Selected Exchange

        private string _selectedExchange = "Binance";
        public string SelectedExchange
        {
            get => _selectedExchange;
            set => Set(ref _selectedExchange, value);
        }

        #endregion

        #region Is Exchange Selection Enabled

        private bool _isExchangeSelectionEnabled = true;
        public bool IsExchangeSelectionEnabled
        {
            get => _isExchangeSelectionEnabled;
            set => Set(ref _isExchangeSelectionEnabled, value);
        }

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
