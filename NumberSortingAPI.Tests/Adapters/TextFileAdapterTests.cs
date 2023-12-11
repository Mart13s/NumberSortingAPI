using NumberSortingAPI.Adapters;

namespace NumberSortingAPI.Tests.Adapters
{
    public class TextFileAdapterTests
    {
        public TextFileAdapterTests()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "numbers*.txt");

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        ~TextFileAdapterTests()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "numbers*.txt");

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        [Fact]
        public void WriteNumbers_ValidNumbers_Success()
        {
            TextFileAdapter textFileAdapter = new TextFileAdapter("numbers1.txt");
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            textFileAdapter.WriteNumbers(numbers);

            Assert.True(File.Exists("numbers1.txt"));
            var content = File.ReadAllText("numbers1.txt");
            Assert.Equal("1 2 3 4 5", content.Trim());
        }

        [Fact]
        public void ReadNumbers_FileExists_ReturnsContent()
        {
            TextFileAdapter textFileAdapter = new TextFileAdapter("numbers2.txt");
            File.WriteAllText("numbers2.txt", "1 2 3 4 5");

            var content = textFileAdapter.ReadNumbers();

            Assert.Equal("1 2 3 4 5", content.Trim());
        }

        [Fact]
        public void ReadNumbers_FileDoesNotExist_ThrowsException()
        {
            TextFileAdapter textFileAdapter = new TextFileAdapter("numbers3.txt");

            var exception = Assert.Throws<Exception>(() => textFileAdapter.ReadNumbers());
            Assert.Equal($"File numbers3.txt not found.", exception.Message);
        }
    }
}
