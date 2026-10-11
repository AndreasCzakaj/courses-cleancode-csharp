namespace CleanCodeCSharp.Examples.Oop.Robot;

public class ControllerProceduralImpl
{
    public void StartProcedurally(Func<Position, AutomatonDto> factory)
    {
        var automaton = factory(new Position(X: 1, Y: 2, Z: 0));

        if (!automaton.Started)
        {
            if (automaton is DroneDto drone)
            {
                if (drone.Battery > 4500)
                {
                    drone.CameraStream = new List<byte>();
                    drone.Started = true;
                }
            }
            else if (automaton is RobotDto)
            {
                if (automaton.Battery > 3500)
                {
                    // do stuff
                    automaton.Started = true;
                }
            }
        }
    }

    // it's easy to add this new function
    public void MoveProcedurally(Func<Position, AutomatonDto> factory)
    {
        var automaton = factory(new Position(X: 1, Y: 2, Z: 0));

        if (automaton.Started)
        {
            if (automaton is DroneDto)
            {
                // ...
            }
            else if (automaton is RobotDto)
            {
                // ...
            }
            // adding a new Dto class will require adding another case
            // to StartProcedurally and all the other functions
            // => just like the car type disaster
        }
    }
}
