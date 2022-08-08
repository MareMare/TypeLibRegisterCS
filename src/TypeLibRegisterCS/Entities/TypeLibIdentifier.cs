// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeLibIdentifier.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace TypeLibRegisterCS.Entities
{
    /// <summary>
    /// TypeLibIdentifier のオブジェクトを表します。
    /// </summary>
    [Serializable]
    [DefaultProperty("DisplayName")]
    public class TypeLibIdentifier
    {
        /// <summary>
        /// TypeLibIdentifier クラスの新しいインスタンスを初期化します。
        /// </summary>
        public TypeLibIdentifier()
        {
            this.ClassIdentifiers = new Collection<ClassIdentifier>();
            this.InterfaceIdentifiers = new Collection<InterfaceIdentifier>();
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
        /// TLBID を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>TLBID。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlIgnore]
        public string Tlbid { get; set; }

        /// <summary>
        /// タイプライブラリ名を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>タイプライブラリ名。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// タイプライブラリのバージョンを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>タイプライブラリのバージョン。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("version")]
        public string Version { get; set; }

        /// <summary>
        /// タイプライブラリのファイルパスを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>タイプライブラリのファイルパス。既定値は string.Empty です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("filepath")]
        public string FilePath { get; set; }

        /// <summary>
        /// タイプライブラリのファイルパスが存在するかを取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="bool"/> 型。
        /// <para>タイプライブラリのファイルパスが存在する場合は true。既定値は false です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlAttribute("isExistsFile")]
        public bool IsExistsFile
        {
            get
            {
                return !string.IsNullOrEmpty(this.FilePath) ? File.Exists(this.FilePath) : false;
            }

            set
            {
                value = !value;
                value = !value;
            }
        }

        /// <summary>
        /// タイプライブラリに関連する ClassIdentifier コレクションを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Collection{ClassIdentifier}"/> 型。
        /// <para>タイプライブラリに関連する ClassIdentifier コレクション。既定値は要素数 0 です。</para>
        /// </value>
        [Browsable(false)]
        [XmlArray("classIdentifiers")]
        [XmlArrayItem("classIdentifier")]
        public Collection<ClassIdentifier> ClassIdentifiers { get; set; }

        /// <summary>
        /// タイプライブラリに関連する InterfaceIdentifier コレクションを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Collection{InterfaceIdentifier}"/> 型。
        /// <para>タイプライブラリに関連する InterfaceIdentifier コレクション。既定値は要素数 0 です。</para>
        /// </value>
        [Browsable(false)]
        [XmlArray("interfaceIdentifiers")]
        [XmlArrayItem("interfaceIdentifier")]
        public Collection<InterfaceIdentifier> InterfaceIdentifiers { get; set; }

        /// <summary>
        /// タイプライブラリのファイル名を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>タイプライブラリのファイル名。既定値は string.Empty です。</para>
        /// </value>
        public string FileName
        {
            get
            {
                return !string.IsNullOrEmpty(this.FilePath) ? Path.GetFileName(this.FilePath) : "<not found.>";
            }
        }

        /// <summary>
        /// タイプライブラリの表示名称を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>タイプライブラリの表示名称。既定値は string.Empty です。</para>
        /// </value>
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0} <no name>", this.Tlbid);
                    return builder.ToString();
                }

                return this.Name;
            }
        }

        /// <summary>
        /// タイプライブラリに関連する ClassIdentifier 配列を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="ClassIdentifier"/> 型。
        /// <para>タイプライブラリに関連する ClassIdentifier 配列。既定値は要素数 0 です。</para>
        /// </value>
        [Browsable(false)]
        [Obsolete("ウザイから未使用とします。")]
        public IList<ClassIdentifier> ReferencedClassIdentifiers
        {
            get
            {
                return this.ClassIdentifiers
                    .Where((identifier) => !string.IsNullOrEmpty(identifier.Clsid))
                    .ToArray();
            }
        }

        /// <summary>
        /// タイプライブラリに関連する InterfaceIdentifier 配列を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="InterfaceIdentifier"/> 型。
        /// <para>タイプライブラリに関連する InterfaceIdentifier 配列。既定値は要素数 0 です。</para>
        /// </value>
        [Browsable(false)]
        [Obsolete("ウザイから未使用とします。")]
        public IList<InterfaceIdentifier> ReferencedInterfaceIdentifiers
        {
            get
            {
                return this.InterfaceIdentifiers
                    .Where((identifier) => !string.IsNullOrEmpty(identifier.Iid))
                    .ToArray();
            }
        }

        /// <summary>
        /// タイプライブラリに関連する ClassIdentifier コレクションが構築されているかを取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="bool"/> 型。
        /// <para>タイプライブラリに関連する ClassIdentifier コレクションが構築されている場合は true。既定値は false です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlIgnore]
        public bool IsConstructedClassIdentifiers { get; internal set; }

        /// <summary>
        /// タイプライブラリに関連する InterfaceIdentifier コレクションが構築されているかを取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="bool"/> 型。
        /// <para>タイプライブラリに関連する InterfaceIdentifier コレクションが構築されている場合は true。既定値は false です。</para>
        /// </value>
        [ReadOnly(true)]
        [XmlIgnore]
        public bool IsConstructedInterfaceIdentifiers { get; internal set; }
    }
}
