// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableConfiguration.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeLibRegisterCS.Serialization;

#endregion

namespace TypeLibRegisterCS.Configurations
{
    /// <summary>
    /// 構成定義データコントラクトのファクトリオブジェクトを表します。
    /// </summary>
    /// <typeparam name="T">構成定義データコントラクトの型。</typeparam>
    public static class SerializableConfiguration<T>
        where T : class, ISerializableConfiguration
    {
        /// <summary>静的メンバへアクセスする際のロックオブジェクトです。</summary>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object SyncRoot = new object();

        /// <summary>キーに関連付けられている構成定義データコントラクトインスタンスのマッピングを表します。</summary>
        private static readonly Dictionary<SerializableConfigurationAttribute, T> Configurations
            = new Dictionary<SerializableConfigurationAttribute, T>();

        /// <summary>
        /// エントリポイントを有するアセンブリが格納されているフォルダを取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>エントリポイントを有するアセンブリが格納されているフォルダ。</para>
        /// </value>
        private static string EntryAssemblyFolder
        {
            get
            {
                var executingAssembly = Assembly.GetEntryAssembly();
                return Path.GetDirectoryName(new Uri(executingAssembly.EscapedCodeBase).LocalPath);
            }
        }

        /// <summary>
        /// 構成定義データコントラクトインスタンスを取得します。
        /// </summary>
        /// <returns>構成定義データコントラクトインスタンス。</returns>
        /// <exception cref="SerializableConfigurationException">指定された型に SerializableConfigurationAttribute 属性が付与されていません。</exception>
        /// <exception cref="SerializableConfigurationException">構成ファイルの読み込み中に例外が発生しました。</exception>
        /// <exception cref="SerializableConfigurationException">アプリケーション構成ファイルに構成ファイルの定義がありません。</exception>
        /// <exception cref="SerializableConfigurationException">構成定義データコントラクトインスタンスの検証で違反を検出しました。</exception>
        public static T GetInstance() =>
            SerializableConfiguration<T>.GetInstanceBy(SerializableConfiguration<T>.GetAttribute);

        /// <summary>
        /// 構成ファイルのパスを指定して構成定義データコントラクトインスタンスを取得します。
        /// </summary>
        /// <param name="filepath">構成ファイルのパス。</param>
        /// <returns>構成定義データコントラクトインスタンス。</returns>
        /// <exception cref="SerializableConfigurationException">指定された型に SerializableConfigurationAttribute 属性が付与されていません。</exception>
        /// <exception cref="SerializableConfigurationException">構成ファイルの読み込み中に例外が発生しました。</exception>
        /// <exception cref="SerializableConfigurationException">アプリケーション構成ファイルに構成ファイルの定義がありません。</exception>
        /// <exception cref="SerializableConfigurationException">構成定義データコントラクトインスタンスの検証で違反を検出しました。</exception>
        public static T GetInstanceByFilePath(string filepath) =>
            SerializableConfiguration<T>.GetInstanceBy(() =>
            {
                var attribute = SerializableConfiguration<T>.GetAttribute();
                var pseudoAttribute = new SerializableConfigurationAttribute(attribute.Mode)
                {
                    FilePath = filepath,
                    ThrowIfEmpty = attribute.ThrowIfEmpty,
                };
                return pseudoAttribute;
            });

        /// <summary>
        /// アプリケーション設定のキー名 (appSettings/add/@key の値) を指定して構成定義データコントラクトインスタンスを取得します。
        /// </summary>
        /// <param name="appSettingKey">アプリケーション設定のキー名 (appSettings/add/@key の値)。</param>
        /// <returns>構成定義データコントラクトインスタンス。</returns>
        /// <exception cref="SerializableConfigurationException">指定された型に SerializableConfigurationAttribute 属性が付与されていません。</exception>
        /// <exception cref="SerializableConfigurationException">構成ファイルの読み込み中に例外が発生しました。</exception>
        /// <exception cref="SerializableConfigurationException">アプリケーション構成ファイルに構成ファイルの定義がありません。</exception>
        /// <exception cref="SerializableConfigurationException">構成定義データコントラクトインスタンスの検証で違反を検出しました。</exception>
        public static T GetInstanceByAppSettingKey(string appSettingKey) =>
            SerializableConfiguration<T>.GetInstanceBy(() =>
            {
                var attribute = SerializableConfiguration<T>.GetAttribute();
                var pseudoAttribute = new SerializableConfigurationAttribute(attribute.Mode)
                {
                    AppSettingKey = appSettingKey,
                    ThrowIfEmpty = attribute.ThrowIfEmpty,
                };
                return pseudoAttribute;
            });

        /// <summary>
        /// 現在のインスタンスを構成ファイルとして保存します。
        /// </summary>
        /// <param name="instanceToSave">保存する T インスタンス。</param>
        [Conditional("DEBUG")]
        public static void Save(T instanceToSave)
        {
            var attribute = SerializableConfiguration<T>.GetAttribute();
            if (SerializableConfiguration<T>.TryGetFilePath(attribute, out var filepath))
            {
                var serializer = SerializerFactory.CreateSerializer(attribute.Mode);
                serializer.Save(filepath, instanceToSave);
            }
        }

        /// <summary>
        /// SerializableConfigurationAttribute 属性を取得します。
        /// </summary>
        /// <returns>取得された SerializableConfigurationAttribute 属性インスタンス。</returns>
        /// <exception cref="SerializableConfigurationException">指定された型に SerializableConfigurationAttribute 属性が付与されていません。</exception>
        private static SerializableConfigurationAttribute GetAttribute()
        {
            SerializableConfigurationAttribute attribute;
            try
            {
                attribute = TypeDescriptor.GetAttributes(typeof(T)).OfType<SerializableConfigurationAttribute>()
                    .First();
            }
            catch (Exception ex)
            {
                throw new SerializableConfigurationException(
                    ex,
                    "指定された型 '{0}' に '{1}' 属性が付与されていません。",
                    typeof(T).Name,
                    nameof(SerializableConfigurationAttribute));
            }

            return attribute;
        }

        /// <summary>
        /// SerializableConfigurationAttribute 属性を指定して構成定義データコントラクトインスタンスを取得します。
        /// </summary>
        /// <param name="getAttribute">SerializableConfigurationAttribute 属性を取得するメソッドのデリゲート。</param>
        /// <returns>構成定義データコントラクトインスタンス。</returns>
        [DebuggerStepThrough]
        private static T GetInstanceBy(Func<SerializableConfigurationAttribute> getAttribute)
        {
            lock (SerializableConfiguration<T>.SyncRoot)
            {
                var attribute = getAttribute();
                if (!SerializableConfiguration<T>.Configurations.ContainsKey(attribute))
                {
                    var config = SerializableConfiguration<T>.LoadOrCreateByAttribute(attribute);
                    SerializableConfiguration<T>.Configurations.Add(attribute, config);
                }

                return SerializableConfiguration<T>.Configurations[attribute];
            }
        }

        /// <summary>
        /// 構成ファイルパスを解決してインスタンスへの逆シリアル化または空の定義を生成します。
        /// </summary>
        /// <param name="attribute">SerializableConfigurationAttribute 属性。</param>
        /// <returns>構成定義データコントラクトインスタンス。</returns>
        /// <exception cref="SerializableConfigurationException">構成ファイルの読み込み中に例外が発生しました。</exception>
        /// <exception cref="SerializableConfigurationException">アプリケーション構成ファイルに構成ファイルの定義がありません。</exception>
        /// <exception cref="SerializableConfigurationException">構成定義データコントラクトインスタンスの検証で違反を検出しました。</exception>
        private static T LoadOrCreateByAttribute(SerializableConfigurationAttribute attribute)
        {
            T config;
            if (SerializableConfiguration<T>.TryGetFilePath(attribute, out var filepath))
            {
                try
                {
                    config = SerializableConfiguration<T>.Load(attribute, filepath);
                }
                catch (Exception ex)
                {
                    if (attribute.ThrowIfEmpty)
                    {
                        // 構成ファイルの読み込み中に例外が発生した場合、必要に応じて例外を発生させます。
                        throw new SerializableConfigurationException(ex, "構成ファイル '{0}' の読み込み中に例外が発生しました。", filepath);
                    }

                    // 構成ファイルの読み込み中に例外が発生した場合、空の定義を使用するようにします。
                    config = SerializableConfiguration<T>.CreateAsEmpty();
                }
            }
            else
            {
                if (attribute.ThrowIfEmpty)
                {
                    // 構成ファイル定義がない場合、必要に応じて例外を発生させます。
                    throw new SerializableConfigurationException("アプリケーション構成ファイルに構成ファイルの定義がありません。属性値: {0}", attribute);
                }

                // 構成ファイル定義がない場合、空の定義を使用するようにします。
                config = SerializableConfiguration<T>.CreateAsEmpty();
            }

            try
            {
                // インスタンスの検証を行います。
                SerializableConfiguration<T>.Validate(config);
            }
            catch (Exception ex)
            {
                // 構成定義データコントラクトインスタンスの検証で違反があります。
                throw new SerializableConfigurationException(ex, "構成定義データコントラクトインスタンスの検証で違反を検出しました。");
            }

            return config;
        }

        /// <summary>
        /// 空の定義として構築します。
        /// </summary>
        /// <returns>構成定義データコントラクトインスタンス。</returns>
        private static T CreateAsEmpty()
        {
            var config = (T)Activator.CreateInstance(typeof(T), true);
            config.ConstructAsEmpty();
            return config;
        }

        /// <summary>
        /// 構成ファイルのパスを取得します。
        /// </summary>
        /// <param name="attribute">SerializableConfigurationAttribute 属性。</param>
        /// <param name="filepath">構成ファイルのパス。</param>
        /// <returns>構成ファイルのパスが正しく取得できた場合は true。それ以外は false。</returns>
        private static bool TryGetFilePath(SerializableConfigurationAttribute attribute, out string filepath)
        {
            filepath = string.Empty;

            var configurationFilePath = attribute.ResolveFilePath();
            if (string.IsNullOrEmpty(configurationFilePath))
            {
                return false;
            }

            filepath = configurationFilePath;
            if (!Path.IsPathRooted(configurationFilePath))
            {
                filepath = Path.Combine(SerializableConfiguration<T>.EntryAssemblyFolder, configurationFilePath);
            }

            return true;
        }

        /// <summary>
        /// 指定された構成ファイルパスからインスタンスへ逆シリアル化します。
        /// </summary>
        /// <param name="attribute">SerializableConfigurationAttribute 属性。</param>
        /// <param name="filepath">構成ファイルのパス。</param>
        /// <returns>読み込まれたインスタンス。</returns>
        private static T Load(SerializableConfigurationAttribute attribute, string filepath)
        {
            var serializer = SerializerFactory.CreateSerializer(attribute.Mode);
            var config = serializer.Load<T>(filepath);
            return config;
        }

        /// <summary>
        /// 指定されたインスタンスを検証します。
        /// </summary>
        /// <param name="config">検証する T インスタンス。</param>
        /// <exception cref="ArgumentNullException">値を Null にすることはできません。</exception>
        private static void Validate(T config)
        {
            Guard.ArgumentNotNull(config, "config");
            config.Validate();
        }
    }
}
