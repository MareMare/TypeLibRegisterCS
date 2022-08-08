// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassIdentifier.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace TypeLibRegisterCS.Entities
{
    /// <summary>
    /// ClassIdentifier のオブジェクトを表します。
    /// </summary>
    [Serializable]
    public class ClassIdentifier
    {
        /// <summary>
        /// ClassIdentifier クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ClassIdentifier()
        {
        }

        /// <summary>
        /// レジストリのパスを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>レジストリのパス。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("registryPath")]
        public string RegistryPath { get; set; }

        /// <summary>
        /// CLSID を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>CLSID。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlIgnore]
        public string Clsid { get; set; }

        /// <summary>
        /// TLBID を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>TLBID。既定値は string.Empty です。</para>
        /// </value>
        [Browsable(false)]
        [ReadOnly(true)]
        [XmlIgnore]
        public string Tlbid { get; set; }

        /// <summary>
        /// ProgID を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>ProgID。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("progId")]
        public string ProgId { get; set; }

        /// <summary>
        /// VersionIndependentProgID を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>VersionIndependentProgID。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("versionIndependentProgId")]
        public string VersionIndependentProgId { get; set; }
    }
}
