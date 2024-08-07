namespace MathQuiz.OOP;

internal static class Program
{
    public static void Main(string[] args)
    {
        var game = new MathQuizGame();
        game.Start();
    }
}
/*
Program.cs: Entry point of the application.
   Difficulty.cs: Enum for difficulty levels.
   MathQuizGame.cs: Main game logic.
   QuestionGenerator.cs: Logic for generating questions.
   ScoreManager.cs: Logic for managing scores.
   ScoreCalculator.cs: Logic for calculating scores.
   */