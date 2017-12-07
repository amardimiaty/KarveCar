﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterModule.Interfaces;
using MasterModule.ViewModels;
using MasterModule.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace MasterModule
{
    public class MasterModule : IModule
    {
        private readonly IUnityContainer _container;
        private IRegionManager _regionManager;
        public const string NAME = "MasterModule";


        public MasterModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

       
        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<object, CommissionAgentInfoView>();
            _container.RegisterType<object, ProviderInfoView>(typeof(ProviderInfoView).FullName);
            _container.RegisterType<object, CommissionAgentControlView>("CommissionAgent");
            _container.RegisterType<object, ClientsControlView>("Clients");
            _container.RegisterType<object, ProvidersControl>("Suppliers");
            _container.RegisterType<object, VehiclesControlView>("Vehicles");
            _container.RegisterType<object, VehicleInfoView>(typeof(VehicleInfoView).FullName);
            _container.RegisterType<object, UpperBarViewModel>("UpperBarViewModel");
            


        }
        public void Initialize()
        {
            RegisterViewsAndServices();
        }

        
    }

}