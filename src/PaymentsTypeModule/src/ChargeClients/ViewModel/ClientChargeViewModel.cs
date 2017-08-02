﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClientesFormaDeCobros.ChargeClients.View;
using Prism.Commands;
using Prism.Mvvm;
using DataAccessLayer;
using Prism.Interactivity.InteractionRequest;


namespace ClientesFormaDeCobros.ChargeClients.ViewModel
{
    public class ClientChargeViewModel : BindableBase
    {
        private string _chargeStateProperties;
        private string _lastUser;
        private string _lastModificationTime;
        private bool _dependencyOnCash;
        private bool _askedCreditCard;
        private string _chargeName;
        private string _chargeAccount1;
        private string _chargeAccount2;
        private IDictionary<string, DelegateCommand> _currentClickCommand = new Dictionary<string, DelegateCommand>();
        private string _chargeCommissionAccount;
        private string _chargeCommissionAccount2;
        private string _commissionPercentage;
        private string _commissionBank;
        private string _office;
        private string _office2;
        private string _swiftValue;
        private string _ibanValue;
        private string _chargeBank1;
        private string _chargeBank2;
        private string _billing1;
        private string _billing2;
        private bool _isClientAccountChecked;
        private bool _isTeleCheck;
        private bool _isAllShowerChecked;
        private bool _isPortfolioEffectChecked;
        private bool _isRemittableChecked;
        private bool _isMultiSeatCommissionChecked;
        private bool _isNoAutoListChecked;
        private string _dataGridColName1;
        private string _dataGridColName2;
        private GridPopUpViewModel viewModel = new GridPopUpViewModel();
        private GridPopUpView popUpView = new GridPopUpView();
        private IDalLocator locator;
        private DataTable _sourceDataTable;
        // interaction requests are of two types: Notification and Confirmation requests.

        public InteractionRequest<INotification> ErrorDialog { get; private set; }
        private DelegateCommand ErrorDialogCommand;

        public ClientChargeViewModel()
        {
            this.ClickCommand = new DelegateCommand<string>(OnClickCommand);
            locator = DalLocator.GetInstance();
            IDalObject obj = locator.FindDalObject("ChargeTypeDAL");
            _currentClickCommand.Add("FindChargeNumber", new DelegateCommand(ChargeNumberPopUp));
            _currentClickCommand.Add("FindChargeName", new DelegateCommand(ChargeNamePopUp));
            _currentClickCommand.Add("FindChargeAccount", new DelegateCommand(ChargeAccountPopUp));
            _currentClickCommand.Add("FindChargeCommissionAccount", new DelegateCommand(ChargeCommisionAccountPopUp));
            _currentClickCommand.Add("FindCommissionBank", new DelegateCommand(ChargeCommissionBankPopUp));
            _currentClickCommand.Add("FindOffice", new DelegateCommand(ChargeOfficePopUp));
            _currentClickCommand.Add("FindChargeBank", new DelegateCommand(ChargeBankPopUp));
            _currentClickCommand.Add("FindBillingAccount", new DelegateCommand(ChargeBillingAccountPopUp));
            InitDataLayer();

        }
        private void InitDataLayer()
        {
            try
            {
                locator = DalLocator.GetInstance();
                IDalObject obj = locator.FindDalObject("ChargeTypeDAL");
                _sourceDataTable = ((ChargeTypeDataAccessLayer)obj).GetChargeObjects();
            }
            catch (Exception e)
            {
                this.ErrorDialog = new InteractionRequest<INotification>();
                this.ErrorDialogCommand = new DelegateCommand(() =>
                {
                    this.ErrorDialog.Raise(
                        new Notification
                        {
                            Title = "Error during data layer loading",
                            Content = e.Message
                        },
                        (dialog) =>
                        {

                        });

                });
            }
        }
        private void ChargeBillingAccountPopUp()
        {
            viewModel.Title = "Formas Cobro e Facturacion";
            viewModel.QueryType = "Formas Cobro e Facturacion";
            ShowPopUp(viewModel);
        }

        private void ChargeBankPopUp()
        {
            viewModel.Title = "Consulta de Bancos";
            viewModel.QueryType = "Consulta de Bancos";
            ShowPopUp(viewModel);
        }

        private void ChargeOfficePopUp()
        {
            viewModel.Title = "Consulta Cuenta de Oficinas";
            viewModel.QueryType = "Consulta Cuenta de Oficinas";
            ShowPopUp(viewModel);
        }

        private void ChargeCommissionBankPopUp()
        {
            viewModel.Title = "Consulta de Bancos";
            viewModel.QueryType = "Consulta de Bancos";
            ShowPopUp(viewModel);

        }

        private void ChargeCommisionAccountPopUp()
        {
            viewModel.Title = "Consulta de Cuentas Contables";
            viewModel.QueryType = "Consulta de Cuentas Contables";
            ShowPopUp(viewModel);
        }

        private void ChargeAccountPopUp()
        {
            viewModel.Title = "Consulta de Cuentas Contables";
            viewModel.QueryType = "Consulta de Cuentas Contables";
            ShowPopUp(viewModel);
        }

        private void ChargeNamePopUp()
        {
            viewModel.Title = "Consulta Nombre";
            viewModel.QueryType = "Consulta Nombre";
            ShowPopUp(viewModel);

        }

        private void ChargeNumberPopUp()
        {
            viewModel.Title = "Consulta Numero";
            viewModel.QueryType = "Consulta Numero";
            ShowPopUp(viewModel);
        }

        public ICommand DataGridChangedSelection { get; set; }

        public ICommand NumberTextChanged { get; set; }


        /// <summary>
        ///  Generic command from the UI. It is a delegate command.
        /// </summary>
        public ICommand ClickCommand { get; set; }
        /// <summary>
        ///  Change of the state of the form.
        /// </summary>
        public string ChargeStateProperties
        {
            get { return _chargeStateProperties; }
            set
            {
                _chargeStateProperties = value;
                RaisePropertyChanged("ChargeStateProperties");
            }

        }
        /// <summary>
        ///  Last user that has done the modification in this table
        /// </summary>
        public string LastUser
        {
            get { return _lastUser; }
            set
            {
                _lastUser = value;
                RaisePropertyChanged("LastUser");
            }
        }
        /// <summary>
        /// The number of the charge from the customer
        /// </summary>
        public string ChargeNumber
        {
            get { return _lastUser; }
            set
            {
                _lastUser = value;
                RaisePropertyChanged("ChargeNumber");
            }
        }
        /// <summary>
        ///  The last time this record has been modified.
        /// </summary>
        public string LastModificationTime
        {
            get { return _lastModificationTime; }
            set
            {
                _lastModificationTime = value;
                RaisePropertyChanged("LastModificationTime");
            }
        }
        /// <summary>
        ///  The checkbox dependency on cash is active or not
        /// </summary>
        public bool IsDependencyOnCash
        {
            get { return _dependencyOnCash; }
            set
            {
                _dependencyOnCash = value;
                RaisePropertyChanged("IsDependencyOnCash");
            }

        }
        /// <summary>
        ///  The checkbox ask credit card is active or not.
        /// </summary>
        public bool IsAskedCreditCard
        {
            get { return _askedCreditCard; }
            set
            {
                _askedCreditCard = value;
                RaisePropertyChanged("IsAskedCreditCard");
            }
        }
        /// <summary>
        ///  The name of charging
        /// </summary>
        public string ChargeName
        {
            get { return _chargeName; }
            set
            {
                _chargeName = value;
                RaisePropertyChanged("ChargeName");
            }
        }
        /// <summary>
        ///  The account to be charged the first part.
        /// </summary>
        public string ChargeAccount1
        {
            get { return _chargeAccount1; }
            set
            {
                _chargeAccount1 = value;
                RaisePropertyChanged("ChargeAccount1");
            }
        }
        /// <summary>
        ///  The account to be charged the second part
        /// </summary>
        public string ChargeAccount2
        {
            get { return _chargeAccount2; }
            set
            {
                _chargeAccount2 = value;
                RaisePropertyChanged("ChargeAccount2");
            }
        }
        /// <summary>
        ///  The charge commission account, the first part.
        /// </summary>
        public string ChargeCommissionAccount1
        {
            get { return _chargeCommissionAccount; }
            set
            {
                _chargeCommissionAccount = value;
                RaisePropertyChanged("ChargeCommissionAccount1");
            }
        }
        /// <summary>
        ///  The charge commission account, second part.
        /// </summary>
        public string ChargeCommissionAccount2
        {
            get { return _chargeCommissionAccount2; }
            set
            {
                _chargeCommissionAccount2 = value;
                RaisePropertyChanged("ChargeCommissionAccount2");
            }
        }
        /// <summary>
        ///  The commission percentage value
        /// </summary>
        public string CommissionPercentage
        {
            get { return _commissionPercentage; }
            set
            {
                _commissionPercentage = value;
                RaisePropertyChanged("CommissionPercentage");
            }
        }
        /// <summary>
        ///  The commission bank value
        /// </summary>
        public string CommissionBank
        {
            get { return _commissionBank; }
            set
            {
                _commissionBank = value;
                RaisePropertyChanged("CommissionBank");
            }
        }
        /// <summary>
        ///  The commision office value
        /// </summary>
        public string Office1
        {
            get { return _office; }
            set
            {
                _office = value;
                RaisePropertyChanged("Office1");
            }
        }
        /// <summary>
        ///  The commission office value 2
        /// </summary>
        public string Office2
        {
            get { return _office2; }
            set
            {
                _office2 = value;
                RaisePropertyChanged("Office2");
            }
        }
        /// <summary>
        ///  The iban value
        /// </summary>
        public string IbanValue
        {
            get { return _ibanValue; }
            set
            {
                _ibanValue = value;
                RaisePropertyChanged("IbanValue");
            }
        }
        /// <summary>
        ///  The swift value 
        /// </summary>
        public string SwiftValue
        {
            get { return _swiftValue; }
            set
            {
                _swiftValue = value;
                RaisePropertyChanged("SwiftValue");
            }
        }
        /// <summary>
        ///  The charge bank value 
        /// </summary>
        public string ChargeBank1
        {
            get { return _chargeBank1; }
            set
            {
                _chargeBank1 = value;
                RaisePropertyChanged("ChargeBank1");
            }
        }
        /// <summary>
        ///  The charge bank2 value
        /// </summary>
        public string ChargeBank2
        {
            get { return _chargeBank2; }
            set
            {
                _chargeBank2 = value;
                RaisePropertyChanged("ChargeBank2");
            }
        }

        /// <summary>
        ///  The billing value first part 
        /// </summary>
        public string Billing1
        {
            get { return _billing1; }
            set
            {
                _billing1 = value;
                RaisePropertyChanged("Billing1");
            }
        }
        /// <summary>
        ///  The billing value second part
        /// </summary>
        public string Billing2
        {
            get { return _billing2; }
            set
            {
                _billing2 = value;
                RaisePropertyChanged("Billing2");
            }
        }
        /// <summary>
        ///  The client account checked.
        /// </summary>
        public bool IsClientAccountChecked
        {
            get { return _isClientAccountChecked; }
            set
            {
                _isClientAccountChecked = value;
                RaisePropertyChanged("IsClientAccountChecked");
            }
        }
        /// <summary>
        ///  The client account checked.
        /// </summary>
        public bool IsAllShowerChecked
        {
            get { return _isAllShowerChecked; }
            set
            {
                _isAllShowerChecked = value;
                RaisePropertyChanged("IsAllShowerChecked");
            }
        }
        /// <summary>
        ///  The client account checked.
        /// </summary>
        public bool IsTeleCheckChecked
        {
            get { return _isTeleCheck; }
            set
            {
                _isTeleCheck = value;
                RaisePropertyChanged("IsTeleCheckChecked");
            }
        }
        // <summary>
        ///  Is portfolio effect checked.
        /// </summary>
        public bool IsPortfolioEffectChecked
        {
            get { return _isPortfolioEffectChecked; }
            set
            {
                _isPortfolioEffectChecked = value;
                RaisePropertyChanged("IsPortfolioEffectChecked");
            }
        }
        // <summary>
        ///  It is remittable checked.
        /// </summary>
        public bool IsRemittableChecked
        {
            get { return _isRemittableChecked; }
            set
            {
                _isRemittableChecked = value;
                RaisePropertyChanged("IsRemittableChecked");
            }
        }

        // <summary>
        ///  It is remittable checked.
        /// </summary>
        public bool IsMultiSeatCommission
        {
            get { return _isMultiSeatCommissionChecked; }
            set
            {
                _isMultiSeatCommissionChecked = value;
                RaisePropertyChanged("IsMultiSeatCommission");
            }
        }
        // <summary>
        ///  It is not auto list
        /// </summary>
        public bool IsNoAutoList
        {
            get { return _isNoAutoListChecked; }
            set
            {
                _isNoAutoListChecked = value;
                RaisePropertyChanged("IsNoAutoList");
            }
        }
        public DataTable SourceTable
        {
            get { return _sourceDataTable; }
            set
            {
                _sourceDataTable = value;
                RaisePropertyChanged("SourceTable");
            }
        }
        /// <summary>
        ///  This is the header of a column
        /// </summary>
        public String HeaderCol1
        {
            get { return _dataGridColName1; }
            set
            {
                _dataGridColName1 = value;
                RaisePropertyChanged("HeaderCol1");
            }
        }
        /// <summary>
        ///  This is the header of a column
        /// </summary>
        public string HeaderCol2
        {
            get { return _dataGridColName2; }
            set
            {
                _dataGridColName2 = value;
                RaisePropertyChanged("HeaderCol2");
            }
        }

        /// <summary>
        ///  Delegate command to the value.
        /// </summary>
        /// <param name="param"></param>
        public void OnClickCommand(string param)
        {
            if (_currentClickCommand.ContainsKey(param))
            {
                DelegateCommand cmd;
                if (_currentClickCommand.TryGetValue(param, out cmd))
                {
                    cmd.Execute();
                }
            }

        }

        private void ShowPopUp(GridPopUpViewModel viewModel)
        {
            // this works but is wrong.
            popUpView = new GridPopUpView();
            popUpView.DataContext = viewModel;
            popUpView.ShowDialog();
        }

    }
}
