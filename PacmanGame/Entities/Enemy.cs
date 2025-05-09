using System;

public class Enemy : Entity
{
    public Enemy(string[] animationFrames, int x = 0, int y = 0, int dx = 0, int dy = 0) : base(animationFrames, x, y, dx, dy) 
    {
    }

    public override void Move()
    {
        base.Move();
    }
    
    public bool needChangeDir(){
        return MapUtils.IsWall(this.X + this.dx, this.Y + this.dy) || MapUtils.CountNearbyWalls(this.X, this.Y) < 2 ;
    } 

    public void ChangeDirection()
    {
        Random random = new Random();
        switch (random.Next(0, 4))
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

public class Chaser : Enemy
{
    public Chaser(string[] animationFrames, int x, int y, int dx, int dy) : base(animationFrames, x, y, dx, dy) 
    {
    }

    private int freezeTime = 1;

    public void MoveTowardsPlayer(int playerX, int playerY)
    {
        // A*アルゴリズムで次の位置を計算
        if(freezeTime == 0){
            freezeTime = 2;
            if(Info.Difficulty == 666) freezeTime = 5;
            return;
        }
        (int nextX, int nextY) = FindPathAStar(this.X, this.Y, playerX, playerY);

        // 次の位置に移動
        this.dx = nextX - this.X;
        this.dy = nextY - this.Y;

        base.Move();
        freezeTime--;
    }

    private (int, int) FindPathAStar(int startX, int startY, int targetX, int targetY)
    {
        // ノード情報
        var openList = new List<Node>();
        var closedList = new HashSet<(int, int)>();

        // 開始ノードを追加
        openList.Add(new Node(startX, startY, null, 0, Heuristic(startX, startY, targetX, targetY)));

        while (openList.Count > 0)
        {
            // F値が最小のノードを取得
            var currentNode = openList.OrderBy(n => n.F).First();
            openList.Remove(currentNode);
            closedList.Add((currentNode.X, currentNode.Y));

            // ゴールに到達した場合
            if (currentNode.X == targetX && currentNode.Y == targetY)
            {
                // 経路を復元
                return BacktrackPath(currentNode);
            }

            // 隣接ノードを探索
            foreach (var neighbor in GetNeighbors(currentNode.X, currentNode.Y))
            {
                if (closedList.Contains((neighbor.X, neighbor.Y)) || MapUtils.IsWall(neighbor.X, neighbor.Y))
                {
                    continue;
                }

                int gCost = currentNode.G + 1; // 移動コストは1
                var existingNode = openList.FirstOrDefault(n => n.X == neighbor.X && n.Y == neighbor.Y);

                if (existingNode == null)
                {
                    // 新しいノードをオープンリストに追加
                    openList.Add(new Node(neighbor.X, neighbor.Y, currentNode, gCost, Heuristic(neighbor.X, neighbor.Y, targetX, targetY)));
                }
                else if (gCost < existingNode.G)
                {
                    // より良い経路が見つかった場合、ノードを更新
                    existingNode.G = gCost;
                    existingNode.Parent = currentNode;
                }
            }
        }

        // 経路が見つからない場合、現在位置を返す
        return (startX, startY);
    }

    private IEnumerable<(int X, int Y)> GetNeighbors(int x, int y)
    {
        // 上下左右の隣接ノードを返す
        return new List<(int, int)>
        {
            (x, y - 1), // 上
            (x, y + 1), // 下
            (x - 1, y), // 左
            (x + 1, y)  // 右
        };
    }

    private int Heuristic(int x, int y, int targetX, int targetY)
    {
        // マンハッタン距離を使用
        return Math.Abs(x - targetX) + Math.Abs(y - targetY);
    }

    private (int, int) BacktrackPath(Node node)
    {
        // ゴールから親ノードをたどり、次の移動先を決定
        while (node.Parent != null && node.Parent.Parent != null)
        {
            node = node.Parent;
        }
        return (node.X, node.Y);
    }

    private class Node
    {
        public int X { get; }
        public int Y { get; }
        public Node Parent { get; set; }
        public int G { get; set; } // 開始地点からのコスト
        public int H { get; }     // ゴールまでの推定コスト
        public int F => G + H;    // 総コスト

        public Node(int x, int y, Node parent, int g, int h)
        {
            X = x;
            Y = y;
            Parent = parent;
            G = g;
            H = h;
        }
    }
}
public class RandomEnemy : Enemy
{
    public RandomEnemy(string[] animationFrames, int x, int y, int dx, int dy) : base(animationFrames, x, y, dx, dy) 
    {
    }
    
    public override void Move()
    {
        Random random = new Random();
        int randomResult = random.Next(0, 100);
  
        if (this.needChangeDir() || randomResult > 95)  
        {
            ChangeDirection();
            base.Move();
        }
        else
        {
            if(randomResult < Info.Difficulty * 20) base.Move();
        }
    }
}

public class Ghost : Enemy
{
    public Ghost(string[] animationFrames, int x, int y, int dx, int dy) : base(animationFrames, x, y, dx, dy) 
    {
        tmpFrame = animationFrames;

    }

    private string[] ghostFrame = [" ", " "];
    private string[] tmpFrame;

    public override void Move()
    {
        Random random = new Random();
        int randomResult = random.Next(0, 100);
        if (this.needChangeDir())  
        {
            ChangeDirection();
            base.Move();
        }
        else if(randomResult > 50)
        {
            base.Move();
        }
    }

    public void ToggleInvisible(int playerX, int playerY)
    {
       if (IsVisible(playerX, playerY))
        {
            AnimationFrames = ghostFrame;
        }
        else
        {
            AnimationFrames = tmpFrame;
        }
    }
    public bool IsVisible(int playerX, int playerY)
    {
        // プレイヤーとの距離を計算
        int distanceX = Math.Abs(this.X - playerX);
        int distanceY = Math.Abs(this.Y - playerY);

        if (distanceX + distanceY > 10)
        {
            return false; // プレイヤーが近いので可視化
        }

        return true;// プレイヤーが遠いので透明化
    }
}