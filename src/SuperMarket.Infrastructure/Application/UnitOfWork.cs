﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Infrastructure.Application
{
    public interface UnitOfWork
    {
        void Commit();
    }
}
