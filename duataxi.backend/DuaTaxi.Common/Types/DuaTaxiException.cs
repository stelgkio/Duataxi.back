using System;

namespace DuaTaxi.Common.Types
{
    public class DuaTaxiException : Exception
    {
        public string Code { get; }

        public DuaTaxiException()
        {
        }

        public DuaTaxiException(string code)
        {
            Code = code;
        }

        public DuaTaxiException(string message, params object[] args) 
            : this(string.Empty, message, args)
        {
        }

        public DuaTaxiException(string code, string message, params object[] args) 
            : this(null, code, message, args)
        {
        }

        public DuaTaxiException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public DuaTaxiException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }        
    }
}