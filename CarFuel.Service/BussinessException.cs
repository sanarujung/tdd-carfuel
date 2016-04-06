using System;

namespace CarFuel.Service
{
    public class BussinessException : Exception
    {
        public Guid? UserId { get; set; }
        public BussinessException()
        {

        }

        public BussinessException(string message) : base(message)
        {

        }
    }
}