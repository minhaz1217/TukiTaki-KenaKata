using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.service.model
{
    interface Component: IDecoratorComponent
    {
        void Display();
    }
}
