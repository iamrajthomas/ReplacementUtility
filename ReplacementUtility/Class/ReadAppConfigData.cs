//  -------------------------------------------------------------------------
//  <copyright file="ReadAppConfigFileData.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Read App Config File Key Value
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Class
{
    using ReplacementUtility.Interface;
    using System.Configuration;

    public class ReadAppConfigData : IReadAppConfigData
    {
        /// <summary>
        /// Constructor of the class
        /// </summary>
        public ReadAppConfigData()
        {

        } 
        
        /// <summary>
        /// Read App Config Key Value by Key
        /// </summary>
        public string ReadValueByKey(string Key)
        {
            string Value = ConfigurationManager.AppSettings.Get(Key);
            return Value;
        }
    }
}
