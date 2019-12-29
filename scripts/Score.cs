using Godot;
using System;

public class Score : Control
{
    private static Label label;

    public static int score = 0;
    public const int BASE_ROW_CLEAR_POINT = 100;
    public const int SOFT_DROP_SCORE = 1;
    public const int HARD_DROP_SCORE = 2;

    public override void _Ready()
    {
        label = GetNode<Label>("/root/World/Score/Label");
    }

    public static void AddClearScore(int clearedRows)
    {
        if (clearedRows <= 0)
        {
            return;
        }

        score += BASE_ROW_CLEAR_POINT * (int)Math.Pow(2, clearedRows);
        Rerender();
    }

    public static void AddDropScore(bool isSoftDrop, int distance = 1)
    {
        score += isSoftDrop ? SOFT_DROP_SCORE : HARD_DROP_SCORE * distance;
        Rerender();
    }

    public static void Clear()
    {
        score = 0;
        Rerender();
    }

    private static void Rerender()
    {
        label.SetText(score.ToString());
    }
}
