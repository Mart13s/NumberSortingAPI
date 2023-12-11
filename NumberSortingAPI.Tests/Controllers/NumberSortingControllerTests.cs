using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NumberSortingAPI.Adapters;
using NumberSortingAPI.Controllers;
using NumberSortingAPI.Dtos;
using NumberSortingAPI.Services;

namespace NumberSortingAPI.Tests.Controllers
{
    public class NumberSortingControllerTests
    {
        private readonly NumberSortingController _controller;
        private readonly INumberSorterService _numberSortingService;
        private readonly ITextFileAdapter _textFileAdapter;

        public NumberSortingControllerTests()
        {
            _numberSortingService = new NumberSorterService();
            _textFileAdapter = Substitute.For<ITextFileAdapter>();
            _controller = new NumberSortingController(_textFileAdapter, _numberSortingService);
        }

        [Fact]
        public void Post_ValidNumbers_ReturnsOkResult()
        {
            var numbersDto = new NumbersDto("5 3 4 2 1");

            var result = _controller.Post(numbersDto);

            Assert.IsType<OkObjectResult>(result.Result);

            var resultValue = ((OkObjectResult)result.Result).Value as NumbersDto;
            Assert.Equal("1 2 3 4 5", resultValue?.Numbers);
        }

        [Fact]
        public void Post_InvalidNumbers_ReturnsBadRequest()
        {
            var numbersDto = new NumbersDto("13 2 3 4 5");

            var result = _controller.Post(numbersDto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Post_NoNumbers_ReturnsOkResult()
        {
            var numbersDto = new NumbersDto("");

            var result = _controller.Post(numbersDto);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void Post_NoNumbersDto_ReturnsNoContent()
        {
            var result = _controller.Post(null);

            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void Post_LettersInsteadOfNumbers_ReturnsBadRequest()
        {
            var result = _controller.Post(new NumbersDto("qweqweawsddwa"));

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Post_TooManyNumbers_ReturnsBadRequest()
        {
            var result = _controller.Post(new NumbersDto("1 2 3 4 5 6 7 8 9 10 11"));

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Post_WriteThrowException_ReturnsBadRequest()
        {
            var mockTextFileAdapter = Substitute.For<ITextFileAdapter>();
            mockTextFileAdapter
                .When(x => x.WriteNumbers(Arg.Any<List<int>>()))
                .Do(x => throw new Exception("Failed writing numbers to file."));

            var controller = new NumberSortingController(mockTextFileAdapter, _numberSortingService);

            var result = controller.Post(new NumbersDto("4 5 "));

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Post_ValidNumbers_InsertionSort_ReturnsOk()
        {
            var result = _controller.Post(new NumbersDto("10 9 8 7"), "insertion");

            Assert.IsType<OkObjectResult>(result.Result);

            var resultValue = ((OkObjectResult)result.Result).Value as NumbersDto;
            Assert.Equal("7 8 9 10", resultValue?.Numbers);
        }

        [Fact]
        public void Post_ValidNumbers_BubbleSort_ReturnsOk()
        {
            var result = _controller.Post(new NumbersDto("10 9 8 7"), "bubble");

            var resultValue = ((OkObjectResult)result.Result).Value as NumbersDto;
            Assert.Equal("7 8 9 10", resultValue?.Numbers);
        }

        [Fact]
        public void Get_ReturnsOkResult()
        {
            var mockTextFileAdapter = Substitute.For<ITextFileAdapter>();
            mockTextFileAdapter.ReadNumbers().Returns("1 2 3 4 5 6");

            var controller = new NumberSortingController(mockTextFileAdapter, _numberSortingService);

            var result = controller.Get();

            Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal("1 2 3 4 5 6", ((OkObjectResult)result.Result).Value);
        }

        [Fact]
        public void Get_Exception_ReturnsBadRequest()
        {
            var mockTextFileAdapter = Substitute.For<ITextFileAdapter>();
            mockTextFileAdapter.ReadNumbers().Throws(new Exception());

            var controller = new NumberSortingController(mockTextFileAdapter, _numberSortingService);

            var result = controller.Get();

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
