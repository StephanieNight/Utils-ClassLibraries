using ScriptRunableLibary.Interfaces;
using System;
using System.Linq;

namespace ScriptRunableLibary
{
	public class ScriptHandler
	{
		public static IRunnable[] GetRunableScripts()
		{
			var type = typeof(IRunnable);
			var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());
			var allRunners = (from t in types
						  where t.GetInterfaces().Contains(typeof(IRunnable))
							 && t.GetConstructor(Type.EmptyTypes) != null							 
						  select Activator.CreateInstance(t) as IRunnable).ToArray();
			return (from r in allRunners where r.IsEnabled == true select r).ToArray();
		}
	}
}