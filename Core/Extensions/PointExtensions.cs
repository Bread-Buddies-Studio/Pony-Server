using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace Core.Extensions;

public static class PointExtensions
{
    public static Point FromDoubleArray(double[] axis) => new(axis[0], axis[1]);
    public static double[] ToDoubleArray(this Point p1) => [p1.X, p1.Y];
    public static Point ToDirectional(this Point p1, Point p2) => p2 - p1;
    /// <summary>
    /// Gets the length of a directional vector
    /// </summary>
    /// <param name="position">The directional vector</param>
    /// <returns>The length of the directional vector</returns>
    public static double GetMagnitude(this Point position)
    {
        // Conditions //
        if (position.X is 0 && position.Y is 0)
            return 0;
        else if (position.X is 0 && position.Y is not 0)
            return Math.Abs(position.Y);
        else if (position.Y is 0 && position.X is not 0)
            return Math.Abs(position.X);
        // If both axis aren't 0 //
        return Math.Sqrt(Math.Pow(position.X, 2) + Math.Pow(position.Y, 2));
    }
    /// <summary>
    /// Sets the length of a directional vector
    /// </summary>
    /// <param name="position">The directional vector</param>
    /// <param name="magnitude">The desired length of the directional vector</param>
    /// <returns>The updated directional vector</returns>
    public static Point SetMagnitude(this Point position, double magnitude)
    {
        // Sign //
        double sign;
        // Conditions //
        if (position.X is 0 && position.Y is 0)
            return position;
        else if (position.X is 0 && position.Y is not 0)
        {
            sign = Math.Abs(position.Y) / position.Y;
            position = new Point(0, magnitude * sign);
            return position;
        }
        else if (position.Y is 0 && position.X is not 0)
        {
            sign = Math.Abs(position.X) / position.X;
            position = new Point(magnitude * sign, 0);
            return position;
        }
        // If both axis aren't 0 //
        sign = Math.Abs(position.X) / position.X;
        double angle = Math.Atan(position.Y / position.X);
        double x = magnitude * Math.Cos(angle) * sign;
        double y = magnitude * Math.Sin(angle) * sign;

        position = new(x, y);
        // return new Position //
        return position;
    }
    /// <summary>
    /// Returns a point between <paramref name="p1"/> and <paramref name="p2"/>
    /// from <paramref name="alpha"/>. 0 is <paramref name="p1"/>, 1 is <paramref name="p2"/>
    /// </summary>
    /// <param name="p1">First Point</param>
    /// <param name="p2">Second Point</param>
    /// <param name="alpha">Distance travelled between points.</param>
    /// <returns>The Interpolated <paramref name="p1"/></returns>
    public static Point Lerp(this Point p1, Point p2, double alpha)
    {
        double xPos = (p2.X - p1.X) * alpha; // 2.5
        double yPos = (p2.Y - p1.Y) * alpha; // 0

        return new(p1.X + xPos, p1.Y + yPos);
    }
}
