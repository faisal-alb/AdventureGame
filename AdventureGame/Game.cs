using System;

namespace AdventureGame;

public enum GameStatus
{
    MainMenu,
    Started,
    Paused,
    InTrain,
    InStation,
    InBackpack,
    InMenu
}

public class Game
{
    private GameStatus gameStatus;
    private Menu gameMenu;
    private Player myPlayer;
    public Station? currentStation;
    public Station[] trainStations { get; }

    public Game()
    {
        this.gameStatus = GameStatus.MainMenu;
        this.gameMenu = new Menu(this);
        this.myPlayer = new Player(this);

        NPC[] npcs = new[]
        {
            new NPC("Marina", "How many days are there in a week? (Type numbers as a word not a number)", "seven", new Item("paper1")),
            new NPC("Ignitus", "How many fingers does the average human have on one hand? (Type numbers as a word not a number)", "five", new Item("paper2")),
            new NPC("Terra", "How many legs does a cat have? (Type numbers as a word not a number)", "four", new Item("paper3")),
            new NPC("", "", "", new Item(""))

        };

        this.trainStations = new[]
        {
            new Station("Aquatic Reef Station", this, npcs[0], myPlayer),
            new Station("Blaze Ember Station", this, npcs[1], myPlayer, 8, 12),
            new Station("Greener Grass Station", this, npcs[2], myPlayer, 8, 14),
            new Station("Cloud Central Station", this, npcs[3], myPlayer, 4, 20, true)
        };
        
        GameLoop();
    }

    private void GameLoop()
    {
        if (gameStatus == GameStatus.MainMenu)
        {
            gameMenu.MainMenu();
        }
        
        while (gameStatus != GameStatus.MainMenu)
        {
            switch (gameStatus)
            {
                case GameStatus.Started:
                    myPlayer.SetupPlayer();
                    UpdateGameStatus(GameStatus.InTrain);
                    break;
                case GameStatus.Paused:
                    gameMenu.PauseMenu(myPlayer);
                    break;
                case GameStatus.InTrain:
                    gameMenu.TrainMenu(myPlayer);
                    break;
                case GameStatus.InStation:
                    InputHandler(Console.ReadKey());
                    break;
                case GameStatus.InBackpack:
                    gameMenu.BackpackMenu(myPlayer);
                    break;
                case GameStatus.InMenu:
                    break;
            }
        }
        
        GameLoop();
    }
    
    public void UpdateGameStatus(GameStatus newStatus)
    {
        this.gameStatus = newStatus;

        if (gameStatus == GameStatus.InStation)
        {
            currentStation?.ShowStation();
        }
    }
    
    public void WinGame()
    {
        this.gameStatus = GameStatus.MainMenu;
        
        myPlayer.ResetPlayer();
        
        Menu.EndGame(myPlayer);

        Environment.Exit(0);
    }

    public void LoseGame()
    {
        myPlayer.ResetPlayer();
    }
    
    private void InputHandler(ConsoleKeyInfo inputKey)
    {
        switch (inputKey.Key)
        {
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.RightArrow:
                currentStation?.MovePlayer(inputKey.Key);
                currentStation?.ShowStation();
                break;
        }

        if (inputKey.Key == ConsoleKey.B)
        {
            gameStatus = GameStatus.InBackpack;
        }
        if (inputKey.Key == ConsoleKey.P)
        {
            gameStatus = GameStatus.Paused;
        }
    }
}