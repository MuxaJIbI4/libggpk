using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibGGPK
{
	class Utils
	{
		public static FileStream OpenFile(string path, out bool isReadOnly)
		{
			isReadOnly = true;
			try
			{
				var ret = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
				isReadOnly = false;
				return ret;
			}
			catch (IOException)
			{
				// File can't be written to, since it's being used (either by the program, or by the game itself)
				return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
		}
	}
}
