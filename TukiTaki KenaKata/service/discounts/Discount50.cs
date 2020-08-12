using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.service.discounts
{
    class Discount50 : IDiscount
    {
        private IDecoratorComponent Component = null;
        public Discount50(IDecoratorComponent component)
        {
            this.Component = component;
        }
        public double GetPrice()
        {
            Helper.MyPrint($"Discount10 {this.Component.GetPrice()}", "g");
            return this.Component.GetPrice() * .50;
        }
    }
}
