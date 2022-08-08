// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializerHelper.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System.Xml;

#endregion

namespace TypeLibRegisterCS.Serialization
{
    /// <summary>
    /// ドキュメントにシリアル化または逆シリアル化するオブジェクト のインターフェイスを表します。
    /// </summary>
    public interface ISerializerHelper
    {
        /// <summary>
        /// 指定した Object をシリアル化し、指定したファイルに XML ドキュメントとして書き込みます。
        /// filePath に指定されたフォルダが存在しない場合は新規に生成します。
        /// </summary>
        /// <param name="filePath">保存するファイルのパス。</param>
        /// <param name="value">シリアル化する Object。</param>
        /// <exception cref="SerializerException"></exception>
        void Save(string filePath, object value);

        /// <summary>
        /// 指定したファイルに格納されている XML ドキュメントを逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="filePath">逆シリアル化する XML ドキュメントを格納しているファイルのパス。</param>
        /// <returns>逆シリアル化される Object。</returns>
        /// <exception cref="SerializerException"></exception>
        T Load<T>(string filePath);

        /// <summary>
        /// 指定した Object を XML ドキュメントにシリアル化します。
        /// </summary>
        /// <param name="value">シリアル化する Object。</param>
        /// <returns>シリアル化された XML ドキュメント。</returns>
        XmlDocument Serialize(object value);

        /// <summary>
        /// 指定した Object を XML 文字列にシリアル化します。
        /// </summary>
        /// <param name="value">シリアル化する Object。</param>
        /// <returns>シリアル化された XML 文字列。</returns>
        string SerializeXml(object value);

        /// <summary>
        /// 指定した XML ドキュメントを逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="document">逆シリアル化する XML ドキュメント。</param>
        /// <returns>逆シリアル化される Object。</returns>
        T Deserialize<T>(XmlDocument document);

        /// <summary>
        /// 指定した XML 文字列を逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="serializedXml">逆シリアル化する XML 文字列。</param>
        /// <returns>逆シリアル化される Object。</returns>
        T DeserializeXml<T>(string serializedXml);
    }
}
