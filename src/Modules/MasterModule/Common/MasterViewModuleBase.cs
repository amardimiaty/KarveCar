﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using KarveCommon.Generic;
using KarveCommon.Services;
using KarveControls.UIObjects;
using KarveDataServices;
using Prism.Regions;

namespace MasterModule.Common
{
    /// <summary>
    ///  Base class for the view model.
    /// </summary>
    public abstract  class MasterViewModuleBase : BindableBase, INavigationAware
    {
        protected const string Dataset = "dataset";
        protected const string Collection = "collection";
        /// <summary>
        ///  DataSet Assist for eache  info view modules
        /// </summary>
        protected DataSet AssistDataSet;
        /// <summary>
        ///  Current dataset info for the view mdouels
        /// </summary>
        protected DataSet CurrentDataSet;
        /// <summary>
        ///  list of view model queries.
        /// </summary>
        protected IDictionary<string, string> ViewModelQueries;
        protected string PrimaryKeyValue = "";

        protected INotifyTaskCompletion<DataSet> InitializationNotifier;
        protected INotifyTaskCompletion InitializationNotifierDo;

        protected ObservableCollection<IUiObject> UpperPartObservableCollection = new ObservableCollection<IUiObject>();

        protected ObservableCollection<IUiObject> MiddlePartObservableCollection =
            new ObservableCollection<IUiObject>();
        /// <summary>
        ///  Each viewmodel refelct different types following the paylod that it receives
        /// </summary>
        protected DataPayLoad.Type CurrentOperationalState;
        /// <summary>
        ///  Each view model receives messages in a mailbox. This allows any part of the system to comunicate with each view model.
        /// </summary>
        protected MailBoxMessageHandler MailBoxHandler;
        /// <summary>
        ///  The configuration service is a way to set/get configurartions
        /// </summary>
        protected IConfigurationService ConfigurationService;
        /// <summary>
        ///  This is a command for open a new window.
        /// </summary>
        protected ICommand OpenItemCommand;
        /// <summary>
        /// The event manager is responsabile to implement the communication between different view models
        /// </summary>
        protected IEventManager EventManager;
        /// <summary>
        ///  The data services allows any view model to communicate with the database via dataset or via dapper/dataobjects
        /// </summary>
        protected IDataServices DataServices;

        protected MailBoxMessageHandler MessageHandlerMailBox;
        /// <summary>
        /// PrimaryKey field used from all view models.
        /// </summary>
        protected string PrimaryKey = "";

        private int _notifyState;

        protected bool IsInsertion = false;


        /// <summary>
        /// Object to warrant the notifications.
        /// </summary>
        protected  object _notifyStateObject = new object();
        /// <summary>
        /// Ok.
        /// </summary>
        protected int NotifyState
        {
            set { _notifyState = 0; }
            get { return _notifyState; }
        }
        /// <summary>
        /// ViewModel base for the master registry
        /// </summary>
        /// <param name="configurationService">It needs the configuration service</param>
        /// <param name="eventManager">The event manager</param>
        /// <param name="services">The dataservices value</param>
        public MasterViewModuleBase(IConfigurationService configurationService,
            IEventManager eventManager,
            IDataServices services)
        {
            ConfigurationService = configurationService;
            EventManager = eventManager;
            DataServices = services;
            _notifyState = 0;
          //  OpenItemCommand = new DelegateCommand<object>(Ope);
        }

        /// <summary>
        ///  This value returns foreach database row a tuple containing the value of the name and the id.
        /// </summary>
        /// <param name="rowView"></param>
        /// <param name="nameColumn"></param>
        /// <param name="codeColumn"></param>
        /// <returns></returns>
        protected Tuple<string, string> ComputeIdName(DataRowView rowView, string nameColumn, string codeColumn)
        {
            DataRow row = rowView.Row;
            string name = row[nameColumn] as string;
            string commissionId = row[codeColumn] as string;
            Tuple<string, string> codeTuple = new Tuple<string, string>(commissionId, name);
            return codeTuple;
        }
        /// <summary>
        /// Extended Data Table
        /// </summary>
        protected DataTable ExtendedDataTable;

        /// <summary>
        ///  This starts the load of data from the lower data layer.
        /// </summary>
        public abstract void StartAndNotify();
        /// <summary>
        ///  This asks to the control view model to open a new item for insertion. 
        /// </summary>
        public abstract void NewItem();

        /// <summary>
        /// Query. Set the query from the ui.
        /// </summary>
        protected string Query { set; get; }
        /// <summary>
        /// This is the message handler.
        /// </summary>
        /// <param name="payLoad"></param>
        protected void MessageHandler(DataPayLoad payLoad)
        {
            if ((payLoad.PayloadType == DataPayLoad.Type.UpdateView) && (NotifyState == 0))
            {
                lock (_notifyStateObject)
                {
                    _notifyState = 1;
                }
                StartAndNotify();
            }
            if (payLoad.PayloadType == DataPayLoad.Type.Delete)
            {
                // forward data to the current payload.
                EventManager.NotifyObserverSubsystem(payLoad.SubsystemName, payLoad);
            }
            if (payLoad.PayloadType == DataPayLoad.Type.Insert)
            {
                NewItem();
            }
        }

        /// <summary>
        /// Init a primary key.
        /// </summary>
        /// <param name="primaryKey">Primary Key</param>
        /// <param name="value">Insert Value.</param>
        protected void Init(string primaryKey, bool value)
        {
            // arriva el payload.
            PrimaryKeyValue = primaryKey;
            IsInsertion = value;
            if (value)
            {
                StartAndNotify();
            }
        }

        /// <summary>
        ///  This is shall be used by the different view model to set the value of the table initialzed;
        /// </summary>
        /// <param name="table"></param>
        protected abstract void SetTable(DataTable table);
        

        /// <summary>
        ///  This shall be used by different view models to set the registration payload.
        /// </summary>
        /// <param name="payLoad"> This is the payalod to be be registered</param>
        protected abstract void SetRegistrationPayLoad(ref DataPayLoad payLoad);

        /// <summary>
        /// This is the initialization notifier on property changed data object
        /// </summary>
        /// <param name="sender">This is the sender</param>
        /// <param name="propertyChangedEventArgs">This are the parameters.</param>
        protected void InitializationNotifierOnPropertyChangedDo(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {

            string propertyName = propertyChangedEventArgs.PropertyName;

            if (propertyName.Equals("Status"))
            {
                if (InitializationNotifierDo.IsSuccessfullyCompleted)
                {
                    var result = InitializationNotifier.Task.Result;
                    SetDataObject(result);
                    lock (_notifyStateObject)
                    {
                        NotifyState = 0;
                    }
                    DataPayLoad payLoad = new DataPayLoad
                    {
                        HasDataObject = true,
                        DataObject = result
                    };
                    SetRegistrationPayLoad(ref payLoad);
                    EventManager.NotifyToolBar(payLoad);
                }
            }
        }


        /// <summary>
        /// SetDataObject. 
        /// </summary>
        /// <param name="result">DataObject to be set.</param>
        protected abstract void SetDataObject(object result);

        protected void RegisterToolBar()
        {
            // each module notify the toolbar.
            DataPayLoad payLoad = new DataPayLoad();
            SetRegistrationPayLoad(ref payLoad);
            EventManager.NotifyToolBar(payLoad);
        }
    /// <summary>
    /// This initializatin notifier for the data object
    /// </summary>
    /// <param name="sender">The sender is</param>
    /// <param name="propertyChangedEventArgs"></param>
    protected void InitializationNotifierOnPropertyChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {

             string propertyName = propertyChangedEventArgs.PropertyName;

            if (propertyName.Equals("Status"))
            {
                if (InitializationNotifier.IsSuccessfullyCompleted)
                {
                    SetTable(InitializationNotifier.Task.Result.Tables[0]);
                    /* ok the first load has been successfully i can do the second one while the UI Thread is refreshing*/
                    lock (_notifyStateObject)
                    {
                        NotifyState = 0;
                    }
                }
                

            }
        }
      
       
        // TODO Fix excessive depth of code.
        protected void
            UpdateSource(DataSet dataSetAssistant, string primaryKey,
                ref ObservableCollection<IUiObject> collection)
        {

            if ((dataSetAssistant != null) && (dataSetAssistant.Tables.Count > 0))
            {

                UiDualDfSearchTextObject update = null;
                foreach (IUiObject obj in collection)
                {
                    if (obj is UiDualDfSearchTextObject)
                    {
                        update = (UiDualDfSearchTextObject)obj;
                        bool isValidKey = ((update.DataFieldFirst == primaryKey) ||
                                           (update.AssistDataFieldFirst == primaryKey));
                        if (isValidKey)
                        {
                            if (dataSetAssistant.Tables.Count > 0)
                            {
                                dataSetAssistant.Tables[0].NewRow();

                                update.SourceView = dataSetAssistant.Tables[0];

                            }
                            break;
                        }
                    }


                    if (obj is UiMultipleDfObject)
                    {
                        if (dataSetAssistant.Tables.Count > 0)
                        {
                            UiMultipleDfObject tmp = (UiMultipleDfObject)obj;
                            tmp.SetSourceView(dataSetAssistant.Tables[0], dataSetAssistant.Tables[0].TableName);
                        }
                    }
                }



            }
        }
        /// <summary>
        /// Set the source item in the components.
        /// </summary>
        /// <param name="set">DataSet. This set is sending </param>
        /// <param name="uiDfObjects">IUiObject. Ok.</param>
        protected void SetItemSourceTable(DataSet set, ref ObservableCollection<IUiObject> uiDfObjects)
        {
            IDictionary<string, DataTable> tablesByNameDictionary = BuildDictionaryFromDataSet(set);
            ObservableCollection<IUiObject> outCollection = new ObservableCollection<IUiObject>();
            if (uiDfObjects.Count == 0)
                return;
            // now i have to check the uiDfObjects
            for(int i = 0; i < uiDfObjects.Count;++i)
            {
                var uiObject = uiDfObjects[i];
                if (tablesByNameDictionary.ContainsKey(uiObject.TableName))
                {
                    DataTable table = null;
                    table = tablesByNameDictionary[uiObject.TableName];
                    uiObject.ItemSource = table;
                    if (uiObject is UiDoubleDfObject)
                    {
                        UiDoubleDfObject valDoubleDfObject = (UiDoubleDfObject) uiObject;
                        valDoubleDfObject.ItemSource = table;
                        valDoubleDfObject.ItemSourceRight = table;
                        
                    }
                }
                if (uiObject is UiMultipleDfObject)
                {
                    UiMultipleDfObject box = (UiMultipleDfObject) uiObject;
                    IList<string> tableNames = box.Tables;
                    if (tableNames != null)
                    {
                        foreach (string tableName in tableNames)
                        {
                            box.SetItemSource(tablesByNameDictionary[tableName], tableName);
                        }
                    }
                }
                uiDfObjects.RemoveAt(i);
                uiDfObjects.Insert(i, uiObject);

            }
           
        }
        /// <summary>
        ///  Navigation support.
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        /// <summary>
        ///  Navigation support 
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        /// <summary>
        /// This is the navigation context.
        /// </summary>
        /// <param name="navigationContext">Naviagation Context.</param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
        /// <summary>
        /// GetRouteName
        /// </summary>
        /// <param name="name">This is the route name.</param>
        /// <returns></returns>
        protected  abstract string GetRouteName(string name);
        /// <summary>
        ///  It detected the payload name following the data occurred.
        /// </summary>
        /// <param name="name">View name</param>
        /// <param name="completeSummary">List of loaded dataset to be passed.</param>
        /// <param name="queries">Dictionary of the queries indexed by table.</param>
        /// <returns></returns>
        protected DataPayLoad BuildShowPayLoad(string name, IList<DataSet> completeSummary, IDictionary<string, string> queries = null)
        {
            DataPayLoad currentPayload = new DataPayLoad();
            string routedName = GetRouteName(name);
            currentPayload.PayloadType = DataPayLoad.Type.Show;
            currentPayload.Registration = routedName;
            currentPayload.SetList = completeSummary;
            currentPayload.HasDataSetList = true;
            if (queries != null)
            {
                currentPayload.Queries = queries;
            }
            return currentPayload;
        }
        /// <summary>
        /// This function shows a payload data object
        /// </summary>
        /// <param name="name">Name of the module</param>
        /// <returns></returns>
        protected DataPayLoad BuildShowPayLoadDo(string name)
        {
            DataPayLoad currentPayload = new DataPayLoad();
            string routedName = GetRouteName(name);
            currentPayload.PayloadType = DataPayLoad.Type.Show;
            currentPayload.Registration = routedName;
            return currentPayload;

        }
        /// <summary>
        /// Makes a payload with a data object or a data transfer object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Name of the form/tab</param>
        /// <param name="Object">Object to be sent to the data</param>
        /// <param name="queries">Queries optional to be sent to the info view.</param>
        /// <returns></returns>
        protected DataPayLoad BuildShowPayLoadDo<T>(string name, T Object, IDictionary<string, string> queries = null)
        {
            DataPayLoad currentPayload = new DataPayLoad();
            // name that it is give from the subclass, it may be a master
            string routedName = GetRouteName(name);
            currentPayload.PayloadType = DataPayLoad.Type.Show;
            currentPayload.Registration = routedName;
            currentPayload.HasDataObject = true;
            currentPayload.Subsystem = DataSubSystem.CommissionAgentSubystem;
            currentPayload.DataObject = Object;
            if (queries != null)
            {
                currentPayload.Queries = queries;
            }
            return currentPayload;
        }

        
        /// <summary>
        ///  This function merges the new table to the current dataset
        /// </summary>
        /// <param name="table">Table to be merged</param>
        /// <param name="currentDataSet">Current dataset to be merged</param>
        protected void MergeTableChanged(DataTable table, ref DataSet currentDataSet)
        {

            string tableName = table.TableName;
            bool foundTable = false;
            if (currentDataSet != null)
            {
                foreach (DataTable currentTable in currentDataSet.Tables)
                {
                    if (currentTable.TableName == tableName)
                    {
                        foundTable = true;
                        break;
                    }
                }
                if (foundTable)
                {
                    currentDataSet.Tables[tableName].Merge(table);
                }
            }
        }
        /// <summary>
        ///  This methods has the resposability to lookup the helpers and set in the component collection
        /// </summary>
        protected async Task<IDictionary<string, object>> SetComponentHelpers(DataSet currentSet, ObservableCollection<IUiObject> currentComponents)
        {
            IDictionary<string, string> assistQueries = new Dictionary<string, string>();
            FillViewModelAssistantQueries(currentComponents, currentSet, ref assistQueries);
            DataSet assitDataSet = await DataServices.GetHelperDataServices().GetAsyncHelper(assistQueries);
            SetSourceViewTable(assitDataSet, ref currentComponents);
            IDictionary<string, object> currentDictionary = new Dictionary<string, object>
            {
                [Dataset] = assitDataSet,
                [Collection] = currentComponents
            };
            return currentDictionary;
        }

        
        /// <summary>
        ///  This merge list merges all the things to an unique collection. TODO: Move to a common value
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>

        protected ObservableCollection<IUiObject> MergeList(IList<ObservableCollection<IUiObject>> values)
        {
            ObservableCollection<IUiObject> obs = new ObservableCollection<IUiObject>();

            foreach (ObservableCollection<IUiObject> current in values)
            {
                if (current != null)
                {
                    foreach (IUiObject o in current)
                    {
                        if (o != null)
                        {
                            if (!string.IsNullOrEmpty(o.TableName))
                            {
                                obs.Add(o);
                            }
                        }
                    }
                }
            }
            return obs;
        }
        /// <summary>
        ///  This function foreach magnifier assigns an assist table
        /// </summary>
        /// <param name="sourceView">DataSet for the magnifier</param>
        /// <param name="uiDfObjects">List of user interface objects</param>
        protected void SetSourceViewTable(DataSet sourceView, ref ObservableCollection<IUiObject> uiDfObjects)
        {
            IDictionary<string, DataTable> tablesByNameDictionary = BuildDictionaryFromDataSet(sourceView);
            if (uiDfObjects.Count == 0)
                return;
            foreach (IUiObject uiObject in uiDfObjects)
            {

                if (uiObject is UiMultipleDfObject)
                {

                    UiMultipleDfObject box = (UiMultipleDfObject)uiObject;
                    IList<string> assistTables = box.AssistTables;
                    if (assistTables != null)
                    {
                        foreach (string tableName in assistTables)
                        {
                            if (tablesByNameDictionary.ContainsKey(tableName))
                            {
                                box.SetSourceView(tablesByNameDictionary[tableName], tableName);
                            }
                        }
                    }
                }
                if (uiObject is UiDualDfSearchTextObject)
                {
                    UiDualDfSearchTextObject box = (UiDualDfSearchTextObject)uiObject;
                    if (tablesByNameDictionary.ContainsKey(box.AssistTableName))
                    {
                        box.SourceView = tablesByNameDictionary[box.AssistTableName];
                    }
                }

            }
        }

       // protected string MasterModuleErrorMessage { set; get; }
        /// <summary>
        ///  This builds a dictionary of the tables that are present in a dataset.
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        /// TODO move to a common file.
        protected IDictionary<string, DataTable> BuildDictionaryFromDataSet(DataSet set)
        {
            IDictionary<string, DataTable> tablesByNameDictionary = new Dictionary<string, DataTable>();
            foreach (DataTable t in set.Tables)
            {
                tablesByNameDictionary.Add(t.TableName, t);
            }
            return tablesByNameDictionary;

        }

        /// <summary>
        /// FillViewModelAssistantQueries.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="itemSource"></param>
        /// <param name="assistantQueries"></param>
        protected void FillViewModelAssistantQueries(ObservableCollection<IUiObject> collection,
            DataSet itemSource,
            ref IDictionary<string, string> assistantQueries)
        {
            foreach (IUiObject nameObject in collection)
            {
                if (!string.IsNullOrEmpty(nameObject.TableName))
                {
                    if (nameObject is UiDualDfSearchTextObject)
                    {
                        UiDualDfSearchTextObject dualDfSearch = (UiDualDfSearchTextObject)nameObject;
                        dualDfSearch.ComputeAssistantRefQuery(itemSource);
                        if (!string.IsNullOrEmpty(dualDfSearch.AssistRefQuery))
                        {
                            if (!assistantQueries.ContainsKey(dualDfSearch.AssistTableName))
                            {
                                assistantQueries.Add(dualDfSearch.AssistTableName, dualDfSearch.AssistRefQuery);
                            }
                        }
                    }
                    if (nameObject is UiMultipleDfObject)
                    {
                        UiMultipleDfObject multipleDfObject = (UiMultipleDfObject)nameObject;
                        IList<IUiObject> listOfSearchTextObjects =
                            multipleDfObject.FindObjects(UiMultipleDfObject.DfKind.UiDualDfSearch);
                        foreach (var currentUiObject in listOfSearchTextObjects)
                        {
                            UiDualDfSearchTextObject dualDfSearch = (UiDualDfSearchTextObject)currentUiObject;
                            dualDfSearch.ComputeAssistantRefQuery(itemSource);
                            if (!string.IsNullOrEmpty(dualDfSearch.AssistRefQuery))
                            {
                                if (!assistantQueries.ContainsKey(dualDfSearch.AssistTableName))
                                {
                                    assistantQueries.Add(dualDfSearch.AssistTableName, dualDfSearch.AssistRefQuery);
                                }
                            }
                        }
                    }
                }
            }
        }
        
    }
}