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

            var result = await service.GetAllIncompleteItemsAsync(null);

            Assert.Equal(Ardalis.Result.ResultStatus.Invalid, result.Status);
            Assert.Equal("searchString is required.", result.ValidationErrors.First().ErrorMessage);
        }

        [Fact]
        public async Task ReturnsErrorGivenDataAccessException()
        {
            string expectedErrorMessage = "Database not there.";
            var repo = new Mock<IRepository>();
            var service = new ToDoItemSearchService(repo.Object);
            repo.Setup(r => r.ListAsync(It.IsAny<ISpecification<ToDoItem>>()))
                .ThrowsAsync(new System.Exception(expectedErrorMessage));

            var result = await service.GetAllIncompleteItemsAsync("something");

            Assert.Equal(Ardalis.Result.ResultStatus.Error, result.Status);
            Assert.Equal(expectedErrorMessage, result.Errors.First());
        }

        [Fact]
        public async Task ReturnsListGivenSearchString()
        {
            var repo = new Mock<IRepository>();
            var service = new ToDoItemSearchService(repo.Object);

            var result = await service.GetAllIncompleteItemsAsync("foo");

            Assert.Equal(Ardalis.Result.ResultStatus.Ok, result.Status);
        }
    }
}
