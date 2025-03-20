using System;
using Antlr4.Runtime;

namespace AltaSoft.Simpra;

public sealed class SimpraException : Exception
{
    public override string Message { get; }

    /// <summary>
    /// The line number
    /// line = 1...n
    /// </summary>
    public int? Line { get; }

    /// <summary>
    /// The column number
    /// column = 1...n
    /// </summary>
    public int? Column { get; }

    public SimpraException(ParserRuleContext context, string message)
    {
        Line = context.Start?.Line;
        Column = context.Start?.Column + 1;

        Message = $"Line:{Line}, Column: {Column}. {message}";
    }
}
