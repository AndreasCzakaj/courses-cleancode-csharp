namespace CleanCodeCSharp.Examples.Oop.Robot;

// Procedural: data structures EXPOSE their data and have no behavior

public abstract class AutomatonDto
{
    public bool Started { get; set; }
    public Position? CurrentPosition { get; set; }
    public int Battery { get; set; }
}

public class RobotDto : AutomatonDto;

public class DroneDto : AutomatonDto
{
    public List<byte>? CameraStream { get; set; }
}
