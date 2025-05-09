using System;
using System.Linq;
using System.Collections.Generic;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 1;
int playerY = 2;
string player = "O";
bool isKeyPressed = false;

int enemyX = width-2;
int enemyY = height-2;
int enemyDirectionX = 0;
int enemyDirectionY = 0;
string enemy = "@";

char[][] field = new char[height][];
int foodCount = 0;
bool winFlag = false;
bool deathFlag = false;

InitializeGame();
while (!shouldExit) 
{
    Move();
    if (TerminalResized()){
        Console.Clear();
        shouldExit = true;
        Console.WriteLine("Console was resized. Proglam exiting.");
    }
    CheckDeath();
    EnemyMove();
    CheckDeath();
    if(deathFlag){
        Console.Clear();
        shouldExit = true;
        Console.WriteLine("Game Over!!");
    }
    if(winFlag){
        Console.Clear();
        shouldExit = true;
        Console.WriteLine("Game Clear!!");
    }
    System.Threading.Thread.Sleep(20);
}

bool isWall(int x, int y)
{
    if(x == 0 || x == width-1 || y == 0 || y == height-1) return true; // 外壁
    if(x == 1 || x == width/2 || x == width-2) return false;
    if(y == 1 || y == height/2 || y == height-2) return false;
    return true;
}   

void CreateField()
{
    for(int y = 0; y < height; y++)
    {
        string line = "";
        for(int x = 0; x < width; x++)
        {
            
            char ch = '#';
            if(!isWall(x,y)){
                foodCount ++;
                ch = '.';
            }
            field[y][x] = ch;
            line = line + ch.ToString();
        }
        Console.WriteLine(line);
    }
    foodCount--;
}

void CheckDeath()
{
    if(playerX == enemyX && playerY == enemyY) deathFlag = true;
    return;
}

// Returns true if the Terminal was resized 
bool TerminalResized() 
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Reads directional input from the Console and moves the player
void Move() 
{
    int lastX = playerX;
    int lastY = playerY;
    if(Console.KeyAvailable)
    {
        var key = Console.ReadKey(true).Key;
        if(!isKeyPressed)
        {
            isKeyPressed = true;
            switch (key) 
            {
                case ConsoleKey.UpArrow:
                    playerY -= 1; 
                    break;
                case ConsoleKey.DownArrow: 
                    playerY += 1; 
                    break;
                case ConsoleKey.LeftArrow:  
                    playerX -= 1; 
                    break;
                case ConsoleKey.RightArrow: 
                    playerX += 1; 
                    break;
                case ConsoleKey.Escape:     
                    shouldExit = true; 
                    break;
                default:
                    shouldExit = true;
                    break;
            }
        }
    }else{
        if(isKeyPressed)isKeyPressed = false;
    }
    

    if(isWall(playerX,playerY)){
        playerX = lastX;
        playerY = lastY;
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    

    if(lastX != playerX || lastY != playerY){  // 動いたらその前のマスを消す
        Console.SetCursorPosition(lastX, lastY);
        Console.Write(" ");
        if(field[playerY][playerX] == '.'){
            field[playerY][playerX] = ' ';
            foodCount--;
        }
        if(foodCount <= 0){
            winFlag = true;
        }
        
    }
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
    
}

void EnemyMove()
{
    if(enemyDirectionX == 0 && enemyDirectionY == 0)EnemyDirectionChange();
    enemyX = (enemyX < 0) ? 0 : (enemyX >= width ? width : enemyX);
    enemyY = (enemyY < 0) ? 0 : (enemyY >= height ? height : enemyY);

    if(random.Next(0,100) < 100){
        int lastX = enemyX;
        int lastY = enemyY;
        enemyX += enemyDirectionX;
        enemyY += enemyDirectionY;
        enemyX = (enemyX < 0) ? 0 : (enemyX >= width ? width : enemyX);
        enemyY = (enemyY < 0) ? 0 : (enemyY >= height ? height : enemyY);
        if(isWall(enemyX, enemyY)){
            EnemyDirectionChange();
            enemyX = lastX;
            enemyY = lastY;
        }
        else
        {
        Console.SetCursorPosition(lastX, lastY);
        Console.Write(field[lastY][lastX]);
        Console.SetCursorPosition(enemyX, enemyY);
        Console.Write(enemy);
        }
    }
    
    return;
}

void EnemyDirectionChange(){
    switch(random.Next(0,4))
    {
        case 0:
            enemyDirectionX = 0;
            enemyDirectionY = 1;
            break;
        case 1:
            enemyDirectionX = 0;
            enemyDirectionY = -1;
            break;
        case 2:
            enemyDirectionX = 1;
            enemyDirectionY = 0;
            break;
        case 3:
            enemyDirectionX = -1;
            enemyDirectionY = 0;
            break;
        default:
            enemyDirectionX = 0;
            enemyDirectionY = 0;
            break;
    }
    return;
}

// Clears the console, displays the food and player
void InitializeGame() 
{
    Console.Clear();
    for(int y = 0; y < height; y++) field[y] = new char[width];
    CreateField();
}
/*
public abstract class Entity
{
    public int X { get; set; }
    public int Y { get; set; }
    public string App { get; set; }

    public Entity(string app = "", int x = 0, int y = 0)
    {
        this.X = x;
        this.Y = y;
        this.App = app;
    }

    public static void Move(int dx, int dy)
    {
        int lastX = this.X;
        int lastY = this.Y;
        
        if(isWall(X+dx, Y+dy)){
            return;
        }
        else
        {
            this.X += dx;
            this.Y += dy;
            this.X = (this.X < 0) ? 0 : (this.X >= width ? width : this.X);
            this.Y = (this.Y < 0) ? 0 : (this.Y >= height ? height : this.Y);
        }
    }

}

public class GeneralInfo
{
    public int height = Console.WindowHeight - 1;
    public int width = Console.WindowWidth - 5;
}
*/