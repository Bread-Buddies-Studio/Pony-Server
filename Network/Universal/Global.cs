// Global Imports //
global using static Network.Universal.Global;
// Local Imports //
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Universal;

public static class Global
{
    public const int FPS = 60;
    public static int PortNum { get; set; } = 11_000;
    public const char DataSeperator = '|';
}