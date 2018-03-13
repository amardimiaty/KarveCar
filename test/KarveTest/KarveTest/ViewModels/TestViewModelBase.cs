﻿using Moq;
using KarveCommonInterfaces;
using KarveCommon.Services;
using KarveDataServices;
using Prism.Regions;
using Microsoft.Practices.Unity;

namespace KarveTest.ViewModels
{
    /// <summary>
    ///  Base class for each view model test.
    ///  Each view model will override by defualt the part of dataservices
    /// </summary>
    internal class TestViewModelBase
    {
        /// <summary>
        ///  Here we have a list of mock object to be used in our view model test case.  
        ///  A mock objects are simulated objects that mimic the behavior of real objects in controlled ways. 
        ///  We create a mock objects to test the behavior of some other object.
        /// </summary>
        protected Mock<IConfigurationService> _mockConfigurationService = new Mock<IConfigurationService>();
        protected Mock<IDataServices> _mockDataServices = new Mock<IDataServices>();
        protected Mock<IEventManager> _mockEventManager = new Mock<IEventManager>();
        protected Mock<IRegionManager> _mockRegionManager = new Mock<IRegionManager>();
        protected Mock<IDialogService> _mockDialogService = new Mock<IDialogService>();
        protected Mock<UnityContainer> _mockUnityContainer = new Mock<UnityContainer>();
      
    }
}