using Godot;
using System;

public class Tile : Node2D
{
    public int x = 0;
    public int y = 0;

    public void Drop()
    {
        GD.Print(y);
        y += 1;
        SetPosition(new Vector2(x + 0.5f, y) * 16); // TODO: Pixel size
    }
}
