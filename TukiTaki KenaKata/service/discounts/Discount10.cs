using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.service.discounts
{
    class Discount10 : Discount
    {
        public Discount10(IDecoratorComponent component):base(component){
            //Console.WriteLine("Drop 10");
        }
        public override double GetPrice()
        {
            //Helper.MyPrint($"{base.GetSourcePrice()} {base.GetPrice() }");
            return base.GetPrice() - (base.GetSourcePrice() * .10);
        }
    }
}
