// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableConfigurationAttribute.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using TypeLibRegisterCS.Extensions;
using TypeLibRegisterCS.Serialization;

#endregion

namespace TypeLibRegisterCS.Configurations
{
    /// <summary>
    /// この型を構成定義データコントラクトの対象として処理する必要があることを指定するオブジェクトを表します。
    /// <para>このクラスは継承できません。</para>
    /// <para>このクラスで ObjectHelper を呼び出すことは禁止です。スタックオーバフローとなり異常終了してしまいます。</para>
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    [DebuggerDisplay("Mode={Mode}, FilePath={FilePath}, AppSettingKey={AppSettingKey}, ExternalSettingKey={ExternalSettingKey}, ThrowIfEmpty={ThrowIfEmpty}")]
    public sealed class SerializableConfigurationAttribute : Attribute, IEquatable<SerializableConfigurationAttribute>
    {
        /// <summary>
        /// SerializableConfigurationAttribute クラスの新しいインスタンスを初期化します。
        /// </summary>
        private SerializableConfigurationAttribute()
        {
            this.FilePath = string.Empty;
            this.AppSettingKey = string.Empty;

            this.KeysFactory = (obj, keys) =>
            {
                keys.Add(obj.Mode);
                keys.Add(obj.FilePath);
                keys.Add(obj.AppSettingKey);
                keys.Add(obj.ThrowIfEmpty);
            };
        }

        /// <summary>
        /// SerializableConfigurationAttribute クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="mode">シリアル化を行う方法。</param>
        public SerializableConfigurationAttribute(SerializerMode mode)
            : this()
        {
            this.Mode = mode;
        }

        /// <summary>
        /// シリアル化を行う方法 を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="SerializerMode"/> 型。
        /// <para>シリアル化を行う方法。既定値は SerializerMode.None です。</para>
        /// </value>
        public SerializerMode Mode
        {
            get;
            private set;
        }

        /// <summary>
        /// 構成ファイルのパス を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>構成ファイルのパス。既定値は string.Empty です。</para>
        /// </value>
        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// アプリケーション設定のキー名 (appSettings/add/@key の値) を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>アプリケーション設定のキー名 (appSettings/add/@key の値)。既定値は string.Empty です。</para>
        /// </value>
        public string AppSettingKey
        {
            get;
            set;
        }

        /// <summary>
        /// 空の定義として構築をせずに例外を発生させるか を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="bool"/> 型。
        /// <para>空の定義として構築をせずに例外を発生させる場合は true。既定値は false です。</para>
        /// </value>
        public bool ThrowIfEmpty
        {
            get;
            set;
        }

        /// <summary>
        /// 対象の型のキーとなるオブジェクトを取得するメソッドのデリゲート を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Action&lt;T1, T2&gt;"/> 型。
        /// <para>対象の型のキーとなるオブジェクトを取得するメソッドのデリゲート。既定値は null です。</para>
        /// </value>
        private Action<SerializableConfigurationAttribute, ICollection<object>> KeysFactory
        {
            get;
            set;
        }

        /// <summary>
        /// 指定した Object が、現在の Object と等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在の Object と比較する Object。</param>
        /// <returns>指定した Object が現在の Object と等しい場合は true。それ以外の場合は false。</returns>
        public override bool Equals(object obj)
        {
            return ((IEquatable<SerializableConfigurationAttribute>)this).Equals(obj as SerializableConfigurationAttribute);
        }

        /// <summary>
        /// 指定した Object インスタンスが等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在の Object と比較する Object。</param>
        /// <returns>指定した Object が現在の Object と等しい場合は true。それ以外の場合は false。</returns>
        bool IEquatable<SerializableConfigurationAttribute>.Equals(SerializableConfigurationAttribute obj)
        {
            return EqualityComparerHelper.Equals(this, obj, this.KeysFactory);
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在の System.Object のハッシュコード。</returns>
        public override int GetHashCode()
        {
            return EqualityComparerHelper.GetHashCode(this, this.KeysFactory);
        }

        /// <summary>
        /// 現在の Object を表す String を返します。
        /// </summary>
        /// <returns>現在の Object を表す String。</returns>
        public override string ToString()
        {
            //return ObjectHelper<SerializableConfigurationAttribute>.ToString(this);
            return base.ToString();
        }

        /// <summary>
        /// 現在のインスタンスで構成ファイルのパスを解決します。
        /// </summary>
        /// <returns>構成ファイルのパス。</returns>
        internal string ResolveFilePath()
        {
            var filepath = string.Empty;
            if (!string.IsNullOrEmpty(this.FilePath))
            {
                filepath = this.FilePath;
            }
            else if (!string.IsNullOrEmpty(this.AppSettingKey))
            {
                var appSettings = ConfigurationManager.AppSettings;
                filepath = appSettings.Keys.OfType<string>()
                    .Where((key) => key == this.AppSettingKey)
                    .Select((key) => appSettings[key])
                    .FirstOrDefault()
                    ?? string.Empty;
            }
            
            return filepath;
        }
    }
}