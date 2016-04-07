﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFuel.DataAccess;
using CarFuel.Models;

namespace CarFuel.Service
{
    public class CarService : ICarService
    {
        public ICarDb CarDb { get; }

        public CarService(ICarDb carDb)
        {
            if (carDb == null)
            {
                throw new ArgumentNullException(nameof(carDb));
            }
            CarDb = carDb;
        }

        public IEnumerable<Car> GetCarsByMember(Guid userId)
        {
            return CarDb.GetAll(c => c.OwnerId == userId);
        }

        public Car AddCar(Car car, Guid userId)
        {
            if (!CanAddMoreCars(userId))
                throw new OverQuotaException("Cannot Add more car");

            car.OwnerId = userId;
            return CarDb.Add(car);
        }

        public bool CanAddMoreCars(Guid userId)
        {
            return GetCarsByMember(userId).Count() < 2;
        }
    }
}
