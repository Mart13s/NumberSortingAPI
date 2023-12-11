using NumberSortingAPI.Services;
using System.Diagnostics;

namespace NumberSortingAPI.Tests.Services
{
    public class NumberSorterServiceTests
    {
        private readonly INumberSorterService _numberSorterService;

        public NumberSorterServiceTests()
        {
            _numberSorterService = new NumberSorterService();
        }

        [Fact]
        public void Sort_BubbleSort()
        {
            var numbers = new List<int>() { 10, 9, 6, 7, 5, 3, 2, 1, 4, 8 };

            var stopwatch = Stopwatch.StartNew();
            var sortedNumbers = _numberSorterService.Sort(numbers, Enums.SortingAlgorithm.BubbleSort);
            stopwatch.Stop();

            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, sortedNumbers);
            Debug.WriteLine($"Bubble sort time: {stopwatch.ElapsedMilliseconds}");
        }

        [Fact]
        public void Sort_InsertionSort()
        {
            var numbers = new List<int>() { 10, 9, 6, 7, 5, 3, 2, 1, 4, 8 };

            var stopwatch = Stopwatch.StartNew();
            var sortedNumbers = _numberSorterService.Sort(numbers, Enums.SortingAlgorithm.InsertionSort);
            stopwatch.Stop();

            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, sortedNumbers);
            Debug.WriteLine($"Insertion sort time: {stopwatch.ElapsedMilliseconds}");
        }

        [Fact]
        public void Sort_Speed_Comparison_Bubble()
        {
            var numbers = GenerateNumbers(10000);

            var stopwatch = Stopwatch.StartNew();
            var sortedNumbers = _numberSorterService.Sort(numbers, Enums.SortingAlgorithm.BubbleSort);
            stopwatch.Stop();

            Assert.NotNull(sortedNumbers);
            Debug.WriteLine($"Bubble sort time: {stopwatch.ElapsedMilliseconds}");
        }

        [Fact]
        public void Sort_Speed_Comparison_Insertion()
        {
            var numbers = GenerateNumbers(10000);

            var stopwatch = Stopwatch.StartNew();
            var sortedNumbers = _numberSorterService.Sort(numbers, Enums.SortingAlgorithm.InsertionSort);
            stopwatch.Stop();

            Assert.NotNull(sortedNumbers);
            Debug.WriteLine($"Insertion sort time: {stopwatch.ElapsedMilliseconds}");
        }

        [Fact]
        public void Sort_Speed_Comparison()
        {
            var numbers = GenerateNumbers(20000);
            long bubbleTime = 0;
            long insertionTime = 0;

            var stopwatch = Stopwatch.StartNew();
            var sortedNumbers = _numberSorterService.Sort(numbers, Enums.SortingAlgorithm.BubbleSort);
            stopwatch.Stop();
            bubbleTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            Assert.NotNull(sortedNumbers);

            GC.Collect();
            GC.Collect();
            GC.Collect();

            stopwatch.Start();
            sortedNumbers = _numberSorterService.Sort(numbers, Enums.SortingAlgorithm.InsertionSort);
            stopwatch.Stop();
            insertionTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            Assert.NotNull(sortedNumbers);


            Debug.WriteLine($"Bubble sort time: {bubbleTime}ms");
            Debug.WriteLine($"Insertion sort time: {insertionTime}ms");


            Assert.True(bubbleTime > insertionTime, "InsertionSort should be faster than BubbleSort on average.");
        }

        private List<int> GenerateNumbers(int count)
        {
            var rand = new Random();
            var numbers = new List<int>();

            for (int i = 0; i < count; i++)
            {
                numbers.Add(rand.Next());
            }

            return numbers;
        }
    }
}
