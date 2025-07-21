namespace TextLogger;
public class BasicTextLogger
{
    private string _Location;
    private bool _logToConsole = true; // set Default Behavior
    public BasicTextLogger(bool logTooConsole = true) : this(AppDomain.CurrentDomain.BaseDirectory,logTooConsole) { }
    public BasicTextLogger(string logFilePath, bool logToConsole = true)
    {
        _Location = $"{logFilePath}/{DateTime.Now:yyyy-MM-dd} - log.txt";
        _logToConsole = logToConsole;   
    }
    public void LogAction(string message, bool logMessageTooConsole = false)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

        // Create or append to the log file
        using (TextWriter textWriter = new StreamWriter(_Location, true))
        {
            textWriter.WriteLine(logEntry);
        }
        if (_logToConsole || logMessageTooConsole)
        {
            // Optional: Print the log message to the console
            Console.WriteLine(logEntry);
        }
    }
    public void LogError(string message, bool logMessageTooConsole = false)
    {
        LogAction($"ERROR : {message}", logMessageTooConsole);
    }
}
