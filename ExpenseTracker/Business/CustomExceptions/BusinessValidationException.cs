﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CustomExceptions
{
    public class BusinessValidationException : Exception
    {
        public BusinessValidationException(string message) : base(message) { }
    }
}
