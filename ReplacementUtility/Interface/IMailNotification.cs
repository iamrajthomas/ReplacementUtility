//  -------------------------------------------------------------------------
//  <copyright file="IMailNotification.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Mail Notification Interface
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Interface
{
    public interface IMailNotification
    {
        bool IsOperationSucceed { get; set; }
        void Initiate();
    }
}
