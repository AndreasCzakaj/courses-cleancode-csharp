namespace CleanCodeCSharp.Examples.Oop.Robot;

public class ControllerOopImpl
{
    public void DoIt(Func<Position, IAutomaton> factory)
    {
        var automaton = factory(new Position(X: 1, Y: 2, Z: 0));

        // this works for all implementations
        // adding a new IAutomaton implementation is non-breaking
        automaton.Startup();
        automaton.Move(new Vector(X: 2, Y: 7, Z: 1), newPosition => { });
        automaton.Shutdown();

        // but adding a new method to the interface
        // will break all existing impl classes
    }
}
