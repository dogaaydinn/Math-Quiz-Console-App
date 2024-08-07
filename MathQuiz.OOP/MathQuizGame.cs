namespace MathQuiz.OOP;

public class MathQuizGame
{
    private const int InitialQuestions = 10;
    private readonly Random _random;
    private readonly ScoreManager _scoreManager;
    private readonly QuestionGenerator _questionGenerator;

    public MathQuizGame()
    {
        _random = new Random();
        _scoreManager = new ScoreManager();
        _questionGenerator = new QuestionGenerator();
    }

    public void Start()
    {
        while (true)
        {
            Console.WriteLine("Welcome To Math Quiz Console App :)");
            Console.WriteLine("You are playing Math Quiz. Choose one of options for playing..");

            var options = new List<string> { "1. easy", "2. medium", "3. hard", "4. exit" };
            Console.WriteLine("Options:");
            options.ForEach(Console.WriteLine);
            Console.Write("Your Choice (1-4): ");

            if (int.TryParse(Console.ReadLine(), out int difficulty) && Enum.IsDefined(typeof(Difficulty), difficulty - 1))
            {
                if (difficulty == 4) break;

                Console.WriteLine($"Your Choice: You chose {((Difficulty)(difficulty - 1)).ToString().ToLower()} mode to play. Try answering math questions in given time.");
                PlayGame((Difficulty)(difficulty - 1));
            }
            else
            {
                Console.WriteLine("Invalid Option. Please enter a valid option.");
            }
        }
    }

    private void PlayGame(Difficulty difficulty)
    {
        int totalQuestions = InitialQuestions;
        int totalPoints = 0;

        int highestScore = _scoreManager.ShowHighestScore();
        int timeLimit = _questionGenerator.GetTimeLimit(difficulty);
        int initialNumber = _questionGenerator.GenerateInitialNumber(_random, difficulty);

        while (totalQuestions > 0)
        {
            var (questionText, correctAnswer, newInitialNumber) = _questionGenerator.GenerateRandomQuestion(_random, difficulty, initialNumber);
            Console.WriteLine(questionText);

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeLimit);
            bool answerValid = false;
            bool timeWarning = false;

            while (DateTime.Now < endTime && !answerValid)
            {
                if (Console.KeyAvailable)
                {
                    var userAnswer = Console.ReadLine();
                    if (int.TryParse(userAnswer, out int userAnswerNumber))
                    {
                        int score = ScoreCalculator.CalculateScore(correctAnswer, userAnswerNumber);
                        totalPoints += score;
                        Console.WriteLine($"Your answer: {userAnswerNumber}. Your Score: {score}");
                        answerValid = true;
                        initialNumber = newInitialNumber;
                        break;
                    }

                    Console.WriteLine("Invalid answer, please write a valid answer.");
                }

                if (!timeWarning && (endTime - DateTime.Now).TotalSeconds <= 5)
                {
                    Console.WriteLine("Last 5 seconds for you to answer :(");
                    timeWarning = true;
                }
            }

            if (!answerValid)
            {
                Console.WriteLine($"Time is up! The correct answer was: {correctAnswer}");
            }

            totalQuestions--;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.Clear();
        }

        Console.WriteLine($"Total Score: {totalPoints}");

        if (totalPoints > highestScore)
        {
            Console.WriteLine($"Congratulations! You have the highest score: {totalPoints} yeyyyy");
            _scoreManager.SaveScore(totalPoints);
        }
        else
        {
            Console.WriteLine($"The highest score is still: {highestScore}. You should be better LOSER :))");
        }
    }
}