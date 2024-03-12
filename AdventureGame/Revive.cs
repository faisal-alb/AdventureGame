using System;
using static System.Console;

namespace AdventureGame;

public static class Revive
{
    private static string[] questions = new[]
    {
        "What color is the sky?",
        "What is the opposite of 'warm'?",
        "What shape has four sides?",
        "What animal says 'meow'?",
        "What is the usual color of grass?",
        "What is the name of the planet we live on?",
        "What is the opposite of 'day'?"
    };
    
    private static string[] answers = new[]
    {
        "blue",
        "cold",
        "square",
        "meow",
        "green",
        "earth",
        "night"
    };

    public static bool StartEndGame(string stat)
    {
        Clear();

        Menu.ShowLogo("../../../data/art/empty-battery.txt", ConsoleColor.Red);
        
        Write("Uh Oh, it seems that you have ran out of ");

        ForegroundColor = ConsoleColor.DarkRed;
        WriteLine($"{stat}\n");
        
        ForegroundColor = ConsoleColor.White;
        WriteLine("In order to proceed with the game, you can attempt to answer a question.\n");
        
        ForegroundColor = ConsoleColor.DarkMagenta;
        WriteLine("Would you like to try and continue, or end the game and lose everything?\n");
        
        ForegroundColor = ConsoleColor.White;
        WriteLine("1) Keep Trying");
        
        ForegroundColor = ConsoleColor.DarkRed;
        WriteLine("2) Lose Everything");
        
        ForegroundColor = ConsoleColor.White;
        ConsoleKeyInfo inputKey = ReadKey();
        
        int menuChoice = MenuHandler.ChoiceMenu(inputKey);

        switch (menuChoice)
        {
            case 1:
                break;
            case 2:
                return false;
        }

        return AskQuestion(4);
    }

    private static bool AskQuestion(int attemptsLeft)
    {
        Clear();

        int randomQuestion = new Random().Next(0, questions.Length);
        
        WriteLine(questions[randomQuestion]);
        
        string? inputLine = ReadLine()?.ToLower();

        if (inputLine == answers[randomQuestion])
        {
            Clear();
            
            WriteLine("Congratulations, you have been given another chance.");

            ReadKey();
            
            return true;
        }

        attemptsLeft--;

        if (attemptsLeft > 0)
        {
            AskQuestion(attemptsLeft);
        }

        LoseText();

        return false;
    }
    
    private static void LoseText()
    {
        Clear();

        for (int i = 0; i < 10; i++)
        {
            ForegroundColor = ConsoleColor.DarkRed;
        
            if (File.Exists("../../../data/art/game-over.txt"))
            {
                string[] logoLines = File.ReadAllLines("../../../data/art/game-over.txt");

                foreach (string logoLine in logoLines)
                {
                    WriteLine(logoLine);
                }
            }
            
            ForegroundColor = ConsoleColor.White;
        }
        
        ReadKey();
    }
}