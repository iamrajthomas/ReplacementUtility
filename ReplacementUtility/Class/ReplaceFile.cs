//  -------------------------------------------------------------------------
//  <copyright file="ReplaceWebConfigFile.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Replace WebConfig File
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Class
{
    using ReplacementUtility.Interface;
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    //  <summary>
    //       Entity For Replace WebConfig File
    //  </summary>
    public class ReplaceFile : IReplaceFile
    {
        private readonly ReadAppConfigData _readAppConfigData = null;
        public bool IsOperationSucceed { get; set; }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public ReplaceFile()
        {
            _readAppConfigData = new ReadAppConfigData();
            IsOperationSucceed = false;
        }

        /// <summary>
        /// Initiate Web Config Manipulation
        /// </summary>
        public bool Initiate()
        {
            IsOperationSucceed = ReplaceWebConfig();
            return IsOperationSucceed;
        }

        /// <summary>
        /// Replace The Web.config File
        /// </summary>
        public bool ReplaceWebConfig()
        {
            Regex reg = new Regex("[*'\",/:_&#^@]");

            #region Read Cofiguration Values and Validate

            string SourceFilePath = _readAppConfigData.ReadValueByKey("SourceFilePath");
            string TargetFilePath = _readAppConfigData.ReadValueByKey("TargetFilePath");
            string DateTimeCustomFormat = _readAppConfigData.ReadValueByKey("DateTimeCustomFormat") != null ? _readAppConfigData.ReadValueByKey("DateTimeCustomFormat") : DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string BackUpFilePathDirectory = _readAppConfigData.ReadValueByKey("BackUpFilePathDirectory") != null ? _readAppConfigData.ReadValueByKey("BackUpFilePathDirectory") + @"\" + reg.Replace(DateTime.Now.ToString(DateTimeCustomFormat), " ") : null;
            string FileNameToReplace = _readAppConfigData.ReadValueByKey("FileNameToReplace");
            string FileIsReplacedFlag = _readAppConfigData.ReadValueByKey("FileIsReplacedFlag");

            bool isValidationSucess = InitialValidations(SourceFilePath, TargetFilePath, FileIsReplacedFlag, BackUpFilePathDirectory, FileNameToReplace);
            if (!isValidationSucess)
            {
                return false;
            }
            else
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("Initial Validation for Web.Config is Successful.");
                Console.WriteLine("================================================================= \n");
            }

            #endregion

            Console.WriteLine("=================================================================");
            Console.WriteLine("WebConfig Replace and Backup Operation Started.");
            Console.WriteLine("================================================================= \n");

            try
            {
                // Ensure that the target does exist.
                if (File.Exists(TargetFilePath))
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The deployment is completed. The webconfig file exists. Hence, Backup/Replacement Operation initiated.");
                    Console.WriteLine("================================================================= \n");

                    #region Backup the existing WebConfig
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("Backup started");
                    Console.WriteLine("================================================================= \n");

                    // Determine whether the backup directory exists.
                    if (Directory.Exists(BackUpFilePathDirectory))
                    {
                        Console.WriteLine("-- The backup directory exists. No need to Craete one.");
                    }
                    else
                    {
                        Console.WriteLine("-- That backup directory does not exist. Create one new backup directory.");

                        DirectoryInfo di = Directory.CreateDirectory(BackUpFilePathDirectory);
                        Console.WriteLine("-- The backup directory was created successfully at {0}.", Directory.GetCreationTime(BackUpFilePathDirectory));
                    }

                    string backupSourcePath = TargetFilePath;
                    string backupTargetPath = string.Concat(BackUpFilePathDirectory, string.Concat(@"\" + FileNameToReplace));

                    // Check if the Web Config backup target file exist, Delete it
                    if (File.Exists(backupTargetPath))
                    {
                        Console.WriteLine("-- The backup target file already exists, so deleting it.");
                        File.Delete(backupTargetPath);
                    }
                    File.Copy(backupSourcePath, backupTargetPath);
                    Console.WriteLine("-- The backup is made under path: " + backupTargetPath + "\n");

                    Console.WriteLine("=================================================================");
                    Console.WriteLine("Backup completed.");
                    Console.WriteLine("================================================================= \n");
                    #endregion

                    #region Replace WebConfig
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("Replecement started.");
                    Console.WriteLine("================================================================= \n");
                    File.Delete(TargetFilePath);
                    // Copy the file from source to target.
                    File.Copy(SourceFilePath, TargetFilePath);
                    Console.WriteLine("{0} was moved to {1}.", SourceFilePath, TargetFilePath);
                    Console.WriteLine("Webconfig Replacement is Completed.");

                    // See if the webconfig exists now.        
                    if (File.Exists(TargetFilePath))
                    {
                        Console.WriteLine("Replaced WebConfig Exists. (after replace For confirmation) \n");
                        Console.WriteLine("=================================================================");
                        Console.WriteLine("Replecement completed.");
                        Console.WriteLine("================================================================= \n");
                    }
                    else
                    {
                        Console.WriteLine("***************************** ERROR *****************************");
                        Console.WriteLine("WebConfig does not exist. Something Wrong. ERROR Alert!!");
                        Console.WriteLine("***************************** ERROR *****************************");
                    }

                    #endregion

                    #region Create FileIsReplacedFlag

                    if (!File.Exists(FileIsReplacedFlag))
                    {
                        // This statement ensures that the webConfigIsReplaced file is created,        
                        // but the handle is not kept.        
                        using (FileStream fs = File.Create(FileIsReplacedFlag)) { }
                    }
                    #endregion

                    Console.WriteLine("=================================================================");
                    Console.WriteLine("WebConfig Replace and Backup Operation completed.");
                    Console.WriteLine("================================================================= \n");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("***************************** ERROR *****************************");
                Console.WriteLine("The Replace WebConfig process failed: {0}", e.ToString());
                Console.WriteLine("***************************** ERROR *****************************");
                return false;
            }
        }

        /// <summary>
        /// Does the Initial Validation before Starting with WeConfig Replacement Opearation
        /// </summary>
        /// <param name="SourceFilePath">Source Web Config Path.</param>
        /// <param name="TargetFilePath">Target Web Config Path.</param>
        /// <param name="FileIsReplacedFlag">FileIsReplacedFlag In Target Path.</param>
        /// <param name="BackUpFilePathDirectory">Backup Target Path Directory.</param>
        /// <param name="FileNameToReplace">Config File Name.</param>
        /// <returns>validation success or failure</returns>
        private static bool InitialValidations(string SourceFilePath, string TargetFilePath, string FileIsReplacedFlag, string BackUpFilePathDirectory, string FileNameToReplace)
        {
            if (SourceFilePath == null || TargetFilePath == null || FileIsReplacedFlag == null || BackUpFilePathDirectory == null || FileNameToReplace == null)
            {
                Console.WriteLine("***************************** ERROR *****************************");
                Console.WriteLine("Configuration is not proper. Please check the configuration and retry!!");
                Console.WriteLine("***************************** ERROR *****************************");
                return false;
            }

            if (!File.Exists(TargetFilePath))
            {
                if (File.Exists(FileIsReplacedFlag))
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The target webConfig is doesn't exist but FileIsReplacedFlag is available. So deleting FileIsReplacedFlag Here.");
                    Console.WriteLine("================================================================= \n");
                    File.Delete(FileIsReplacedFlag);
                }
                Console.WriteLine("=================================================================");
                Console.WriteLine("The target webconfig file doesn't exists. Hence, no replacements will be done.");
                Console.WriteLine("================================================================= \n");
                return false;
            }

            if (!File.Exists(SourceFilePath))
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("Source WebConfig doesn't exist. No Action will be performed.");
                Console.WriteLine("================================================================= \n");
                return false;
            }

            if (File.Exists(FileIsReplacedFlag))
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("WebConfig is already Replaced. FileIsReplacedFlag is available. No Action Required.");
                Console.WriteLine("================================================================= \n");
                return false;
            }

            return true;
        }
    }
}
