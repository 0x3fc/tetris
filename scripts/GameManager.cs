using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node
{
    public Brick currentBrick;
    public PackedScene[] nextBrickScenes;
    public int currentBrickIndex = 0;
    public Board board;

    public PackedScene IBrickScene = (PackedScene)GD.Load("res://bricks/IBrick.tscn");
    public PackedScene JBrickScene = (PackedScene)GD.Load("res://bricks/JBrick.tscn");
    public PackedScene LBrickScene = (PackedScene)GD.Load("res://bricks/LBrick.tscn");
    public PackedScene OBrickScene = (PackedScene)GD.Load("res://bricks/OBrick.tscn");
    public PackedScene SBrickScene = (PackedScene)GD.Load("res://bricks/SBrick.tscn");
    public PackedScene TBrickScene = (PackedScene)GD.Load("res://bricks/TBrick.tscn");
    public PackedScene ZBrickScene = (PackedScene)GD.Load("res://bricks/ZBrick.tscn");

    const int INITIAL_DROP_SPEED = 50;
    const int MIN_DROP_SPEED = 30;
    int dropSpeed = INITIAL_DROP_SPEED;
    int dropCooldown = INITIAL_DROP_SPEED;

    const int DROP_PHRASE_INTERVAL = 3000;
    int dropPhraseCooldown = DROP_PHRASE_INTERVAL;

    private Next next;
    private bool inGame = false;

    public override void _Ready()
    {
        nextBrickScenes = new PackedScene[] { IBrickScene, JBrickScene, LBrickScene, OBrickScene, SBrickScene, TBrickScene, ZBrickScene, IBrickScene, JBrickScene, LBrickScene, OBrickScene, SBrickScene, TBrickScene, ZBrickScene };
    }

    public void Start()
    {
        Viewport root = GetTree().GetRoot();
        board = root.GetNode<Board>("World/Board");
        Board.Initialize();
        next = root.GetNode<Next>("World/Next");
        root.GetNode<Label>("World/StartIndicator").Hide();
        ShuffleNextBrickScenes(true);

        currentBrickIndex = 0;
        dropSpeed = INITIAL_DROP_SPEED;
        dropCooldown = INITIAL_DROP_SPEED;
        dropPhraseCooldown = DROP_PHRASE_INTERVAL;
        Score.Clear();

        if (currentBrick != null)
        {
            currentBrick.Remove();
        }
        next.Clear();
        SpawnNextBrick();
        SpawnBrick();
        inGame = true;
    }

    public void SpawnBrick()
    {
        if (currentBrickIndex >= nextBrickScenes.Length)
        {
            ShuffleNextBrickScenes();
            currentBrickIndex = 0;
        }

        currentBrick = next.PopBrick();
        SpawnNextBrick();
        board.AddChild(currentBrick);
    }

    public void SpawnNextBrick()
    {
        next.PushBrick((Brick)nextBrickScenes[currentBrickIndex++].Instance());
    }

    public override void _Process(float delta)
    {
        if (!inGame)
        {
            if (Input.IsActionJustPressed("START"))
            {
                Start();
            }
            return;
        }

        SpawnBrickCheck();
        HandlePlayerInput();

        if (dropPhraseCooldown > 0)
        {
            dropPhraseCooldown--;
        }
        else
        {
            dropSpeed = Math.Max(MIN_DROP_SPEED, dropSpeed - 1);
            dropPhraseCooldown = DROP_PHRASE_INTERVAL;
        }

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
        else if (Input.IsActionJustPressed("RESTART"))
        {
            Start();
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
