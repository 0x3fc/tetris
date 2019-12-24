using Godot;
using System;

public class GameManager : Node
{
    public Brick currentBrick;
    public Brick[] nextBricks;
    public Board board;
    public PackedScene boardScene = (PackedScene)GD.Load("res://Board.tscn");
    public PackedScene testBrickScene = (PackedScene)GD.Load("res://Brick.tscn");

    int dropSpeed = 50;
    int dropCooldown = 50;

    public override void _Ready()
    {
        board = (Board)boardScene.Instance();

        GetTree().GetRoot().GetNode<Node>("World").AddChild(board);
        Board.Initialize();
        SpawnBrick();
    }

    public void SpawnBrick()
    {
        GD.Print("Spawn brick");
        currentBrick = (Brick)testBrickScene.Instance();
        board.AddChild(currentBrick);
    }

    public override void _Process(float delta)
    {
        if (dropCooldown > 0)
        {
            dropCooldown--;
            return;
        }

        currentBrick.Drop();
        dropCooldown = dropSpeed;
    }
}
