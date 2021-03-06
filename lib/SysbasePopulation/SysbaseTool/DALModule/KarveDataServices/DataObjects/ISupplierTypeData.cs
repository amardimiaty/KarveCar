﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarveDataServices.DataObjects
{
    public interface ISupplierTypeData
    {
        string User { get; set; }
        string Account { get; set; }
        string LastModification { get; set; }
        string Name { get; set; }
        short Number { get; set; }
    }
}
