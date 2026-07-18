using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder
{
    public int ColorsForLevel(int level) => Mathf.Clamp(3 + level, 3, 6);

    public List<List<BallColor>> Build(int level, int capacity, out int tubeCount)
    {
        int colors = ColorsForLevel(level);
        tubeCount = colors + 2;

        var tubes = new List<List<BallColor>>();
        for (int c = 0; c < colors; c++)
        {
            var full = new List<BallColor>();
            for (int i = 0; i < capacity; i++) full.Add((BallColor)c);
            tubes.Add(full);
        }
        for (int e = 0; e < tubeCount - colors; e++)
            tubes.Add(new List<BallColor>());

        Scramble(tubes, capacity, 40 + level * 10);
        return tubes;
    }

    private void Scramble(List<List<BallColor>> tubes, int capacity, int moves)
    {
        if (moves <= 0) return;

        int from = Random.Range(0, tubes.Count);
        int to = Random.Range(0, tubes.Count);
        if (from != to && tubes[from].Count > 0 && tubes[to].Count < capacity)
        {
            var ball = tubes[from][tubes[from].Count - 1];
            tubes[from].RemoveAt(tubes[from].Count - 1);
            tubes[to].Add(ball);
        }
        Scramble(tubes, capacity, moves - 1);
    }
}
