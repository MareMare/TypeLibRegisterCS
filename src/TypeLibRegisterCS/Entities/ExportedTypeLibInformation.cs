// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportedTypeLibInformation.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using TypeLibRegisterCS.Configurations;

#endregion

namespace TypeLibRegisterCS.Entities
{
    /// <summary>
    /// ExportedTypeLibInformation のオブジェクトを表します。
    /// </summary>
    [Serializable]
    [XmlRoot("exportedTypeLibInformation")]
    public class ExportedTypeLibInformation
    {
        /// <summary>
        /// ExportedTypeLibInformation クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ExportedTypeLibInformation()
        {
            this.TypeLibIdentifiers = new Collection<TypeLibIdentifier>();
            this.MachineInfo = new MachineInformation();
        }

        /// <summary>
        /// マシン情報を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="MachineInformation" /> 型。
        /// <para>マシン情報。既定値は null です。</para>
        /// </value>
        [XmlElement("machineInfo")]
        public MachineInformation MachineInfo { get; set; }

        /// <summary>
        /// フィルタ項目を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="FilterItem" /> 型。
        /// <para>フィルタ項目。既定値は null です。</para>
        /// </value>
        [XmlElement("filter", IsNullable = true)]
        public FilterItem Filter { get; set; }

        /// <summary>
        /// TypeLibIdentifier のコレクションを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Collection{T}" /> 型。
        /// <para>TypeLibIdentifier のコレクション。既定値は要素数 0 です。</para>
        /// </value>
        [XmlArray("typeLibIdentifiers")]
        [XmlArrayItem("typeLibIdentifier")]
        public Collection<TypeLibIdentifier> TypeLibIdentifiers { get; set; }

        /// <summary>
        /// TypeLibIdentifier コレクションを追加します。
        /// </summary>
        /// <param name="identifiers">追加される TypeLibIdentifier コレクション。</param>
        public void Add(IEnumerable<TypeLibIdentifier> identifiers)
        {
            if (identifiers != null)
            {
                foreach (var identifier in identifiers)
                {
                    this.TypeLibIdentifiers.Add(identifier);
                }
            }
        }
    }
}
