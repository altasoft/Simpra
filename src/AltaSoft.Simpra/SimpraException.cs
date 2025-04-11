using System;
using Antlr4.Runtime;

namespace AltaSoft.Simpra;

/// <summary>
/// Represents a custom exception used within the Simpra DSL parsing system.
/// Includes line and column information from the parsing context.
/// </summary>
public sealed class SimpraException : Exception
{
    /// <summary>
    /// Gets the error message that describes the current exception,
    /// including line and column details.
    /// </summary>
    public override string Message { get; }

    /// <summary>
    /// Gets the line number in the parsed input where the exception occurred.
    /// Line numbers start from 1.
    /// </summary>
    public int? Line { get; }

    /// <summary>
    /// Gets the column number in the parsed input where the exception occurred.
    /// Column numbers start from 1.
    /// </summary>
    public int? Column { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpraException"/> class using the specified parsing context and error message.
    /// </summary>
    /// <param name="context">The parsing context that provides line and column information.</param>
    /// <param name="message">The error message to include in the exception.</param>
    public SimpraException(ParserRuleContext context, string message)
    {
        Line = context.Start?.Line;
        Column = context.Start?.Column + 1;

        Message = $"Line:{Line}, Column: {Column}. {message}";
    }
}
