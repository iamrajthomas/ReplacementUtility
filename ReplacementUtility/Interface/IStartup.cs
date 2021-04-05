//  -------------------------------------------------------------------------
//  <copyright file="IStartup.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Startup For Quick Replacement Utility Application Interface
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Interface
{
    public interface IStartup 
    {
        bool IsOperationSucceed { get; set; }
        void Initiate();
    }
}
