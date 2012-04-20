using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGGPK
{
	class FreeListMaker
	{
		/// <summary>
		/// Builds a linked list of FREE records by traversing FREE records in GGPK file
		/// </summary>
		/// <param name="recordOffsets">Map of record offsets in GGPK file</param>
		/// <returns>Linked list containing list of FREE records</returns>
		internal static LinkedList<FreeRecord> BuildFreeList(Dictionary<long, BaseRecord> recordOffsets)
		{
			LinkedList<FreeRecord> freeList = new LinkedList<FreeRecord>();

			// This offset is a directory, add it as a child of root and process all of it's entries
			GGPKRecord currentDirectory = recordOffsets[0] as GGPKRecord;
			FreeRecord currentFreeRecord = null;

			foreach (var item in currentDirectory.RecordOffsets)
			{
				if (recordOffsets[item] is FreeRecord)
				{
					currentFreeRecord = recordOffsets[item] as FreeRecord;
					break;
				}
			}

			if (currentFreeRecord == null)
				throw new Exception("Failed to find FREE record root in GGPK header");

			while(true)
			{
				freeList.AddLast(currentFreeRecord);
				long nextFreeOFfset = currentFreeRecord.NextFreeOffset;

				if (nextFreeOFfset == 0)
				{
					break;
				}

				if (!recordOffsets.ContainsKey(nextFreeOFfset))
					throw new Exception("Failed to find next FREE record in map of record offsets");

				currentFreeRecord = recordOffsets[currentFreeRecord.NextFreeOffset] as FreeRecord;

				if (currentFreeRecord == null)
					throw new Exception("Found a record that wasn't a FREE record while looking for next FREE record");
			}

			return freeList;
		}
	}
}
