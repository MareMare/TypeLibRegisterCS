// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualityComparerHelper.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// オブジェクトが等しいかどうかの比較をサポートするオブジェクトを表します。
    /// </summary>
    public static class EqualityComparerHelper
    {
        /// <summary>
        /// T 型の 2 つのオブジェクトが等しいかどうかを確認します。
        /// </summary>
        /// <typeparam name="T">比較するオブジェクトの型。</typeparam>
        /// <param name="objA">比較対象の第 1 オブジェクト。</param>
        /// <param name="objB">比較対象の第 2 オブジェクト。</param>
        /// <param name="keysFactory">対象の型のキーとなるオブジェクトを取得するメソッドのデリゲート。</param>
        /// <returns>指定したオブジェクトが等しい場合は true。それ以外の場合は false。</returns>
        public static bool Equals<T>(T objA, T objB, Action<T, ICollection<object>> keysFactory)
        {
            if ((objA == null) ^ (objB == null))
            {
                return false;
            }

            if (objA == null && objB == null)
            {
                return true;
            }

            ICollection<object> keysA = new List<object>();
            ICollection<object> keysB = new List<object>();
            keysFactory(objA, keysA);
            keysFactory(objB, keysB);
            return EqualityComparerHelper.EnumerableEquals(keysA, keysB);
        }

        /// <summary>
        /// ハッシュアルゴリズムや、ハッシュテーブルなどのデータ構造体の指定したオブジェクトに使用するハッシュ関数として機能します。
        /// </summary>
        /// <typeparam name="T">ハッシュコードを取得するオブジェクトの型。</typeparam>
        /// <param name="obj">ハッシュコードを取得する対象となるオブジェクト。</param>
        /// <param name="keysFactory">対象の型のキーとなるオブジェクトを取得するメソッドのデリゲート。</param>
        /// <returns>指定したオブジェクトのハッシュコード。</returns>
        public static int GetHashCode<T>(T obj, Action<T, ICollection<object>> keysFactory)
        {
            if (obj == null)
            {
                return 0;
            }

            ICollection<object> keys = new List<object>();
            keysFactory(obj, keys);
            return EqualityComparerHelper.EnumerableGetHashCode(keys);
        }

        /// <summary>
        /// 指定した列挙子が等しいかどうかを判断します。
        /// </summary>
        /// <param name="enumerableA">比較対象の第 1 列挙子。</param>
        /// <param name="enumerableB">比較対象の第 2 列挙子。</param>
        /// <returns>enumerableA と enumerableB が要素単位で同じインスタンスである場合、または両方のインスタンスが null 参照の場合は true。それ以外の場合は false。</returns>
        internal static bool EnumerableEquals(IEnumerable enumerableA, IEnumerable enumerableB)
        {
            if ((enumerableA == null) ^ (enumerableB == null))
            {
                return false;
            }

            if (enumerableA == null && enumerableB == null)
            {
                return true;
            }

            // どちらも空で無い列挙子を列挙操作し、順次比較します。
            var enumeratorA = enumerableA.GetEnumerator();
            var enumeratorB = enumerableB.GetEnumerator();
            while (enumeratorA.MoveNext())
            {
                if (!enumeratorB.MoveNext())
                {
                    return false;
                }

                var valueA = enumeratorA.Current;
                var valueB = enumeratorB.Current;
                if (!Equals(valueA, valueB))
                {
                    return false;
                }
            }

            return !enumeratorB.MoveNext();
        }

        /// <summary>
        /// 指定した列挙子からハッシュコードを算出します。
        /// </summary>
        /// <param name="enumerable">ハッシュコードの算出要素となる列挙子。</param>
        /// <returns>算出したオブジェクトのハッシュコード。</returns>
        internal static int EnumerableGetHashCode(IEnumerable enumerable)
        {
            /*
            if (enumerable == null)
            {
                return 0;
            }
            int hashCode = 0;
            foreach (var value in enumerable)
            {
                if (value != null)
                {
                    hashCode ^= value.GetHashCode();
                }
            }
            return hashCode;
            */

            if (enumerable == null)
            {
                return 0;
            }

            int? hashCode = null;
            foreach (var value in enumerable)
            {
                if (value != null)
                {
                    if (!hashCode.HasValue)
                    {
                        hashCode = 17;
                    }

                    hashCode = (37 * hashCode) + value.GetHashCode();
                }
            }

            return hashCode ?? 0;
        }
    }
}
