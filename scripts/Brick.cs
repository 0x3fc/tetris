using Godot;
using System;

public class Brick : Node2D
{
    [Export]
    public int dimension = 1;
    public int x = 3;
    public int y = 0;
    public bool[,] collisionMap;
    public Tile[] tiles;
    public PackedScene tileScene = (PackedScene)GD.Load("res://Tile.tscn");
    public bool placed;

    public virtual void Initialize()
    {
        tiles = new Tile[1] { MakeTile(0, 0) };
    }

    public override void _Ready()
    {
        collisionMap = new bool[dimension, dimension];
        Initialize();
        foreach (Tile tile in tiles)
        {
            AddChild(tile);
        }
        for (int i = new Random().Next(3); i >= 0; i--, Rotate()) ;
    }

    public Tile MakeTile(int x, int y)
    {
        Tile tile = (Tile)tileScene.Instance();
        tile.x = x + this.x;
        tile.y = y + this.y;
        collisionMap[y, x] = true;
        tile.Rerender();
        return tile;
    }

    public void Rotate()
    {
        bool[,] rotated = (bool[,])collisionMap.Clone();

        if (!ClockwiseRotateMatrix(rotated))
        {
            return;
        }

        collisionMap = rotated;
        RotateTiles();
    }

    public void Move(int direction)
    {
        direction = Math.Sign(direction);

        if (!CanMoveToward(direction, 0))
        {
            return;
        }

        foreach (Tile tile in tiles)
        {
            tile.Move(direction, 0);
        }

        x += direction;
    }

    public bool IsStuck()
    {
        return !CanMoveToward(0, 0);
    }

    public int FastDrop()
    {
        int distance = 0;

        while (CanMoveToward(0, 1))
        {
            foreach (Tile tile in tiles)
            {
                tile.Move(0, 1);
            }
            y += 1;
            distance++;
        }

        Board.PlaceBrick(this);
        placed = true;
        QueueFree();
        return distance;
    }

    public bool Drop()
    {
        if (!CanMoveToward(0, 1))
        {
            Board.PlaceBrick(this);
            placed = true;
            QueueFree();
            return false;
        }

        foreach (Tile tile in tiles)
        {
            tile.Move(0, 1);
        }

        y += 1;
        return true;
    }

    public void Remove()
    {
        QueueFree();
    }

    private bool CanMoveToward(int x, int y)
    {
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (!collisionMap[i, j])
                {
                    continue;
                }

                int horizontal = j + x + this.x;
                int verticle = i + y + this.y;

                if (horizontal < 0 || horizontal >= Board.BOARD_WIDTH || verticle < 0 || verticle >= Board.BOARD_HEIGHT || Board.WillLocationCollide(horizontal, i + this.y) || Board.WillLocationCollide(j + this.x, verticle))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void RotateTiles()
    {
        int k = 0;
        for (int i = 0; i < dimension && k < tiles.Length; i++)
        {
            for (int j = 0; j < dimension && k < tiles.Length; j++)
            {
                if (collisionMap[i, j])
                {
                    tiles[k++].MoveTo(j + x, i + y);
                }
            }
        }
    }

    private bool ClockwiseRotateMatrix(bool[,] matrix)
    {
        int matrixLength0 = matrix.GetLength(0);
        int matrixLength1 = matrix.GetLength(1);

        // Flip left to right
        for (int i = 0; i < matrixLength0; i++)
        {
            for (int j = 0; j < matrixLength1 / 2; j++)
            {
                Swap(matrix, i, j, i, matrixLength1 - j - 1);
            }
        }

        // Flip along topright bottomleft diagonally
        for (int i = 0; i < matrixLength0; i++)
        {
            for (int j = 0; j < matrixLength1 - i; j++)
            {
                int swapToI = matrixLength0 - j - 1;
                int swapToJ = matrixLength1 - i - 1;

                Swap(matrix, i, j, swapToI, swapToJ);

                if ((matrix[swapToI, swapToJ] && Board.WillLocationCollide(swapToJ + x, swapToI + y)) || (matrix[i, j] && Board.WillLocationCollide(j + x, i + y)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void Swap(bool[,] matrix, int i, int j, int i2, int j2)
    {
        bool tmp = matrix[i, j];
        matrix[i, j] = matrix[i2, j2];
        matrix[i2, j2] = tmp;
    }
}
