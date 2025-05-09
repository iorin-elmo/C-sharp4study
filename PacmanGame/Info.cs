public static class Info
{
    public static int ConsoleHeight = Console.WindowHeight;
    public static int ConsoleWidth = Console.WindowWidth;
    public static int Height = 25;

    public static int Width = 100;

    public static char[][] Field = new char[Height][];
    public static int FoodCount = 0;
    public static bool WinFlag = false;
    public static bool DeathFlag = false;
    public static int GameSpeed = 33; // milliseconds
    public static int Difficulty = -1; // 1: Easy, 2: Normal, 3: Hard, 4: Ex-Hard, 5: Hazard
}