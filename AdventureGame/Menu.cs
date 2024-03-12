using System;
using System.IO;
using static System.Console;

namespace AdventureGame;

public class Menu
{
    private Game currentGame;
    private GameStatus currentStatus;

    public Menu(Game game)
    {
        this.currentGame = game;
    }
    
    public void MainMenu()
    {
        Clear();
        
        ShowLogo("../../../data/art/game-logo.txt");


        WriteLine("Welcome to Subway Mysteries, Please Enjoy the Game!\n");
        
        Write("Made by ");
        WriteLine("Faisal A.");

        WriteLine("\nPlease select one of the following options by pressing its corresponding number:\n");
        WriteLine("1) Play Game");
        WriteLine("2) Credits");
        WriteLine("3) Quit");

        ConsoleKeyInfo inputKey = ReadKey();

        int menuChoice = MenuHandler.ChoiceMenu(inputKey);

        switch (menuChoice)
        {
            case 100:
                break;
            case 1:
                currentGame.UpdateGameStatus(GameStatus.Started);
                return;
            case 2:
                Clear();
                WriteLine("Faisal - Game");
                ReadKey();
                break;
            case 3:
                Environment.Exit(0);
                return;
        }

        MainMenu();
    }

    public void PauseMenu(Player player)
    {
        Clear();

        ShowLogo("../../../data/art/paused.txt");
        
        WriteLine("\nPlease select one of the following options by pressing its corresponding number:\n");
        WriteLine("1) Resume");
        WriteLine("2) Save");
        WriteLine("3) Quit");
        
        ConsoleKeyInfo inputKey = ReadKey();

        int menuChoice = MenuHandler.ChoiceMenu(inputKey);

        switch (menuChoice)
        {
            case 50:
            case 1:
                currentGame.UpdateGameStatus(currentStatus);
                return;
            case 2:
                player.SavePlayer();
                currentGame.UpdateGameStatus(currentStatus);
                return;
            case 3:
                Environment.Exit(0);
                return;
        }

        PauseMenu(player);
    }

    public void TrainMenu(Player player)
    {
        currentStatus = GameStatus.InTrain;
        
        Clear();

        ShowLogo("../../../data/art/train.txt");
        
        string? playerName = player.Name;
        
        Write($"Hello {playerName}, ");
        WriteLine("you are currently onboard the Train.\n");

        ShowStats(player);

        WriteLine("Keys: (Note - These keys work at anytime during the game)");

        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("Press (P) to pause the game.");
        WriteLine("Press (B) to open your backpack.\n");

        ForegroundColor = ConsoleColor.White;
        WriteLine("Choose from one of the following stations to travel:\n");
        
        Station[] trainStations = currentGame.trainStations;

        ConsoleColor[] colors =
        {
            ConsoleColor.Blue,
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Yellow
        };

        for (int i = 0; i < trainStations.Length; i++)
        {
            ForegroundColor = colors[i];
            WriteLine($"{i + 1}) {trainStations[i].name}");
        }
        
        ForegroundColor = ConsoleColor.White;
        
        ConsoleKeyInfo inputKey = ReadKey();
        
        int menuChoice = MenuHandler.ChoiceMenu(inputKey);

        if (menuChoice <= trainStations.Length)
        {
            if (menuChoice == trainStations.Length)
            {
                bool isPassCorrect = ConfirmPasscode();

                if (!isPassCorrect) return;
            }
            currentGame.currentStation = trainStations[menuChoice - 1];
            currentStatus = GameStatus.InStation;
            currentGame.UpdateGameStatus(GameStatus.InStation);
            return;
        }
        
        switch (menuChoice)
        {
            case 60:
                currentGame.UpdateGameStatus(GameStatus.InBackpack);
                return;
            case 70:
                currentGame.UpdateGameStatus(GameStatus.Paused);
                return;
        }
        
        TrainMenu(player);
    }

    public void BackpackMenu(Player player)
    {
        Clear();
        
        ShowLogo("../../../data/art/backpack.txt");

        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("Press the number of the item you would like to use or press (Backspace) to return.\n");

        ForegroundColor = ConsoleColor.White;
        WriteLine("Your current items include:\n");

        List<Item> backpack = player.Backpack;

        for (int i = 0; i < backpack.Count; i++)
        {
            string itemName = backpack[i].GetItemData()[0];
            string itemDescription = backpack[i].GetItemData()[1];
            
            WriteLine($"{i + 1} - {itemName}");
            WriteLine($"{itemDescription}\n");
        }
        
        ConsoleKeyInfo inputKey = ReadKey();
        
        int menuChoice = MenuHandler.ChoiceMenu(inputKey);

        if (menuChoice <= backpack.Count)
        {
            bool shouldDelete = backpack[menuChoice - 1].UseItem(player);

            if (shouldDelete) backpack.RemoveAt(menuChoice - 1);
            
            currentGame.UpdateGameStatus(currentStatus);
            return;
        }

        switch (menuChoice)
        {
            case 50:
                currentGame.UpdateGameStatus(currentStatus);
                return;
        }
        
        BackpackMenu(player);
    }

    public static void ShowStats(Player player)
    {
        int playerHealth = player.GetPlayerStats()[0];
        int playerHunger = player.GetPlayerStats()[1];
        int playerStrength = player.GetPlayerStats()[2];
        
        WriteLine("Player Stats:\n");
        
        ForegroundColor = ConsoleColor.Red;
        
        if (playerHealth >= 8)
        {
            ForegroundColor = ConsoleColor.Green;
        } else if (playerHealth >= 5)
        {
            ForegroundColor = ConsoleColor.Yellow;
        }
        
        Write("Health: ");
        WriteLine($"{playerHealth}");

        ForegroundColor = ConsoleColor.White;
        Write("Hunger: ");
        WriteLine($"{playerHunger}");
        
        Write("Strength: ");
        WriteLine($"{playerStrength}\n");
    }

    public static void PickupItem(Item item)
    {
        Clear();

        ShowLogo("../../../data/art/item-found.txt", ConsoleColor.Cyan);
        
        string itemName = item.GetItemData()[0];
        
        Write($"You have found ");

        ForegroundColor = ConsoleColor.Cyan;
        WriteLine(itemName);

        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("\nPress any button to continue...\n");

        ForegroundColor = ConsoleColor.White;
        ReadKey();
    }

    public static void EndGame(Player player)
    {
        ShowLogo("../../../data/art/game-over.txt", ConsoleColor.Green);
        
        WriteLine();
        WriteLine();
        WriteLine();
        
        WriteLine("Congratulations!!! You have completed the game.\n");
        WriteLine("Your final Stats are\n");
        
        ShowStats(player);

        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("Press any button to continue...\n");
        
        ForegroundColor = ConsoleColor.White;
        ReadKey();
    }
    
    public static void ShowLogo(string pathToFile, ConsoleColor customColor = ConsoleColor.DarkCyan)
    {
        Clear();
        
        ForegroundColor = customColor;
        
        if (File.Exists(pathToFile))
        {
            string[] logoLines = File.ReadAllLines(pathToFile);

            foreach (string logoLine in logoLines)
            {
                WriteLine(logoLine);
            }
            
            WriteLine();
        }
        
        ForegroundColor = ConsoleColor.White;
    }

    private bool ConfirmPasscode()
    {
        Clear();
        
        WriteLine("Note - It is recommended to have max Stats for this Station...\n");
        WriteLine("Please enter the password for this Station:\n");
        
        string? inputLine = ReadLine()?.ToLower();

        if (inputLine == "adventure")
        {
            return true;
        }

        return false;
    }
}