// Command pattern: one pour, stored as an object that can undo itself.
public class PourCommand : IMove
{
    private readonly Tube from;
    private readonly Tube to;
    private Ball moved;

    public PourCommand(Tube from, Tube to)
    {
        this.from = from;
        this.to = to;
    }

    public bool IsValid()
    {
        if (from == null || to == null || from == to) return false;
        if (from.IsEmpty) return false;
        return to.CanReceive(from.TopBall);
    }

    // Do the move, remembering the ball so it can be put back.
    public void Execute()
    {
        moved = from.Pop();
        to.PushWithArc(moved, 4f);
    }

    // Reverse the move.
    public void Undo()
    {
        Ball back = to.Pop();
        from.Push(back);
    }
}
