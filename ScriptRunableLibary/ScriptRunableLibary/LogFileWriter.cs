using ScriptRunableLibary.Log;
using System.Threading.Tasks;

namespace ScriptRunableLibary
{
    internal class LogFileWriter
    {
        public TaskStatus WriteLog(LogTask[] logTasks)
        {
            return TaskStatus.RanToCompletion;
        }
    }
}
