﻿using System;

namespace HomerNCalc
{
    public class EvaluationException : ApplicationException
    {
        public EvaluationException(string message)
            : base(message)
        {
        }

        public EvaluationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
