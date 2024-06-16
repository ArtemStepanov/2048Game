namespace _2048Game.Tests.Helpers;

/// <summary>
/// Helper class to capture console output for testing
/// </summary>
public sealed class ConsoleOutput : IDisposable
{
    // todo: заменить на StringBuilder
    private readonly TextWriter _originalOutput;
    private readonly StringWriter _stringWriter;

    public ConsoleOutput()
    {
        _stringWriter = new StringWriter();
        _originalOutput = Console.Out;
        Console.SetOut(_stringWriter);
    }


    public void Dispose()
    {
        Console.SetOut(_originalOutput);
        _stringWriter.Dispose();
    }

    public void Write(string message)
    {
        _stringWriter.Write(message);
    }

    public void WriteLine(string message)
    {
        _stringWriter.WriteLine(message);
    }

    public string GetOutput()
    {
        return _stringWriter.ToString();
    }

    public void Clear()
    {
        _stringWriter.GetStringBuilder().Clear();
    }
}