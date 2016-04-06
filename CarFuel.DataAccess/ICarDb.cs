using CarFuel.Models;
using System;
using System.Collections.Generic;
namespace CarFuel.DataAccess
{
    public interface ICarDb
    {
        IEnumerable<Car> GetAll(Func<Car, bool> preducate);
        Car Get(Guid Id);
        Car Add(Car item);
         
    }
}
