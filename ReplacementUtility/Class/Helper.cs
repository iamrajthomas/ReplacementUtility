//  -------------------------------------------------------------------------
//  <copyright file="Helper.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Helper Class
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Class
{
    using ReplacementUtility.Interface;
    using System;
    using System.ServiceProcess;

    //  <summary>
    //       Entity For Helper Utilities
    //  </summary>
    public class Helper : IHelper
    {
        private readonly ServiceController _scW3SVC = null;
        private readonly ReadAppConfigData _readAppConfigData = null;

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public Helper()
        {
            _readAppConfigData = new ReadAppConfigData();            
            _scW3SVC = new ServiceController(_readAppConfigData.ReadValueByKey("W3SVCServiceName"));
        }

        /// <summary>
        /// IIS Reset Stop
        /// </summary>
        public bool IISResetStop()
        {
            if (_scW3SVC.Status == ServiceControllerStatus.Running)
            {
                // Start the service if the current status is stopped.
                Console.WriteLine("=================================================================");
                Console.WriteLine("Stopping the W3SVC service...");
                Console.WriteLine("================================================================= \n");

                try
                {
                    // Stop the service, and wait until its status is "Running".
                    _scW3SVC.Stop();
                    _scW3SVC.WaitForStatus(ServiceControllerStatus.Stopped);

                    // Display the current service status.
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The W3SVC service status is currently set to {0}", _scW3SVC.Status.ToString());
                    Console.WriteLine("================================================================= \n");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not stop the W3SVC service.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not stop the W3SVC service.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// IIS Reset Start
        /// </summary>
        public bool IISResetStart()
        {
            if (_scW3SVC.Status == ServiceControllerStatus.Running)
            {
                // Start the service if the current status is running.
                Console.WriteLine("=================================================================");
                Console.WriteLine("Starting the W3SVC service...");
                Console.WriteLine("================================================================= \n");

                try
                {
                    // Start the service, and wait until its status is "Running".
                    _scW3SVC.Start();
                    _scW3SVC.WaitForStatus(ServiceControllerStatus.Running);

                    // Display the current service status.
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The W3SVC service status is currently set to {0}", _scW3SVC.Status.ToString());
                    Console.WriteLine("================================================================= \n");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not start the W3SVC service: {0}", ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not start the W3SVC service: {0}", ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Start The ElasticSearch Service
        /// </summary>
        public bool StartESService(ServiceController sc)
        {
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                // Start the service if the current status is stopped.
                Console.WriteLine("=================================================================");
                Console.WriteLine("Starting the ES service...");
                Console.WriteLine("================================================================= \n");
                try
                {
                    // Start the service, and wait until its status is "Running".
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);

                    // Display the current service status.
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The ES service status is currently set to {0}", sc.Status.ToString());
                    Console.WriteLine("================================================================= \n");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not start the ES service.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not start the ES service.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Stop The ElasticSearch Service
        /// </summary>
        public bool StopESService(ServiceController sc)
        {
            if (sc.Status == ServiceControllerStatus.Running)
            {
                // Start the service if the current status is stopped.
                Console.WriteLine("=================================================================");
                Console.WriteLine("Stopping the ES service...");
                Console.WriteLine("================================================================= \n");

                try
                {
                    // Start the service, and wait until its status is "Running".
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);

                    // Display the current service status.
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("The ES service status is currently set to {0}", sc.Status.ToString());
                    Console.WriteLine("================================================================= \n");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not stop the ES service.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("***************************** ERROR *****************************");
                    Console.WriteLine("Could not stop the ES service.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("***************************** ERROR *****************************");
                    return false;
                }
            }
            return true;
        }


    }
}
