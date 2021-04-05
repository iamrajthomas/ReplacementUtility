//  -------------------------------------------------------------------------
//  <copyright file="ReplaceElasticSearchFolder.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Replace ElasticSearch Folder (0 Folder)
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Class
{
    using ReplacementUtility.Interface;
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.ServiceProcess;
    using System.Text.RegularExpressions;

    //  <summary>
    //       Entity For Replace ElasticSearch Folder (0 Folder)
    //  </summary>
    public class ReplaceFolder : IReplaceFolder
    {
        private readonly ReadAppConfigData _readAppConfigData = null;
        private readonly Helper _helper = null;
        public bool IsOperationSucceed { get; set; }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public ReplaceFolder()
        {
            _readAppConfigData = new ReadAppConfigData();
            _helper = new Helper();
            IsOperationSucceed = false;
        }

        /// <summary>
        /// Initiate ES Manipulation
        /// </summary>
        public bool Initiate()
        {
            string ESServiceName = _readAppConfigData.ReadValueByKey("ESServiceName");
            ServiceController sc = new ServiceController(ESServiceName);

            Console.WriteLine("=================================================================");
            Console.WriteLine("The ES service status is currently set to {0}", sc.Status.ToString());
            Console.WriteLine("================================================================= \n");

            bool isESStopped = _helper.StopESService(sc);
            Console.WriteLine("ESStopped: {0}", isESStopped ? "Yes" : "No");

            if (isESStopped)
            {
                IsOperationSucceed = ReplaceElasticSearch();
            }

            bool isESRunning = _helper.StartESService(sc);
            Console.WriteLine("ESRunning: {0}", isESRunning ? "Yes" : "No");
            return IsOperationSucceed;
        }

        /// <summary>
        /// Replace The ReplaceElasticSearch 0 Folder
        /// </summary>
        public bool ReplaceElasticSearch()
        {
            Regex reg = new Regex("[*'\",/:_&#^@]");

            #region Read Cofiguration Values and Validate
            string SourceFolderPath = _readAppConfigData.ReadValueByKey("SourceFolderPath");
            string TargetFolderPath = _readAppConfigData.ReadValueByKey("TargetFolderPath");
            string dateTimeCustomFormat = _readAppConfigData.ReadValueByKey("DateTimeCustomFormat") != null ? _readAppConfigData.ReadValueByKey("DateTimeCustomFormat") : DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string BackUpFolderPathDirectory = _readAppConfigData.ReadValueByKey("BackUpFolderPathDirectory") != null ? _readAppConfigData.ReadValueByKey("BackUpFolderPathDirectory") + @"\" + reg.Replace(DateTime.Now.ToString(dateTimeCustomFormat), " ") : null;
            string FolderIsReplacedFlag = _readAppConfigData.ReadValueByKey("FolderIsReplacedFlag");
            string FolderIsReplacedFlagAtSameFolderLevel = _readAppConfigData.ReadValueByKey("FolderIsReplacedFlagAtSameFolderLevel");

            bool isValidationSucess = InitialValidations(SourceFolderPath, TargetFolderPath, BackUpFolderPathDirectory, FolderIsReplacedFlag, FolderIsReplacedFlagAtSameFolderLevel);
            if (!isValidationSucess)
            {
                return false;
            }
            else
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("Initial Validation for Elastic Search is Successful.");
                Console.WriteLine("================================================================= \n");
            }
            #endregion

            try
            {
                // Ensure that the target does exist.
                if (Directory.Exists(TargetFolderPath))
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The deployment is completed. The ES 0 folder exists. Hence, ES Backup/Replacement Operation initiated.");
                    Console.WriteLine("================================================================= \n");

                    #region Backup the existing ES 0 folder
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("Backup started");
                    Console.WriteLine("================================================================= \n");

                    // Determine whether the backup directory exists, if not create one.
                    if (Directory.Exists(BackUpFolderPathDirectory))
                    {
                        Console.WriteLine("-- The backup directory exists. No need to Craete one.");
                    }
                    else
                    {
                        Console.WriteLine("-- That backup directory does not exist. Create one new backup directory.");

                        DirectoryInfo di = Directory.CreateDirectory(BackUpFolderPathDirectory);
                        Console.WriteLine("-- The backup directory was created successfully at {0}.", Directory.GetCreationTime(BackUpFolderPathDirectory));
                    }

                    string backupSourceFolderPath = TargetFolderPath;
                    string backupTargetFolderPath = string.Concat(BackUpFolderPathDirectory, string.Concat(@"\" + "0_backup.zip"));

                    // Check if te backup target ES Zip file exist, Delete it
                    if (File.Exists(backupTargetFolderPath))
                    {
                        Console.WriteLine("-- The backup target file already exists, so deleting it.");
                        File.Delete(backupTargetFolderPath);
                    }

                    ZipFile.CreateFromDirectory(backupSourceFolderPath, backupTargetFolderPath, CompressionLevel.Fastest, true);

                    Console.WriteLine("-- The ES backup is made under path: " + backupTargetFolderPath + "\n");

                    Console.WriteLine("=================================================================");
                    Console.WriteLine("ES Backup completed.");
                    Console.WriteLine("================================================================= \n");

                    #endregion

                    #region Replace ES 0 folder
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("ES 0 Folder Replecement started.");
                    Console.WriteLine("================================================================= \n");

                    Console.WriteLine("=================================================================");
                    Console.WriteLine("Deleted ES 0 Folder at the Target ES Path");
                    Console.WriteLine("================================================================= \n");

                    Directory.Delete(TargetFolderPath, true);

                    Console.WriteLine("=================================================================");
                    Console.WriteLine("Copy ES 0 Folder from Source to the Target ES Path Started");
                    Console.WriteLine("================================================================= \n");

                    CopyDirectoryAndAllContents(SourceFolderPath, TargetFolderPath, true);

                    Console.WriteLine("{0} was moved to {1}.", SourceFolderPath, TargetFolderPath);
                    Console.WriteLine("ES 0 Folder Replacement is Completed.");
                    #endregion

                    #region Create FolderIsReplacedFlag
                    if (!File.Exists(FolderIsReplacedFlag))
                    {
                        // This statement ensures that the webConfigIsReplaced file is created,        
                        // but the handle is not kept.        
                        using (FileStream fs = File.Create(FolderIsReplacedFlag)) { }
                    }
                    if (!File.Exists(FolderIsReplacedFlagAtSameFolderLevel))
                    {
                        // This statement ensures that the webConfigIsReplaced file is created,        
                        // but the handle is not kept.        
                        using (FileStream fs = File.Create(FolderIsReplacedFlagAtSameFolderLevel)) { }
                    }

                    #endregion

                    Console.WriteLine("=================================================================");
                    Console.WriteLine("Copy ES 0 Folder from Source to the Target ES Path Completed");
                    Console.WriteLine("================================================================= \n");
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("***************************** ERROR *****************************");
                Console.WriteLine("The Replace ES process failed: {0}", ex.ToString());
                Console.WriteLine("***************************** ERROR *****************************");
                return false;
            }

        }

        /// <summary>
        /// Copy Source Directory To Destination Directory With All Contents and Sub Directories
        /// </summary>
        /// <param name="sourceDirName">Source Directory.</param>
        /// <param name="destDirName">Destination Directory.</param>
        /// <param name="copySubDirs">copy Sub Directories Flag.</param>
        private static void CopyDirectoryAndAllContents(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectoryAndAllContents(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        private static bool InitialValidations(string SourceFolderPath, string TargetFolderPath, string BackUpFolderPathDirectory, string FolderIsReplacedFlag, string FolderIsReplacedFlagAtSameFolderLevel)
        {
            if (SourceFolderPath == null || TargetFolderPath == null || BackUpFolderPathDirectory == null || FolderIsReplacedFlag == null || FolderIsReplacedFlagAtSameFolderLevel == null)
            {
                Console.WriteLine("***************************** ERROR *****************************");
                Console.WriteLine("Configuration is not proper. Please check the configuration and retry!!");
                Console.WriteLine("***************************** ERROR *****************************");
                return false;
            }

            if (!Directory.Exists(TargetFolderPath))
            {
                if (File.Exists(FolderIsReplacedFlag))
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The target ES 0 folder is doesn't exist but FolderIsReplacedFlag is available. So deleting FolderIsReplacedFlag Here.");
                    Console.WriteLine("================================================================= \n");
                    File.Delete(FolderIsReplacedFlag);
                }
                if (File.Exists(FolderIsReplacedFlagAtSameFolderLevel))
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The target ES 0 folder is doesn't exist but FolderIsReplacedFlagAtSameFolderLevel is available. So deleting FolderIsReplacedFlagAtSameFolderLevel Here.");
                    Console.WriteLine("================================================================= \n");
                    File.Delete(FolderIsReplacedFlagAtSameFolderLevel);
                }

                Console.WriteLine("=================================================================");
                Console.WriteLine("The target TargetFolderPath doesn't exists. Hence, no replacements will be done.");
                Console.WriteLine("================================================================= \n");
                return false;
            }

            if (!Directory.Exists(SourceFolderPath))
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("The SourceFolderPath doesn't exist. No Action will be performed.");
                Console.WriteLine("================================================================= \n");
                return false;
            }

            if (File.Exists(FolderIsReplacedFlag))
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("ES 0 Folder is already Replaced. FolderIsReplacedFlag is available. No Action Required.");
                Console.WriteLine("================================================================= \n");
                return false;
            }

            if (Directory.Exists(SourceFolderPath) || Directory.Exists(TargetFolderPath) || Directory.Exists(BackUpFolderPathDirectory))
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("The backup directory exists. No need to Craete one.");
                Console.WriteLine("================================================================= \n");
            }
            else
            {
                Console.WriteLine("***************************** ERROR *****************************");
                Console.WriteLine("SourceFolderPath, TargetFolderPath or BackUpFolderPathDirectory doesn't exist. Hence No Action To Perform.");
                Console.WriteLine("***************************** ERROR *****************************");
            }

            return true;
        }

    }
}
