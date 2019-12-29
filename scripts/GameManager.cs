using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node
{
    public Brick currentBrick;
    public PackedScene[] nextBrickScenes;
    public int currentBrickIndex = 0;
    public Board board;
    public PackedScene boardScene = (PackedScene)GD.Load("res://Board.tscn");

    public PackedScene IBrickScene = (PackedScene)GD.Load("res://bricks/IBrick.tscn");
    public PackedScene JBrickScene = (PackedScene)GD.Load("res://bricks/JBrick.tscn");
    public PackedScene LBrickScene = (PackedScene)GD.Load("res://bricks/LBrick.tscn");
    public PackedScene OBrickScene = (PackedScene)GD.Load("res://bricks/OBrick.tscn");
    public PackedScene SBrickScene = (PackedScene)GD.Load("res://bricks/SBrick.tscn");
    public PackedScene TBrickScene = (PackedScene)GD.Load("res://bricks/TBrick.tscn");
    public PackedScene ZBrickScene = (PackedScene)GD.Load("res://bricks/ZBrick.tscn");

    int dropSpeed = 50;
    int dropCooldown = 50;

    public override void _Ready()
    {
        board = (Board)boardScene.Instance();

        GetTree().GetRoot().GetNode<Node>("World").AddChild(board);
        Board.Initialize();
        nextBrickScenes = new PackedScene[] { IBrickScene, JBrickScene, LBrickScene, OBrickScene, SBrickScene, TBrickScene, ZBrickScene, IBrickScene, JBrickScene, LBrickScene, OBrickScene, SBrickScene, TBrickScene, ZBrickScene };
        ShuffleNextBrickScenes(true);
        SpawnBrick();
    }

    public void SpawnBrick()
    {
        if (currentBrickIndex >= nextBrickScenes.Length)
        {
            ShuffleNextBrickScenes();
            currentBrickIndex = 0;
        }

        currentBrick = (Brick)nextBrickScenes[currentBrickIndex++].Instance();
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
            bool dropped = currentBrick.Drop();
            if (dropped)
            {
                Score.AddDropScore(true);
                dropCooldown = dropSpeed;
            }
        }
        else if (Input.IsActionJustPressed("DROP"))
        {
            int distance = currentBrick.FastDrop();
            Score.AddDropScore(false, distance);
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

    private void ShuffleNextBrickScenes(bool initialShuffle = false)
    {
        int size = nextBrickScenes.Length;
        int i = initialShuffle ? 0 : 1;

        for (; i < size; i++)
        {
            Random rand = new Random();
            int randInt = rand.Next(i, size);
            Swap(nextBrickScenes, i, randInt);
        }
    }

    private void Swap(PackedScene[] scenes, int i, int j)
    {
        PackedScene scene = scenes[i];
        scenes[i] = scenes[j];
        scenes[j] = scene;
    }
}
