using System;
using static System.Console;

namespace AdventureGame;

public class NPC
{
    public int xPos;
    public int yPos;
    public string Name { get; }
    private string question;
    private string answer;
    private Item item;
    private Player? player;
    private int attempts;
    
    public NPC(string name, string question, string answer, Item item)
    {
        this.Name = name;
        this.question = question;
        this.answer = answer;
        this.item = item;
        this.attempts = 0;
    }

    public void AskQuestion(string stationName, Player currentPlayer)
    {
        this.player = currentPlayer;
        
        Clear();
        
        WriteLine($"Hello there! My name is {this.Name}\n");
        WriteLine($"Welcome to {stationName}\n");
        
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("Press any button to continue...");
                
        ForegroundColor = ConsoleColor.White;

        ReadKey();

        HandleChoice();
    }

    private void HandleChoice()
    {
        Clear();
        
        WriteLine("Would you like to ask me a question?\n");
        
        WriteLine("1) How can I unlock the final station?");
        WriteLine("2) How do I leave this station?");
        WriteLine("3) No thanks!");

        
        ConsoleKeyInfo inputKey = ReadKey();

        int menuChoice = MenuHandler.ChoiceMenu(inputKey);

        switch (menuChoice)
        {
            case 1:
                ContinueChoice();
                return;
            case 2:
                Clear();
                WriteLine("In order to leave this station, go to the red Exit marked with an E\n");
                
                ForegroundColor = ConsoleColor.DarkYellow;
                WriteLine("Press any button to continue...");
                
                ForegroundColor = ConsoleColor.White;
                ReadKey();
                break;
            case 3:
                return;
        }
        
        HandleChoice();
    }

    private void ContinueChoice()
    {
        Clear();
        
        WriteLine("To unlock the final Station, you need to get the password.\n");
        WriteLine("The password contains three parts that can be found in the first three Stations.\n");
        WriteLine("To get this password, you need to solve a question provided by the person living in each Station.\n");

        ForegroundColor = ConsoleColor.DarkMagenta;
        WriteLine("Would you like to attempt this question now? (Note - You can come back and try at any time)\n");

        ForegroundColor = ConsoleColor.White;
        WriteLine("1) Yes");
        WriteLine("2) No thanks!");
        
        ConsoleKeyInfo inputKey = ReadKey();

        int menuChoice = MenuHandler.ChoiceMenu(inputKey);

        switch (menuChoice)
        {
            case 1:
                AskRiddle();
                return;
            case 2:
                return;
        }
    }

    private void AskRiddle()
    {
        
        Clear();
        
        WriteLine(this.question);

        string? inputLine = ReadLine()?.ToLower();
        
        if (attempts >= 3)
        {
            Clear();
            
            WriteLine("Unfortunately, you were not able complete this question.\n");
            WriteLine("You can come back and try again later. Goodbye for now!");

            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Press any button to continue...");
                
            ForegroundColor = ConsoleColor.White;
            
            ReadKey();
            
            attempts = 0;
            return;
        }

        if (inputLine == this.answer)
        {
            Clear();

            player?.GiveItem(item);
            
            Clear();
            
            WriteLine("Congratulations, you have solved the question! You can try finding the rest of the code in the other Stations.");

            ReadKey();
            
            return;
        }

        attempts++;
        AskRiddle();
    }
}