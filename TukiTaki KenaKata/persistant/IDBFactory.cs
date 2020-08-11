using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant
{
    interface IDBFactory
    {
        IDBRepository GetCassandraDB();
        IDBRepository GetMongoDB();
    }
}
