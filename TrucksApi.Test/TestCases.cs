using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TrucksApi.Controllers;
using TrucksApi.Models;
using Xunit;

namespace TrucksApi.Test
{
    public class TestCases : IClassFixture<TrucksControllerFixture>
    {
        private readonly TrucksController controller;
        private readonly TrucksAdminController adminController;

        public TestCases(TrucksControllerFixture fixture)
        {
            controller = fixture.TrucksController;
            adminController = fixture.TrucksAdminController;
        }

        [Fact]
        public async void Create_Truck_Succeeds()
        {
            //Arrange
            Truck truck = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "12345",
                Block = "12345"
            };

            //Act
            var actionResult = await adminController.PostTruck(truck);

            //Assert
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.NotNull(result);
            var truck2 = result.Value as Truck;
            Assert.NotNull(truck2);
            Assert.True(truck2.Id.Equals(truck.Id));
            Assert.True(truck2.Name.Equals(truck.Name));
            Assert.True(truck2.Block.Equals(truck.Block));
        }

        [Theory]
        [InlineData("", "someName")]
        [InlineData("someId", "")]
        public async void Create_Truck_BadRequest(string id, string name)
        {
            //Arrange
            Truck truck = new()
            {
                Id = id,
                Name = name
            };

            //Model state validations not automatically reflected.
            adminController.ModelState.AddModelError("field", "missing required field");

            //Act
            var actionResult = await adminController.PostTruck(truck);

            //Assert
            var result = actionResult.Result as BadRequestResult;
            Assert.NotNull(result);

            //Cleanup - clear validation state
            adminController.ModelState.Clear();
        }

        [Fact]
        public async void Get_Truck_Success()
        {   //Arrange
            Truck truck = new()
            {
                Id = "AAA", //Guid.NewGuid().ToString(),
                Name = "AAA",
                Block = "BBB"
            };

            //Act
            var actionResult = await adminController.PostTruck(truck);

            //Assert
            actionResult = await adminController.GetTruck(truck.Id);
            var truck2 = actionResult.Value as Truck;
            Assert.NotNull(truck2);
            Assert.Equal(truck.Id, truck2.Id);
        }


        [Fact]
        public async void Get_Truck_NotFound()
        {
            //Arrange
            string fakeId = Guid.NewGuid().ToString();

            //Act
            var actionResult = await adminController.GetTruck(fakeId);

            //Assert
            var result = actionResult.Result as NotFoundResult;
            Assert.NotNull(result);
        }


        [Fact]
        public async void Get_Trucks_Empty_Success()
        {
            //Arrange with clean db
            using (TrucksContextMock tempContext = new(new DbContextOptionsBuilder<TrucksContext>().UseInMemoryDatabase("Temp").Options))
            {
                TrucksAdminController tempController = new(tempContext);

                //Act
                var actionResult = await tempController.GetTrucks();

                //Assert
                var trucks = Assert.IsType<List<Truck>>(actionResult.Value);
                Assert.Empty(trucks);
            }
        }

        [Fact]
        public async void Get_Trucks_Admin_Success()
        {
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "AAA" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "BBB" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "CCC" });

            //Act
            var actionResult = await adminController.GetTrucks();

            //Assert
            var trucks = Assert.IsType<List<Truck>>(actionResult.Value);
            Assert.True(trucks.Count >= 3); 
        }


        [Fact]
        public async void Get_Trucks_ByBlock_Success()
        {
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "AAA", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "BBB", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "CCC", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "DDD", Block = "B200" });

            //Act
            var actionResult = await controller.GetTrucks("B100");

            //Assert
            var trucks = Assert.IsType<List<Truck>>(actionResult.Value);
            Assert.True(trucks.Count >= 3);
            Assert.All(trucks, t => t.Block.Equals("B100"));
            
        }


        [Fact]
        public async void Get_Trucks_ByBlock_Empty_Success()
        {
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "AAA", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "BBB", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "CCC", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "DDD", Block = "B200" });

            //Act
            var actionResult = await controller.GetTrucks("B300");

            //Assert
            var trucks = Assert.IsType<List<Truck>>(actionResult.Value);
            Assert.Empty(trucks);
        }



        [Fact]
        public async void Get_Trucks_ByBlock_BadRequest()
        {
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "AAA", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "BBB", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "CCC", Block = "B100" });
            await adminController.PostTruck(new Truck() { Id = Guid.NewGuid().ToString(), Name = "DDD", Block = "B200" });

            //Act
            var actionResult = await controller.GetTrucks("B100", "Unknown ZoneType");

            //Assert
            var result = actionResult.Result as BadRequestResult;
            Assert.NotNull(result);

        }
    }
}
