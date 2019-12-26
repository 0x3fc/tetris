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
        currentBrick = (Brick)testBrickScene.Instance();
        board.AddChild(currentBrick);
    }

    public override void _Process(float delta)
    {
        SpawnBrickCheck();
        HandlePlayerInput();

        if (dropCooldown > 0)
        {
            dropCooldown--;
            return;
        }

        currentBrick.Drop();
        dropCooldown = dropSpeed;
    }

    private void HandlePlayerInput()
    {
        if (Input.IsActionJustPressed("LEFT"))
        {
            currentBrick.Move(-1);
        }
        else if (Input.IsActionJustPressed("RIGHT"))
        {
            currentBrick.Move(1);
        }
        else if (Input.IsActionJustPressed("DOWN"))
        {
            currentBrick.Drop();
            dropCooldown = dropSpeed;
        }
        else if (Input.IsActionJustPressed("DROP"))
        {
            currentBrick.FastDrop();
            dropCooldown = dropSpeed;
        }
        else if (Input.IsActionJustPressed("ROTATE"))
        {
            currentBrick.Rotate();
        }
    }

    private void SpawnBrickCheck()
    {
        if (currentBrick.placed)
        {
            SpawnBrick();
        }
    }
}
