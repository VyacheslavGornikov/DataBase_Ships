﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class ShipException : ApplicationException // Пользовательский класс исключений 
    {
        public ShipException(string message) : base(message) { }        
    }
}
