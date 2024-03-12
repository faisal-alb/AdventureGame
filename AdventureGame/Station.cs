using System;
using System.Threading;
using static System.Console;

namespace AdventureGame;

public class Station
{
    public string name { get; }
    private Game currentGame;
    private Map stationMap;
    private Player player;
    private int playerX;
    private int playerY;
    private int exitX;
    private int exitY;
    private int numberOfMoves;
    private List<Item> stationItems;
    private NPC stationNPC;
    private int timeLimit;
    private bool isFinal;
    private bool timeStarted;
    private DateTime enteredTime;

    public Station(string name, Game game, NPC npc, Player currentPlayer)
    {
        this.name = name;
        this.currentGame = game;
        this.stationMap = new Map(8, 10);
        this.stationItems = new List<Item>();
        this.stationNPC = npc;
        this.numberOfMoves = 0;
        this.player = currentPlayer;

        InitializeMapValues();
        InitializeNPCPosition();
    }
    
    public Station(string name, Game game, NPC npc, Player currentPlayer, int width, int height, bool isFinalStation = false)
    {
        this.name = name;
        this.currentGame = game;
        this.stationMap = new Map(width, height);
        this.stationItems = new List<Item>();
        this.stationNPC = npc;
        this.numberOfMoves = 0;
        this.player = currentPlayer;
        this.isFinal = isFinalStation;

        InitializeMapValues();
        InitializeNPCPosition();
    }
    
    public void ShowStation()
    {
        WriteLine();
        Clear();

        if (isFinal)
        {
            stationItems.Clear();
            
            if (!timeStarted)
            {
                this.timeLimit = 30;
                this.enteredTime = DateTime.Now;

                timeStarted = true;
            }

            if (IsTimerFinished())
            {
                currentGame.UpdateGameStatus(GameStatus.InTrain);
                timeStarted = false;
                return;
            }
            
            ForegroundColor = ConsoleColor.DarkRed;
            WriteLine("WARNING: You have 30 Seconds to get to the end before this station is destroyed.\n");

            ForegroundColor = ConsoleColor.White;
            WriteLine("The timer has started.\n");
        }

        for (int x = 0; x < stationMap.gridWidth; x++)
        {
            for (int y = 0; y < stationMap.gridHeight; y++)
            {
                bool isItem = false;
                
                if (x == playerX && y == playerY)
                {
                    SetColorAndWrite('X', ConsoleColor.Blue);

                    isItem = true;
                }
                else if (x == exitX && y == exitY)
                {
                    SetColorAndWrite('E', ConsoleColor.Red);

                    isItem = true;
                }
                else if (x == stationNPC.xPos && y == stationNPC.yPos)
                {
                    SetColorAndWrite('N', ConsoleColor.Magenta);

                    isItem = true;
                    break;
                }
                else
                {
                    foreach (Item item in stationItems)
                    {
                        if (x == item.xPos && y == item.yPos)
                        {
                            SetColorAndWrite('I', ConsoleColor.DarkGreen);
                            
                            isItem = true;
                            break;
                        }
                    }
                }

                if (!isItem)
                {
                    SetColorAndWrite(stationMap.grid[x, y]);
                }
            }
            
            WriteLine();
        }
        
        WriteLine();
        
        ForegroundColor = ConsoleColor.White;
        
        ShowPlayerDetails();
        ShowMapLegend();
    }
    
    public void MovePlayer(ConsoleKey inputKey)
    {
        stationMap.grid[playerX, playerY] = '0';
        
        switch (inputKey)
        {
            case ConsoleKey.UpArrow:
                if (playerX > 0) playerX--;
                break;
            case ConsoleKey.DownArrow:
                if (playerX < stationMap.gridWidth - 1) playerX++;
                break;
            case ConsoleKey.LeftArrow:
                if (playerY > 0) playerY--;
                break;
            case ConsoleKey.RightArrow:
                if (playerY < stationMap.gridHeight - 1) playerY++;
                break;
        }

        this.numberOfMoves++;
        
        int randomMaxMoves = new Random().Next(2, 7);

        if (numberOfMoves > randomMaxMoves)
        {
            this.numberOfMoves = 0;
            
            player.UpdatePlayerStats("health", -1);
            player.UpdatePlayerStats("hunger", -1);
            player.UpdatePlayerStats("strength", -1);
        }

        CheckOverlap();
        
        stationMap.grid[playerX, playerY] = 'X';
    }

    private void SetColorAndWrite(char character, ConsoleColor color = ConsoleColor.White)
    {
        ForegroundColor = color;

        Write(' ');
        Write(character);
        Write(' ');

        ForegroundColor = ConsoleColor.White;
    }
    
    private void InitializeMapValues()
    {
        this.playerX = 0;
        this.playerY = 0;
        this.exitX = stationMap.gridWidth - 1;
        this.exitY = stationMap.gridHeight - 1;

        InitializeMapItems();
    }

    private void InitializeMapItems()
    {
        string[] itemsToRandomize = new[]
        {
            "health-potion",
            "strength-potion",
            "strength-potion",
            "burger",
            "burger",
            "lemon",
            "lemon",
            "lemon"
        };

        Random random = new Random();

        for (int i = 0; i < itemsToRandomize.Length; i++)
        {
            int x = random.Next(1, stationMap.gridWidth - 1);
            int y = random.Next(1, stationMap.gridWidth - 1);
            
            stationItems.Add(new Item(itemsToRandomize[i], x, y));
        }
    }

    private void InitializeNPCPosition()
    {
        Random random = new Random();
        
        int x = 0;
        int y = stationMap.gridHeight - 1;
        
        stationNPC.xPos = x;
        stationNPC.yPos = y;

        if (isFinal)
        {
            stationNPC.xPos = 1000;
            stationNPC.yPos = 1000;
        }
    }
    
    private void CheckOverlap()
    {
        if (playerX == exitX && playerY == exitY)
        {
            if (isFinal)
            {
                currentGame.WinGame();
                return;
            }
            
            ExitStation();
        }
        
        if (playerX == stationNPC.xPos && playerY == stationNPC.yPos)
        {
            currentGame.UpdateGameStatus(GameStatus.InMenu);
            
            stationNPC.AskQuestion(this.name, player);

            currentGame.UpdateGameStatus(GameStatus.InStation);
            
            Refresh();
        }
        
        for (int i = 0; i < stationItems.Count; i++)
        {
            Item item = stationItems[i];

            if (playerX == item.xPos && playerY == item.yPos)
            {
                player.GiveItem(item);
                
                player.UpdatePlayerStats("strength", -1);
                
                stationItems.RemoveAt(i);
                i--;

                Refresh();
            }
        }
    }
    
    private void ShowPlayerDetails()
    {
        ForegroundColor = ConsoleColor.DarkYellow;
        Write("\nUse the arrow keys to move. ");
        
        ForegroundColor = ConsoleColor.White;
        WriteLine("(Warning: Holding Down or Spamming the arrow keys may result in flickering)\n");
        
        ForegroundColor = ConsoleColor.White;
        Write($"Hello {player.Name}, ");
        WriteLine($"you are currently at {name}\n");

        Menu.ShowStats(player);
    }

    private void ShowMapLegend()
    {
        WriteLine("Map Legend:\n");
        
        ForegroundColor = ConsoleColor.Blue;
        WriteLine("X - Player");
        
        ForegroundColor = ConsoleColor.DarkGreen;
        WriteLine("I - Item");
        
        ForegroundColor = ConsoleColor.Magenta;
        WriteLine("N - Person of Interest");
        
        ForegroundColor = ConsoleColor.Red;
        WriteLine("E - Station Exit");
        
        ForegroundColor = ConsoleColor.White;
    }

    private bool IsTimerFinished()
    {
        DateTime currentTime = DateTime.Now;
        DateTime exitTime = enteredTime.AddSeconds(timeLimit);
        
        return currentTime > exitTime;
    }

    private void Refresh()
    {
        Clear();
        ShowStation();
    }

    private void ExitStation()
    {
        playerX = 0;
        playerY = 0;
        currentGame.UpdateGameStatus(GameStatus.InTrain);
    }
}