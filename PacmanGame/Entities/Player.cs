using System;

public class Player : Entity
{
    private int nextDx = 0; // 次の移動方向（X軸）
    private int nextDy = 0; // 次の移動方向（Y軸）

    public Player(string[] animationFrames, int x, int y, int dx, int dy) : base(animationFrames, x, y, dx, dy)
    {
    }

    public override void Move()
    {
        // 次の移動方向を予約
        if (Keyboard.IsKeyDown(ConsoleKey.UpArrow))
        {
            nextDx = 0;
            nextDy = -1;
        }
        else if (Keyboard.IsKeyDown(ConsoleKey.DownArrow))
        {
            nextDx = 0;
            nextDy = 1;
        }
        else if (Keyboard.IsKeyDown(ConsoleKey.LeftArrow))
        {
            nextDx = -1;
            nextDy = 0;
        }
        else if (Keyboard.IsKeyDown(ConsoleKey.RightArrow))
        {
            nextDx = 1;
            nextDy = 0;
        }

        // 次の方向に進める場合、方向を変更
        if (!MapUtils.IsWall(this.X + nextDx, this.Y + nextDy))
        {
            dx = nextDx;
            dy = nextDy;
        }

        // 現在の方向に進める場合のみ移動
        if (!MapUtils.IsWall(this.X + dx, this.Y + dy))
        {
            base.Move();
        }
        else if(!MapUtils.IsWall(this.X + dx*2, this.Y + dy*2) && Keyboard.IsKeyDown(ConsoleKey.Spacebar))
        {
            // スペースキーが押された場合、2マス先に移動
            dx *= 2;
            dy *= 2;
            base.Move();
        }

        // 食べ物を処理
        if (Info.Field[this.Y][this.X] == '.')
        {
            Info.Field[this.Y][this.X] = ' ';
            Info.FoodCount--;
            if (Info.FoodCount == 0) Info.WinFlag = true;
        }
    }
}