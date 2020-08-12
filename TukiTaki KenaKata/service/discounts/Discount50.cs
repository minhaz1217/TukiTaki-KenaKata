using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.service.discounts
{
    class Discount50 : Discount
    {
        public Discount50(IDecoratorComponent component) : base(component)
        {
            //Console.WriteLine("Drop 50");
        }
        public override double GetPrice()
        {
            //Helper.MyPrint($"Discount10 {this.Component.GetPrice()}", "g");
            return base.GetPrice() - (base.GetSourcePrice()*.50);
        }
    }
}
