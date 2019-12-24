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
    }

    public static void RemoveRows()
    {
    }
}
