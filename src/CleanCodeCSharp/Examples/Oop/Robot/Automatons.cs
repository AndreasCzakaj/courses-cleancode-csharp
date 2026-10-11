namespace CleanCodeCSharp.Examples.Oop.Robot;

// OOP: objects HIDE their data and expose behavior

public interface IAutomaton
{
    void Move(Vector v, MovementListener listener);

    void Startup();

    void Shutdown();

    //void FindSarahConnor();
}

public class Robot(Position initialPosition) : IAutomaton
{
    private Position currentPosition = initialPosition;
    private int battery = 5000;
    private bool started;

    public void Move(Vector v, MovementListener listener)
    {
        if (!started)
        {
            return;
        }
        currentPosition = new Position(currentPosition.X + v.X, currentPosition.Y + v.Y, currentPosition.Z + v.Z);
        listener(currentPosition);
    }

    public void Startup()
    {
        if (battery > 3500)
        {
            // do stuff
            started = true;
        }
    }

    public void Shutdown()
    {
        started = false;
    }
}

public class Drone(Position initialPosition) : IAutomaton
{
    private Position currentPosition = initialPosition;

    public void Move(Vector v, MovementListener listener)
    {
        //...
        listener(currentPosition);
    }

    public void Startup()
    {
    }

    public void Shutdown()
    {
    }
}

public class VacuumCleaner : IAutomaton
{
    public void Move(Vector v, MovementListener listener)
    {
    }

    public void Startup()
    {
    }

    public void Shutdown()
    {
    }
}
