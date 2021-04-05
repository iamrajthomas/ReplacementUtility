//  -------------------------------------------------------------------------
//  <copyright file="Startup.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Startup For Quick Replacement Utility Application
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Class
{
    using ReplacementUtility.Interface;
    using System;

    public class Startup : IStartup
    {
        private readonly ReadAppConfigData _readAppConfigData = null;
        private readonly Helper _helper = null;
        private readonly ReplaceFile _replaceFile = null;
        private readonly ReplaceFolder _replaceFolder = null;
        private readonly MailNotification _mailNotification = null;
        public bool IsOperationSucceed { get; set; }
        private bool IsFileReplaced = false;
        private bool IsFolderReplaced = false;
        private bool IsMailSent = false;
        /// <summary>
        /// Constructor of the class
        /// </summary>
        public Startup()
        {
            _readAppConfigData = new ReadAppConfigData();
            _helper = new Helper();
            _replaceFile = new ReplaceFile();
            _replaceFolder = new ReplaceFolder();
            _mailNotification = new MailNotification();
            IsOperationSucceed = false;
            IsFileReplaced = false;
            IsFolderReplaced = false;
            IsMailSent = false;
        }

        /// <summary>
        /// Responsible for All The Initial Tasks in the Application
        /// </summary>
        public void Initiate()
        {
            try
            {
                string ReplaceFile_Enabled = _readAppConfigData.ReadValueByKey("ReplaceFile_Enabled");
                string ReplaceFolder_Enabled = _readAppConfigData.ReadValueByKey("ReplaceFolder_Enabled");
                string SendNotification_Enabled = _readAppConfigData.ReadValueByKey("SendNotification_Enabled");

                if (ReplaceFile_Enabled != null && Convert.ToBoolean(ReplaceFile_Enabled))
                {
                    Console.WriteLine("IISStopped: {0}", _helper.IISResetStop() ? "Yes" : "No");
                    IsFileReplaced = _replaceFile.Initiate();
                    Console.WriteLine("IISRunning: {0}", _helper.IISResetStart() ? "Yes" : "No");
                }
                if (ReplaceFolder_Enabled != null && Convert.ToBoolean(ReplaceFolder_Enabled))
                {
                    IsFolderReplaced = _replaceFolder.Initiate();
                }
                if (SendNotification_Enabled != null && Convert.ToBoolean(SendNotification_Enabled) && IsFileReplaced && IsFolderReplaced)
                {
                    IsMailSent = _mailNotification.Initiate();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("***************************** ERROR *****************************");
                Console.WriteLine("SOME ERROR OCCURRED!!");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("***************************** ERROR *****************************");
            }
            finally
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine(string.Format("\n IsFileReplaced: {0} \n IsFolderReplaced: {1} \n IsMailSent: {0}", IsFileReplaced ? "Yes" : "No", IsFolderReplaced ? "Yes" : "No", IsMailSent ? "Yes" : "No"));
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ \n");

                Console.WriteLine("=================================================================");
                Console.WriteLine("TASK COMPLETED!!");
                Console.WriteLine("================================================================= \n");
            }

        }

    }
}
