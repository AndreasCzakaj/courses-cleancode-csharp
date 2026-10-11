namespace CleanCodeCSharp.Examples.Order;

public class Circle2
{
    private readonly double radius;

    public Circle2(double radius)
    {
        this.radius = radius;
    }

    public double Circumference()
    {
        return Diameter() * Math.PI;
    }

    private double Diameter()
    {
        return radius * 2;
    }
}
