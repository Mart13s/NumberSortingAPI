using NumberSortingAPI.Enums;

namespace NumberSortingAPI.Services
{
    public class NumberSorterService : INumberSorterService
    {
        public List<int> Sort(List<int> numbers, SortingAlgorithm sortingAlgorithm = SortingAlgorithm.BubbleSort)
        {
            if (numbers == null || numbers.Count <= 1)
            {
                return numbers;
            }

            return sortingAlgorithm switch
            {
                SortingAlgorithm.BubbleSort => BubbleSort(numbers),
                SortingAlgorithm.InsertionSort => InsertionSort(numbers),
                _ => BubbleSort(numbers),
            };
        }

        private static List<int> BubbleSort(List<int> numbers)
        {
            List<int> sortedNumbers = new(numbers);

            int endIndex = sortedNumbers.Count - 1;
            bool wasSwapped = true;

            while (wasSwapped)
            {
                wasSwapped = false;
                for (int i = 0; i < endIndex; i++)
                {
                    if (sortedNumbers[i + 1] < sortedNumbers[i])
                    {
                        (sortedNumbers[i + 1], sortedNumbers[i]) = (sortedNumbers[i], sortedNumbers[i + 1]);
                        wasSwapped = true;
                    }
                }
                endIndex--;
            }

            return sortedNumbers;
        }

        private static List<int> InsertionSort(List<int> numbers)
        {
            List<int> sortedNumbers = new(numbers);

            for (int i = 0; i < sortedNumbers.Count - 1; i++)
            {
                int minIndex = i;
                int minValue = sortedNumbers[i];

                for (int j = i + 1; j < sortedNumbers.Count; j++)
                {
                    if (sortedNumbers[j] < minValue)
                    {
                        minIndex = j;
                        minValue = sortedNumbers[j];
                    }
                }

                if (minIndex != i)
                {
                    (sortedNumbers[minIndex], sortedNumbers[i]) = (sortedNumbers[i], sortedNumbers[minIndex]);
                }
            }

            return sortedNumbers;
        }
    }
}
