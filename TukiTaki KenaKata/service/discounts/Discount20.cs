using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.service.discounts
{
    class Discount20 : Discount
    {
        public Discount20(IDecoratorComponent component) : base(component) {
            Console.WriteLine("Drop 20");
        }
        public override double GetPrice()
        {
            //Helper.MyPrint($"Discount20 {this.Component.GetPrice()}", "g");
            return base.GetPrice() * .80;
        }
    }
}
