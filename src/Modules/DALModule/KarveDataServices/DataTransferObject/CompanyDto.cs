﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarveDataServices.DataTransferObject
{
    public class CompanyDto
    {
        public string Code { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        public string CommercialName { get; set; }
        public string Poblacion { get; set; }
        public string Nif { get; set; }
    }
}
