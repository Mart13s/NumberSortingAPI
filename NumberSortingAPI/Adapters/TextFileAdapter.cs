namespace NumberSortingAPI.Adapters
{
    public class TextFileAdapter : ITextFileAdapter
    {
        private readonly string _path = "numbers.txt";

        public TextFileAdapter(string path = "numbers.txt")
        {
            _path = path;
        }

        public void WriteNumbers(List<int> numbers)
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            using StreamWriter sw = new(_path);
            for (int i = 0; i < numbers.Count; i++)
            {
                sw.Write(numbers[i]);

                if (i + 1 < numbers.Count) sw.Write(" ");
            }
        }

        public string ReadNumbers()
        {
            if (!File.Exists(_path)) throw new Exception($"File {_path} not found.");

            using StreamReader sr = new(_path);
            return sr.ReadToEnd().Trim();
        }
    }
}
