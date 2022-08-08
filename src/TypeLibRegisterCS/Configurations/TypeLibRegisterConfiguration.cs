// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeLibRegisterConfiguration.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Serialization;
using TypeLibRegisterCS.Serialization;

#endregion

namespace TypeLibRegisterCS.Configurations
{
    /// <summary>
    /// TypeLibRegisterConfiguration のオブジェクトを表します。
    /// </summary>
    [Serializable]
    [XmlRoot("typeLibRegisterConfiguration")]
    [SerializableConfiguration(SerializerMode.XmlSerializer, AppSettingKey = "TypeLibRegisterConfiguration.FilePath", ThrowIfEmpty = false)]
    public class TypeLibRegisterConfiguration : ISerializableConfiguration
    {
        /// <summary>
        /// TypeLibRegisterConfiguration クラスの新しいインスタンスを初期化します。
        /// </summary>
        public TypeLibRegisterConfiguration()
        {
            this.FilterItems = new Collection<FilterItem>();
        }

        /// <summary>
        /// フィルタ項目のコレクションを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Collection{T}"/> 型。
        /// <para>フィルタ項目のコレクション。既定値は要素数 0 です。</para>
        /// </value>
        [XmlArray("filters")]
        [XmlArrayItem("item")]
        public Collection<FilterItem> FilterItems
        {
            get;
            set;
        }

        /// <summary>
        /// 空の定義として構築します。
        /// </summary>
        public void ConstructAsEmpty()
        {
            this.FilterItems = new Collection<FilterItem>();
            this.FilterItems.Add(FilterItem.All);
        }

        /// <summary>
        /// 現在のインスタンスを検証します。
        /// </summary>
        public void Validate()
        {
            var query = this.FilterItems.Where((item) => item.SearchPattern == FilterItem.All.SearchPattern);
            if (query.Count() == 0)
            {
                this.FilterItems.Insert(0, FilterItem.All);
            }
        }
    }
}
