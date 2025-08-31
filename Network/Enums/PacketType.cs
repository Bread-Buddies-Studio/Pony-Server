using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Enums;

public enum PacketType : byte
{
    PlayerUpdate = 0,
    ChatMessage = 1,
}