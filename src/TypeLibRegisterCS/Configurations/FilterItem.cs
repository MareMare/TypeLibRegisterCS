// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterItem.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Xml.Serialization;

#endregion

namespace TypeLibRegisterCS.Configurations
{
    /// <summary>
    /// FilterItem のオブジェクトを表します。
    /// </summary>
    [Serializable]
    public class FilterItem
    {
        /// <summary>すべてを表す FilterItem。</summary>
        private static FilterItem _allItem = new FilterItem
            { DisplayName = "すべて", SearchPattern = "*.*" };

        /// <summary>
        /// FilterItem クラスの新しいインスタンスを初期化します。
        /// </summary>
        public FilterItem()
        {
            this.DisplayName = "すべて";
            this.SearchPattern = "*.*";
        }

        /// <summary>
        /// すべてを表す FilterItem を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="FilterItem" /> 型。
        /// <para>すべてを表す FilterItem。既定値は FilterItem.All です。</para>
        /// </value>
        public static FilterItem All => FilterItem._allItem;

        /// <summary>
        /// 表示名を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>表示名。既定値は string.Empty です。</para>
        /// </value>
        [XmlAttribute("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// ワイルドカードを取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>ワイルドカード。既定値は "*.*" です。</para>
        /// </value>
        [XmlAttribute("searchPattern")]
        public string SearchPattern { get; set; }

        /// <summary>
        /// 現在の System.Object を表す System.String を返します。
        /// </summary>
        /// <returns>現在の System.Object を表す System.String。</returns>
        public override string ToString() => this.DisplayName;
    }
}
