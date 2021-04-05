//  -------------------------------------------------------------------------
//  <copyright file="IReadAppConfigData.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Read App Config File Key Value Interface
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Interface
{
    public interface IReadAppConfigData
    {
        string ReadValueByKey(string Key);
    }
}
