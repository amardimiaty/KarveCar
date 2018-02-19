﻿using System.Collections.Generic;
using MasterModule.Common;
using KarveCommon.Services;
using KarveDataServices;
using Prism.Regions;
using KarveCommonInterfaces;
using KarveDataServices.DataTransferObject;
using KarveDataServices.DataObjects;
using System.Windows.Input;
using Prism.Commands;
using System.ComponentModel;
using KarveCommon.Generic;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Threading.Tasks;

namespace MasterModule.ViewModels
{
    // This view model is useful fro 
  public class CompanyInfoViewModel : MasterInfoViewModuleBase, IEventObserver, IDisposeEvents
    {
        #region Constructor 
        public CompanyInfoViewModel(IEventManager eventManager, IConfigurationService configurationService, IDataServices dataServices, IRegionManager manager) : base(eventManager, configurationService, dataServices, manager)
        {
            base.ConfigureAssist();
            AssistCommand = new DelegateCommand<object>(OnAssistCommand);
            ItemChangedCommand = new DelegateCommand<object>(OnChangedField);
            ShowOfficesCommand = new DelegateCommand<object>(ShowOffices);
            AssistExecuted += CompanyAssistResult;
            EventManager.RegisterObserverSubsystem(MasterModuleConstants.CompanySubSystemName, this);
            DataObject = new CompanyDto();
        }
        #endregion
        #region Properties
        /// <summary>
        ///  Data object
        /// </summary>
        public CompanyDto DataObject {
            set
            {
                _currentCompanyDto = value;
                RaisePropertyChanged();
            }
            get
            {
                return _currentCompanyDto;
            }
        }
        /// <summary>
        ///  Helper data.
        /// </summary>
        public IHelperBase CompanyHelper
        {
            set
            {
                _companyHelper = value;
                RaisePropertyChanged();
            }
            get
            {
                return _companyHelper;
            }
        }
        /// <summary>
        ///  Show offices command.
        /// </summary>
        public ICommand ShowOfficesCommand { set; get; }
        /// <summary>
        ///  Show brokers data objects.
        /// </summary>
        public IEnumerable<CommissionAgentSummaryDto> BrokersDto {
            get
            {
                return _brokers;
            }
            set
            {
                _brokers = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        ///  Show client data transfer object.
        /// </summary>
        public IEnumerable<ClientSummaryDto> ClientDto
        {
            get
            {
                return _clientDto;
            }
            set
            {
                _clientDto = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Public Methods

        /// <summary>
        ///  Incoming Payload
        /// </summary>
        /// <param name="payload">Data payload</param>
        public void IncomingPayload(DataPayLoad payload)
        {
            

            if (payload != null)
            {

                RegisterPrimaryKey(payload);
                // here i can fix the primary key
                switch (payload.PayloadType)
                {
                    case DataPayLoad.Type.UpdateData:
                        {
                            if (payload.HasDataObject)
                            {
                                var clientData = payload.DataObject as CompanyDto;
                                DataObject = clientData;
                            }
                            break;
                        }
                    case DataPayLoad.Type.UpdateView:
                    case DataPayLoad.Type.Show:
                        {
                            Init(PrimaryKeyValue, payload, false);
                            CurrentOperationalState = DataPayLoad.Type.Show;
                            break;
                        }
                    case DataPayLoad.Type.Insert:
                        {
                            CurrentOperationalState = DataPayLoad.Type.Insert;
                            if (string.IsNullOrEmpty(PrimaryKeyValue))
                            {
                                PrimaryKeyValue = DataServices.GetCompanyDataServices().GetNewId();

                                DataServices.GetClientDataServices().GetNewId();

                                CurrentOperationalState = DataPayLoad.Type.Insert;
                            }
                            Init(PrimaryKeyValue, payload, true);
                            break;
                        }
                    case DataPayLoad.Type.Delete:
                        {

                            if (PrimaryKey == payload.PrimaryKey)
                            {
                                DeleteEventCleanup(payload.PrimaryKeyValue, PrimaryKeyValue, DataSubSystem.ClientSubsystem,
                                    MasterModuleConstants.CompanySubSystemName);
                                DeleteRegion(payload.PrimaryKeyValue);

                            }
                            break;
                        }
                }
            }
        }

        public void Init(string primaryKey, DataPayLoad payload, bool isInsert)
        {
            if (payload.HasDataObject)
            {
                Logger.Info("CompanyInfoViewModel has received payload type " + payload.PayloadType.ToString());
                var companyData = payload.DataObject as ICompanyData;
                if (companyData != null)
                {
                    _companyData = companyData;
                    DataObject = _companyData.Value;
                    CompanyHelper = companyData;
                    // When the view model receive a message broadcast to its child view models.                
                    //EventManager.SendMessage(UpperBarCompanyViewModel.Name, payload);
                    Logger.Info("CompanyInfoViewModel has activated the client subsystem as current with directive " +
                                payload.PayloadType.ToString());
                    ActiveSubSystem();
                    RaisePropertyChanged("Helper");
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        ///  This method set the registration payload.
        /// </summary>
        /// <param name="payLoad"></param>
        protected override void SetRegistrationPayLoad(ref DataPayLoad payLoad)
        {
            payLoad.PayloadType = DataPayLoad.Type.RegistrationPayload;
            payLoad.Subsystem = DataSubSystem.CompanySubsystem;
        }
        #endregion

        #region Private Methods

        private void OnAssistCommand(object param)
        {
            IDictionary<string, string> values = (Dictionary<string, string>)param;
            string assistTableName = values.ContainsKey("AssistTable") ? values["AssistTable"] as string : null;
            string assistQuery = values.ContainsKey("AssistQuery") ? values["AssistQuery"] as string : null;
            this.AssistNotifierInitialized = NotifyTaskCompletion.Create<bool>(AssistQueryRequestHandler(assistTableName, assistQuery), AssistExecuted);
        }

        private void CompanyAssistResult(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName.Equals("Status"))
            {
                if (AssistNotifierInitialized.IsSuccessfullyCompleted)
                {
                    bool value = AssistNotifierInitialized.Task.Result;
                    if (!value)
                    {
                        Logger.Error("Executed Assist invalid");
                    }

                }
            }

        }
        private async Task<bool> AssistQueryRequestHandler(string assistTableName, string assistQuery)
        {
            var value = await AssistMapper.ExecuteAssist(assistTableName, assistQuery);
            bool retValue = false;
            if (value != null)
            {
                switch (assistTableName)
                {
                    case "CITY_ASSIST":
                        {
                            CompanyHelper.CityDto = (IEnumerable<CityDto>)value;
                            retValue = true;
                            break;
                        }
                    case "PROVINCE_ASSIST":
                        {
                            CompanyHelper.ProvinciaDto = (IEnumerable<ProvinciaDto>)value;
                            retValue = true;
                            break;
                        }
                    case "COUNTRY_ASSIST":
                        {
                            CompanyHelper.CountryDto = (IEnumerable<CountryDto>)value;
                            retValue = true;
                            break;
                        }

                    case "BROKER_ASSIST":
                        {
                            BrokersDto = (IEnumerable<CommissionAgentSummaryDto>)value;
                            retValue = true;
                            break;
                        }
                    case "CLIENT_ASSIST":
                        {
                            ClientDto = (IEnumerable<ClientSummaryDto>)value;
                            retValue = true;
                            break;
                        }
                }
                RaisePropertyChanged("CompanyHelper");
            }
            return retValue;
        }

        /// <summary>
        /// Change field.
        /// </summary>
        /// <param name="objectChanged"></param>
        private void OnChangedField(object objectChanged)
        {
            IDictionary<string, object> eventDictionary = (IDictionary<string, object>)objectChanged;
            OnChangedField(eventDictionary);
        }

        private void OnChangedField(IDictionary<string, object> eventDictionary)
        {
            DataPayLoad payLoad = new DataPayLoad();
            payLoad.Subsystem = DataSubSystem.CompanySubsystem;
            payLoad.SubsystemName = MasterModuleConstants.CompanySubSystemName;
            payLoad.PayloadType = DataPayLoad.Type.Update;
            if (string.IsNullOrEmpty(payLoad.PrimaryKeyValue))
            {
                payLoad.PrimaryKeyValue = PrimaryKeyValue;

            }
            if (eventDictionary.ContainsKey("DataObject"))
            {
                if (eventDictionary["DataObject"] == null)
                {
                    MessageBox.Show("DataObject is null.");
                }
            }
            // remove ViewModelQueries.
            ChangeFieldHandlerDo<CompanyDto> handlerDo = new ChangeFieldHandlerDo<CompanyDto>(EventManager,DataSubSystem.CompanySubsystem);

            if (CurrentOperationalState == DataPayLoad.Type.Insert)
            {
                handlerDo.OnInsert(payLoad, eventDictionary);
            }
            else
            {
                payLoad.PayloadType = DataPayLoad.Type.Update;
                handlerDo.OnUpdate(payLoad, eventDictionary);
            }
        }

        /// <summary>
        ///  Show or Hide offices.
        /// </summary>
        /// <param name="obj"></param>
        private void ShowOffices(object obj)
        {
            RegionManager.RequestNavigate("OfficeShow", "Office");
        }
        // <summary>
        ///  This register the primary key
        /// </summary>
        /// <param name="payLoad">Payload to be registered</param>
        private void RegisterPrimaryKey(DataPayLoad payLoad)
        {
            Contract.Assert(PrimaryKeyValue != null, "RegisterPrimaryKey error");
            Contract.Assert(payLoad != null, "RegisterPrimaryKey error");
            if (PrimaryKeyValue.Length == 0)
            {
                PrimaryKeyValue = payLoad.PrimaryKeyValue;
                _mailBoxName = "Company." + PrimaryKeyValue + "." + UniqueId;
                if (!string.IsNullOrEmpty(PrimaryKeyValue))
                {
                    if (MailBoxHandler != null)
                    {
                        EventManager.RegisterMailBox(_mailBoxName, MailBoxHandler);
                    }
                }
            }

        }
        public override void DisposeEvents()
        {
            EventManager.DeleteMailBoxSubscription(_mailBoxName);
            EventManager.DeleteObserverSubSystem(MasterModuleConstants.CompanySubSystemName, this);
        }
        #endregion

        #region Private Fields
        private ICompanyData _companyData;
        private string _mailBoxName;
        private IHelperBase _companyHelper;
        private CompanyDto _currentCompanyDto;
        private IEnumerable<CommissionAgentSummaryDto> _brokers;
        private IEnumerable<ClientSummaryDto> _clientDto;
        #endregion


    }
}