using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.service.mapper
{
    class DBModelToServiceModelMapper
    {
        public static ProductDTO ProductMapper()
        {
            return new ProductDTO();
        }

    }
}
