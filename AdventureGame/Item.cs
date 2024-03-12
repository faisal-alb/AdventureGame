using System;
using System.IO;

namespace AdventureGame;

public class Item
{
    public int xPos;
    public int yPos;
    
    public string Id { get; }
    private string name;
    private string description;
    
    public Item(string itemId)
    {
        this.Id = itemId;
        this.name = "N/A";
        this.description = "Item Not Found";

        SetItemData();
    }
    
    public Item(string itemId, int xSpawnPos, int ySpawnPos)
    {
        this.Id = itemId;
        this.name = "N/A";
        this.description = "Item Not Found";
        this.xPos = xSpawnPos;
        this.yPos = ySpawnPos;

        SetItemData();
    }

    public string[] GetItemData()
    {
        return new[] { this.name, this.description };
    }

    public bool UseItem(Player player)
    {
        switch (Id)
        {
            case "health-potion":
                player.UpdatePlayerStats("health", 1);
                return true;
            case "strength-potion":
                player.UpdatePlayerStats("strength", 1);
                return true;
            case "burger":
                player.UpdatePlayerStats("hunger", 1);
                return true;
            case "lemon":
                player.UpdatePlayerStats("hunger", 1);
                player.UpdatePlayerStats("strength", 1);
                player.UpdatePlayerStats("health", -1);
                return true;
        }

        return false;
    }

    private void SetItemData()
    {
        string pathToItems = "../../../data/items.txt";
        
        if(!File.Exists(pathToItems)) return;

        string[] itemsInfo = File.ReadAllLines(pathToItems);

        for (int i = 0; i < itemsInfo.Length; i++)
        {
            if (itemsInfo[i] == Id)
            {
                string[] itemData = itemsInfo[i + 1].Split(':');

                this.name = itemData[0];
                this.description = itemData[1];
            }
        }
    }
}