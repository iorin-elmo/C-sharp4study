public abstract class Entity
{
    public int X { get; set; }
    public int Y { get; set; }
    public int dx { get; set; }
    public int dy { get; set; }
    public string[] AnimationFrames { get; set; }

    private int animationFrame = 0;
    private int animationDelay = 3;
    private int animationTick = 0;

    public Entity(string[] animationFrames, int x = 0, int y = 0, int deltax = 0, int deltay = 0)
    {
        this.X = x;
        this.Y = y;
        this.dx = deltax;
        this.dy = deltay;
        this.AnimationFrames = animationFrames;
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
        }
        Console.SetCursorPosition(X, Y);
        Console.Write(AnimationFrames[animationFrame]);
        animationTick++;
        if(animationTick >= animationDelay)
        {
            animationTick = 0;
        }
        if(animationTick == 0)
        {
            animationFrame++;
            if(animationFrame >= AnimationFrames.Length)
            {
                animationFrame = 0;
            }
        }
    }

    public async Task AnimateAsync()
    {
        for(int i = 0; i < AnimationFrames.Length; i++)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(AnimationFrames[i]);
            await Task.Delay(100);
        }
    }
}