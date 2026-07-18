using System.Collections.Generic;
using System.Text;

public class BoardState
{
    public List<List<BallColor>> Tubes;
    public int Capacity;

    public BoardState(List<List<BallColor>> tubes, int capacity)
    {
        Tubes = tubes;
        Capacity = capacity;
    }

    public bool IsSolved()
    {
        foreach (var t in Tubes)
        {
            if (t.Count == 0) continue;
            if (t.Count != Capacity) return false;
            for (int i = 1; i < t.Count; i++)
                if (t[i] != t[0]) return false;
        }
        return true;
    }

    public string Key()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var t in Tubes)
        {
            foreach (var c in t) sb.Append((int)c);
            sb.Append('|');
        }
        return sb.ToString();
    }

    public BoardState Clone()
    {
        var copy = new List<List<BallColor>>();
        foreach (var t in Tubes) copy.Add(new List<BallColor>(t));
        return new BoardState(copy, Capacity);
    }
}