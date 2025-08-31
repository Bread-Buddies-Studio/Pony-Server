using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Enums;

public enum ConnectionFail
{
    // Log In //
    IncorrectPassword,
    AccountNotFound,
    InvalidTagLine,
    // Sign Up //
    AccountAlreadyExists,
}