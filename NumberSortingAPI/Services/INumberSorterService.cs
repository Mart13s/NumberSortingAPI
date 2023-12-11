
using NumberSortingAPI.Enums;

namespace NumberSortingAPI.Services
{
    public interface INumberSorterService
    {
        List<int> Sort(List<int> numbers, SortingAlgorithm sortingAlgorithm = SortingAlgorithm.BubbleSort);
    }
}