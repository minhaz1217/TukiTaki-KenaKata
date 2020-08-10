using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant.model
{
    class Wish
    {
        public const string COL_ID = "id";
        public const string COL_NAME = "name";

        string id;
        string name;
        public Wish(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
