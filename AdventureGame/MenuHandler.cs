using System;

namespace AdventureGame;

public static class MenuHandler
{
    public static int ChoiceMenu(ConsoleKeyInfo inputKey)
    {
        if (char.IsDigit(inputKey.KeyChar))
        {
            return int.Parse(inputKey.KeyChar.ToString());
        }
        
        // Codes:
        // 50 = Backspace
        // 60 = B (For Backpack)
        // 70 = P (For Pause)
        // 100 = N/A
        
        if (inputKey.Key == ConsoleKey.Backspace) return 50;
        if (inputKey.Key == ConsoleKey.B) return 60;
        if (inputKey.Key == ConsoleKey.P) return 70;

        return 100;
    }
}