using Godot;
using System;

public class HighScore : Control
{
    private int[] highscores = new int[3];
    private const string HIGHSCORE_PATH = "user://highscore.save";
    private Label first;
    private Label second;
    private Label thrid;

    public override void _Ready()
    {
        first = GetNode<Label>("First");
        second = GetNode<Label>("Second");
        thrid = GetNode<Label>("Third");
        Load();
        Rerender();
    }

    public void AddScore(int score)
    {
        if (score >= highscores[0])
        {
            highscores[2] = highscores[1];
            highscores[1] = highscores[0];
            highscores[0] = score;
        }
        else if (score >= highscores[1])
        {
            highscores[2] = highscores[1];
            highscores[1] = score;
        }
        else if (score >= highscores[2])
        {
            highscores[2] = score;
        }
        else
        {
            return;
        }

        WriteHighScores();
        Rerender();
    }

    private void Load()
    {
        File file = new File();
        if (!file.FileExists(HIGHSCORE_PATH))
        {
            WriteHighScores();
            return;
        }

        file.Open(HIGHSCORE_PATH, (int)File.ModeFlags.Read);
        string[] scores = file.GetLine().Split(' ');
        for (int i = 0; i < 3; i++)
        {
            highscores[i] = scores[i].ToInt();
        }
        file.Close();
    }

    private void WriteHighScores()
    {
        File file = new File();
        file.Open(HIGHSCORE_PATH, (int)File.ModeFlags.Write);
        file.StoreLine(string.Format("{0} {1} {2}", highscores[0], highscores[1], highscores[2]));
        file.Close();
    }

    private void Rerender()
    {
        first.SetText(highscores[0].ToString());
        second.SetText(highscores[1].ToString());
        thrid.SetText(highscores[2].ToString());
    }
}
