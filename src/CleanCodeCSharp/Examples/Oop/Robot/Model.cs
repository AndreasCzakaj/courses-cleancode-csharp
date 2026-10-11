namespace CleanCodeCSharp.Examples.Oop.Robot;

public record Position(int X, int Y, int Z);

public record Vector(int X, int Y, int Z);

public delegate void MovementListener(Position newPosition);
