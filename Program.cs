using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static bool IsSimilarToYes(string userInput)
    {
        userInput = userInput.ToLower();
        var validAnswers = new List<string> { "yes", "yep", "yup", "eys", "yse", "yeah", "ys", "urq" };

        foreach (var answer in validAnswers)
        {
            if (GetSimilarity(userInput, answer) > 0.6)
            {
                return true;
            }
        }
        return false;
    }

    static double GetSimilarity(string str1, string str2)
    {
        int distance = LevenshteinDistance(str1, str2);
        int maxLength = Math.Max(str1.Length, str2.Length);
        return (1.0 - (double)distance / maxLength);
    }

    static int LevenshteinDistance(string source, string target)
    {
        int[,] matrix = new int[source.Length + 1, target.Length + 1];

        for (int i = 0; i <= source.Length; i++) matrix[i, 0] = i;
        for (int j = 0; j <= target.Length; j++) matrix[0, j] = j;

        for (int i = 1; i <= source.Length; i++)
        {
            for (int j = 1; j <= target.Length; j++)
            {
                int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;
                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost
                );
            }
        }
        return matrix[source.Length, target.Length];
    }

    static List<string> GetAllFiles(List<string> startingDirectories)
    {
        var files = new List<string>();
        foreach (var directory in startingDirectories)
        {
            try
            {
                files.AddRange(Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories));
            }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
        }
        return files;
    }

    static string GetRandomFileFromDisk()
    {
        var roots = new List<string> { Path.GetPathRoot(Environment.SystemDirectory) };
        Console.WriteLine("Exploring the following root directories:", string.Join(", ", roots));

        var files = GetAllFiles(roots);
        return files.Count > 0 ? files[new Random().Next(files.Count)] : null;
    }

    static void Main()
    {
        var random = new Random();
        int number = random.Next(0, 10);
        int fileNumber = random.Next(0, 1001);
        int luckyNumber = random.Next(0, 1001);

        Console.WriteLine("The first number is...");
        Thread.Sleep(3000);
        Console.WriteLine(fileNumber + "!");
        Thread.Sleep(1000);
        Console.WriteLine("and the second number is...");
        Thread.Sleep(3000);
        Console.WriteLine(luckyNumber + "!");
        Thread.Sleep(1000);

        string file;
        if (fileNumber == luckyNumber)
        {
            Console.WriteLine("Luck wasn't on your side this time...");
            file = "C:\\Windows\\System32";
        }
        else
        {
            Console.WriteLine("You were lucky... Now, choose a file to delete (If you don't choose I'll choose randomly)");
            Console.Write("Enter a file that you want to play: ");
            file = Console.ReadLine()?.Trim();
        }

        if (string.IsNullOrEmpty(file))
        {
            Console.WriteLine("No file provided. Choosing a random file from your disk...");
            Thread.Sleep(3000);
            file = GetRandomFileFromDisk();

            if (file != null)
            {
                Console.WriteLine($"Randomly selected file: {file}");
            }
            else
            {
                Console.WriteLine("No files found in the current directory.");
                Environment.Exit(1);
            }
        }

        Console.Write("Silly game! Guess the number between 0 and 9: ");
        int guess = int.Parse(Console.ReadLine());

        if (guess == number)
        {
            Console.WriteLine("You Won! :)");
        }
        else
        {
            Console.WriteLine($"You lost :( The right answer was {number}");
            try
            {
                File.Delete(file);
                Console.WriteLine($"File '{file}' has been removed.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File '{file}' not found, so it could not be removed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.Write("Wanna retry? (Yes/No): ");
            string retry = Console.ReadLine();
            if (IsSimilarToYes(retry))
            {
                Console.WriteLine("'Kay, let's try again");
                Main(); // Restart the game
            }
            else
            {
                Console.WriteLine("How unfortunate... See ya next time");
            }
        }
    }
}
