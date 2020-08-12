using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.service.model
{
    interface IDecoratorComponent
    {
        double GetPrice();
        double GetSourcePrice();
    }
}
