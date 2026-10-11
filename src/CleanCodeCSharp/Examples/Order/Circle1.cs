namespace CleanCodeCSharp.Examples.Order;

public class Circle1
{
    private readonly double radius;

    private double Diameter()
    {
        return radius * 2;
    }

    public double Circumference()
    {
        return Diameter() * Math.PI;
    }

    public Circle1(double radius)
    {
        this.radius = radius;
    }
}
