using ScriptRunableLibary.Log;
using System.Threading.Tasks;

namespace ScriptRunableLibary.Interfaces
{
	public interface IRunnable
    {
        public RuntimeLogCollection GetRuntimeLog();
        public int Run();
        public string Scriptname { get; }
        public bool IsEnabled { get; }
    }
}
