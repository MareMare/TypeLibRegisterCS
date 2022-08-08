// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerCache.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

#endregion

namespace TypeLibRegisterCS.Serialization
{
    /// <summary>
    /// <see cref="XmlSerializerHelper"/> に共通の具体的なシリアル化および逆シリアル化するオブジェクトを表します。
    /// <para>このクラスは継承できません。</para>
    /// </summary>
    /// <typeparam name="TSerializer">シリアル化、逆シリアル化を行うシリアライザの型。</typeparam>
    internal sealed class SerializerCache<TSerializer> 
        where TSerializer : class
    {
        /// <summary>SerializerCache&lt;TSerializer&gt; の唯一のインスタンスを表します。</summary>
        private static readonly SerializerCache<TSerializer> singleton = new SerializerCache<TSerializer>();

        /// <summary>Type に対応した TSerializer のキャッシュを表します。</summary>
        private readonly Dictionary<Type, TSerializer> cachedSerializer = null;

        /// <summary>
        /// SerializerCache クラスの新しいインスタンスを初期化します。
        /// </summary>
        private SerializerCache()
        {
            this.cachedSerializer = new Dictionary<Type, TSerializer>();
        }

        /// <summary>
        /// エントリポイントを有するアセンブリが格納されているフォルダを取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>エントリポイントを有するアセンブリが格納されているフォルダ。</para>
        /// </value>
        private static string EntryAssemblyFolder
        {
            get
            {
                Assembly executingAssembly = Assembly.GetEntryAssembly();
                return Path.GetDirectoryName(new Uri(executingAssembly.EscapedCodeBase).LocalPath);
            }
        }

        /// <summary>
        /// 指定した Object をシリアル化し、指定したファイルに XML ドキュメントとして書き込みます。
        /// <para>ファイルリソースへの排他制御は呼び出し側で必要に応じて実装してください。</para>
        /// </summary>
        /// <param name="filePath">保存するファイルのパス。</param>
        /// <param name="obj">シリアル化する Object。</param>
        /// <param name="serialize">
        /// シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;object, XmlDocument&gt; である必要があります。
        /// </param>
        /// <exception cref="SerializerException"></exception>
        public static void Save(string filePath, object obj, Func<object, XmlDocument> serialize)
        {
            // filePath のルートがフォルダでない場合、実行時のフォルダをルートとする
            string path = SerializerCache<TSerializer>.ResolvePath(filePath);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            try
            {
                string tempFilePath = Path.ChangeExtension(path, ".tmp");
                string backFilePath = Path.ChangeExtension(path, ".bak");

                XmlDocument document = serialize(obj);
                document.Save(tempFilePath);
                StreamAsyncHelper.GenerateFile(path, tempFilePath, backFilePath);
            }
            catch (Exception ex)
            {
                throw new SerializerException(ex);
            }
        }

        /// <summary>
        /// 指定した Object をシリアル化し、指定したファイルに XML ドキュメントとして非同期で書き込みます。
        /// <para>ファイルリソースへの排他制御は呼び出し側で必要に応じて実装してください。</para>
        /// </summary>
        /// <param name="filePath">保存するファイルのパス。</param>
        /// <param name="obj">シリアル化する Object。</param>
        /// <param name="serialize">
        /// シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;object, XmlDocument&gt; である必要があります。
        /// </param>
        /// <param name="success">非同期書き込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期書き込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        public static void SaveAsync(string filePath, object obj, Func<object, XmlDocument> serialize, Action success, Action<Exception> failure)
        {
            // filePath のルートがフォルダでない場合、実行時のフォルダをルートとする
            string path = SerializerCache<TSerializer>.ResolvePath(filePath);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            try
            {
                string tempFilePath = Path.ChangeExtension(path, ".tmp");
                string backFilePath = Path.ChangeExtension(path, ".bak");

                StreamAsyncHelper.SaveAndXmlSerializeAsync(
                    tempFilePath,
                    obj,
                    serialize,
                    () =>
                    {
                        try
                        {
                            StreamAsyncHelper.GenerateFile(path, tempFilePath, backFilePath);
                            success();
                        }
                        catch (Exception ex)
                        {
                            failure(new SerializerException(ex));
                        }
                    },
                    (ex) =>
                    {
                        failure(new SerializerException(ex));
                    });
            }
            catch (Exception ex)
            {
                throw new SerializerException(ex);
            }
        }

        /// <summary>
        /// 指定したファイルに格納されている XML ドキュメントを逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="filePath">逆シリアル化する XML ドキュメントを格納しているファイルのパス。</param>
        /// <param name="deserialize">
        /// 逆シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;XmlDocument, T&gt; である必要があります。
        /// </param>
        /// <returns>逆シリアル化される Object。</returns>
        /// <exception cref="SerializerException"></exception>
        public static T Load<T>(string filePath, Func<XmlDocument, T> deserialize)
        {
            try
            {
                string path = SerializerCache<TSerializer>.ResolvePath(filePath);
                XmlDocument document = new XmlDocument();
                document.Load(path);
                return deserialize(document);
            }
            catch (Exception ex)
            {
                throw new SerializerException(ex);
            }
        }

        /// <summary>
        /// 指定したファイルに格納されている XML ドキュメントを非同期で逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="filePath">逆シリアル化する XML ドキュメントを格納しているファイルのパス。</param>
        /// <param name="deserialize">
        /// 逆シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;XmlDocument, T&gt; である必要があります。
        /// </param>
        /// <param name="success">非同期読み込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期読み込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        public static void LoadAsync<T>(string filePath, Func<XmlDocument, T> deserialize, Action<T> success, Action<Exception> failure)
        {
            try
            {
                string path = SerializerCache<TSerializer>.ResolvePath(filePath);
                StreamAsyncHelper.LoadAndXmlDeserializeAsync<T>(path, deserialize, success, failure);
            }
            catch (Exception ex)
            {
                failure(new SerializerException(ex));
            }
        }

        /// <summary>
        /// 指定した Object を XML ドキュメントにシリアル化します。
        /// </summary>
        /// <param name="obj">シリアル化する Object。</param>
        /// <param name="factory">
        /// 実際にシリアル化する際に使用される TSerializer を生成するメソッドのデリゲート。
        /// これは Func&lt;Type, TSerializer&gt; である必要があります。
        /// </param>
        /// <param name="serialize">
        /// シリアル化を行うメソッドのデリゲート。
        /// これは Action&lt;TSerializer, XmlWriter, object&gt; である必要があります。
        /// </param>
        /// <returns>シリアル化された XML ドキュメント。</returns>
        public static XmlDocument Serialize(object obj, Func<Type, TSerializer> factory, Action<TSerializer, XmlWriter, object> serialize)
        {
            XmlDocument document = new XmlDocument();
            TSerializer serializer = SerializerCache<TSerializer>.GetSerializer(obj.GetType(), factory);
            using (MemoryStream stream = new MemoryStream())
            {
                // XML ドキュメントへ出力する設定を初期化します。
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.ConformanceLevel = ConformanceLevel.Document;

                // XmlWriterSettings を使用して XmlTextWriter を生成し MemoryStream を媒体として XmlDocument にシリアル化します。
                using (XmlWriter writer = XmlTextWriter.Create(stream, settings))
                {
                    serialize(serializer, writer, obj);
                    writer.Flush();
                    stream.Position = 0;
                    document.Load(stream);
                    writer.Close();
                }
            }

            return document;
        }

        /// <summary>
        /// 指定した XML ドキュメントを逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="document">逆シリアル化する XML ドキュメントを格納している XmlDocument。</param>
        /// <param name="factory">
        /// 実際にシリアル化する際に使用される TSerializer を生成するメソッドのデリゲート。
        /// これは Func&lt;Type, TSerializer&gt; である必要があります。
        /// </param>
        /// <param name="deserialize">
        /// 逆シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;TSerializer, XmlReader, T&gt; である必要があります。
        /// </param>
        /// <returns>逆シリアル化される Object。</returns>
        /// <remarks>
        /// XML 仕様に基づき CR+LF は LF に正規化されるので空白、改行を保持するために XmlReader からの復元を行います。
        /// <para>XML 仕様については http://www.w3.org/TR/2004/REC-xml-20040204/#sec-line-ends を参照してください。</para>
        /// </remarks>
        public static T Deserialize<T>(XmlDocument document, Func<Type, TSerializer> factory, Func<TSerializer, XmlReader, T> deserialize)
        {
            // XML 仕様に基づき CR+LF は LF に正規化されるので
            // 空白、改行を保持するために XmlReader からの復元をおこなう
            // http://www.w3.org/TR/2004/REC-xml-20040204/#sec-line-ends
            document.PreserveWhitespace = true;
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(new XmlNodeReader(document), settings);
            TSerializer serializer = SerializerCache<TSerializer>.GetSerializer(typeof(T), factory);
            object deserialized = deserialize(serializer, reader);
            if (deserialized == null)
            {
                return default(T);
            }

            return (T)deserialized;
        }

        /// <summary>
        /// 指定した Type に対応したキャッシュされた TSerializer を取得します。
        /// </summary>
        /// <param name="type">TSerializer がシリアル化できるオブジェクトの型。</param>
        /// <param name="factory">
        /// 実際にシリアル化する際に使用される TSerializer を生成するメソッドのデリゲート。
        /// これは Func&lt;Type, TSerializer&gt; である必要があります。
        /// </param>
        /// <returns>取得される TSerializer。</returns>
        private static TSerializer GetSerializer(Type type, Func<Type, TSerializer> factory)
        {
            return singleton.GetCachedSerializer(type, factory);
        }

        /// <summary>
        /// 指定したファイルパスを解決します。
        /// </summary>
        /// <param name="filePath">対象となるファイルパス。</param>
        /// <returns>絶対パスを表す文字列。</returns>
        /// <remarks>
        /// 指定したファイルパスに絶対パス情報または相対パスの情報が含まれない場合は
        /// 現在実行中のコードを格納しているアセンブリの親フォルダを用いて絶対パスを取得します。
        /// </remarks>
        private static string ResolvePath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }

            return Path.Combine(SerializerCache<TSerializer>.EntryAssemblyFolder, filePath);
        }

        /// <summary>
        /// 指定された XmlNode より、XmlDocument を生成します。
        /// </summary>
        /// <param name="xmlNode">XML ドキュメントとして作成される XmlNode 。</param>
        /// <returns>生成された XmlDocument 。</returns>
        internal static XmlDocument CreateXmlDocument(XmlNode xmlNode)
        {
            try
            {
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.AppendChild(xmldocument.ImportNode(xmlNode, true));
                return xmldocument;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 指定された XmlElement の XML 文字列を UTF8 形式のエンコーディングで取得します。
        /// </summary>
        /// <param name="element">XML 文字列を取得する XmlElement。</param>
        /// <returns>UTF8 形式でエンコーディングされた文字列。</returns>
        internal static string ToString(XmlElement element)
        {
            return SerializerCache<TSerializer>.ToString(element, Encoding.UTF8);
        }

        /// <summary>
        /// 指定された XmlElement の XML 文字列を 指定のエンコーディングで取得します。
        /// </summary>
        /// <param name="element">XML 文字列を取得される XmlElement。</param>
        /// <param name="encoding">エンコーディングに使用される Encoding。</param>
        /// <returns>指定エンコーディングされた文字列。</returns>
        internal static string ToString(XmlElement element, Encoding encoding)
        {
            try
            {
                string encodedText = String.Empty;
                using (MemoryStream stream = new MemoryStream())
                {
                    // XML 文字列を取得される XmlElement を新しい XmlDocument へインポートし MemoryStream へ保存します。
                    XmlDocument dumpDoc = new XmlDocument();
                    dumpDoc.AppendChild(dumpDoc.ImportNode((XmlNode)element, true));
                    dumpDoc.Save(stream);

                    // 保存された MemoryStream の先頭からバイト配列を指定のエンコードで文字列化します。
                    stream.Position = 0;
                    encodedText = encoding.GetString(stream.GetBuffer());
                }

                return encodedText;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 指定した Type に対応したキャッシュされた TSerializer を取得します。
        /// </summary>
        /// <param name="type">TSerializer がシリアル化できるオブジェクトの型。</param>
        /// <param name="factory">
        /// 実際にシリアル化する際に使用される TSerializer を生成するメソッドのデリゲート。
        /// これは Func&lt;Type, TSerializer&gt; である必要があります。
        /// </param>
        /// <returns>取得される TSerializer。</returns>
        private TSerializer GetCachedSerializer(Type type, Func<Type, TSerializer> factory)
        {
            lock (this.cachedSerializer)
            {
                if (!this.cachedSerializer.ContainsKey(type))
                {
                    if (factory != null)
                    {
                        this.cachedSerializer[type] = factory(type);
                    }
                }

                return this.cachedSerializer[type];
            }
        }
    }
}
