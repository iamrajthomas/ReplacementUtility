//  -------------------------------------------------------------------------
//  <copyright file="IReplaceFolder.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Replace ElasticSearch Folder (0 Folder) Interace
//  </summary> 
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Interface
{
    public interface IReplaceFolder
    {
        bool IsOperationSucceed { get; set; }
        bool Initiate();
    }
}
