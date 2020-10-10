// using Microsoft.VisualBasic;
using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace Utilities
{

	public class ManageArchiveClass
	{
		//		public string startingSourceFolder = @"D:\";
		//		public string startingSourceFolder = @"D:\SP_JD";
		public string startingSourceFolder = @"C:\zz_TEMP_IN";
		public string startingDestinationFolder = @"C:\zz_TEMP_OUT\";

		///////////////////////////////		public string finalFileLocation


		/*
				 public int MaxFileIterations = 500000;
				public int MaxPathIterations = 500000;
				public int totalIterations = 1000000;
		*/
		public int MaxFileIterations = 500;
		public int MaxPathIterations = 500;
		public int totalIterations = 1000;

		public bool printDetailAccepted = true;
		public bool printDetailRejected = false;
		public bool printDetailMoved = true;
		public bool writeFilesToDisk = true;
		public string logFileName_Accepted = @"C:\zzz_TST\LOG\logFile_Accepted.txt";
		public string logFileName_Rejected = @"C:\zzz_TST\LOG\logFile_Rejected.txt";
		public string logFileName_Moved = @"C:\zzz_TST\LOG\logFile_Moved.txt";
		public string fullyQualifiedfileName;
		public string pathNamePart1;
		public string pathAndFileNamePart2;
		public string movedFileName;
		public int currentFileIteration = 0;
		public int currentPathIteration = 0;
		public int totalAccepted = 0;
		public int totalRejected = 0;
		public int totalMoved = 0;

		// Folder parameters
		Stack<string> sourceFolderNameStack = new Stack<string>();
		string[] sourceFolderNameList = new string[0];
		int sourceFolderNameListCount;
		string sourceFolderName;

		// File parameters
		string[] sourceFileNameList;
		int sourceFileNameListCount;
		string sourceFileName;

		public ManageArchiveClass()
		{
			// Constructor
			Console.WriteLine("Running constructor for ManageArchiveClass");

			// Put starting source folder in stack
			sourceFolderNameStack.Push(startingSourceFolder);

			// Get array of file names in starting source folder
			sourceFileNameList = Directory.GetFiles(startingSourceFolder);
			sourceFileNameListCount = sourceFileNameList.Count();

		}
		public void ExecuteProcess()
		{
			Console.WriteLine("Executing Process");
			// set up output text files to contain the results of this process


			// Create log files.
			File.WriteAllText(logFileName_Accepted, "Log for Accepted files" + Environment.NewLine);
			File.WriteAllText(logFileName_Rejected, "Log for Rejected files" + Environment.NewLine);
			File.WriteAllText(logFileName_Moved, "Log for Moved files" + Environment.NewLine);


			// Loop until we run out of iterations
			while (totalIterations-- > 0)
			{
				// Get the name of the next source file to process.
				if (!getNextSourceFileName())
				{
					// If we have run out of iterations files to process, go back.
					break;
				}

				// If we don't want to keep this file, go to the next one.
				if (!keepFile())
				{
					if (printDetailRejected)
					{
						Console.WriteLine("REJECTED: " + sourceFileName + " ");

					}
					else
					{
						Console.Write("R");
					}
					File.AppendAllText(logFileName_Rejected, "REJECTED: " + sourceFileName + " " + Environment.NewLine);
					totalRejected++;
					continue;
				}
				if (moveFile())
				{
					totalMoved++;

					if (printDetailMoved)
					{
						Console.WriteLine("");
						Console.WriteLine("MOVEDFRM: " + sourceFileName + " ");
						Console.WriteLine("MOVED TO: " + movedFileName + " ");
						Console.WriteLine("");
					}
					else
					{
						Console.Write("M");
					}

					File.AppendAllText(logFileName_Moved, "MOVEDFROM: " + sourceFileName + " " + Environment.NewLine);
					File.AppendAllText(logFileName_Moved, "  MOVEDTO: " + movedFileName + " " + Environment.NewLine + Environment.NewLine);

                    //
                    // Start of section that copies files to diskd
                    //
/*
                    if (writeFilesToDisk)
                    {
						// 
						System.IO.Directory.CreateDirectory(movedFileName);
					}

					//
					// End of section that copies files to disk
					//

					string dosCommand = "\"" + sourceFolderName + "\"";

					
					FileInfo fInfo = new FileInfo(dosCommand);
					if (fInfo.Exists)
					{
						Console.WriteLine("File exits");
					}
                    else
                    {
						Console.WriteLine("File does not exist");
                    }
*/				}
                else
                {
					// File was accepted but not moved
					totalAccepted++;
					if (printDetailAccepted)
                    {

						Console.WriteLine("ACCEPTED: " + sourceFileName + " ");
					}
					else
					{
						Console.Write("A");
					}
					File.AppendAllText(logFileName_Accepted, "ACCEPTED: " + sourceFileName + " " + Environment.NewLine);
				}


			}
			Console.WriteLine("");
			Console.WriteLine("Total Accepted: " + totalAccepted);
			Console.WriteLine("Total Rejected: " + totalRejected);
			Console.WriteLine("Total Moved: " + totalMoved);
		}


		public bool getNextSourceFileName()
		{

			// as long as there are more file names in current folder
			while (totalIterations-- > 0)
			{
				if (sourceFileNameListCount > 0)
				{
					// get next file name, decrement array pointer and return
					sourceFileName = sourceFileNameList[--sourceFileNameListCount];

					return true;
				}
				// if we get here, we need to make sure there are more things to do
				if (sourceFolderNameStack.Count == 0)
				{
					// We are done. Get back Jo JO
					return false;
				}
				// Always do the next three things to get ready for next time
				// 1. Pop from stack to get the next folder to work on
				sourceFolderName = sourceFolderNameStack.Pop();

				// 2. Push to stack each of the subfolders in this new folder
				// Get list of subfolders in source folder
				sourceFolderNameList = Directory.GetDirectories(sourceFolderName);

				sourceFolderNameListCount = sourceFolderNameList.Count();

				for (int i = 0; i < sourceFolderNameListCount; i++)
				{
					sourceFolderNameStack.Push(sourceFolderNameList[i]);
				}
				// 3.create arry of file names in the currentFolder



				// Get list of file names in source folder
				sourceFileNameList = Directory.GetFiles(sourceFolderName);

				// get copy of initial length to be used to iterate on string array later
				sourceFileNameListCount = sourceFileNameList.Count();

			}
			return true;
		}

		private bool keepFile()
		{
			// MOVE TO getNextFile
			if (currentFileIteration >= MaxFileIterations) return false;
			if (currentPathIteration >= MaxPathIterations) return false;

			if (sourceFileName.Contains("RECYCLE.BIN")) return false;
			if (sourceFileName.Contains(@"\from 1MM\Users\margaret\Documents\_R")) return false;
			if (sourceFileName.Contains(@"\from 1MM\Users\margaret\Documents\_R_archive")) return false;
			if (sourceFileName.Contains(@"\from 1MM\Users\margaret\Documents\_M\people\RGP")) return false;
			if (sourceFileName.Contains(@"\from 3MBPro\Users\ronpearl\_M")) return false;
			if (sourceFileName.Contains(@"untitled folder")) return false;
			if (sourceFileName.Contains(@"New folder")) return false;
			if (sourceFileName.Contains(@"\.")) return false;
			if (sourceFileName.Contains(@"\from 2MBook\Users\apple\Library")) return false;
			if (sourceFileName.Contains(@"\from 2MBook\Users\apple\src\ltcadm")) return false;
			if (sourceFileName.Contains(@"\from 2MBook\Users\apple\src\ltcuat")) return false;
			if (sourceFileName.Contains(@"\from 2MBook\Users\apple\tmp")) return false;
			if (sourceFileName.Contains(@"\Address Book -")) return false;
			if (sourceFileName.Contains(@"\ScanSnap\")) return false;
			if (sourceFileName.Contains(@"z_backups\")) return false;
			if (sourceFileName.Contains(@"\z_Archive")) return false;
			if (sourceFileName.Contains(@"trash")) return false;
			if (sourceFileName.Contains(@"MasterFileUpdates")) return false;
			if (sourceFileName.Contains(@"\Metadata\")) return false;
			if (sourceFileName.Contains(@"1Password")) return false;
			if (sourceFileName.Contains(@"- DELETE -")) return false;
			if (sourceFileName.Contains(@"Django")) return false;
			if (sourceFileName.Contains(@"\temp\")) return false;
			if (sourceFileName.Contains(@".DS_Store")) return false;
			if (sourceFileName[0] == '.') return false;
			return true;
		}
		private bool moveFile()
        {
			// Check the patterns against full path & file names, 
			// generate destination path & file name.
			if (buildMovePath(@"_emergency\"))	return true;
			if (buildMovePath(@"music\")) return true;
			if (buildMovePath(@"Music\")) return true;
			if (buildMovePath(@"things\")) return true;
			if (buildMovePath(@"Things\")) return true;
			if (buildMovePath(@"people\")) return true;
			if (buildMovePath(@"People\")) return true;
			if (buildMovePath(@"places\")) return true;
			if (buildMovePath(@"events\")) return true;
			if (buildMovePath(@"Pictures\")) return true;
			if (buildMovePath(@"photos\")) return true;
			if (buildMovePath(@"Photos\")) return true;
			if (buildMovePath(@"orgs\")) return true;
			if (buildMovePath(@"Organizations\")) return true;
			if (buildMovePath(@"_PEARL\")) return true;
			if (buildMovePath(@"Seabolt\")) return true;
			if (buildMovePath(@"neighborhoods\")) return true;
			if (buildMovePath(@"cdsir\")) return true;
			if (buildMovePath(@"Desktop\")) return true;
			if (buildMovePath(@"zip\")) return true;
			if (buildMovePath(@"src\")) return true;
			if (buildMovePath(@"Src\")) return true;
			return false;
		}
        private bool buildMovePath(string pattern)
        {
            if (!sourceFileName.Contains(pattern))
            {
				// The fully qualified path and file name do not contain the pattern.
				return false;
            }
			// The path and file name do contain the pattern.
			// Build to new path and file name after calculating the
			// characters to chop out of the middle of the original path name
			string tempString = startingDestinationFolder + sourceFileName;
			int cutStart = startingDestinationFolder.Length;
			int cutEnd = tempString.IndexOf(pattern);





			// loop backwards through temporary string until "\" is found.
			// This allows patterns to not start with "\".
			while (cutEnd > 0)
            {
				if (tempString[cutEnd] == '\\')
				{
					break;
				}
				cutEnd--;
            }




			// cut out the old portion of the path that will not be used after the move
			string tempString2 = tempString.Remove(cutStart, cutEnd - cutStart + 1);

			if (sourceFileName.Contains("3MBPro"))
			{
				movedFileName = tempString2.Insert(cutStart + pattern.Length + 1, @"3MBPro\");
			}
			else
			if (sourceFileName.Contains("2MBook"))
			{
				movedFileName = tempString2.Insert(cutStart + pattern.Length + 1, @"2MBook\");
			}
			else

			if (sourceFileName.Contains("1MM"))
			{
				movedFileName = tempString2.Insert(cutStart + pattern.Length + 1, @"1MM\");
			}
			else
			{
				movedFileName = tempString2.Insert(cutStart + pattern.Length + 1, @"0_other\");
			}
			return true;
        }
/*
		private bool copyFile()
        {
			// copies file from "sourceFileName" to "movedFileName"

			// See whether source file still exists


			// See whether movedFileName (destination) file exists


			// execute the windows copy command (AFTER DEALING WITH THE CREATION OF THE FOLDER WITHIN WHICH THE FILE IS TO BE COPIED)
			return true;
		}
*/
	}
}


