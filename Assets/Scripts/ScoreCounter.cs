using System;

public class ScoreCounter
{
    private readonly int[] generationScore;
    private readonly int UFOScore;

    public event Action<int> ScoreChanged;

    public int CurrentScore { get; private set; }


    public ScoreCounter(int[] generationScore, int UFOScore) 
    {
        this.generationScore = generationScore;
        this.UFOScore = UFOScore;
    }

    private void AddScore(int value) 
    {
        CurrentScore += value;
        ScoreChanged?.Invoke(CurrentScore);
    }

    public void OnAsteroidDestroyed(int generation)
    {
        AddScore(generationScore[generation]);
    }

    public void OnUFODestroyed()
    {
        AddScore(UFOScore);
    }
}
