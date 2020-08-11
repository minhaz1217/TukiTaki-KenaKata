using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
namespace TukiTaki_KenaKata.persistant
{
    class DBFactory : IDBFactory
    {
        public IDBRepository GetCassandraDB()
        {
            return CassandraDBRepository.Instance();
        }

        public IDBRepository GetMongoDB()
        {
            //return MongoDBRepository.Instance();

            throw new NotImplementedException();
        }
    }
}
