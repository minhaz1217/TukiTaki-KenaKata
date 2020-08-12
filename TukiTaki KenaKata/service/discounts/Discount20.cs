using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.service.discounts
{
    class Discount20 : IDiscount
    {
        private IDecoratorComponent Component = null;
        public Discount20(IDecoratorComponent component)
        {
            this.Component = component;
        }
        public double GetPrice()
        {
            Helper.MyPrint($"Discount20 {this.Component.GetPrice()}", "g");
            return this.Component.GetPrice() * .80;
        }
    }
}
