﻿using System;
using System.Data;
using System.Threading.Tasks;
using Apache.Ibatis.DataMapper;
using Apache.Ibatis.DataMapper.Session;

namespace DataAccessLayer
{
    internal class QueryAsyncForObjectCommandRetValue<T> : IMapperCommand
    {
        private string v1;
        private string v2;

        public QueryAsyncForObjectCommandRetValue(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public DataMapper Mapper { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISessionScope Scope { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<DataTable> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}