using Microsoft.AspNetCore.Mvc;
using NumberSortingAPI.Adapters;
using NumberSortingAPI.Dtos;
using NumberSortingAPI.Enums;
using NumberSortingAPI.Services;
using System.Text.RegularExpressions;

namespace NumberSortingAPI.Controllers
{
    [ApiController]
    [Route("numbers")]
    public class NumberSortingController : ControllerBase
    {
        private readonly ITextFileAdapter _textFileAdapter;
        private readonly INumberSorterService _numberSorterService;

        public NumberSortingController(ITextFileAdapter textFileAdapter, INumberSorterService numberSorter)
        {
            _textFileAdapter = textFileAdapter;
            _numberSorterService = numberSorter;
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] NumbersDto numbersDto, [FromQuery] string sortingAlgorithm = "bubble")
        {
            if (numbersDto == null) return NoContent();

            List<int> numbers = new();

            try
            {

                numbers = ParseNumbersDto(numbersDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to read number line. Message: {ex.Message}");
            }

            if (numbers.Count > 10) return BadRequest("Too many numbers.");
            List<int> sortedNumbers = _numberSorterService.Sort(numbers, ParseSortingAlgorithm(sortingAlgorithm));

            try
            {
                _textFileAdapter.WriteNumbers(sortedNumbers);
            }
            catch
            {
                return BadRequest("Failed writing numbers to file.");
            }

            return Ok(new NumbersDto(string.Join(" ", sortedNumbers)));
        }

        [HttpGet]
        public ActionResult<NumbersDto> Get()
        {
            try
            {
                return Ok(_textFileAdapter.ReadNumbers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static List<int> ParseNumbersDto(NumbersDto numbersDto)
        {
            if (numbersDto == null || numbersDto.Numbers == null) return new List<int>();
            List<int> usedNumbers = new();

            return numbersDto.Numbers.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(number =>
                {
                    if (!Regex.IsMatch(number, @"^(10|([1-9]))$"))
                    {
                        throw new InvalidDataException("Number line may only contain numbers from 1-10.");
                    }

                    var num = int.Parse(number);

                    if (usedNumbers.Contains(num))
                    {
                        throw new InvalidDataException("Numbers may not be repeating.");
                    }

                    usedNumbers.Add(num);
                    return num;
                }
                )
                .ToList();
        }

        private static SortingAlgorithm ParseSortingAlgorithm(string sortingAlgorithm)
        {
            return sortingAlgorithm.ToLower() switch
            {
                "bubble" or "bub" or "bbl" or "b" => SortingAlgorithm.BubbleSort,
                "insertion" or "insert" or "insrt" or "in" or "i" => SortingAlgorithm.InsertionSort,
                _ => SortingAlgorithm.BubbleSort
            };
        }
    }
}
