using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant.model
{
    class Wish
    {
        public const string COL_ID = "id";
        public const string COL_NAME = "name";

        public string Id { get; set; }
        public string Name { get; set; }
        public Wish(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
