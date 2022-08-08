// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerException.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Globalization;
using System.Runtime.Serialization;

#endregion

namespace TypeLibRegisterCS.Serialization
{
    /// <summary>
    /// <see cref="XmlSerializerHelper" /> の各クラスにて例外が発生した場合にスローされる例外のオブジェクトを表します。
    /// </summary>
    [Serializable]
    public class SerializerException : Exception
    {
        /// <summary>
        /// SerializerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SerializerException()
        {
        }

        /// <summary>
        /// 指定したエラー メッセージを使用して、SerializerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">エラーを説明するメッセージ。</param>
        public SerializerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 指定したエラー メッセージの複合書式指定文字列を使用して、SerializerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="format">エラーを説明するメッセージの複合書式指定文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ Object 配列。</param>
        public SerializerException(string format, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, format, args))
        {
        }

        /// <summary>
        /// この例外の原因である内部例外への参照を使用して、SerializerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="innerException">現在の例外の原因である例外。</param>
        public SerializerException(Exception innerException)
            : base(innerException != null ? innerException.Message : string.Empty, innerException)
        {
        }

        /// <summary>
        /// 指定したエラー メッセージとこの例外の原因である内部例外への参照を使用して、SerializerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">エラーを説明するメッセージ。</param>
        /// <param name="innerException">現在の例外の原因である例外。</param>
        public SerializerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// シリアル化したデータを使用して、SerializerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info">シリアル化されたオブジェクト データを保持するオブジェクト。</param>
        /// <param name="context">転送元または転送先に関するコンテキスト情報。</param>
        protected SerializerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
