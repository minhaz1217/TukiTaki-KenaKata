﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant
{
    interface IDBRepository: IProductRepository, IWishRepository, IWishListRepository
    {
    }
}
