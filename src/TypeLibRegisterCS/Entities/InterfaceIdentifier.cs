// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceIdentifier.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.ComponentModel;
using System.Xml.Serialization;

#endregion

namespace TypeLibRegisterCS.Entities
{
    /// <summary>
    /// InterfaceIdentifier のオブジェクトを表します。
    /// </summary>
    [Serializable]
    public class InterfaceIdentifier
    {
        /// <summary>
        /// InterfaceIdentifier クラスの新しいインスタンスを初期化します。
        /// </summary>
        public InterfaceIdentifier()
        {
        }

        /// <summary>
        /// レジストリのパスを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>レジストリのパス。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("registryPath")]
        public string RegistryPath { get; set; }

        /// <summary>
        /// IID を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>IID。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlIgnore]
        public string Iid { get; set; }

        /// <summary>
        /// TLBID を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>TLBID。既定値は string.Empty です。</para>
        /// </value>
        [Browsable(false)]
        [ReadOnly(true)]
        [XmlIgnore]
        public string Tlbid { get; set; }
    }
}
