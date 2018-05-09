﻿using System;
using KarveCommon.Services;
using NUnit.Framework;
using KarveDataServices.DataTransferObject;
using MasterModule.Common;

namespace KarveTest.Common
{
    [TestFixture]
    public class TestPayLoadInterpeter 
    {

        /// <summary>
        ///  The payload should call initialization if called with success.
        /// </summary>
        [Test]
        public void Should_Call_AnInitPayload()
        {
            // arrange
            var initValue = 0;
            var cleanUpValue = 0;

            var payLoad = new DataPayLoad
            {
                DataObject = new OfficeDtos(),
                PrimaryKeyValue = "3893893",
                Subsystem = DataSubSystem.OfficeSubsystem,
                SubsystemName = MasterModuleConstants.OfficeSubSytemName,
                PayloadType = DataPayLoad.Type.Show
            };
            var parser = new PayloadInterpeter<OfficeDtos>
            {
                Init = (value, packet, insertion) => { initValue++; },
                CleanUp = (key, system, name) => { cleanUpValue++; }
            };
            // act
            parser.ActionOnPayload(payLoad, "3893893", "838393", DataSubSystem.OfficeSubsystem, MasterModuleConstants.OfficeSubSytemName);
            // assert.
            Assert.AreEqual(initValue, 1);
            Assert.AreEqual(cleanUpValue, 0);
        }
        /// <summary>
        ///  The Payload interpeter should discard invalid payloads.
        /// </summary>
        [Test]
        public void Should_Discard_AnInvalidPayload()
        {
            var initValue = 0;
            var cleanUpValue = 0;

            DataPayLoad payLoad = new NullDataPayload();
            var parser = new PayloadInterpeter<OfficeDtos>
            {
                Init = (value, packet, insertion) => { initValue++; },
                CleanUp = (key, system, name) => { cleanUpValue++; }
            };

            // act
            parser.ActionOnPayload(payLoad, "3893893", "838393", DataSubSystem.OfficeSubsystem, MasterModuleConstants.OfficeSubSytemName);
            // assert.
            Assert.AreEqual(initValue, 1);
            Assert.AreEqual(cleanUpValue, 0);
        }

        /// <summary>
        ///  In this case the Payload interpeter should delete an invalid payload.
        /// </summary>
        [Test]
        public void Should_Call_ADeletePayload()
        {
            var initValue = 0;
            var cleanUpValue = 0;
            var payLoad = new DataPayLoad
            {
                DataObject = new OfficeDtos(),
                PrimaryKeyValue = "3893893",
                Subsystem = DataSubSystem.OfficeSubsystem,
                SubsystemName = MasterModuleConstants.OfficeSubSytemName,
                PayloadType = DataPayLoad.Type.Delete
            };
            var parser = new PayloadInterpeter<OfficeDtos>
            {
                Init = (value, packet, insertion) => { initValue++; },
                CleanUp = (key, system, name) => { cleanUpValue++; }
            };
            // act 
            parser.ActionOnPayload(payLoad, "3893893", "838393", DataSubSystem.OfficeSubsystem, MasterModuleConstants.OfficeSubSytemName);
            // assert.
            Assert.AreEqual(initValue, 1);
            Assert.AreEqual(cleanUpValue, 0);
        }
    }
}