using Godot;
using System;

public class Tile : Node2D
{
    public int x = 0;
    public int y = 0;

    public void Move(int x, int y)
    {
        this.x += x;
        this.y += y;
        Rerender();
    }

    public void Remove()
    {
        Hide();
        QueueFree();
    }

    private void Rerender()
    {
        SetPosition(new Vector2(x + 0.5f, y) * 16);
    }
}
