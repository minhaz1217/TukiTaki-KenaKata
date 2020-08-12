using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.service.discounts
{
    class Discount : IDiscount
    {
        IDecoratorComponent Component;
        public Discount(IDecoratorComponent component)
        {
            this.Component = component;
        }
        public Discount() { }

        public virtual double GetPrice()
        {
            return this.Component.GetPrice();
        }
    }
}
