
namespace NumberSortingAPI.Adapters
{
    public interface ITextFileAdapter
    {
        string ReadNumbers();
        void WriteNumbers(List<int> numbers);
    }
}