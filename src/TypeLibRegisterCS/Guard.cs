// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Guard.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Globalization;

#endregion

namespace TypeLibRegisterCS
{
    /// <summary>
    /// 引数に異常があった場合に例外を発生させるオブジェクトを表します。
    /// <para>このクラスは継承できません。</para>
    /// </summary>
    public sealed class Guard
    {
        /// <summary>文字列が null または長さ 0 の文字列である場合のエラーメッセージです。</summary>
        private const string StringCannotBeEmpty = "文字列が Null、または長さ 0 の文字列 \"\" です。\r\nパラメータ名: {0}";

        /// <summary>値が null である場合のエラーメッセージです。</summary>
        private const string ObjectCannotBeNull = "値を Null にすることはできません。";

        /// <summary>配列が null または要素数 0 である場合のエラーメッセージです。</summary>
        private const string ArrayCannotBeEmpty = "配列が Null、または要素数 0 です。\r\nパラメータ名: {0}";

        /// <summary>値が有効な範囲でない場合のエラーメッセージです。</summary>
        private const string InvalidRangeValue = "指定された引数は、有効な値の範囲内にありません。";

        /// <summary>値が妥当でない場合のエラーメッセージです。</summary>
        private const string InvalidParameterValue = "指定された引数が妥当ではありません。\r\nパラメータ名: {0}";

        /// <summary>値が列挙型に含まれていない場合のエラーメッセージです。</summary>
        private const string InvalidEnumValue = "指定された列挙値 '{0}' は、指定した列挙型 '{1}' には定義されていません。";

        /// <summary>対象の型が指定の型へ代入不可な場合のエラーメッセージです。</summary>
        private const string TypeNotCompatible = "比較対象の型 '{0}' は、指定された提供される型 '{1}' へ代入できません。";

        /// <summary>
        /// Guard クラスの新しいインスタンスを初期化します。
        /// </summary>
        private Guard()
        {
        }

        /// <summary>
        /// 指定された文字列型の引数値が String.IsNullOrEmpty である場合 ArgumentException 例外を発生させます。
        /// </summary>
        /// <param name="argumentValue">引数の値を表す文字列。</param>
        /// <param name="argumentName">引数の名称。</param>
        /// <exception cref="ArgumentException">文字列が Null、または長さ 0 の文字列 "" です。</exception>
        public static void ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
        {
            if (argumentValue == null || argumentValue.Length == 0)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    Guard.StringCannotBeEmpty,
                    argumentName));
            }
        }

        /// <summary>
        /// 指定された Object 型の引数値が null である場合 ArgumentNullException 例外を発生させます。
        /// </summary>
        /// <param name="argumentValue">引数の値を表す Object。</param>
        /// <param name="argumentName">引数の名称。</param>
        /// <exception cref="ArgumentNullException">値を Null にすることはできません。</exception>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName, Guard.ObjectCannotBeNull);
            }
        }

        /// <summary>
        /// 指定された配列の引数値が null または要素数が 0 である場合 ArgumentException 例外を発生させます。
        /// </summary>
        /// <param name="argumentValue">引数の値を表す Object 配列。</param>
        /// <param name="argumentName">引数の名称。</param>
        /// <exception cref="ArgumentException">配列を null または要素数 0 にすることはできません。</exception>
        public static void ArgumentNotNullOrEmptyArray(Array argumentValue, string argumentName)
        {
            if (argumentValue == null || argumentValue.Length == 0)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    Guard.ArrayCannotBeEmpty,
                    argumentName));
            }
        }

        /// <summary>
        /// 指定された T 型の引数値が validate メソッドでの評価が false の場合 ArgumentOutOfRangeException 例外を発生させます。
        /// </summary>
        /// <typeparam name="T">引数の値の型。</typeparam>
        /// <param name="argumentValue">引数の値。</param>
        /// <param name="argumentName">引数の名称。</param>
        /// <param name="validate">引数値の範囲を検証するメソッドの Predicate&lt;T&gt; デリゲート。
        /// このデリゲートに渡された値を検証し、範囲内である場合は true、それ以外の場合は false を返します。
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">指定された引数は、有効な値の範囲内にありません。</exception>
        public static void ArgumentNotOutOfRange<T>(T argumentValue, string argumentName, Predicate<T> validate)
        {
            if (validate == null)
            {
                return;
            }

            if (validate(argumentValue) == false)
            {
                throw new ArgumentOutOfRangeException(argumentName, Guard.InvalidRangeValue);
            }
        }

        /// <summary>
        /// 指定された T 型の引数値が validate メソッドでの評価が false の場合 ArgumentException 例外を発生させます。
        /// </summary>
        /// <typeparam name="T">引数の値の型。</typeparam>
        /// <param name="argumentValue">引数の値。</param>
        /// <param name="argumentName">引数の名称。</param>
        /// <param name="validate">引数値の妥当性を検証するメソッドの Predicate&lt;T&gt; デリゲート。
        /// このデリゲートに渡された値を検証し、妥当である場合は true、それ以外の場合は false を返します。
        /// </param>
        /// <exception cref="ArgumentException">指定された引数が妥当ではありません。</exception>
        public static void ArgumentIsValid<T>(T argumentValue, string argumentName, Predicate<T> validate)
        {
            if (validate == null)
            {
                return;
            }

            if (validate(argumentValue) == false)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    Guard.InvalidParameterValue,
                    argumentName));
            }
        }

        /// <summary>
        /// 指定された列挙型に指定された値が定義されていない場合 ArgumentException 例外を発生させます。
        /// </summary>
        /// <param name="enumType">検索する列挙型。</param>
        /// <param name="value">列挙値を表す Object。</param>
        /// <param name="argumentName">引数の名称。</param>
        /// <exception cref="ArgumentException">指定した列挙型には、指定された列挙値が定義されていません。</exception>
        public static void EnumValueIsDefined(Type enumType, object value, string argumentName)
        {
            if (Enum.IsDefined(enumType, value) == false)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    Guard.InvalidEnumValue,
                    argumentName,
                    enumType.ToString()));
            }
        }

        /// <summary>
        /// 指定した assignee の型を指定の providedType の型に代入できない場合 ArgumentException 例外を発生させます。
        /// </summary>
        /// <param name="assignee">比較対象の型。</param>
        /// <param name="providedType">提供される型。</param>
        /// <param name="argumentName">引数の名称。</param>
        /// <exception cref="ArgumentException">比較対象の型は、指定された提供される型へ代入できません。</exception>
        public static void TypeIsAssignableFromType(Type assignee, Type providedType, string argumentName)
        {
            if (!providedType.IsAssignableFrom(assignee))
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, Guard.TypeNotCompatible, assignee, providedType),
                    argumentName);
            }
        }
    }
}
