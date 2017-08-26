namespace ForeignExchange.ViewModels
{
    using ForeignExchange.Models;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System;
    using ForeignExchange.Services;
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Linq;
    using ForeignExchange.Helpers;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private string _amount;
        private bool _isEnabled;
        private bool _isRunning;
        private string _result;
        private string _resultReady;
        private string _resultLoadRate;
        private Rate _sorceRate;
        private Rate _targetRate;
        private ApiService apiService;
        private DialogService dialogService;

        #endregion Attributes

        #region Commands

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new RelayCommand(Clear);
            }
        }

        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }
        
        #endregion Commands

        #region Events

        public event PropertyChangedEventHandler PropertyChanged; 

        #endregion

        #region Properties

        public string Amount
        {
            get
            {
                return _amount;
            }

            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (value != _isRunning)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (value != _result)
                {
                    _result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }

        public Rate SourceRate
        {
            get
            {
                return _sorceRate;
            }
            set
            {
                if (value != _sorceRate)
                {
                    _sorceRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Rate TargetRate
        {
            get
            {
                return _targetRate;
            }
            set
            {
                if (value != _targetRate)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke(this, new  PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }

        #endregion Properties

        #region Constructor

        public MainViewModel()
        {
            //  Genera una instancia de los objetos \\
            apiService = new ApiService();
            dialogService = new DialogService();

            //  Carga variables locales \\
            _resultReady = "Ready to convert...!!!";
            _resultLoadRate = "Load rates, plase wait...!!!";            

            //  ObservableCollection    \\
            Rates = new ObservableCollection<Rate>();

            //  Invoca el metodo que hace la carga de tasas (Rates) \\
            LoadRates();
        }

        #endregion Constructor

        #region Methods
        private async void LoadRates()
        {
            try
            {
                IsRunning = true;
                IsEnabled = false;

                Result = _resultLoadRate;

                //  Invoca al metodo que genera la carga de las tasas (Rates)   \\
                var response = await apiService.GetRates();

                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    IsEnabled = false;
                    Result = null;
                    await dialogService.ShowMessage(Lenguages.Error, response.Message, Lenguages.Accept);
                    return;
                }

                //  Invoca al metodo que hace la carga de datos de las tasas (Rates)    \\
                ReloadRates((List<Rate>)response.Result);
                Result = _resultReady;
                IsRunning = false;
                IsEnabled = true;
            }
            catch (Exception ex)
            {
                IsEnabled = false;
                IsRunning = false;
                await dialogService.ShowMessage(Lenguages.Error, ex.Message.ToString().Trim(), Lenguages.Accept);
            }
        }

        private void ReloadRates(List<Rate> rates)
        {
            //  Inicializa el objeto Rate()   \\
            Rates.Clear();

            //  Carga el objeto Rates() \\
            foreach (var rate in rates.OrderBy(r => r.Name))
            {
                Rates.Add(new Rate
                {
                    Code = rate.Code,
                    Name = rate.Name,
                    RateId = rate.RateId,
                    TaxRate = rate.TaxRate,
                });
            }
        }

        private async void Convert()
        {
            try
            {
                if (string.IsNullOrEmpty(Amount))
                {
                    await dialogService.ShowMessage(Lenguages.Error, Lenguages.AmountValidation, Lenguages.Accept);
                    return;
                }

                var amount = 0.0m;
                if (!decimal.TryParse(Amount, out amount))
                {
                    await dialogService.ShowMessage(Lenguages.Error, Lenguages.AmountNumericValidation, Lenguages.Accept);
                    return;
                }

                if (SourceRate == null)
                {
                    await dialogService.ShowMessage(Lenguages.Error, Lenguages.SourceRateValidation, Lenguages.Accept);
                    return;
                }

                if (TargetRate == null)
                {
                    await dialogService.ShowMessage(Lenguages.Error, Lenguages.TargetRateValidation, Lenguages.Accept);
                    return;
                }

                //   Genera el calculo de la tasa   \\
                var result = (amount / (decimal)SourceRate.TaxRate) * (decimal)TargetRate.TaxRate;
                Result = string.Format("The amount: {0:N2} in {1} is equal to {2:N2} in {3}", amount, SourceRate.Name, result, TargetRate.Name);
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage(Lenguages.Error, ex.Message.ToString().Trim(), Lenguages.Accept);
            }
        }

        private async void Clear()
        {
            LoadRates();
            Amount = null;
            Result = _resultReady;
            SourceRate = null;
            TargetRate = null;
            await dialogService.ShowMessage("Information", "The screen it's clean...!!!", "Accept");
        }

        private void Switch()
        {
            var auxVar001 = SourceRate;
            SourceRate = TargetRate;
            TargetRate = auxVar001;
            Convert();
        }

        #endregion Methods
    }
}
