using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions;

public static partial class DoubleExtensions
{
    public static double Clamp01(this double number) => Math.Clamp(number, 0, 1);
}
