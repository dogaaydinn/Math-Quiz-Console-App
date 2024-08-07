namespace MathQuiz;

internal static class Program
{
    # region Constants

    const int InitialQuestions = 10;
    private static readonly string WorkingDirectory = Environment.CurrentDirectory;
    private static readonly string FilePath = Path.Combine(Directory.GetParent(WorkingDirectory).Parent.Parent.FullName,"scores.txt");
    
    #endregion Constants

    public static void Main(string[] args)
    {
        var random = new Random();

        # region Game Info

        start:
        Console.WriteLine("Welcome To Math Quiz Console App :)");
        Console.WriteLine("You are playing Math Quiz. Choose one of options for playing..");

        var options = new List<string> { "1. easy", "2. medium", "3. hard", "4. exit" };
        Console.WriteLine("Options:");
        foreach (var option in options)
            Console.WriteLine(option);
        Console.Write("Your Choice (1-4): ");

        # endregion Game Info

        # region Game Mode

        var playMode = Console.ReadLine();

        try
        {
            var difficulty = Convert.ToInt32(playMode);
            switch (difficulty)
            {
                case 1:
                    Console.WriteLine(
                        "Your Choice: You chose easy mode to play. Try answering math questions in given time.");
                    GuessAnswer(random, Difficulty.Easy);
                    break;
                case 2:
                    Console.WriteLine(
                        "Your Choice: You chose medium mode to play. Try answering math questions in given time.");
                    GuessAnswer(random, Difficulty.Medium);
                    break;
                case 3:
                    Console.WriteLine(
                        "Your Choice: You chose hard mode to play. Try answering math questions in given time.");
                    GuessAnswer(random, Difficulty.Hard);
                    break;
                case 4:
                    Console.WriteLine("You Choose: Exit");
                    return;
                default:
                    Console.WriteLine("Invalid Option. Please enter a valid option.");
                    goto start;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine(
                $"Invalid input: {playMode} is not a valid number. Please enter a number between 1 and 4.");
            goto start;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            goto start;
        }

        #endregion Game Mode

        static void GuessAnswer(Random random, string difficulty)
        {
            var totalQuestions = InitialQuestions;
            var totalPoints = 0;

            Console.WriteLine(
                $"You will given {GetTimeLimit(difficulty)} seconds. Now try answering question for {difficulty} game mode");
            var initialNumber = GenerateInitialNumber(random, difficulty);
            var timeLimit = GetTimeLimit(difficulty);

            var highestScore = ShowHighestScore(FilePath);

            while (totalQuestions > 0)
            {
                var (questionText, correctAnswer, newInitialNumber) =
                    GenerateRandomQuestion(random, difficulty, initialNumber);
                Console.WriteLine(questionText);

                var startTime = DateTime.Now;
                var endTime = startTime.AddSeconds(timeLimit);
                var userAnswer = string.Empty;
                var answerValid = false;
                bool timeWarning = false;

                while (DateTime.Now < endTime && !answerValid)
                {
                    if (Console.KeyAvailable)
                    {
                        userAnswer = Console.ReadLine();
                        if (int.TryParse(userAnswer, out int userAnswerNumber))
                        {
                            var score = CalculateScore(correctAnswer, userAnswerNumber);
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
            }
            else
            {
                Console.WriteLine($"The highest score is still: {highestScore}. You should be better LOSER :))");
            }

            File.AppendAllText(FilePath, $"{totalPoints} in {DateTime.UtcNow}\n");
        }


        static int CalculateScore(int correctAnswer, int userAnswer)
        {
            var difference = Math.Abs(correctAnswer - userAnswer);
            return difference switch
            {
                0 => 10,
                <= 5 => 5,
                _ => 1
            };
        }

        static (string QuestionText, int CorrectAnswer, int NewInitialNumber) GenerateRandomQuestion(Random random,
            string difficulty, int initialNumber)
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

        static int GenerateInitialNumber(Random random, string difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => random.Next(1, 11),
                Difficulty.Medium => random.Next(11, 100),
                Difficulty.Hard => random.Next(50, 501),
                _ => throw new ArgumentException("Invalid difficulty level.")
            };
        }

        static int GetTimeLimit(string difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 40,
                Difficulty.Medium => 30,
                Difficulty.Hard => 20,
                _ => throw new ArgumentException("Invalid difficulty level")
            };
        }

        static int ShowHighestScore(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "");
                Console.WriteLine("Score file created. There are no scores available at the moment.");
                return 0;
            }

            var scores = new List<int>();
            foreach (var line in File.ReadLines(filePath))
            {
                var scoreStr = line.Split(" ")[0];
                if (int.TryParse(scoreStr, out int score))
                {
                    scores.Add(score);
                }
            }
            
            if (scores.Any())
            {
                var highestScore = scores.Max();
                Console.WriteLine("Highest score from the game: " + highestScore);
                return highestScore;
            }

            Console.WriteLine("No scores have been recorded yet.");
            return 0;
        }
    }

    # region Classes

    private static class Difficulty
    { 
        public const string Easy = "Easy";
        public const string Medium = "Medium";
        public const string Hard = "Hard";
    }

# endregion Classes
} 