using System;

namespace LemballEditor.LegacyModels
{
    /// <summary>
    /// Exception is thrown when an attempt has been made to append a level to a level group
    /// that has reached its maximum capacity
    /// </summary>
    internal class LevelGroupFullException : Exception
    {
    }
}
