using Godot;
using System;

public class Brick : Node2D
{
    public const int DIMENSION = 4;

    public int x = 0;
    public int y = 0;
    public bool[,] collisionMap;
    public Tile[] tiles;
    public PackedScene tileScene = (PackedScene)GD.Load("res://Tile.tscn");

    public override void _Ready()
    {
        Tile t = (Tile)tileScene.Instance();
        tiles = new Tile[] { t };

        foreach (Tile tile in tiles)
        {
            AddChild(tile);
        }
    }

    public void Rotate()
    {

    }

    public void Drop()
    {
        // if (Board.IsBrickPlacable(this))
        // {
        //     Board.PlaceBrick(this);
        //     return;
        // }

        foreach (Tile tile in tiles)
        {
            tile.Drop();
        }
    }
}
