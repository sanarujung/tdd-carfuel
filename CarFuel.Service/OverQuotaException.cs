using System;

namespace CarFuel.Service
{
    public class OverQuotaException : BussinessException
    {
        public OverQuotaException()
        {

        }
        public OverQuotaException(string message) : base(message)
        {
        }
    }
}