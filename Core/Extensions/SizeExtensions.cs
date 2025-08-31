using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace Core.Extensions;

public static class SizeExtensions
{
    public static double[] ToDoubleArray(this Size size) => [size.Width, size.Height];
    public static Size FromDoubleArray(double[] array) => new(array[0], array[1]);
}
