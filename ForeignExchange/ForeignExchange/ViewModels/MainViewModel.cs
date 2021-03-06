﻿namespace ForeignExchange.ViewModels
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
        private string _status;

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

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
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
            //  _resultReady = "Ready to convert...!!!";
            _resultReady = Lenguages.TitleReadyConvert;
            //  _resultLoadRate = "Load rates, plase wait...!!!";
            _resultLoadRate = Lenguages.TitleLoadRate;

            //  ObservableCollection    \\
            Rates = new ObservableCollection<Rate>();

            //  Invoca el metodo que hace la carga de tasas (Rates) \\
            LoadRates();

            //  Captura el estatus de la carga de las tasas \\
            _status = "Nikole A. Herrera V.";
        }

        #endregion Constructor

        #region Methods
        private async void LoadRates()
        {
            try
            {
                //  Invoca el metodo que coloca o define el estatus de los controles    \\
                StatusControl(true, false, _resultLoadRate, string.Empty);

                //  Invoca el metodo que verifica el estado de la conexion a internet   \\
                var connection = await apiService.CheckConnection();
                if(!connection.IsSuccess)
                {
                    StatusControl(false, false, connection.Message, string.Empty);
                    //  await dialogService.ShowMessage(Lenguages.Error, connection.Message, Lenguages.Accept);
                    //  La linea anterior da error
                    return;
                }

                ////  Invoca al metodo que genera la carga de las tasas (Rates)   \\
                //var response = await apiService.GetRates();

                //if (!response.IsSuccess)
                //{
                //    //  Invoca el metodo que coloca o define el estatus de los controles    \\
                //    StatusControl(false, false, string.Empty, string.Empty);
                //    await dialogService.ShowMessage(Lenguages.Error, response.Message, Lenguages.Accept);
                //    return;
                //}

                //  Invoca al metodo generico que genera el objeto para la carga de las tasas (Rates)   \\
                var response = await apiService.GetList<Rate>("http://apiexchangerates.azurewebsites.net", "/api/Rates");

                if (!response.IsSuccess)
                {
                    //  Invoca el metodo que coloca o define el estatus de los controles    \\
                    StatusControl(false, false, string.Empty, string.Empty);
                    await dialogService.ShowMessage(Lenguages.Error, response.Message, Lenguages.Accept);
                    return;
                }

                //  Invoca al metodo que hace la carga de datos de las tasas (Rates)    \\
                ReloadRates((List<Rate>)response.Result);

                //  Invoca el metodo que coloca o define el estatus de los controles    \\
                StatusControl(false, true, _resultReady, Lenguages.TitleStatusInternet);
            }
            catch (Exception ex)
            {
                //  Invoca el metodo que coloca o define el estatus de los controles    \\
                StatusControl(false, false, string.Empty, string.Empty);
                await dialogService.ShowMessage(Lenguages.Error, ex.Message.ToString().Trim(), Lenguages.Accept);
            }
        }

        private void StatusControl(bool isRunning, bool isEnabled, string result, string status)
        {
            IsRunning = isRunning;
            IsEnabled = isEnabled;
            Result = result;
            Status = status;
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
                Result = string.Format("{0} {1:N2} {2} {3} {4} {5:N2} {6} {7}",
                    Lenguages.TitleTheAmount, amount, Lenguages.TitleIn, SourceRate.Name, Lenguages.TitleIsEqual, result,
                    Lenguages.TitleIn, TargetRate.Name);
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
