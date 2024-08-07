namespace MathQuiz.OOP;
public class QuestionGenerator
{
    public int GetTimeLimit(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 40,
            Difficulty.Medium => 30,
            Difficulty.Hard => 20,
            _ => throw new ArgumentException("Invalid difficulty level")
        };
    }

    public int GenerateInitialNumber(Random random, Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => random.Next(1, 11),
            Difficulty.Medium => random.Next(11, 100),
            Difficulty.Hard => random.Next(50, 501),
            _ => throw new ArgumentException("Invalid difficulty level.")
        };
    }

    public (string QuestionText, int CorrectAnswer, int NewInitialNumber) GenerateRandomQuestion(Random random, Difficulty difficulty, int initialNumber)
    {
        int minValue, maxValue;

        switch (difficulty)
        {
            case Difficulty.Easy:
                minValue = 0;
                maxValue = 10;
                break;
            case Difficulty.Medium:
                minValue = 11;
                maxValue = 99;
                break;
            case Difficulty.Hard:
                minValue = 50;
                maxValue = 500;
                break;
            default:
                throw new ArgumentException("Invalid difficulty level.");
        }

        var randomNumber = random.Next(minValue, maxValue + 1);
        var operation = random.Next(5);
        int correctAnswer;
        int newInitialNumber;

        (string questionText, int result) = operation switch
        {
            0 => ($"{initialNumber} + {randomNumber} = ?", initialNumber + randomNumber),
            1 => ($"{initialNumber} - {randomNumber} = ?", initialNumber - randomNumber),
            2 => ($"{initialNumber} * {randomNumber} = ?", initialNumber * randomNumber),
            3 => ($"{initialNumber} / {randomNumber} = ?", initialNumber / randomNumber),
            4 => ($"{initialNumber} % {randomNumber} = ?", initialNumber % randomNumber),
            _ => throw new InvalidOperationException("Invalid operation")
        };

        correctAnswer = result;
        newInitialNumber = result;

        return (questionText, correctAnswer, newInitialNumber);
    }
}