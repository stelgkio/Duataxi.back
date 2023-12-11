using DuaTaxi.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuaTaxi.Common.WebApiClient
{
    public class ValidationException : Exception
    {

        public BaseEntity ErrorEntity { get; private set; }

        public ValidationException(BaseEntity errorEntity)
        {
            ErrorEntity = errorEntity;
        }

    }
}
