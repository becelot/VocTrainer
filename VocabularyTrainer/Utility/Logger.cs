using System;
using System.Diagnostics;

namespace VocabularyTrainer
{
	[DebuggerStepThrough]
	internal static class Logger
	{
		/// <summary>
		/// Writes line to trace
		/// </summary>
		public static void WriteLine(string line, int logLevel = 0)
		{
			WriteLine(line, "", logLevel);
		}

		/// <summary>
		/// Writes line to trace
		/// </summary>		
		public static void WriteLine(string line, string category, int logLevel = 0)
		{
			Trace.WriteLine(string.Format("[{0}]{1}: {2}", DateTime.Now.ToLongTimeString(), category, line));
		}
	}
}