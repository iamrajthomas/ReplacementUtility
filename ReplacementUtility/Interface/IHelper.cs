//  -------------------------------------------------------------------------
//  <copyright file="IHelper.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Helper Interface
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Interface
{
    using System.ServiceProcess;
    public interface IHelper
    {
        bool IISResetStop();
        bool IISResetStart();
        bool StartESService(ServiceController sc);
        bool StopESService(ServiceController sc);
    }
}
