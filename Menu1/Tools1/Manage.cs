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
		public string startingDestinationFolder = @"C:\zzz_TST\DEST\";
		public int MaxFileIterations = 500000;
		public int MaxPathIterations = 500000;
		public int totalIterations = 1000000;
		public bool printDetailAccepted = false;
		public bool printDetailRejected = false;
		public bool printDetailMoved = true;

		public string fullyQualifiedfileName;
		public string fileName;
		public string fullPathName;
		public string pathNamePart1;
		public string pathAndFileNamePart2;
		public string movedPathName;
		public string[] filePathParts;
		public int currentFileIteration = 0;
		public int currentPathIteration = 0;
		public int totalAccepted = 0;
		public int totalRejected = 0;

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
						Console.WriteLine("REJECTED: " + fileName + " ");
					}
					else
					{
						Console.Write("R");
					}
					totalRejected++;
					continue;
				}
				// file accepted. SAVE IT somewhere.
				if (printDetailAccepted)
				{
					Console.WriteLine("ACCEPTED: " + fileName + " ");
				}
				else
				{
					Console.Write("A");
				}
				totalAccepted++;
				if (moveFile())
				{

					if (printDetailMoved)
					{
						Console.WriteLine("");
						Console.WriteLine("MOVEDFRM: " + fileName + " ");
						Console.WriteLine("MOVED TO: " + movedPathName + " ");
						Console.WriteLine("");
					}
					else
					{
						Console.Write("M");
					}

				}


			}
			Console.WriteLine("");
			Console.WriteLine("Total Accepted: " + totalAccepted);
			Console.WriteLine("Total Rejected: " + totalRejected);
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
					fullPathName = sourceFileName; // ////////////////////////////////  FIX THIS ///////////////////
					fileName = sourceFileName; // ////////////////////////////////  FIX THIS ///////////////////

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

			if (fullPathName.Contains("RECYCLE.BIN")) return false;
			//		if (fullPathName.Contains(@"\Desktop")) return false;
			if (fullPathName.Contains(@"\from 1MM\Users\margaret\Documents\_R")) return false;
			if (fullPathName.Contains(@"\from 3MBPro\Users\ronpearl\_M")) return false;
			if (fullPathName.Contains(@"untitled folder")) return false;
			if (fullPathName.Contains(@"New folder")) return false;
			if (fullPathName.Contains(@"\.")) return false;
			if (fullPathName.Contains(@"\from 2MBook\Users\apple\Library")) return false;
			if (fullPathName.Contains(@"\from 2MBook\Users\apple\src\ltcadm")) return false;
			if (fullPathName.Contains(@"\from 2MBook\Users\apple\src\ltcuat")) return false;
			if (fullPathName.Contains(@"\from 2MBook\Users\apple\tmp")) return false;
			if (fullPathName.Contains(@"\Address Book -")) return false;
			if (fullPathName.Contains(@"\ScanSnap\")) return false;
			if (fullPathName.Contains(@"z_backups\")) return false;
			if (fullPathName.Contains(@"\z_Archive")) return false;
			if (fullPathName.Contains(@"trash")) return false;
			if (fullPathName.Contains(@"MasterFileUpdates")) return false;
			if (fullPathName.Contains(@"\Metadata\")) return false;
			if (fileName.Contains(@".DS_Store")) return false;
			if (fileName[0] == '.') return false;
			return true;
		}
		private bool moveFile()
        {
			// Check the patterns against full path & file names, 
			// generate destination path & file name.
			if (buildMovePath(@"\_emergency"))	return true;
			if (buildMovePath(@"\music\"))		return true;
			if (buildMovePath(@"things\"))	return true;
//			if (buildMovePath(@"zz")) return true;
			return false;
		}
        private bool buildMovePath(string pattern)
        {
            if (!fullPathName.Contains(pattern))
            {
				// The fully qualified path and file name do not contain the pattern.
				return false;
            }
			// The path and file name do contain the pattern.
			// Build to new path and file name after calculating the
			// characters to chop out of the middle of the original path name
			string tempString = startingDestinationFolder + fullPathName;
			int cutStart = startingDestinationFolder.Length;
			int cutEnd = tempString.IndexOf(pattern);

			// cut out the old portion of the path that will not be used after the move
			movedPathName = tempString.Remove(cutStart, cutEnd - cutStart - 1);

			// What's left is the new path name to which the file will be moved
			return true;
       }
	}
}


