using System;

public static partial class Program
{
    public static void Main(string[] args)
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        if(args.Length == 0){

        }
        else if(args[0] == "-help" || args[0] == "--help" || args[0] == "-h")
        {
            Console.WriteLine("Usage: dotnet run [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help    Show this help message");
            Console.WriteLine("  -d, --difficulty <level>|easy|normal|hard|ex-hard|hazard Set the difficulty level (1-5)");
            Console.WriteLine("  -t, --gameTick <ticks(ms)> Set the game tick rate(default: 33ms)");
            return;
        }
        else if(args[0] == "-d" || args[0] == "--difficulty")
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Please specify a difficulty level");
                return;
            }
            else
            {
                if(SetDifficultyFromArgs(args) == -1) return;
            }
            
        }
        else if(args[0] == "-t" || args[0] == "--gameTick")
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Please specify a game tick rate");
                return;
            }
            else
            {
                if(SetGameTickFromArgs(args) == -1) return;
            }
        }
        else
        {
            Console.WriteLine("Invalid option. Use -h or --help for usage information.");
            return;
        }
        if(args.Length == 4){
            if(args[2] == "-d" || args[2] == "--difficulty")
            { 
                if(SetDifficultyFromArgs([args[2],args[3]]) == -1) return;   
            }
            else if(args[2] == "-t" || args[2] == "--gameTick")
            {
                if(SetGameTickFromArgs([args[2],args[3]]) == -1) return;
            }
        }else if(args.Length == 3 || args.Length == 5 || args.Length > 6){
            Console.WriteLine("Invalid option. Use -h or --help for usage information.");
            return;
        }
        if(Info.Difficulty == -1)SetDifficultyFromCLI();
        RunGame();
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static int SetGameTickFromArgs(string[] args)
    {
        if (int.TryParse(args[1], out int gameTick))
        {
            if (gameTick < 1)
            {
                Console.WriteLine("Invalid game tick. Please choose above 16ms");
                return -1;
            }
            else
            {
                if(gameTick < 16){
                    Console.WriteLine("Game tick is too fast. Please choose above 16ms");
                    return -1;
                }else if(gameTick > 1000){
                    Console.WriteLine("Game tick is too slow. Please choose below 1000ms");
                    return -1;
                }else{
                    Info.GameSpeed = gameTick;
                }
            }
            
        }
        else
        {
            Console.WriteLine("Invalid game tick. Please choose a positive number");
            return -1;
        }
        return 0;
    }

    public static int SetDifficultyFromArgs(string[] args)
    {
        if (int.TryParse(args[1], out int difficulty))
        {
            if (difficulty < 1 || difficulty > 5)
            {
                Console.WriteLine("Invalid difficulty level. Please choose a number between 1 and 5");
                return -1;
            }
            else
            {
                Info.Difficulty = difficulty;
            }
            
        }
        else
        {
            if(args[1] == "easy")        Info.Difficulty = 1;
            else if(args[1] == "normal") Info.Difficulty = 2;
            else if(args[1] == "hard")   Info.Difficulty = 3;
            else if(args[1] == "exhard") Info.Difficulty = 4;
            else if(args[1] == "hazard") Info.Difficulty = 5;
            else
            {
                Console.WriteLine("Invalid difficulty level. Please choose from easy, normal, hard, ex-hard, or hazard");
                return -1;
            }
        }
        return 0;
    }

    public static void SetDifficultyFromCLI()
    {
        Console.Clear();
        Console.WriteLine("Please choose a difficulty level:");
        Console.WriteLine("[*]: Easy");
        Console.WriteLine("[ ]: Normal");
        Console.WriteLine("[ ]: Hard");
        Console.WriteLine("[ ]: Ex-Hard");
        Console.WriteLine("[ ]: Hazard");
        int difficulty = 1;
        int count2hell = 0;
        while(true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.Enter)
            {
                Info.Difficulty = difficulty;
                break;
            }
            else
            {
                
                
                switch(key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if(count2hell >= 20) break;
                        difficulty--;
                        if(difficulty < 1) difficulty = 1;
                        break;
                    case ConsoleKey.DownArrow:
                        if(difficulty > 5 && count2hell < 20){
                            difficulty = 5;
                            count2hell++;
                        }else if(count2hell >= 20){
                            difficulty = 666;
                            Console.SetCursorPosition(0,7);
                            Console.WriteLine("[*]: ????");
                            break;
                        }else{
                            difficulty++;
                        }
                        break;
                    default:
                        break;
                }
                for(int i = 1; i <= 5; i++)
                {
                    Console.SetCursorPosition(1, i);
                    if(i == difficulty)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                if(count2hell >= 20){
                    difficulty = 666;
                    Console.SetCursorPosition(0,7);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[*]: ????");
                }
            }
        }
    }


    public static void RunGame()
    {
        Player player = new Player(["O","0"], 1, 1, 0, 0);
        Chaser chaser = new Chaser(["x","+"], Info.Width - 2, Info.Height - 2, 0, -1);
        Ghost ghost = new Ghost(["@","@"], Info.Width - 2, Info.Height - 2, 0, -1);
        RandomEnemy enemy = new RandomEnemy(["-","/","|","\\"], Info.Width - 2, Info.Height - 2, 0, -1);
        InitializeGame();
        bool isRunning = true;
        while (isRunning)
        {
            if(hasTerminalProblems())
            {
                isRunning = false;
                break;
            }

            player.Move();
            CheckDeath(player, enemy);
            enemy.Move();
            CheckDeath(player, enemy);
            
            if(Info.Difficulty > 2){
                chaser.MoveTowardsPlayer(player.X, player.Y);
                CheckDeath(player, chaser);
            }
            
            if(Info.Difficulty > 4 && Info.Difficulty != 666){
                ghost.ToggleInvisible(player.X, player.Y);
                ghost.Move();
                CheckDeath(player, ghost);
            }
            
            
            
            if(Info.DeathFlag){
                Console.Clear();
                Console.WriteLine("Game Over!!");
                isRunning = false;
            }

            if(Info.WinFlag){
                Console.Clear();
                Console.WriteLine("Game Clear!!");
                isRunning = false;
            }

            System.Threading.Thread.Sleep(Info.GameSpeed);
        }
        Console.ResetColor();
    }

    public static void InitializeGame() 
    {
        Console.Clear();
        for(int y = 0; y < Info.Height; y++) Info.Field[y] = new char[Info.Width];
        MapUtils.CreateMap();
    }

    public static bool hasTerminalProblems() 
    {
        if(Info.ConsoleHeight < Info.Height || Info.ConsoleWidth < Info.Width){
            Console.Clear();
            Console.WriteLine("Terminal too small");
            return true;
        }
        if(Info.ConsoleHeight != Console.WindowHeight || Info.ConsoleWidth != Console.WindowWidth)
        {
            Console.Clear();
            Console.WriteLine("Terminal resized");
            return true;
        }
        return false;
    }

    public static void CheckDeath(Player player, Enemy enemy)
    {
        if(player.X == enemy.X && player.Y == enemy.Y) Info.DeathFlag = true;
        return;
    }
}