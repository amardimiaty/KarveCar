﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DataAccessLayer.DataObjects;
using KarveCommon.Services;
using KarveDataServices;
using Prism.Commands;
using Prism.Mvvm;
using AutoMapper;
using KarveCommon.Generic;
using KarveDataServices.DataObjects;
using KarveDataServices.DataTransferObject;
using MasterModule.Common;

namespace MasterModule.ViewModels
{
    /// <summary>
    ///  UpperBarView model.
    /// </summary>
    public class UpperBarViewModel : UpperBarViewModelBase, IDisposable
    {
        public const string Name = "MasterModule.UpperBarViewModel";
        private IEnumerable<TIPOCOMI> _tipocomis = new List<TIPOCOMI>();
        private string _pathType = "";
        private string _pathPerson = "";
        private string _labelType = "";
        private string _dataFieldSearch = "";
        private string _assistDataFieldFirst = "";
        private string _assistDataFieldSecond = "";
        private string _assistTable = "";

        /// <summary>
        /// This is the upperBarView that it can be customized as we wish
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="services"></param>
        public UpperBarViewModel(IEventManager manager,
            IDataServices services) : base(manager, services)
        {

            ChangedItem = new DelegateCommand<object>(OnChangedItem);
            AssistCommand = new DelegateCommand<object>(OnAssistCommand);
            MailBoxHandler += MailBoxHandlerMethod;
            EventManager.RegisterMailBox(Name, MailBoxHandler);
            // initialize the mapper to the automap for the upper view model.
            InitMapping();
        }

        /// <summary>
        /// Init the mapping.
        /// </summary>
        private void InitMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TIPOCOMI, CommissionTypeDto>().ConvertUsing(src =>
                {
                    var tipoComi = new CommissionTypeDto();
                    tipoComi.Codigo = src.NUM_TICOMI;
                    tipoComi.Nombre = src.NOMBRE;
                    return tipoComi;
                });
                cfg.CreateMap<TIPOPROVE, SupplierTypeDto>().ConvertUsing(src =>
                {
                    var tipoComi = new SupplierTypeDto();
                    tipoComi.Codigo = src.NUM_TIPROVE;
                    tipoComi.Nombre = src.NOMBRE;
                    return tipoComi;
                });

            });            
            _status = UpperBarViewModelState.Init;
        }
        /// <summary>
        ///  PathType
        /// </summary>
        public string PathCode
        {
            set { _pathType = value;  RaisePropertyChanged();}
            get { return _pathType; }
        }

        public string PathPerson
        {
            set { _pathPerson = value; RaisePropertyChanged();}
            get { return _pathPerson; }
        }

        public string LabelTypeSearch {
            get { return _labelType; }
            set { _labelType = value;
                RaisePropertyChanged();
            }
        }

        public string DataFieldSearch
        {
            get { return _dataFieldSearch; }
            set { _dataFieldSearch = value; RaisePropertyChanged();}
        }
   

        public string AssistDataFieldFirst {
            get
            {
                return _assistDataFieldFirst; 
                
            }
            set { _assistDataFieldFirst = value; }
          }
        public string AssistDataFieldSecond
        {
            get
            {
                return _assistDataFieldSecond;

            }
            set { _assistDataFieldSecond = value; RaisePropertyChanged(); }
        }

        public string AssistTable
        {
            get { return _assistTable; }
            set { _assistTable = value; RaisePropertyChanged(); }
        }

        /// <summary>
        ///  Each view model has a correct mailbox handler to receive the data form other forms.
        /// </summary>
        /// <param name="payLoad"></param>
        private void MailBoxHandlerMethod(DataPayLoad payLoad)
        {
            if (payLoad.HasDataObject)
            {
                DataObject = payLoad.DataObject;
                _subsystem = payLoad.Subsystem;
                if (DataObject is ICommissionAgent)
                {
                    NotifyTaskCompletion.Create(HandleCommission(DataObject));
                }
                else if (DataObject is ISupplierData)
                {
                    NotifyTaskCompletion.Create(HandleSupplier(DataObject));
                }
            }
        }
        /// <summary>
        ///  This is a supplier handler.
        /// </summary>
        /// <param name="dataObject">Data Object as supplier</param>
        /// <returns></returns>
        private async Task HandleSupplier(object dataObject)
        {
            ISupplierData supplier = dataObject as ISupplierData;
            DataObject = supplier;
            PathCode = "NUM_PROVEE";
            PathPerson = "COMERCIAL";
            LabelTypeSearch = "Tipo.Proveedor";
            DataFieldSearch = "TIPOPROVE";
            AssistDataFieldFirst = "NUM_TIPROVE";
            AssistDataFieldSecond = "NOMBRE";
            AssistTable = "TIPOPROVE";
            var supplierValue = supplier.Type.FirstOrDefault().Number;
            string value = string.Format("SELECT NUM_TIPROVE, NOMBRE FROM TIPOPROVE WHERE NUM_TIPROVE='{0}'", supplierValue);
            IHelperDataServices helperDataServices = DataServices.GetHelperDataServices();
            var supplierType = await helperDataServices.GetAsyncHelper<TIPOPROVE>(value);
            SourceView = Mapper.Map<IEnumerable<TIPOPROVE>, IEnumerable<SupplierTypeDto>>(supplierType);
            
        }
        /// <summary>
        ///  This is a commission handler
        /// </summary>
        /// <param name="dataObject">Commission handler as commision</param>
        /// <returns></returns>
        private async Task HandleCommission(object dataObject)
        {
            ICommissionAgent agent = dataObject as ICommissionAgent;
            IHelperDataServices helperDataServices = DataServices.GetHelperDataServices();
            DataObject = agent;
            PathCode = "NUM_COMI";
            PathPerson = "NOMBRE";
            LabelTypeSearch = "Tipo.Comm.";
            DataFieldSearch = "TIPOCOMI";
            AssistDataFieldFirst = "NUM_TICOMI";
            AssistDataFieldSecond = "NOMBRE";
            AssistTable = "TIPOCOMI";
            var agentValue = agent.CommisionTypeDto.FirstOrDefault().Codigo;
            if (agentValue != null)
            {

                string value = string.Format("SELECT NUM_TICOMI, NOMBRE FROM TIPOCOMI WHERE NUM_TICOMI='{0}'",agentValue);
                var tipoComi = await helperDataServices.GetAsyncHelper<TIPOCOMI>(value);
                SourceView = Mapper.Map<IEnumerable<TIPOCOMI>, IEnumerable<CommissionTypeDto>>(tipoComi);
            }
  
        }
     

        private async void OnAssistCommand(object assistData)
        {
                IHelperDataServices helperDataServices = DataServices.GetHelperDataServices();
                IDictionary<string, string> currentData = assistData as Dictionary<string, string>;
                if (currentData != null)
                {
                    string assistQuery = currentData[AssistQuery] as string;
                    // TODO: replace conditional with polymorphism. Introduce an assistSmasher delegate.
                     if (assistQuery.Contains("TIPOCOMI"))
                    {
                        var tipoComi = await helperDataServices.GetAsyncHelper<TIPOCOMI>(assistQuery);
                        SourceView = Mapper.Map<IEnumerable<TIPOCOMI>, IEnumerable<CommissionTypeDto>>(tipoComi);
                    }
                    else if (assistQuery.Contains("TIPOPROVE"))
                    {
                        var tipoProve = await helperDataServices.GetAsyncHelper<TIPOPROVE>(assistQuery);
                        SourceView = Mapper.Map<IEnumerable<TIPOPROVE>, IEnumerable<SupplierTypeDto>>(tipoProve);
                }
                    _status = UpperBarViewModelState.Loaded;
                }
            
        }
        /// <summary>
        ///  This dispose the mailbox subscription.
        /// </summary>
        public void Dispose()
        {
            EventManager.DeleteMailBoxSubscription(UpperBarViewModel.Name);
        }
    }
}
