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

    public void Execute()
    {
        moved = from.Pop();
        to.Push(moved);
    }

    public void Undo()
    {
        Ball back = to.Pop();
        from.Push(back);
    }
}
