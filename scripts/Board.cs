using Godot;
using System;

public class Board : Node2D
{
    public const int BOARD_WIDTH = 10;
    public const int BOARD_HEIGHT = 20;
    public static Tile[,] collisionMap;

    public static void Initialize()
    {
        collisionMap = new Tile[BOARD_HEIGHT, BOARD_WIDTH];
    }

    public static bool WillLocationCollide(int x, int y)
    {
        if (x < 0 || x >= BOARD_WIDTH || y < 0 || y >= BOARD_HEIGHT)
        {
            return true;
        }

        return collisionMap[y, x] != null;
    }

    public static void PlaceBrick(Brick brick)
    {
        foreach (Tile tile in brick.tiles)
        {
            Node parent = tile.GetParent().GetParent();
            tile.GetParent().RemoveChild(tile);
            parent.AddChild(tile);
            collisionMap[tile.y, tile.x] = tile;
        }

        RemoveRows();
    }

    public static void RemoveRows()
    {
        bool scannedAllRows = true;
        for (int i = BOARD_HEIGHT - 1; i >= 0; i--)
        {
            if (IsRowFull(i))
            {
                RemoveRow(i);
                scannedAllRows = false;
                break;
            }
        }

        if (!scannedAllRows)
        {
            RemoveRows();
        }
    }

    private static bool IsRowFull(int i)
    {
        for (int j = 0; j < BOARD_WIDTH; j++)
        {
            if (collisionMap[i, j] == null)
            {
                return false;
            }
        }

        return true;
    }

    private static void RemoveRow(int i)
    {
        for (int j = 0; j < BOARD_WIDTH; j++)
        {
            collisionMap[i, j].Remove();
            collisionMap[i, j] = null;
        }

        DropBoardFrom(i);
    }

    private static void DropBoardFrom(int i)
    {
        for (i = BOARD_HEIGHT - 1; i >= 1; i--)
        {
            for (int j = 0; j < BOARD_WIDTH; j++)
            {
                collisionMap[i, j] = collisionMap[i - 1, j];
                Tile tile = collisionMap[i, j];
                if (tile != null)
                {
                    tile.Move(0, 1);
                }
            }
        }
    }
}
