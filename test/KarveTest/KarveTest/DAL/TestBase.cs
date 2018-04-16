﻿using DataAccessLayer.SQL;
using KarveCar.Logic.Generic;
using KarveCommon.Services;
using KarveDataServices;

namespace KarveTest.DAL
{
    public class TestBase
    {
        protected const string TestConnectionString =
            "EngineName=DBRENT_NET16;DataBaseName=DBRENT_NET16;Uid=cv;Pwd=1929;Host=172.26.0.45";
        
        public IConfigurationService SetupConfigurationService()
        {
            IConfigurationService configurationService = new ConfigurationService();
            return configurationService;
        }

        public ISqlExecutor SetupSqlQueryExecutor()
        {
            ISqlExecutor executor = new OleDbExecutor(TestConnectionString);
            return executor;
        }
        public ISqlExecutor SetupSqlOdbcQueryExecutor()
        {
            ISqlExecutor executor = new OdbcExecutor(TestConnectionString);
            return executor;
        }

    }
}