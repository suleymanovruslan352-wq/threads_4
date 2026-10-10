
string[] files = Directory.GetFiles("Files");

List<Task<FileStatistics>> tasks = new List<Task<FileStatistics>>();

foreach (var item in files)
{
    tasks.Add(Task.Run(() =>
    {
        string result = File.ReadAllText(item);
        FileStatistics statistics = new FileStatistics();
        statistics.Characters = result.Length;
        statistics.Words = result.Split(new[] { ' ', '\n', '\r', '\t' },  StringSplitOptions.RemoveEmptyEntries).Length;
        statistics.Lines = result.Split( new[] { "\r\n", "\n" },StringSplitOptions.RemoveEmptyEntries).Length;
        /// strinhsplitoption none - eto chtobi ne udalyat pustye stroki, a chtobi poschitat vse stroki daje pustye
        statistics.Errors = result.ToLower().Split("error").Length - 1;
        /// koqda razdelyayem kol-vo error toest pri delenii na podstroki "error" poluchaem kol-vo elementov, a chtoby poluchit kol-vo error nado otnyat 1

        string output = $"File: {Path.GetFileName(item)}\n" +
                $"Characters: {statistics.Characters}\n" +
                $"Words: {statistics.Words}\n" +
                $"Lines: {statistics.Lines}\n" +
                $"Errors: {statistics.Errors}\n\n";

        Console.WriteLine(output);

        return statistics;
    }));
}

FileStatistics[] results = await Task.WhenAll(tasks);

int totalSymbols = 0;
int totalWords = 0;
int totalLines = 0;
int totalErrors = 0;

foreach (var statistics in results)
{
    totalSymbols += statistics.Characters;
    totalWords += statistics.Words;
    totalLines += statistics.Lines;
    totalErrors += statistics.Errors;
}

Console.WriteLine("===TOTAL===");
Console.WriteLine($"Files: {files.Length}");
Console.WriteLine($"Characters: {totalSymbols}");
Console.WriteLine($"Words: {totalWords}");
Console.WriteLine($"Lines: {totalLines}");
Console.WriteLine($"Errors: {totalErrors}");

class FileStatistics
{
    public int Characters { get; set; }
    public int Words { get; set; }
    public int Lines { get; set; }
    public int Errors { get; set; }
}





