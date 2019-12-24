using Godot;
using System;

public class Board : Node2D
{
    public const int BOARD_WIDTH = 10;
    public const int BOARD_HEIGHT = 20;
    public static bool[,] collisionMap;

    public static void Initialize()
    {
        collisionMap = new bool[BOARD_HEIGHT, BOARD_WIDTH];
    }

    public static bool WillLocationCollide(int x, int y)
    {
        if (x < 0 || x >= BOARD_WIDTH || y < 0 || y >= BOARD_HEIGHT)
        {
            return true;
        }

        return collisionMap[y, x];
    }

    public static bool IsBrickCollided(Brick brick)
    {
        int xOffset = brick.x;
        int yOffset = brick.y;

        for (int i = 0; i < Brick.DIMENSION; i++)
        {
            for (int j = 0; j < Brick.DIMENSION; j++)
            {
                if (brick.collisionMap[i, j] && collisionMap[i + yOffset, j + xOffset])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool IsBrickPlacable(Brick brick)
    {
        int xOffset = brick.x;
        int yOffset = brick.y;

        for (int i = 0; i < Brick.DIMENSION; i++)
        {
            for (int j = 0; j < Brick.DIMENSION; j++)
            {
                if (brick.collisionMap[i, j] && i + yOffset - 1 < Brick.DIMENSION && collisionMap[i + yOffset - 1, j + xOffset])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static void PlaceBrick(Brick brick)
    {
    }

    public static void RemoveRows()
    {
    }
}
