using System.Collections.Generic;
using UnityEngine;

// State pattern: a tube is always in one of these states.
public enum TubeState { Idle, Selected, Sorted }

// Encapsulation: the ball list is private; the pour rules live inside this class.
// Also implements ISelectable (abstraction) and holds a TubeState (state pattern).
public class Tube : MonoBehaviour, ISelectable
{
    public int Capacity = 4;
    public TubeState State { get; private set; } = TubeState.Idle;

    private readonly List<Ball> balls = new List<Ball>();

    public bool IsEmpty => balls.Count == 0;
    public bool IsFull => balls.Count >= Capacity;
    public Ball TopBall => IsEmpty ? null : balls[balls.Count - 1];

    // The core game rule: when a ball is allowed to enter this tube.
    public bool CanReceive(Ball ball)
    {
        if (IsFull) return false;
        if (IsEmpty) return true;
        return TopBall.Color == ball.Color;
    }

    public void Push(Ball ball)
    {
        ball.transform.SetParent(transform);
        balls.Add(ball);
        RestackVisuals();
        RefreshState();
    }

    public void PushWithArc(Ball ball, float travelHeight)
    {
        ball.transform.SetParent(transform);
        balls.Add(ball);
        for (int i = 0; i < balls.Count - 1; i++)
        {
            Vector3 pos = transform.position + Vector3.up * (-1.5f + i * 0.75f);
            balls[i].MoveTo(pos);
        }
        Vector3 topPos = transform.position + Vector3.up * (-1.5f + (balls.Count - 1) * 0.75f);
        ball.ArcTo(topPos, travelHeight);
        RefreshState();
    }

    public Ball Pop()
    {
        if (IsEmpty) return null;
        Ball top = balls[balls.Count - 1];
        balls.RemoveAt(balls.Count - 1);
        top.transform.SetParent(null);
        RefreshState();
        return top;
    }

    // Returns just the colours, used by the solver.
    public List<BallColor> Snapshot()
    {
        List<BallColor> colors = new List<BallColor>();
        foreach (Ball b in balls) colors.Add(b.Color);
        return colors;
    }

    private void RestackVisuals()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Vector3 pos = transform.position + Vector3.up * (-1.5f + i * 0.75f);
            balls[i].MoveTo(pos);
        }
    }

    // State pattern: recalculate the tube's state when its balls change.
    private void RefreshState()
    {
        if (IsSortedComplete()) State = TubeState.Sorted;
        else if (State == TubeState.Sorted) State = TubeState.Idle;
    }

    public bool IsSortedComplete()
    {
        if (IsEmpty) return true;
        if (!IsFull) return false;
        BallColor first = balls[0].Color;
        foreach (Ball b in balls)
            if (b.Color != first) return false;
        return true;
    }

    public void OnSelected()
    {
        State = TubeState.Selected;
        transform.position += Vector3.up * 0.3f;
        RestackVisuals();
    }

    public void OnDeselected()
    {
        if (State == TubeState.Selected) State = TubeState.Idle;
        Vector3 p = transform.position;
        p.y = 0f;
        transform.position = p;
        RestackVisuals();
    }
}
