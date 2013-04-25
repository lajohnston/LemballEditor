using System;
using System.Collections.Generic;
using System.Text;

namespace LemballEditor.Model
{
    /// <summary>
    /// Exception is thrown when an attempt has been made to append a level to a level group
    /// that has reached its maximum capacity
    /// </summary>
    class LevelGroupFullException : Exception
    {
    }
}
