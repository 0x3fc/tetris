using Godot;
using System;

public class IBrick : Brick
{
    public override void Initialize()
    {
        tiles = new Tile[4];
        tiles[0] = MakeTile(2, 0);
        tiles[1] = MakeTile(2, 1);
        tiles[2] = MakeTile(2, 2);
        tiles[3] = MakeTile(2, 3);
    }
}
