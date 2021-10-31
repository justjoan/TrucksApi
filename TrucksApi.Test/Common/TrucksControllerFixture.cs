using Microsoft.EntityFrameworkCore;
using System;
using TrucksApi.Controllers;
using TrucksApi.Models;

namespace TrucksApi.Test
{
    public class TrucksControllerFixture: IDisposable
    {
        public readonly TrucksController TrucksController;
        public readonly TrucksAdminController TrucksAdminController;
        public readonly TrucksContext TrucksContextMock;

        public TrucksControllerFixture()
        {
            TrucksContextMock = new TrucksContextMock(new DbContextOptionsBuilder<TrucksContext>().UseInMemoryDatabase("TrucksTest").Options);
            TrucksController = new TrucksController(TrucksContextMock);
            TrucksAdminController = new TrucksAdminController(TrucksContextMock);
        }

        public void Dispose()
        {
            TrucksContextMock.Dispose();
        }
    }
}
