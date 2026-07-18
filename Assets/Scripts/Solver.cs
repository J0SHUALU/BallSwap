using System.Collections.Generic;

public class Solver
{
    public struct Move { public int From; public int To; }

    public bool TryFindNextMove(BoardState start, out Move nextMove)
    {
        nextMove = new Move();
        var visited = new HashSet<string>();
        var queue = new Queue<BoardState>();
        var firstMoveOf = new Dictionary<string, Move>();

        visited.Add(start.Key());
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            BoardState current = queue.Dequeue();
            if (current.IsSolved())
            {
                if (firstMoveOf.TryGetValue(current.Key(), out nextMove))
                    return true;
                return false;
            }

            for (int f = 0; f < current.Tubes.Count; f++)
            {
                if (current.Tubes[f].Count == 0) continue;
                BallColor top = current.Tubes[f][current.Tubes[f].Count - 1];

                for (int t = 0; t < current.Tubes.Count; t++)
                {
                    if (f == t) continue;
                    var dest = current.Tubes[t];
                    if (dest.Count >= current.Capacity) continue;
                    if (dest.Count > 0 && dest[dest.Count - 1] != top) continue;

                    BoardState next = current.Clone();
                    var ball = next.Tubes[f][next.Tubes[f].Count - 1];
                    next.Tubes[f].RemoveAt(next.Tubes[f].Count - 1);
                    next.Tubes[t].Add(ball);

                    string key = next.Key();
                    if (visited.Contains(key)) continue;
                    visited.Add(key);

                    Move thisMove = new Move { From = f, To = t };
                    firstMoveOf[key] = firstMoveOf.TryGetValue(current.Key(), out Move prev)
                        ? prev
                        : thisMove;

                    queue.Enqueue(next);
                }
            }
        }
        return false;
    }
}
