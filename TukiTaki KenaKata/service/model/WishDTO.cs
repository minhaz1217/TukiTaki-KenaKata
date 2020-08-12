using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.persistant;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.model
{
    class WishDTO : Component
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Component> Items { get; set; }
        public WishDTO() {}
        public WishDTO(Guid id, string name, List<Component> items)
        {
            this.Id = id;
            this.Name = name;
            this.Items = items;
        }

        public override string ToString()
        {
            StringBuilder outputString = new StringBuilder();
            outputString.Append(Helper.MyOutputString(this.Id.ToString() +"\n", "y"));
            outputString.Append(Helper.MyOutputString(this.Name.ToString()+"\n", "c"));
            this.Items.ForEach( delegate(Component product)
                {
                    outputString.Append($"({product.ToString()})\n");
                });
            return outputString.ToString() ;
        }

        public void Display()
        {
            Console.WriteLine(Helper.MyOutputString(this.Id.ToString() + "", "y"));
            Console.WriteLine("Name: "+Helper.MyOutputString(this.Name.ToString() + "", "c"));
            //Helper.MyPrint(this.Items.Count.ToString(), "g");
            foreach(Component c in this.Items)
            {
                c.Display();
            }
            Console.WriteLine();
        }

        public double GetPrice()
        {
            double total = 0;
            foreach(Component i in this.Items)
            {
                total += i.GetPrice();
            }
            return total;
        }

        public double GetDiscount()
        {
            return 0;
        }
    }
}
