using Godot;
using System;

public class Score : Control
{
    public static int score = 0;
    public const int BASE_ROW_CLEAR_POINT = 100;
    public const int SOFT_DROP_SCORE = 2;
    public const int HARD_DROP_SCORE = 4;

    public static void AddClearScore(int clearedRows)
    {
        if (clearedRows <= 0)
        {
            return;
        }

        score += BASE_ROW_CLEAR_POINT * (int)Math.Pow(2, clearedRows);
    }

    public static void AddDropScore(bool isSoftDrop, int distance = 1)
    {
        score += isSoftDrop ? SOFT_DROP_SCORE : HARD_DROP_SCORE * distance;
    }
}
