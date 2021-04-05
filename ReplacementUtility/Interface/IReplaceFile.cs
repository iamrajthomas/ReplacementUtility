//  -------------------------------------------------------------------------
//  <copyright file="IReplaceFile.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Replace File Interface
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Interface
{
    public interface IReplaceFile
    {
        bool IsOperationSucceed { get; set; }
        bool Initiate();
    }
}
