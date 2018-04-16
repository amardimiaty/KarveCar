﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using KarveCommon.Services;
using KarveDataServices;
using MasterModule.Common;
using KarveDataServices.DataObjects;
using KarveCommon.Generic;
using System.ComponentModel;
using System.Diagnostics;
using AutoMapper;
using DataAccessLayer.DataObjects;
using DataAccessLayer.Logic;
using KarveDataServices.DataTransferObject;
using MasterModule.Views.Vehicles;
using Prism.Commands;
using Prism.Regions;
using KarveCommonInterfaces;
using DataAccessLayer.SQL;

namespace MasterModule.ViewModels
{
    /// <summary>
    ///  This represent the detailed class for the view model in case of each car.
    /// </summary>
    partial class VehicleInfoViewModel: MasterInfoViewModuleBase, IEventObserver, IDataErrorInfo
    {
        private const string DefaultViewInfoRegionName = "TabRegion";

        /// <summary>
        ///  Vehicle Data Services.
        /// </summary>
        private IVehicleDataServices _vehicleDataServices;
        private IVehicleData _vehicleDo;
        private object _dataObject;
        private object _deleteNotifyTaskCompletion;
        private INotifyTaskCompletion<IVehicleData> _initializationTable;
       
        private string _assistQueryOwner;
        private ObservableCollection<VehicleActivitiesDto> _activity;
        private ObservableCollection<AgentDto> _agents;
        private ObservableCollection<OwnerDto> _owner;
        private ObservableCollection<SupplierSummaryDto> _supplier;
        private ObservableCollection<PaymentFormDto> _paymentFormDto;
        private ObservableCollection<ClientsSummaryDto> _clients;
        private ObservableCollection<ResellerDto> _vendedor;
        private ObservableCollection<ClientsSummaryDto> _clientDto;
        private ObservableCollection<SupplierSummaryDto> _assuranceCompany;
        private ObservableCollection<SupplierSummaryDto> _assuranceAgent;
        private ObservableCollection<SupplierSummaryDto> _assuranceAdditionalCompany;
        private ObservableCollection<SupplierSummaryDto> _assistanceAssuranceCompany;
        private ObservableCollection<SupplierSummaryDto> _assistancePolicyAssuranceCompany;
        private ObservableCollection<MaintainanceDto> _maintenaince = new ObservableCollection<MaintainanceDto>();
        
        private INotifyTaskCompletion<ObservableCollection<ElementDto>> _elementLoadNotifyTaskCompletion;

        // this is in the vehicle stuff.
        private PropertyChangedEventHandler _deleteEventHandler;
        /// <summary>
        ///  vehicle revision
        /// </summary>
        private ObservableCollection<UiComposedFieldObject> _vehicleRevision;
        /// <summary>
        ///  account stuff.
        /// </summary>
        private ObservableCollection<AccountDto> _accountDto;
        private ObservableCollection<ElementDto> _elementDto;
        private ObservableCollection<AccountDto> _accountPreviousRepaymentDto;
        private ObservableCollection<AccountDto> _accountImmobilizedDto;
        private ObservableCollection<AccountDto> _accountDtoPaymentAccountDto;
        private ObservableCollection<AccountDto> _accountDtoCapitalCp;
        private ObservableCollection<AccountDto> _accomulatedRepayment;
        private IRegionManager _regionManager;
        private ObservableCollection<CityDto> _cityDto;
        private ObservableCollection<CityDto> _roadTaxesCities = new ObservableCollection<CityDto>();
        private ObservableCollection<ZonaOfiDto> _officeZoneRoadTaxes;
        private ObservableCollection<CurrentSituationDto> _situationDto;
        private ObservableCollection<CompanyDto> _otherOffice1Dto;
        private ObservableCollection<CompanyDto> _otherOffice2Dto = new ObservableCollection<CompanyDto>();
        private ObservableCollection<CompanyDto> _otherOffice3Dto = new ObservableCollection<CompanyDto>();
        private IEnumerable<BrandVehicleDto> _brandDtos;
        private IEnumerable<ModelVehicleDto> _modelDtos;
        private IEnumerable<VehicleGroupDto> _vehicleGroupDtos;
        private IEnumerable<ColorDto> _colorDto;
        private QueryStoreFactory _queryStoreFactory;


        // This returns the list of activity when asked.
        public ObservableCollection<VehicleActivitiesDto> ActivityDtos
        {
            get
            {
                return _activity;
            }
            set
            {
                _activity= value;
                RaisePropertyChanged();
            }
            
        }

        public ObservableCollection<ResellerDto> VendedorDtos
        {
            get { return _vendedor; }
            set { _vendedor = value; RaisePropertyChanged(); }
        }


        /// <summary>
        ///  This returns the list of agents.
        /// </summary>
        public ObservableCollection<AccountDto> AccountDtos
        {
            get
            {
                return _accountDto;
            }
            set
            {
                _accountDto = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///  This returns the list of agents.
        /// </summary>
        public ObservableCollection<AccountDto> AccountDtosImmobilized
        {
            get
            {
                return _accountImmobilizedDto;
            }
            set
            {
                _accountImmobilizedDto = value;
                RaisePropertyChanged();
            }
        }



        /// <summary>
        ///  This returns the list of agents.
        /// </summary>
        public ObservableCollection<AccountDto> AccountDtoPreviousRepayment
        {
            get
            {
                return _accountPreviousRepaymentDto;
            }
            set
            {
                _accountPreviousRepaymentDto = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///  This returns the list of agents.
        /// </summary>
        public ObservableCollection<AccountDto> AccountDtoPaymentAccount
        {
            get
            {
                return _accountDtoPaymentAccountDto;
            }
            set
            {
                _accountDtoPaymentAccountDto = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        ///  This returns the list of agents.
        /// </summary>
        public ObservableCollection<AccountDto> AccountDtoCapitalCp
        {
            get
            {
                return _accountDtoCapitalCp;
            }
            set
            {
                _accountDtoCapitalCp = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///  This returns the list of agents.
        /// </summary>
        public ObservableCollection<ElementDto> ElementDtos
        {
            get
            {
                return _elementDto;
            }
            set
            {
                _elementDto = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///  This returns the list of agents.
        /// </summary>
        public ObservableCollection<AgentDto> AgentDtos
        {
            get
            {
                return _agents;
            }
            set
            {
                _agents = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        ///  This returns the list of owners.
        /// </summary>
        public ObservableCollection<OwnerDto> OwnerDtos
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                RaisePropertyChanged();
            }
        }

        
        /// </summary>
        public ObservableCollection<SupplierSummaryDto> AssuranceDtos
        {
            get
            {
                return _assuranceCompany;
            }
            set
            {
                _assuranceCompany = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<SupplierSummaryDto> AdditionalAssuranceDtos
        {
            get
            {
                return _assuranceAdditionalCompany;
            }
            set
            {
                _assuranceAdditionalCompany = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<SupplierSummaryDto> AssistanceAssuranceDtos
        {
            get
            {
                return _assistanceAssuranceCompany;
            }
            set
            {
                _assistanceAssuranceCompany = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<SupplierSummaryDto> AssistancePolicyAssuranceDtos
        {
            get
            {
                return _assistancePolicyAssuranceCompany;
            }
            set
            {
                _assistancePolicyAssuranceCompany = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        ///  This returns a maintenance collection 
        /// </summary>
        public ObservableCollection<MaintainanceDto> MaintenanceCollection
        {
            get
            {
                return _maintenaince;
            }
            set
            {
                _maintenaince = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<AccountDto> AccountDtoAccmulatedRepayment
        {
            get { return _accomulatedRepayment; }
            set
            {
                _accomulatedRepayment = value; RaisePropertyChanged();
            }
        }

        /// <summary>
        ///  This is an assurance agent dtos.
        /// </summary>
        public ObservableCollection<SupplierSummaryDto> AssuranceAgentDtos
        {
            get
            {
                return _assuranceAgent;
            }
            set
            {
                _assuranceAgent = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        ///  This returns a client dtpo.
        /// </summary>
        public ObservableCollection<ClientsSummaryDto> ClientesDtos
        {
            get
            {
                return _clients;
            }
            set {
                _clients = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="configurationService">This is the configurartion service</param>
        /// <param name="eventManager"> The event manager for sending/recieving messages from the view model</param>
        /// <param name="services">Data access layer interface</param>
        public VehicleInfoViewModel(IConfigurationService configurationService, IEventManager eventManager, 
            IDialogService dialogService,
            IDataServices services, IRegionManager regionManager, IInteractionRequestController requestController) : 
            base(eventManager, configurationService,services ,dialogService,regionManager, requestController)
        {
 			MailBoxHandler += MessageHandler;
            _vehicleDataServices = services.GetVehicleDataServices();
            ItemChangedCommand = new DelegateCommand<object>(ChangeUnpack);
            MetaDataObject = InitAssuranceObject();
            DataFieldCollection = InitDataField();
            RevisionObject = InitRevisionComposedFieldObjects();
            MaintenanceCollection = _maintenaince;
            _regionManager = regionManager;
            _deleteEventHandler+=DeleteEventHandler;
            EventManager.RegisterObserverSubsystem(MasterModuleConstants.VehiclesSystemName, this);
            AssistCommand = new DelegateCommand<object>(AssistCommandHelper);
            _queryStoreFactory = new QueryStoreFactory();
            ActiveSubSystem();
        }
      
        private async Task<ObservableCollection<ElementDto>> InitElements()
        {
            IEnumerable<ElementDto> value = await DataServices.GetHelperDataServices().GetAsyncHelper<ElementDto>(GenericSql.ElementsSummaryQuery);
            return new ObservableCollection<ElementDto>(value);
        }
        public ObservableCollection<UiComposedFieldObject> RevisionObject
        {
             get { return _vehicleRevision; }
            set { _vehicleRevision = value; RaisePropertyChanged(); }
        }
        // TODO: using the configure
        private async void AssistCommandHelper(object param)
        {
            IDictionary<string, string> values = param as Dictionary<string, string>;
            string assistTableName = values.ContainsKey("AssistTable") ? values["AssistTable"]  : null;
            string assistQuery = values.ContainsKey("AssistQuery") ? values["AssistQuery"]  : null;
            /*  ok now i have to handle the assist helper.
             *  The smartest thing is to detect the table from the query.
             */
            IMapper mapper = MapperField.GetMapper();
            IHelperDataServices helperDataServices = DataServices.GetHelperDataServices();
            assistTableName = assistTableName.ToUpper();

            switch (assistTableName)
            {
                
               
                case "ACTIVEHI":
                {
                    var act =
                        await helperDataServices.GetMappedAsyncHelper<VehicleActivitiesDto, ACTIVEHI>(assistQuery);
                    ActivityDtos = new ObservableCollection<VehicleActivitiesDto>(act);
                    break;
                }
                case "PROPIE":
                {
                   var propie = await helperDataServices.GetMappedAllAsyncHelper<OwnerDto, PROPIE>();
                   var ownerDtos = new ObservableCollection<OwnerDto>(propie);
                   OwnerDtos = ownerDtos;
                   break;
                }
                case "AGENTES":
                {
                   
                    var agents = await helperDataServices.GetAsyncHelper<AgentDto>(assistQuery);
                    ObservableCollection<AgentDto> agentDtos = new ObservableCollection<AgentDto>(agents);   
                    AgentDtos = agentDtos;
                    break;
                }
                case "ASSURANCE":
                {
                    var provee =
                        await helperDataServices.GetAsyncHelper<SupplierSummaryDto>(GenericSql.SupplierSummaryQuery);
                    AssuranceDtos = new ObservableCollection<SupplierSummaryDto>(provee);
                    break;
                }
                case "ASSURANCE_1":
                {
                    AssistancePolicyAssuranceDtos = await FetchSupplierCollection();
                    break;
                }
                case "ASSURANCE_2":
                {

                    AdditionalAssuranceDtos = await FetchSupplierCollection();
                    break;
                }
                case "ASSURANCE_3":
                {
                    AssistanceAssuranceDtos = await FetchSupplierCollection();
                    break;
                }
                case "ASSURANCE_AGENT":
                {
                    AssuranceAgentDtos = await FetchSupplierCollection();
                    break;
                }
                case "PROVEE1":
                {
                    ProveeDto = await FetchSupplierCollection();
                    break;
                }
                case "PROVEE2":
                {
                    var provee =
                        await helperDataServices.GetAsyncHelper<SupplierSummaryDto>(GenericSql.SupplierSummaryQuery);
                    ProveeDto2 = new ObservableCollection<SupplierSummaryDto>(provee);
                    break;
                }
                  
                case "CU1":
                {
                    var contable = await helperDataServices.GetAsyncHelper<AccountDto>(GenericSql.AccountSummaryQuery);
                    AccountDtos = new ObservableCollection<AccountDto>(contable);
                    break;
                }
                case "FORMAS":
                {
                    var paymentForm = await helperDataServices.GetMappedAllAsyncHelper<PaymentFormDto, FORMAS>();    
                    PaymentFormDto = new ObservableCollection<PaymentFormDto>(paymentForm);
                    break;
                }
                case "CLIENTES1":
                {
                    var clientes = await helperDataServices.GetAsyncHelper<ClientsSummaryDto>(GenericSql.SupplierSummaryQuery);
                    ClientsDto = new ObservableCollection<ClientsSummaryDto>(clientes);
                    break;
                }
                case "ACCOUNT_INMOVILIZADO":
                {
                    var contable = await helperDataServices.GetAsyncHelper<AccountDto>(GenericSql.AccountSummaryQuery);
                    AccountDtosImmobilized= new ObservableCollection<AccountDto>(contable);
                    break;
                }
                case "ACCOUNT_PAYMENT_ACCOUNT":
                {
                    var contable = await helperDataServices.GetAsyncHelper<AccountDto>(GenericSql.AccountSummaryQuery);
                    AccountDtoPaymentAccount = new ObservableCollection<AccountDto>(contable);
                    break;
                }
                case "ACCOUNT_PREVIUOS_PAYMENT":
                {
                    var contable = await helperDataServices.GetAsyncHelper<AccountDto>(GenericSql.AccountSummaryQuery);
                    AccountDtoPreviousRepayment = new ObservableCollection<AccountDto>(contable);
                    break;
                }
                case "ACCOUNT_ACCUMULATED_REPAYMENT":
                {
                    var contable = await helperDataServices.GetAsyncHelper<AccountDto>(GenericSql.AccountSummaryQuery);
                    AccountDtoAccmulatedRepayment = new ObservableCollection<AccountDto>(contable);
                    break;
                }
                case "POBLACIONES":
                {
                    var prov = await helperDataServices.GetAsyncHelper<POBLACIONES>(assistQuery);
                    IEnumerable<CityDto> cities = mapper.Map<IEnumerable<POBLACIONES>, IEnumerable<CityDto>>(prov);
                    CityDto = new ObservableCollection<CityDto>(cities);
                    break;

                 }
                case "OFICINA1":
                {
                    var oficina = await helperDataServices.GetMappedAllAsyncHelper<CompanyDto, SUBLICEN>();
                    OtherOffice1Dto = new ObservableCollection<CompanyDto>(oficina);
                    break;
                }
                case "OFICINA2":
                {
                    var oficina2 = await helperDataServices.GetMappedAllAsyncHelper<CompanyDto, SUBLICEN>();
                    OtherOffice2Dto = new ObservableCollection<CompanyDto>(oficina2);
                        break;
                }
                case "OFICINA3":
                {
                    var oficina = await helperDataServices.GetMappedAllAsyncHelper<CompanyDto, SUBLICEN>();
                    OtherOffice3Dto = new ObservableCollection<CompanyDto>(oficina);
                    break;
                }
                   
                case "COLORFL":
                    {
                        var colos = await helperDataServices.GetMappedAllAsyncHelper<ColorDto, COLORFL>();
                        ColorDtos = new ObservableCollection<ColorDto>(colos);
                        break;
                    }
                case "MARCAS":
                    {
                   
                        var brands = await helperDataServices.GetMappedAllAsyncHelper<BrandVehicleDto, MARCAS>();
                        BrandDtos = brands;
                        break;
                    }
                case "MODELO":
                    {
                        var models = await helperDataServices.GetMappedAllAsyncHelper<ModelVehicleDto, MODELO>();
                        ModelDtos = models;
                        break;
                    }
                case "GRUPOS":
                    {
                        var vehicles = await helperDataServices
                            .GetMappedAllAsyncHelper<VehicleGroupDto, GRUPOS>();
                        VehicleGroupDtos = vehicles;
                        break;
                    }
                case "SITUATION":
                {
                    var sit = await helperDataServices.GetMappedAllAsyncHelper<CurrentSituationDto,SITUACION>();
                    CurrentSituationDto = new ObservableCollection<CurrentSituationDto>(sit);
                    break;
                }
                case "ROAD_TAXES_CITY":
                    {
                        var prov = await helperDataServices.GetMappedAllAsyncHelper<CityDto, POBLACIONES>();
                        RoadTaxesCityDto = new ObservableCollection<CityDto>(prov);
                        break;
                    }
                case "ROAD_TAXES_ZONAOFI":
                {
                    var oficinas = await helperDataServices.GetMappedAllAsyncHelper<ZonaOfiDto, ZONAOFI>();
                    RoadTaxesOfficeZoneDto = new ObservableCollection<ZonaOfiDto>(oficinas);
                    break;
                }
                case "VENDEDOR":
                {
                    var vendedor = await helperDataServices.GetMappedAllAsyncHelper<ResellerDto, VENDEDOR>();
                    VendedorDtos = new ObservableCollection<ResellerDto>(vendedor);
                    break;
                }
            }
        }
        /// <summary>
        ///  SupplierAssitQuery
        /// </summary>
        public string SupplierAssistQuery
        {
            get
            {
                IQueryStore store = _queryStoreFactory.GetQueryStore();
                store.AddParam(QueryType.QuerySupplierSummary);
                return store.BuildQuery();
            }
        }
        /// <summary>
        ///  SellerAssistQuery.
        /// </summary>
        public string SellerAssistQuery
        {
            get
            {
                IQueryStore store = _queryStoreFactory.GetQueryStore();
                store.AddParam(QueryType.QuerySellerSummary);
                return store.BuildQuery();
            }
        }
        public ObservableCollection<ZonaOfiDto> RoadTaxesOfficeZoneDto
        {
            get
            {
                return _officeZoneRoadTaxes;
            }
            set
            {
                _officeZoneRoadTaxes = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<CurrentSituationDto> SituationDto
        {
            get
            {
                return _situationDto;
            }
            set
            {
                _situationDto = value;
                RaisePropertyChanged();
            }
        }
        private async Task<ObservableCollection<SupplierSummaryDto>> FetchSupplierCollection()
        {
            var provee =
                await DataServices.GetHelperDataServices().GetAsyncHelper<SupplierSummaryDto>(GenericSql.SupplierSummaryQuery);
            var collection = new ObservableCollection<SupplierSummaryDto>(provee);
            return collection;
        }
        public ObservableCollection<SupplierSummaryDto> ProveeDto
        {
            get { return _supplier; }
            set { _supplier = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<SupplierSummaryDto> ProveeDto2
        {
            get { return _supplier; }
            set { _supplier = value; RaisePropertyChanged(); }
        }

        public object DataObject
        {
            get { return _dataObject; }
            set { _dataObject = value;
                MetaDataObject = InitAssuranceObject();
                DataFieldCollection = InitDataField();
                RaisePropertyChanged();
            }
        }
        
        
        
        public string Error => throw new NotImplementedException();

        public string this[string columnName] => throw new NotImplementedException();

        /// <summary>
        /// This is the start notify.
        /// </summary>
        public override void StartAndNotify()
        {
            _initializationTable =
                NotifyTaskCompletion.Create<IVehicleData>(LoadDataValue(PrimaryKeyValue, IsInsertion), InitializationDataObjectOnPropertyChanged);
            
        }
        /// <summary>
        /// Load element account.
        /// </summary>
        private void LoadElementAccounts()
        {
            _elementLoadNotifyTaskCompletion =
                NotifyTaskCompletion.Create<ObservableCollection<ElementDto>>(InitElements(), InitializationElementDto);
        }

        private void InitializationElementDto(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ObservableCollection<ElementDto>)
            {
                ElementDtos = (ObservableCollection<ElementDto>) sender;

            }
        }

        private void InitializationDataObjectOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IVehicleData)
            {
                IVehicleData vehicle = (IVehicleData) sender;
                DataObject = vehicle;
            }
        }
      

        private void ChangeUnpack(object value)
        {
            IDictionary<string,object> changedItem = value as IDictionary<string,object>;
            if (changedItem != null)
            {
                OnChangedField(changedItem);
            }
        }
        /// <summary>
        ///  OnChangedField. This method shall be changed moved to the upper level.
        /// </summary>
        /// <param name="eventDictionary">Dictionary of events.</param>
        private void OnChangedField(IDictionary<string, object> eventDictionary)
        {
            DataPayLoad payLoad = BuildDataPayload(eventDictionary);
            payLoad.Subsystem = DataSubSystem.VehicleSubsystem;
            ChangeFieldHandlerDo<IVehicleData> handlerDo = new ChangeFieldHandlerDo<IVehicleData>(EventManager,DataSubSystem.VehicleSubsystem);

            if (CurrentOperationalState == DataPayLoad.Type.Insert)
            {
                handlerDo.OnInsert(payLoad, eventDictionary);

            }
            else
            {
                handlerDo.OnUpdate(payLoad, eventDictionary);
            }
        }
        /// <summary>
        /// This program loads the data from the data values.
        /// </summary>
        /// <param name="primaryKeyValue">Primary Key.</param>
        /// <param name="isInsertion">Inserted key.</param>
        /// <returns></returns>
        private async Task<IVehicleData> LoadDataValue(string primaryKeyValue, bool isInsertion)
        {

            IVehicleData vehicle = null;
            if (isInsertion)
            {
                vehicle = _vehicleDataServices.GetNewVehicleDo(PrimaryKeyValue);
                if (vehicle != null)
                {
                    DataObject = vehicle;
                }
            }
            else
            {
                vehicle = await _vehicleDataServices.GetVehicleDo(primaryKeyValue);
                DataObject = vehicle;
                
            }
            return vehicle;
        }
        /// <summary>
        ///  assist owner
        /// </summary>
        public string AssistQueryOwner
        {
            get { return _assistQueryOwner; }
            set { _assistQueryOwner = value; }
        }
        
        /// <summary>
        ///  BuyerAssistQuery.
        /// </summary>
        public string BuyerAssistQuery
        {
            get { return GenericSql.ClientsSummaryQuery;  }
        }
        /// <summary>
        ///  name.
        /// </summary>
        public string LeasingSupplierQuery
        {
            get { return GenericSql.SupplierSummaryQuery; }
        }
            
            public ObservableCollection<PaymentFormDto> PaymentFormDto {
            get { return _paymentFormDto; }
            private set { _paymentFormDto = value; RaisePropertyChanged();}
        }

        public ObservableCollection<ClientsSummaryDto> ClientsDto
        {
            get { return _clientDto; }
            set
            {
                _clientDto = value;
                RaisePropertyChanged();
            }
            
        }
        /// <summary>
        ///  This method set the registration payload.
        /// </summary>
        /// <param name="payLoad"></param>
        protected override void SetRegistrationPayLoad(ref DataPayLoad payLoad)
        {
            payLoad.PayloadType = DataPayLoad.Type.RegistrationPayload;
            payLoad.Subsystem = DataSubSystem.VehicleSubsystem;
        }

        /// <summary>
        ///  This give usa the routing name of a vehicle module.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string GetRouteName(string name)
        {
            return "VehicleMasterModule." + name;
        }

        /// <summary>
        /// This adds a primary and a payload
        /// </summary>
        /// <param name="primaryKeyValue">PrimaryKey</param>
        /// <param name="payload">Data payload to be loaded</param>
        /// <param name="insertable">Is an insert operation</param>
        private void Init(string primaryKeyValue, DataPayLoad payload, bool insertable)
        {
            Stopwatch srStopwatch = new Stopwatch();
            srStopwatch.Start();
            if (payload.HasDataObject)
            {
                _vehicleDo = (IVehicleData) payload.DataObject;
              
                DataObject = _vehicleDo;
                if (_vehicleDo.MaintenanceHistory != null)
                {
                    MaintenanceCollection = new ObservableCollection<MaintainanceDto>(_vehicleDo.MaintenanceHistory);
                }
                RevisionObject = InitRevisionComposedFieldObjects();
             //  EventManager.SendMessage(UpperBarViewVehicleViewModel.Name, payload);
                ActiveSubSystem();
            }
            srStopwatch.Stop();
            var value = srStopwatch.ElapsedMilliseconds;

        }
        /// <summary>
        /// Primary key payload.
        /// </summary>
        /// <param name="primaryKeyValue">This deletes an item.</param>
        private void DeleteItem(string primaryKeyValue)
        {
            string primaryKey = primaryKeyValue;
            if (primaryKey == PrimaryKeyValue)
            {
                DataPayLoad dataPayload = new DataPayLoad();
                dataPayload.HasDataObject = true;
                dataPayload.PrimaryKeyValue = PrimaryKeyValue;
                _deleteNotifyTaskCompletion = NotifyTaskCompletion.Create<DataPayLoad>(HandleDeleteItem(dataPayload), _deleteEventHandler);
                
              
            }
        }
        
        /// <summary>
        /// Delete a commission agent.
        /// </summary>
        /// <param name="inDataPayLoad"></param>
        /// <returns></returns>
        private async Task<DataPayLoad> HandleDeleteItem(DataPayLoad inDataPayLoad)
        {
            IVehicleData vehicle = await _vehicleDataServices.GetVehicleDo(inDataPayLoad.PrimaryKeyValue);
            DataPayLoad payload = new DataPayLoad();
            if (vehicle.Valid)
            {
                bool returnValue = await _vehicleDataServices.DeleteVehicleDo(vehicle);
              
                if (returnValue)
                {
                    payload.Subsystem = DataSubSystem.VehicleSubsystem;
                    payload.PrimaryKeyValue = inDataPayLoad.PrimaryKeyValue;
                    payload.PayloadType = DataPayLoad.Type.Delete;
                    EventManager.NotifyToolBar(payload);
                    PrimaryKeyValue = "";
                    _vehicleDo = null;
                }
               // DeleteRegion(payload.PrimaryKeyValue);

            }
            return payload;
        }
    
        public ObservableCollection<CityDto> CityDto
        {
            get
            {
                return _cityDto;
            }
            set {
                _cityDto = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CityDto> RoadTaxesCityDto
        {
            get
            {
                return _roadTaxesCities;
            }
            set
            {
                _roadTaxesCities = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<CurrentSituationDto> CurrentSituationDto
        {
            get { return _situationDto; }
            set { _situationDto = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Other office 1 dto
        /// </summary>
        public ObservableCollection<CompanyDto> OtherOffice1Dto
        {
            get { return _otherOffice1Dto; }
            set { _otherOffice1Dto = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Other office 2 dto
        /// </summary>
        public ObservableCollection<CompanyDto> OtherOffice2Dto
        {
            get { return _otherOffice2Dto; }
            set { _otherOffice2Dto = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Other office 3 dto
        /// </summary>
        public ObservableCollection<CompanyDto> OtherOffice3Dto
        {
            get { return _otherOffice3Dto; }
            set { _otherOffice3Dto = value; RaisePropertyChanged(); }
        }
        /// <summary>
        ///  BrandDtos. Brand Dto.
        /// </summary>
        public IEnumerable<BrandVehicleDto> BrandDtos
        {
            get
            {
                return _brandDtos;
            }
            set
            {
                _brandDtos = value;
                RaisePropertyChanged("BrandDtos");
            }
        }
        /// <summary>
        ///  ModelDtos. Model vehicle dto.
        /// </summary>
        public IEnumerable<ModelVehicleDto> ModelDtos
        {
            get
            {
                return _modelDtos;
            }
            set
            {
                _modelDtos = value;
                RaisePropertyChanged("ModelDtos");
            }
        }
        /// <summary>
        ///  VehicleGroup Dtos.
        /// </summary>
        public IEnumerable<VehicleGroupDto> VehicleGroupDtos {
            get
            {
                return _vehicleGroupDtos;
            }
            set
            {
                _vehicleGroupDtos = value;
                RaisePropertyChanged("VehicleGroupDtos");
            }
        }
        /// <summary>
        /// ColorDtos. 
        /// </summary>
        public IEnumerable<ColorDto> ColorDtos
        {
            get
            {
                return _colorDto;
            }
            set
            {
                _colorDto = value;
                RaisePropertyChanged("ColorDtos");
            }
        }

        // move this to the upper interface.
        /// <summary>
        /// Incoming payload
        /// </summary>
        /// <param name="dataPayLoad">Payload to be used.</param>
        public void IncomingPayload(DataPayLoad dataPayLoad)
        {

            DataPayLoad payload = dataPayLoad;

            if (payload != null)
            {
                if (PrimaryKeyValue.Length == 0)
                {
                    PrimaryKeyValue = payload.PrimaryKeyValue;
                    string mailboxName = "Vehicles." + PrimaryKeyValue;
                    if (!string.IsNullOrEmpty(PrimaryKeyValue))
                    {
                        if (MailBoxHandler != null)
                        {
                            EventManager.RegisterMailBox(mailboxName, MailBoxHandler);
                        }
                    }
                }
                // here i can fix the primary key
                switch (payload.PayloadType)
                {
                    case DataPayLoad.Type.UpdateData:
                    {
                        if (payload.HasDataObject)
                        {
                          DataObject = payload.DataObject;
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
                            PrimaryKeyValue =
                                _vehicleDataServices.GetNewId();

                            CurrentOperationalState = DataPayLoad.Type.Insert;
                        }
                        Init(PrimaryKeyValue, payload, true);
                        break;
                    }
                    case DataPayLoad.Type.Delete:
                    {
                    
                            if (PrimaryKey == payload.PrimaryKey)
                            {
                                DeleteEventCleanup(payload.PrimaryKeyValue, PrimaryKeyValue, DataSubSystem.VehicleSubsystem, 
                                    MasterModuleConstants.VehiclesSystemName);
                                DeleteRegion(payload.PrimaryKeyValue);
                               
                            }
                        break;
                    }
                }
            }

        }

        internal override Task SetClientData(ClientSummaryExtended p, VisitsDto b)
        {
            throw new NotImplementedException();
        }

        internal override Task SetVisitContacts(ContactsDto p, VisitsDto visitsDto)
        {
            throw new NotImplementedException();
        }

        internal override Task SetBranchProvince(ProvinciaDto p, BranchesDto b)
        {
            throw new NotImplementedException();
        }

        internal override Task SetVisitReseller(ResellerDto param, VisitsDto b)
        {
            throw new NotImplementedException();
        }
    }
}
