using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.presentation
{
    interface IProductView
    {
        void ShowAllProductsView();
        void ViewSingleProduct();
        void CreateProduct();
        void UpdateProduct();
        void DeleteProduct();
    }
}
