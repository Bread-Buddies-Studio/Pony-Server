using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Exceptions;

public class AlreadyStoredException(string message = "Element Already Stored!") : Exception(message) {  }

public class InvalidTaglineExtension(string message = "Invalid Tagline!") : Exception(message) { }