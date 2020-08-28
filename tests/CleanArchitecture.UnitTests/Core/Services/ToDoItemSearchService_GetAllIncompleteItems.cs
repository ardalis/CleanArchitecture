using Ardalis.Specification;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Services;
using CleanArchitecture.SharedKernel.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Core.Services
{
    public class ToDoItemSearchService_GetAllIncompleteItems
    {
        [Fact]
        public async Task ReturnsInvalidGivenNullSearchString()
        {
            var repo = new Mock<IRepository>();
            var service = new ToDoItemSearchService(repo.Object);

            var result = await service.GetAllIncompleteItems(null);

            Assert.Equal(Ardalis.Result.ResultStatus.Invalid, result.Status);
        }

        private List<ToDoItem> GetTestItems()
        {
            // Note: could use AutoFixture
            var builder = new ToDoItemBuilder();

            var items = new List<ToDoItem>();

            var item1 = builder.WithDefaultValues().Build();
            items.Add(item1);

            var item2 = builder.WithDefaultValues().Id(2).Build();
            items.Add(item2);

            var item3 = builder.WithDefaultValues().Id(3).Build();
            items.Add(item3);

            return items;
        }
    }
}
