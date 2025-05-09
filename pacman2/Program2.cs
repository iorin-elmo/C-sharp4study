using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;



public static class Program2
{
    public static void Main()
    {
        Console.CursorVisible = false;
        Player player = new Player();
        Enemy enemy = new Enemy("@", Info.Width - 2, Info.Height - 2, 0, -1);
        InitializeGame();
        while (!Info.ShouldExit)
        {
            if(TerminalResized()){
                Console.Clear();
                Info.ShouldExit = true;
                Console.WriteLine("Terminal Resized");
            }

            player.Move();
            CheckDeath(player, enemy);
            enemy.Move();
            CheckDeath(player, enemy);

            if(Info.DeathFlag){
                Console.Clear();
                Info.ShouldExit = true;
                Console.WriteLine("Game Over!!");
            }

            if(Info.WinFlag){
                Console.Clear();
                Info.ShouldExit = true;
                Console.WriteLine("Game Clear!!");
            }

            System.Threading.Thread.Sleep(33);
        }
    }

    public static void InitializeGame() 
    {
        Console.Clear();
        for(int y = 0; y < Info.Height; y++) Info.Field[y] = new char[Info.Width];
        MapUtils.CreateMap();
    }

    public static bool TerminalResized() 
    {
        return Info.Height != Console.WindowHeight - 1 || Info.Width != Console.WindowWidth - 5;
    }

    public static void CheckDeath(Player player, Enemy enemy)
    {
        if(player.X == enemy.X && player.Y == enemy.Y) Info.DeathFlag = true;
        return;
    }

}


public class Player : Entity
{
    public Player(string app = "O", int x = 1, int y = 1, int dx = 0, int dy = 0) : base (app, x, y, dx, dy)
    {
    }
    public override void Move()
    {
        if (Keyboard.IsKeyDown(ConsoleKey.UpArrow))
        {
            dx = 0;
            dy = -1;
        }
        else if (Keyboard.IsKeyDown(ConsoleKey.DownArrow))
        {
            dx = 0;
            dy = 1;
        }
        else if (Keyboard.IsKeyDown(ConsoleKey.LeftArrow))
        {
            dx = -1;
            dy = 0;
        }
        else if (Keyboard.IsKeyDown(ConsoleKey.RightArrow))
        {
            dx = 1;
            dy = 0;
        }
        else
        {
            dx = 0;
            dy = 0;
        }

        base.Move();

        if(Info.Field[this.Y][this.X] == '.')
        {
            Info.Field[this.Y][this.X] = ' ';
            Info.FoodCount--;
            if(Info.FoodCount == 0) Info.WinFlag = true;
        }
    }

}

public class Enemy : Entity
{
    public Enemy(string app = "@", int x = 0, int y = 0, int dx = 0, int dy = 0) : base(app, x, y, dx, dy) 
    {
    }
    public override void Move()
    {
        Random random = new Random();
        int randomResult = random.Next(0,100);
        bool needChangeDir = MapUtils.IsWall(this.X + this.dx, this.Y + this.dy) || MapUtils.IsTshapePoint(this.X, this.Y);
        if(needChangeDir || randomResult > 95)  
        {
            ChangeDirection();
            base.Move();
        }
        else
        {
            base.Move();
        }
    }

    public void ChangeDirection()
    {
        Random random = new Random();
        switch(random.Next(0,4))
        {
            case 0:
                this.dx = 0;
                this.dy = 1;
                break;
            case 1:
                this.dx = 0;
                this.dy = -1;
                break;
            case 2:
                this.dx = 1;
                this.dy = 0;
                break;
            case 3:
                this.dx = -1;
                this.dy = 0;
                break;
            default:
                this.dx = 0;
                this.dy = 0;
                break;
        }
        return;
    }
}


public abstract class Entity
{
    public int X { get; set; }
    public int Y { get; set; }
    public int dx { get; set; }
    public int dy { get; set; }
    public string App { get; set; }

    public Entity(string app = "", int x = 0, int y = 0, int deltax = 0, int deltay = 0)
    {
        this.X = x;
        this.Y = y;
        this.dx = deltax;
        this.dy = deltay;
        this.App = app;
    }

    public virtual void Move()
    {
        int lastX = this.X;
        int lastY = this.Y;
        
        if(MapUtils.IsWall(this.X + this.dx, this.Y + this.dy))
        {
            return;
        }
        else
        {
            X = X + dx;
            Y = Y + dy;
            X = (X < 0) ? 0 : (X >= Info.Width ? Info.Width : X);
            Y = (Y < 0) ? 0 : (Y >= Info.Height ? Info.Height : Y);
            Console.SetCursorPosition(lastX, lastY);
            Console.Write(Info.Field[lastY][lastX]);
            Console.SetCursorPosition(X, Y) ;
            Console.Write(App);
        }
       

    }

}

public static class Info
{
    public static int Height = Console.WindowHeight - 1;
    public static int Width = Console.WindowWidth - 5;
    public static char[][] Field = new char[Info.Width][];
    public static int FoodCount = 0;
    public static bool WinFlag = false;
    public static bool DeathFlag = false;
    public static bool ShouldExit = false;
    public static bool IsKeyPressed = false;
}

public static class MapUtils
{
    public static void CreateMap()
    {
        for(int y = 0; y < Info.Height; y++)
        {
            string line = "";
            for(int x = 0; x < Info.Width; x++)
            {
                
                char ch = '#';
                if(!IsWall(x,y)){
                    Info.FoodCount ++;
                    ch = '.';
                }
                Info.Field[y][x] = ch;
                line = line + ch.ToString();
            }
            Console.WriteLine(line);
        }
        Info.FoodCount--;
    }

    public static bool IsWall(int x, int y)
    {
        if(x == 0 || x == Info.Width-1 || y == 0 || y == Info.Height-1) return true; // 外壁
        if(x == 1 || x == Info.Width/2 || x == Info.Width-2) return false;
        if(y == 1 || y == Info.Height/2 || y == Info.Height-2) return false;
        //if(x == )
        return true;
    }

    public static bool IsTshapePoint(int x, int y)
    {
        int wallCount = 0;
        if(IsWall(x+1,y))wallCount++;
        if(IsWall(x-1,y))wallCount++;
        if(IsWall(x,y+1))wallCount++;
        if(IsWall(x,y-1))wallCount++;
        return wallCount == 1;
    }
}

public static class Keyboard
{
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    public static bool IsKeyDown(ConsoleKey key)
    {
        return (GetAsyncKeyState((int)key) & 0x8000) != 0;
    }
}
