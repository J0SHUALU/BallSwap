// Command pattern: interface for an action that can run and undo itself.
public interface IMove
{
    void Execute();
    void Undo();
}
