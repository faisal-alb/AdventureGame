using System;
using static System.Console;

namespace AdventureGame;

public class Player
{
    public string? Name;
    private int health;
    private int hunger;
    private int strength;
    public List<Item> Backpack { get; }

    private Game currentGame;
    private string pathToPlayerInfo = "../../../data/saves/player-info.txt";
    private string pathToPlayerInventory = "../../../data/saves/player-inventory.txt";

    public Player(Game game)
    {
        this.Name = "Anonymous";
        this.Backpack = new List<Item>();
        this.health = 10;
        this.hunger = 10;
        this.strength = 10;
        this.currentGame = game;
    }

    public void SetupPlayer()
    {
        Clear();

        if (File.Exists(pathToPlayerInfo))
        {
            LoadPlayer();

            WriteLine($"Welcome back, {Name}\n");
            WriteLine("How would you like to continue? Pick one of the options below:\n");
            
            WriteLine("1) Load from a Previous Save.");
            Write("2) Start a New Save. ");
            
            ForegroundColor = ConsoleColor.DarkRed;
            WriteLine("(Warning - Your save will be deleted)");

            ForegroundColor = ConsoleColor.White;
            ConsoleKey inputKey = ReadKey().Key;

            switch (inputKey)
            {
                case ConsoleKey.D1:
                    return;
                case ConsoleKey.D2:
                    ResetPlayer();
                    break;
                default:
                    return;
            }
        }
        
        Clear();

        WriteLine("What would you like to name your Character?");

        string? inputLine = ReadLine();

        if (inputLine != null || inputLine != "")
        {
            this.Name = inputLine;
        }

        SavePlayer();
    }

    public int[] GetPlayerStats()
    {
        return new[] { health, hunger, strength };
    }

    public void UpdatePlayerStats(string stat, int multiple)
    {
        int randomUpdateValue = new Random().Next(0, 2);
        
        switch (stat)
        {
            case "health":
                this.health += (randomUpdateValue * multiple);
                if (health <= 0)
                {
                    this.health = 0;
                    bool shouldRevive = Revive.StartEndGame("health");
                    
                    if (shouldRevive)
                    {
                        this.health = 8;
                        this.hunger = 8;
                        this.strength = 8;
                        break;
                    }
                    
                    currentGame.LoseGame();
                    currentGame.UpdateGameStatus(GameStatus.MainMenu);
                }
                if (health >= 10) this.health = 10;
                break;
            case "hunger":
                this.hunger += (randomUpdateValue * multiple);
                if (hunger <= 0)
                {
                    this.hunger = 0;
                    bool shouldRevive = Revive.StartEndGame("hunger");
                    
                    if (shouldRevive)
                    {
                        this.health = 8;
                        this.hunger = 8;
                        this.strength = 8;
                        break;
                    }
                    
                    currentGame.LoseGame();
                    currentGame.UpdateGameStatus(GameStatus.MainMenu);
                }
                if (hunger >= 10) this.hunger = 10;
                break;
            case "strength":
                this.strength += (randomUpdateValue * multiple);
                if (strength <= 0)
                {
                    this.strength = 0;
                    bool shouldRevive = Revive.StartEndGame("strength");
                    
                    if (shouldRevive)
                    {
                        this.health = 8;
                        this.hunger = 8;
                        this.strength = 8;
                        break;
                    }
                    
                    currentGame.LoseGame();
                    currentGame.UpdateGameStatus(GameStatus.MainMenu);
                }
                if (strength >= 10) this.strength = 10;
                break;
        }
    }

    public void GiveItem(Item item)
    {
        this.Backpack.Add(item);
        
        Menu.PickupItem(item);
    }
    
    public void ResetPlayer()
    {
        File.Delete(pathToPlayerInfo);
        File.Delete(pathToPlayerInventory);
        
        Backpack.Clear();

        this.health = 10;
        this.hunger = 10;
        this.strength = 10;
    }

    public void SavePlayer()
    {
        if (File.Exists(pathToPlayerInfo))
        {
            File.Delete(pathToPlayerInfo);
        }

        using (StreamWriter streamWriter = File.CreateText(pathToPlayerInfo))
        {
            streamWriter.WriteLine(this.Name);
            streamWriter.WriteLine(this.health);
            streamWriter.WriteLine(this.hunger);
            streamWriter.WriteLine(this.strength);
        }
        
        SavePlayerInventory();
    }

    private void SavePlayerInventory()
    {
        if (File.Exists(pathToPlayerInventory))
        {
            File.Delete(pathToPlayerInventory);
        }
        
        using (StreamWriter streamWriter = File.CreateText(pathToPlayerInventory))
        {
            foreach (Item item in Backpack)
            {
                streamWriter.WriteLine(item.Id);
            }
        }
    }

    private void LoadPlayer()
    {
        if (!File.Exists(pathToPlayerInfo)) return;

        string[] playerInfo = File.ReadAllLines(pathToPlayerInfo);

        this.Name = playerInfo[0];
        
        int healthInt = int.Parse(playerInfo[1]);
        this.health = healthInt;
        
        int hungerInt = int.Parse(playerInfo[2]);
        this.hunger = hungerInt;
        
        int strengthInt = int.Parse(playerInfo[3]);
        this.strength = strengthInt;
        
        
        LoadPlayerInventory();
    }

    private void LoadPlayerInventory()
    {
        Backpack.Clear();

        if (!File.Exists(pathToPlayerInventory)) return;
        
        string[] playerInventory = File.ReadAllLines(pathToPlayerInventory);

        foreach (string item in playerInventory)
        {
            Item newItem = new Item(item);
            Backpack.Add(newItem);
        }
    }
}