using Godot;
using System;

public class Next : Control
{
    Brick brick;

    public Brick PopBrick()
    {
        RemoveChild(brick);
        return brick;
    }

    public void PushBrick(Brick brick)
    {
        this.brick = brick;
        AddChild(brick);
    }
}
