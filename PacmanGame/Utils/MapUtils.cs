using System;

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
                    Info.FoodCount++;
                    ch = '.';
                }
                Info.Field[y][x] = ch;
                line = line + ch.ToString();
            }
            Console.WriteLine(line);
        }
    }

    public static bool IsWall(int x, int y)
    {
        if(x <= 0 || x >= Info.Width-1 || y <= 0 || y >= Info.Height-1) return true; // Outer wall
        if(Info.Difficulty == 666 && x == 1 && y == Info.Height/2 ) return true;
        if(Info.Difficulty == 666 && x == Info.Width-2 && y == Info.Height/2 ) return true;
        if(x == 1 || x == Info.Width/2 || x == Info.Width-2) return false;
        if(y == 1 || y == Info.Height/2 || y == Info.Height-2) return false;
        if(Info.Difficulty == 1) return true; // Easy mode
        if(Info.Difficulty < 5)
        {
            if(x == Info.Width/4 && Math.Abs(Info.Height/2 - y) < Info.Height/4+1) return false;
            if(x == Info.Width/4*3 && Math.Abs(Info.Height/2 - y) < Info.Height/4+1) return false;
        }
        else
        {
            if(x == Info.Width/4 && Math.Abs(Info.Height/2 - y) < Info.Height/4) return false;
            if(x == Info.Width/4*3 && Math.Abs(Info.Height/2 - y) < Info.Height/4) return false;
        }
        if(y == Info.Height/4 && Math.Abs(Info.Width/2 - x) < Info.Width/4) return false;
        if(y == Info.Height/4*3 && Math.Abs(Info.Width/2 - x) < Info.Width/4) return false;
        return true;
    }

    public static int CountNearbyWalls(int x, int y)
    {
        int wallCount = 0;
        if(IsWall(x+1,y)) wallCount++;
        if(IsWall(x-1,y)) wallCount++;
        if(IsWall(x,y+1)) wallCount++;
        if(IsWall(x,y-1)) wallCount++;
        return wallCount;
    }

}