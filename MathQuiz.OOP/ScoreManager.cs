namespace MathQuiz.OOP;

public class ScoreManager
{
    private readonly string _filePath;

    public ScoreManager()
    {
        var workingDirectory = Environment.CurrentDirectory;
        _filePath = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, "scores.txt");

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "");
        }
    }

    public int ShowHighestScore()
    {
        var scores = File.ReadLines(_filePath)
            .Select(line => int.TryParse(line.Split(' ')[0], out int score) ? score : 0)
            .ToList();

        if (scores.Any())
        {
            var highestScore = scores.Max();
            Console.WriteLine("Highest score from the game: " + highestScore);
            return highestScore;
        }

        Console.WriteLine("No scores have been recorded yet.");
        return 0;
    }

    public void SaveScore(int score)
    {
        File.AppendAllText(_filePath, $"{score} in {DateTime.UtcNow}\n");
    }
}