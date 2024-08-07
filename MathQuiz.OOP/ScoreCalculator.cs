namespace MathQuiz.OOP;

public static class ScoreCalculator
{
    public static int CalculateScore(int correctAnswer, int userAnswer)
    {
        var difference = Math.Abs(correctAnswer - userAnswer);
        return difference switch
        {
            0 => 10,
            <= 5 => 5,
            _ => 1
        };
    }
}