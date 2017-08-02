﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientesFormaDeCobros.ChargeClients.ValidationRules
{
    public class NumberValidationRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string currentValue = value as string;
            Int64 codeNumber;
            if (!Int64.TryParse(currentValue, out codeNumber))
            {
                return new ValidationResult(false, null);
            }
            return new ValidationResult(true, null);
        }
    }
}
