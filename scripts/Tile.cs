using Godot;
using System;

public class Tile : Node2D
{
    public int x = 0;
    public int y = 0;
    public const float RENDER_OFFSET_X = 1.5f;
    public const float RENDER_OFFSET_Y = 1.5f;

    public void Move(int x, int y)
    {
        this.x += x;
        this.y += y;
        Rerender();
    }

    public void MoveTo(int x, int y)
    {
        this.x = x;
        this.y = y;
        Rerender();
    }

    public void Remove()
    {
        Hide();
        QueueFree();
    }

    public void Rerender()
    {
        SetPosition(new Vector2(x + RENDER_OFFSET_X, y + RENDER_OFFSET_Y) * 16);
    }
}
