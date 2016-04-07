using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFuel.DataAccess;
using CarFuel.Models;
using CarFuel.Service;
using Xunit;
using Should;
using Xunit.Abstractions;
using Moq;

namespace CarFuel.Facts.Services
{
    public class CarServiceFacts
    {
        public class SharedService
        {
            public CarService CarService { get; set; }
            public FakeCarDb Db { get; set; }
            public SharedService()
            {
                Db = new FakeCarDb();
                CarService = new CarService(Db);
            }
        }

        [CollectionDefinition("collection1")]
        public class CarserviceFactCollection : ICollectionFixture<SharedService>
        {
            // no code
        }

        [Collection("collection1")]
        public class AddCarMethod
        {
            //private ICarDb db;
            private CarService s;
            private FakeCarDb db;
            private ITestOutputHelper output;

            public AddCarMethod(ITestOutputHelper output, SharedService service)
            {
                //db = new FakeCarDb();
                //s = new CarService(db);
                this.output = output;
                s = service.CarService;
                db = service.Db;
                output.WriteLine("ctor");
            }


            [Fact]
            public void AddSingleCar()
            {
                var mock = new Mock<ICarDb>();

                mock.Setup(db => db.Add(It.IsAny<Car>())).Returns((Car car) => car);

                var service = new CarService(mock.Object);



                //var db = new FakeCarDb();      /ไม่ต้องมีแล้วเพราะสร้าง contructor แล้ว set default ไว้ แต่ 2 function จะมองว่า คนละตัวแปร function 2 ตัวเรียกใช้ db,s 2 ครั้ง จะมองว่าเป็นตัวแปรคนละตัว
                //var s = new CarService(db);
                var c = new Car();
                c.Make = "Honda";
                c.Model = "Civic";
                var userId = Guid.NewGuid();

                //db.AddMethodHasCalled = false;

                var c2 = service.AddCar(c, userId);
                //Console.WriteLine("test output");

                Assert.NotNull(c2);
                Assert.Equal(c2.Make, c.Make);
                Assert.Equal(c2.Model, c.Model);

                mock.Verify(db => db.Add(It.IsAny<Car>()), Times.Once);  // check ว่าตัว moq ที่ทำถูกเรียก 1 ครั้ง

                //Assert.True(db.AddMethodHasCalled);
                //var cars = s.GetCarsByMember(userId);

                //Assert.Equal(1, cars.Count());
                //Assert.Contains(cars, x => x.OwnerId == userId);
            }

            [Fact]
            public void Add3Cars_ThrowException()
            {
                //var db = new FakeCarDb();
                //var s = new CarService(db);

                var memberid = Guid.NewGuid();
                s.AddCar(new Car(), memberid);
                s.AddCar(new Car(), memberid);

                var ex = Assert.Throws<OverQuotaException>(() =>
                {
                    s.AddCar(new Car(), memberid);

                });

                ex.Message.ShouldEqual("Cannot Add more car");


            }
        }


        [Collection("collection1")]
        public class GetCarsByMemberMethod
        {

            //private ICarDb db;
            private CarService s;

            public GetCarsByMemberMethod(SharedService service)
            {
                //db = new FakeCarDb();
                //s = new CarService(db);
                s = service.CarService;
            }
            [Fact]
            public void MemberCanGetOnlyHisorHerOwnCar()
            {
                //var db = new FakeCarDb();
                //var s = new CarService(db);
                var member1_Id = Guid.NewGuid();
                var member2_Id = Guid.NewGuid();
                var member3_Id = Guid.NewGuid();

                s.AddCar(new Car(), member1_Id);

                s.AddCar(new Car(), member2_Id);
                s.AddCar(new Car(), member2_Id);

                Assert.Equal(1, s.GetCarsByMember(member1_Id).Count());
                Assert.Equal(2, s.GetCarsByMember(member2_Id).Count());
                Assert.Equal(0, s.GetCarsByMember(member3_Id).Count());
            }
        }

        [Collection("collection1")]
        public class CanAddMoreCarsMethod
        {
            private CarService s;
            public CanAddMoreCarsMethod(SharedService service)
            {
                s = service.CarService;
            }

            [Fact]
            public void MemberCanAddNotMore2Cars()
            {
                var member_Id = Guid.NewGuid();

                Assert.True(s.CanAddMoreCars(member_Id)); // no car

                s.AddCar(new Car(), member_Id); //1st car
                Assert.True(s.CanAddMoreCars(member_Id));

                s.AddCar(new Car(), member_Id); // 2rd car
                Assert.False(s.CanAddMoreCars(member_Id));
            }

        }
    }
}
