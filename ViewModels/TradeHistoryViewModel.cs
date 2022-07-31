using BinanceApiLibrary;
using BitrueApiLibrary;
using ShababTrade.Commands;
using ShababTrade.Data.Models;
using ShababTrade.Models;
using ShababTrade.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradingCommonTypes;

namespace ShababTrade.ViewModels
{
    internal class TradeHistoryViewModel : BaseViewModel
    {
        #region Properties

        #region Exchange Users

        private List<ExchangeUser> _exchangeUsers = new List<ExchangeUser>();

        public List<ExchangeUser> ExchangeUsers
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

        private string _selectedExchange;
        public string SelectedExchange
        {
            get => _selectedExchange;
            set => Set(ref _selectedExchange, value);
        }

        #endregion



        #region Свойство видимости Списка BaseAssets

        private Visibility _baseAssetsVisibility = Visibility.Collapsed;
        public Visibility BaseAssetsVisibility
        {
            get => _baseAssetsVisibility;
            set { Set(ref _baseAssetsVisibility, value); }
        }

        #endregion

        #region Свойство видимости Списка QuoteAssets

        private Visibility _quoteAssetsVisibility = Visibility.Collapsed;
        public Visibility QuoteAssetsVisibility
        {
            get => _quoteAssetsVisibility;
            set { Set(ref _quoteAssetsVisibility, value); }
        }

        #endregion

        #region Свойство видимости секции торговых операций

        private Visibility _tradeHistorySectionVisibility = Visibility.Collapsed;
        public Visibility TradeHistorySectionVisibility
        {
            get => _tradeHistorySectionVisibility;
            set { Set(ref _tradeHistorySectionVisibility, value); }
        }

        #endregion        

        #region Свойство видимости Списка TradingSides
        private Visibility _tradingSidesVisibility = Visibility.Collapsed;
        public Visibility TradingSidesVisibility
        {
            get => _tradingSidesVisibility;
            set { Set(ref _tradingSidesVisibility, value); }
        }
        #endregion

        #region Свойство видимости Pagination кнопок
        private Visibility _tradуHistoryPaginationVisibility = Visibility.Collapsed;
        public Visibility TradуHistoryPaginationVisibility
        {
            get => _tradуHistoryPaginationVisibility;
            set { Set(ref _tradуHistoryPaginationVisibility, value); }
        }
        #endregion



        #region Свойство StartDate

        private DateTime _startDate;

        public DateTime StartDate
        {
            get => _startDate;
            set { Set(ref _startDate, value); }
        }

        #endregion

        #region Свойство EndDate

        private DateTime _endDate;

        public DateTime EndDate
        {
            get => _endDate;
            set { Set(ref _endDate, value); }
        }

        #endregion  

        #region Свойство Поиска символа (Текстовое)

        private string _txtSearchSymbol = "";

        public string TxtSearchSymbol
        {
            get => _txtSearchSymbol;
            set
            {
                Set(ref _txtSearchSymbol, value);
            }
        }

        #endregion

        #region Свойство Items Source для BaseAsset

        private List<string> _baseAssets;

        public List<string> BaseAssets
        {
            get => _baseAssets;
            set { Set(ref _baseAssets, value); }
        }

        #endregion

        #region Свойство Items Source для QuoteAsset

        private List<string> _quoteAssets;

        public List<string> QuoteAssets
        {
            get => _quoteAssets;
            set { Set(ref _quoteAssets, value); }
        }

        #endregion

        #region Свойство TradingSide

        private string _side = "ALL";

        public string Side
        {
            get => _side;
            set { Set(ref _side, value); }
        }

        #endregion

        #region Свойство BaseAsset

        private string _baseAsset;

        public string BaseAsset
        {
            get => _baseAsset;
            set { Set(ref _baseAsset, value); }
        }

        #endregion

        #region Свойство QuoteAsset

        private string _quoteAsset;

        public string QuoteAsset
        {
            get => _quoteAsset;
            set { Set(ref _quoteAsset, value); }
        }

        #endregion

        #region Свойство BaseAssetSelected

        private string _baseAssetSelected;

        public string BaseAssetSelected
        {
            get => _baseAssetSelected;
            set { Set(ref _baseAssetSelected, value); }
        }

        #endregion

        #region Свойство QuoteAssetSelected

        private string _quoteAssetSelected;

        public string QuoteAssetSelected
        {
            get => _quoteAssetSelected;
            set { Set(ref _quoteAssetSelected, value); }
        }

        #endregion

        #region Свойство SideSelected

        private string _sideSelected = "ALL";

        public string SideSelected
        {
            get => _sideSelected;
            set { Set(ref _sideSelected, value); }
        }

        #endregion        

        #region Свойство базовых активов (Текстовое, общее)

        private List<string> _baseAssetsShared;

        public List<string> BaseAssetsShared
        {
            get => _baseAssetsShared;
            set
            {
                Set(ref _baseAssetsShared, value);
            }
        }

        #endregion

        #region Свойство ItemsSource исполненных ордеров

        private List<AppFilledTrade> _filledTrades = new List<AppFilledTrade>();

        public List<AppFilledTrade> FilledTrades
        {
            get => _filledTrades;
            set { Set(ref _filledTrades, value); }
        }

        #endregion

        #region Свойство ItemsSource исполненных ордеров
        private List<IFilledTrade> _filledTradesShared = new List<IFilledTrade>();

        public List<IFilledTrade> FilledTradesShared
        {
            get => _filledTradesShared;
            set { Set(ref _filledTradesShared, value); }
        }
        #endregion

        #region Свойство Текущая страница

        private int _currentPage = 1;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                Set(ref _currentPage, value);
            }
        }

        #endregion

        #region Свойство Количество страниц

        private int _numberOfPages;

        public int NumberOfPages
        {
            get => _numberOfPages;
            set
            {
                Set(ref _numberOfPages, value);
            }
        }

        #endregion

        #endregion

        #region Commands

        #region Команда "Открыть список базовых активов"
        public ICommand OpenBaseAssetsList_Command { get; }

        private bool CanOpenBaseAssetsListCommandExecute(object o) => true;
        private void OnOpenBaseAssetsListCommandEcexuted(object o)
        {
            QuoteAssetsVisibility = Visibility.Collapsed;
            TradingSidesVisibility = Visibility.Collapsed;
            if (BaseAssetsVisibility == Visibility.Visible)
                BaseAssetsVisibility = Visibility.Collapsed;
            else
                BaseAssetsVisibility = Visibility.Visible;
        }
        #endregion

        #region Команда "Открыть список запрашиваемых активов"
        public ICommand OpenQuoteAssetsList_Command { get; }

        private bool CanOpenQuoteAssetsListCommandExecute(object o) => true;
        private void OnOpenQuoteAssetsListCommandEcexuted(object o)
        {
            BaseAssetsVisibility = Visibility.Collapsed;
            TradingSidesVisibility = Visibility.Collapsed;
            if (QuoteAssetsVisibility == Visibility.Visible)
                QuoteAssetsVisibility = Visibility.Collapsed;
            else
                QuoteAssetsVisibility = Visibility.Visible;
        }
        #endregion

        #region Команда "Открыть список сторон Buy/Sell"
        public ICommand OpenTradingSidesList_Command { get; }

        private bool CanOpenTradingSidesListCommandExecute(object o) => true;
        private void OnOpenTradingSidesListCommandEcexuted(object o)
        {
            QuoteAssetsVisibility = Visibility.Collapsed;
            BaseAssetsVisibility = Visibility.Collapsed;
            if (TradingSidesVisibility == Visibility.Visible)
                TradingSidesVisibility = Visibility.Collapsed;
            else
                TradingSidesVisibility = Visibility.Visible;
        }
        #endregion

        #region Команда "Поиск базового актива"
        public ICommand SearchAsset_Command { get; }

        private bool CanSearchAssetCommandExecute(object o) => true;
        private void OnSearchAssetCommandEcexuted(object o)
        {
            BaseAssets = BaseAssetsShared.Distinct().Where(asset => asset.StartsWith(TxtSearchSymbol.ToUpper())).ToList();
            BaseAssetsVisibility = Visibility.Visible;
        }
        #endregion

        #region Команда "Базовый актив выбран"
        public ICommand BaseAssetSelected_Command { get; }

        private bool CanBaseAssetSelectedCommandExecute(object o) => true;
        private void OnBaseAssetSelectedCommandEcexuted(object o)
        {
            if (BaseAssetSelected != null)
            {
                BaseAsset = BaseAssetSelected;
            }
            BaseAssetsVisibility = Visibility.Collapsed;
        }
        #endregion

        #region Команда "Запрашиваемый актив выбран"
        public ICommand QuoteAssetSelected_Command { get; }

        private bool CanQuoteAssetSelectedCommandExecute(object o) => true;
        private void OnQuoteAssetSelectedCommandEcexuted(object o)
        {
            QuoteAssetsVisibility = Visibility.Collapsed;
            QuoteAsset = QuoteAssetSelected;
        }
        #endregion

        #region Команда "Сторона Buy/Sell выбрана"
        public ICommand SideSelected_Command { get; }

        private bool CanSideSelectedSelectedCommandExecute(object o) => true;
        private void OnSideSelectedSelectedCommandEcexuted(object o)
        {
            TradingSidesVisibility = Visibility.Collapsed;
            if (SideSelected.Contains("ALL"))
            {
                Side = "ALL";
            }
            else
            {
                Side = SideSelected.Contains("BUY") ? "BUY" : "SELL";
            }
        }
        #endregion

        #region Команда "Конечная дата выбрана"
        public ICommand EndDateSelected_Command { get; }

        private bool CanEndDateSelectedCommandExecute(object o) => true;
        private void OnEndDateSelectedCommandEcexuted(object o)
        {
            EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
        }
        #endregion

        #region Команда "Поиск торговых операций"
        public ICommand SearchFilledTrades_Command { get; }

        private bool CanSearchFilledTradesCommandExecute(object o) => true;
        private void OnSearchFilledTradesCommandEcexuted(object o)
        {
            try
            {
                FilledTrades = new List<AppFilledTrade>();
                FilledTradesShared = new List<IFilledTrade>();
                IAccountInfo accountInfo = GetAccountInfoObject();
                FilledTradesShared = accountInfo.GetTrades(GetExchangeUserObject(), BaseAsset + QuoteAsset);
                if (Side != "ALL")
                {
                    FilledTradesShared = FilledTradesShared.Where(trade => trade.Side.ToString() == Side).ToList();
                }

                FilledTradesShared = FilledTradesShared.Where(trade => trade.TimeStamp >= StartDate && trade.TimeStamp <= EndDate).ToList();

                foreach (var trade in FilledTradesShared)
                {
                    FilledTrades.Add(new AppFilledTrade(trade));
                }
                NumberOfPages = (int)Math.Ceiling(FilledTrades.Count / 20M);
                CurrentPage = 1;
                var filledTrades = FilledTradesShared.Take(20).ToList();
                FilledTrades = AppFilledTrade.ConvertToAppFilledTrades(filledTrades);
            }
            catch (Exception)
            {
                MessageBox.Show("No Data.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (FilledTrades != null && FilledTrades.Count > 0)
            {
                TradуHistoryPaginationVisibility = TradeHistorySectionVisibility = Visibility.Visible;
            }
            else
            {
                TradуHistoryPaginationVisibility = TradeHistorySectionVisibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Команда "Открыть первую страницу"
        public ICommand OpenFirstPage_Command { get; }

        private bool CanOpenFirstPageCommandExecute(object o) => true;
        private void OnOpenFirstPageCommandEcexuted(object o)
        {
            var firstTrades = FilledTradesShared.Take(20).ToList();
            FilledTrades = AppFilledTrade.ConvertToAppFilledTrades(firstTrades);
            CurrentPage = 1;
        }
        #endregion

        #region Команда "Открыть предыдущую страницу"
        public ICommand OpenPreviousPage_Command { get; }

        private bool CanOpenPreviousPageCommandExecute(object o) => true;
        private void OnOpenPreviousPageCommandEcexuted(object o)
        {
            if (CurrentPage > 1)
            {
                var prevTrades = FilledTradesShared.Skip((CurrentPage - 2) * 20).Take(20).ToList();
                FilledTrades = AppFilledTrade.ConvertToAppFilledTrades(prevTrades);
                CurrentPage--;
            }
        }
        #endregion

        #region Команда "Открыть следущую страницу"
        public ICommand OpenNextPage_Command { get; }

        private bool CanOpenNextPageCommandExecute(object o) => true;
        private void OnOpenNextPageCommandEcexuted(object o)
        {
            if (CurrentPage < NumberOfPages)
            {
                var nextTrades = FilledTradesShared.Skip(CurrentPage * 20).Take(20).ToList();
                FilledTrades = AppFilledTrade.ConvertToAppFilledTrades(nextTrades);
                CurrentPage++;
            }
        }
        #endregion

        #region Команда "Открыть последнюю страницу"
        public ICommand OpenLastPage_Command { get; }

        private bool CanOpenLastPageCommandExecute(object o) => true;
        private void OnOpenLastPageCommandEcexuted(object o)
        {
            if (CurrentPage != NumberOfPages)
            {
                var lastTrades = FilledTradesShared.Skip((NumberOfPages - 1) * 20).ToList();
                FilledTrades = AppFilledTrade.ConvertToAppFilledTrades(lastTrades);
                CurrentPage = NumberOfPages;
            }
        }
        #endregion

        #region Selected Exchange Changed Command

        public ICommand SelectedExchangeChangedCommand { get; }

        public bool CanSelectedExchangeChangedCommandExecute(object p) => true;

        public void OnSelectedExchangeChangedCommandExecuted(object p)
        {
            switch (SelectedExchange)
            {
                case "Binance":
                    GetSymbolsForBinanceUser();
                    break;

                case "Bitrue":
                    GetSymbolsForBitrueUser();
                    break;
            }
        }

        #endregion


        #region Команда "Закрыть списки секции Trade History"
        private void TradeHistory_CloseComboBoxes()
        {
            BaseAssetsVisibility = Visibility.Collapsed;
            QuoteAssetsVisibility = Visibility.Collapsed;
            TradingSidesVisibility = Visibility.Collapsed;
        }
        #endregion




        #endregion

        #region Methods

        private IAccountInfo GetAccountInfoObject()
        {
            switch (SelectedExchange)
            {
                case "Binance":
                    return new BinanceAccountInfo();
                    break;
                case "Bitrue":
                    return new BitrueAccountInfo();
                    break;
                default:
                    return new BinanceAccountInfo();
            }
        }

        private IExchangeUser GetExchangeUserObject()
        {
            ExchangeUser user;

            switch (SelectedExchange)
            {
                case "Binance":
                    user = ExchangeUsers.Where(user => user.Exchange == "Binance").First();
                    return new BinanceApiUser(user.PublicKey, user.PrivateKey);
                    break;
                case "Bitrue":
                    user = ExchangeUsers.Where(user => user.Exchange == "Bitrue").First();
                    return new BitrueApiUser(user.PublicKey, user.PrivateKey);
                    break;
                default:
                    user = ExchangeUsers.Where(user => user.Exchange == "Binance").First();
                    return new BinanceApiUser(user.PublicKey, user.PrivateKey);
            }
        }

        #region Get Symbols For Binance User

        private void GetSymbolsForBinanceUser()
        {
            var binanceExchangeInfo = new BinanceExchangeInfo();
            binanceExchangeInfo = (BinanceExchangeInfo)binanceExchangeInfo.GetExchangeInfo();

            BaseAssets = new List<string>();
            QuoteAssets = new List<string>();
            foreach (var asset in binanceExchangeInfo.ExchangeSymbols)
            {
                BaseAssets.Add(asset.BaseAsset);
                QuoteAssets.Add(asset.QuoteAsset);
            }

            BaseAssets = BaseAssetsShared = BaseAssets.Distinct().ToList();
            QuoteAssets = QuoteAssets.Distinct().ToList();       
        }

        #endregion

        #region Get Account Data For Bitrue User

        private void GetSymbolsForBitrueUser()
        {
            var bitrueExchangeInfo = new BitrueExchangeInfo();
            bitrueExchangeInfo = (BitrueExchangeInfo)bitrueExchangeInfo.GetExchangeInfo();

            BaseAssets = new List<string>();
            QuoteAssets = new List<string>();
            foreach (var asset in bitrueExchangeInfo.ExchangeSymbols)
            {
                BaseAssets.Add(asset.BaseAsset);
                QuoteAssets.Add(asset.QuoteAsset);
            }

            BaseAssets = BaseAssetsShared = BaseAssets.Distinct().ToList();
            QuoteAssets = QuoteAssets.Distinct().ToList();
        }

        #endregion

        #endregion

        public TradeHistoryViewModel()
        {

        }

        public TradeHistoryViewModel(List<ExchangeUser> appUsers, string selectedExchange)
        {
            SelectedExchange = selectedExchange;
            ExchangeUsers = appUsers;

            OpenBaseAssetsList_Command = new RelayCommand(OnOpenBaseAssetsListCommandEcexuted, CanOpenBaseAssetsListCommandExecute);
            OpenQuoteAssetsList_Command = new RelayCommand(OnOpenQuoteAssetsListCommandEcexuted, CanOpenQuoteAssetsListCommandExecute);
            OpenTradingSidesList_Command = new RelayCommand(OnOpenTradingSidesListCommandEcexuted, CanOpenTradingSidesListCommandExecute);

            SelectedExchangeChangedCommand = new RelayCommand(OnSelectedExchangeChangedCommandExecuted, CanSelectedExchangeChangedCommandExecute);
            SearchAsset_Command = new RelayCommand(OnSearchAssetCommandEcexuted, CanSearchAssetCommandExecute);
            BaseAssetSelected_Command = new RelayCommand(OnBaseAssetSelectedCommandEcexuted, CanBaseAssetSelectedCommandExecute);
            QuoteAssetSelected_Command = new RelayCommand(OnQuoteAssetSelectedCommandEcexuted, CanQuoteAssetSelectedCommandExecute);
            SideSelected_Command = new RelayCommand(OnSideSelectedSelectedCommandEcexuted, CanSideSelectedSelectedCommandExecute);
            SearchFilledTrades_Command = new RelayCommand(OnSearchFilledTradesCommandEcexuted, CanSearchFilledTradesCommandExecute);
            SearchFilledTrades_Command = new RelayCommand(OnSearchFilledTradesCommandEcexuted, CanSearchFilledTradesCommandExecute);
            EndDateSelected_Command = new RelayCommand(OnEndDateSelectedCommandEcexuted, CanEndDateSelectedCommandExecute);
            OpenFirstPage_Command = new RelayCommand(OnOpenFirstPageCommandEcexuted, CanOpenFirstPageCommandExecute);
            OpenPreviousPage_Command = new RelayCommand(OnOpenPreviousPageCommandEcexuted, CanOpenPreviousPageCommandExecute);
            OpenNextPage_Command = new RelayCommand(OnOpenNextPageCommandEcexuted, CanOpenNextPageCommandExecute);
            OpenLastPage_Command = new RelayCommand(OnOpenLastPageCommandEcexuted, CanOpenLastPageCommandExecute);


            foreach (var user in ExchangeUsers)
            {
                AvailableExchanges.Add(user.Exchange);
            }

            IExchangeInfo exchangeInfo = new BinanceExchangeInfo();
            exchangeInfo = exchangeInfo.GetExchangeInfo();

            BaseAssets = new List<string>();
            QuoteAssets = new List<string>();
            foreach (var asset in exchangeInfo.ExchangeSymbols)
            {
                BaseAssets.Add(asset.BaseAsset);
                QuoteAssets.Add(asset.QuoteAsset);
            }

            BaseAssets = BaseAssetsShared = BaseAssets.Distinct().ToList();
            QuoteAssets = QuoteAssets.Distinct().ToList();

            StartDate = DateTime.Today;
            EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        }
    }
}
