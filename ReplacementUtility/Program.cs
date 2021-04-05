//  -------------------------------------------------------------------------
//  <copyright file="Program.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Main Entry Point For Quick Replacement Utility Application
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility
{
    using ReplacementUtility.Class;
    public static class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        static void Main(string[] args)
        {
            new Startup().Initiate();

            System.Console.ReadKey();

        }
    }

}
