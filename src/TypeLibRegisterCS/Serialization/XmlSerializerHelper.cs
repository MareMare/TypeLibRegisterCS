// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlSerializerHelper.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace TypeLibRegisterCS.Serialization
{
    /// <summary>
    /// オブジェクトから XML ドキュメントへのシリアル化および XML ドキュメントからオブジェクトへの逆シリアル化するオブジェクトを表します。
    /// <para>このクラスは継承できません。</para>
    /// </summary>
    internal sealed class XmlSerializerHelper : ISerializerHelper
    {
        /// <summary>
        /// XmlSerializerHelper クラスの新しいインスタンスを初期化します。
        /// </summary>
        internal XmlSerializerHelper()
        {
        }

        /// <summary>
        /// 指定した Object をシリアル化し、指定したファイルに XML ドキュメントとして書き込みます。
        /// filePath に指定されたフォルダが存在しない場合は新規に生成します。
        /// </summary>
        /// <param name="filePath">保存するファイルのパス。</param>
        /// <param name="value">シリアル化する Object。</param>
        /// <exception cref="SerializerException"></exception>
        public void Save(string filePath, object value)
        {
            XmlDocument SerializeCore(object graph) => this.Serialize(graph);
            SerializerCache<XmlSerializer>.Save(filePath, value, SerializeCore);
        }

        /// <summary>
        /// 指定したファイルに格納されている XML ドキュメントを逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="filePath">逆シリアル化する XML ドキュメントを格納しているファイルのパス。</param>
        /// <returns>逆シリアル化される Object。</returns>
        /// <exception cref="SerializerException"></exception>
        public T Load<T>(string filePath)
        {
            T DeserializeCore(XmlDocument document) => this.Deserialize<T>(document);
            return SerializerCache<XmlSerializer>.Load(filePath, DeserializeCore);
        }

        /// <summary>
        /// 指定した Object を XML ドキュメントにシリアル化します。
        /// </summary>
        /// <param name="value">シリアル化する Object。</param>
        /// <returns>シリアル化された XML ドキュメント。</returns>
        public XmlDocument Serialize(object value)
        {
            XmlSerializer Factory(Type type) => XmlSerializerHelper.Create(type);

            void Action(XmlSerializer serializer, XmlWriter writer, object graph) =>
                serializer.Serialize(writer, graph);

            return SerializerCache<XmlSerializer>.Serialize(value, Factory, Action);
        }

        /// <summary>
        /// 指定した Object を XML 文字列にシリアル化します。
        /// </summary>
        /// <param name="value">シリアル化する Object。</param>
        /// <returns>シリアル化された XML 文字列。</returns>
        public string SerializeXml(object value)
        {
            var document = this.Serialize(value);
            return SerializerCache<XmlSerializer>.ToString(document.DocumentElement);
        }

        /// <summary>
        /// 指定した XML ドキュメントを逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="document">逆シリアル化する XML ドキュメント。</param>
        /// <returns>逆シリアル化される Object。</returns>
        public T Deserialize<T>(XmlDocument document)
        {
            XmlSerializer Factory(Type type) => XmlSerializerHelper.Create(type);
            T DeserializeCore(XmlSerializer serializer, XmlReader reader) => (T)serializer.Deserialize(reader);

            return SerializerCache<XmlSerializer>.Deserialize(document, Factory, DeserializeCore);
        }

        /// <summary>
        /// 指定した XML 文字列を逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="serializedXml">逆シリアル化する XML 文字列。</param>
        /// <returns>逆シリアル化される Object。</returns>
        public T DeserializeXml<T>(string serializedXml)
        {
            var document = new XmlDocument();
            document.LoadXml(serializedXml);
            return this.Deserialize<T>(document);
        }

        /// <summary>
        /// 指定した型のオブジェクトを XML ドキュメントにシリアル化したり、XML ドキュメントを指定した型のオブジェクトに逆シリアル化したりできる、XmlSerializer クラスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="type">XmlSerializer がシリアル化できるオブジェクトの型。</param>
        /// <returns>生成された XmlSerializer。</returns>
        private static XmlSerializer Create(Type type) => new XmlSerializer(type);
    }
}
