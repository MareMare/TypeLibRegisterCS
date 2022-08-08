// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerMode.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System.Runtime.Serialization;

#endregion

namespace TypeLibRegisterCS.Serialization
{
    /// <summary>
    /// シリアル化を行う方法 の列挙体を表します。
    /// </summary>
    [DataContract]
    public enum SerializerMode
    {
        /// <summary>XmlSerializer によるシルアル化を行います。</summary>
        [EnumMember]
        XmlSerializer,

        /// <summary>DataContractSerializer によるシルアル化を行います。</summary>
        [EnumMember]
        DataContractSerializer,
    }
}
