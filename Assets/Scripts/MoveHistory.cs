using System.Collections.Generic;

public class MoveHistory
{
    private readonly Stack<IMove> history = new Stack<IMove>();

    public int Count => history.Count;

    public void Record(IMove move) => history.Push(move);

    public bool UndoLast()
    {
        if (history.Count == 0) return false;
        history.Pop().Undo();
        return true;
    }

    public void Clear() => history.Clear();
}
