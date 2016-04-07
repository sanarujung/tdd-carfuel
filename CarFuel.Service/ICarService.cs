using System;
using System.Collections.Generic;
using CarFuel.Models;

namespace CarFuel.Service
{
    public interface ICarService
    {
        Car AddCar(Car car, Guid userId);
        bool CanAddMoreCars(Guid userId);
        IEnumerable<Car> GetCarsByMember(Guid userId);
    }
}