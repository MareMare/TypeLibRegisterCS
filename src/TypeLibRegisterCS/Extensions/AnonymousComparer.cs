// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnonymousComparer.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// IComparer&lt;T&gt; インターフェイスおよび IEqualityComparer&lt;T&gt; インターフェイスを実装した匿名のオブジェクトを表します。
    /// </summary>
    public static class AnonymousComparer
    {
        /// <summary>
        /// IComparer&lt;T&gt; インターフェイスを実装した匿名インスタンスを生成します。
        /// </summary>
        /// <typeparam name="T">比較するオブジェクトの型。</typeparam>
        /// <param name="compares">比較を実行する Func&lt;T, T, int&gt; デリゲートの配列。</param>
        /// <returns>IComparer&lt;T&gt; インターフェイスを実装した匿名インスタンス。</returns>
        public static IComparer<T> Create<T>(params Func<T, T, int>[] compares)
        {
            int AllCompare(T x, T y)
            {
                foreach (var compare in compares)
                {
                    var compared = new Comparer<T>(compare).Compare(x, y);
                    if (compared != 0)
                    {
                        return compared;
                    }
                }

                return 0;
            }

            return new Comparer<T>(AllCompare);
        }

        /// <summary>
        /// IComparer&lt;T&gt; インターフェイスを実装した匿名インスタンスを生成します。
        /// </summary>
        /// <typeparam name="T">比較するオブジェクトの型。</typeparam>
        /// <param name="compare">比較を実行する Func&lt;T, T, int&gt; デリゲート。</param>
        /// <returns>IComparer&lt;T&gt; インターフェイスを実装した匿名インスタンス。</returns>
        public static IComparer<T> Create<T>(Func<T, T, int> compare)
        {
            Guard.ArgumentNotNull(compare, "compare");
            return new Comparer<T>(compare);
        }

        /// <summary>
        /// IEqualityComparer&lt;T&gt; インターフェイスを実装した匿名インスタンスを生成します。
        /// これは比較するオブジェクトの型が実装するすべてのプロパティを比較対象とします。
        /// </summary>
        /// <typeparam name="T">比較するオブジェクトの型。</typeparam>
        /// <returns>IEqualityComparer&lt;T&gt; インターフェイスを実装した匿名インスタンス。</returns>
        public static IEqualityComparer<T> Create<T>()
        {
            Action<T, ICollection<object>> keysFactory = (item, keys) =>
            {
                var query = typeof(T).GetProperties().Select(info => info.GetValue(item, null));
                query.ForEach((key, index) => keys.Add(key));
            };
            return AnonymousComparer.Create(keysFactory);
        }

        /// <summary>
        /// IEqualityComparer&lt;T&gt; インターフェイスを実装した匿名インスタンスを生成します。
        /// </summary>
        /// <typeparam name="T">比較するオブジェクトの型。</typeparam>
        /// <param name="keysFactory">対象の型のキーとなるオブジェクトを取得するメソッドのデリゲート。</param>
        /// <returns>IEqualityComparer&lt;T&gt; インターフェイスを実装した匿名インスタンス。</returns>
        public static IEqualityComparer<T> Create<T>(Action<T, ICollection<object>> keysFactory)
        {
            Guard.ArgumentNotNull(keysFactory, "keysFactory");
            return new EqualityComparer<T>(
                (x, y) => EqualityComparerHelper.Equals(x, y, keysFactory),
                obj => EqualityComparerHelper.GetHashCode(obj, keysFactory));
        }

        /// <summary>
        /// 2 つのオブジェクトを比較するために型が実装するメソッドを定義します。
        /// </summary>
        /// <typeparam name="T">比較するオブジェクトの型。</typeparam>
        private class Comparer<T> : IComparer<T>
        {
            /// <summary>比較を行う Func&lt;T, T, int&gt; デリゲートを表します。</summary>
            private readonly Func<T, T, int> compare;

            /// <summary>
            /// Comparer クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="compare">比較を行う Func&lt;T, T, int&gt; デリゲート。</param>
            public Comparer(Func<T, T, int> compare)
            {
                this.compare = compare;
            }

            /// <summary>
            /// 2 つのオブジェクトを比較し、一方が他方より小さいか、等しいか、大きいかを示す値を返します。
            /// </summary>
            /// <param name="x">比較対象の第 1 オブジェクト。</param>
            /// <param name="y">比較対象の第 2 オブジェクト。</param>
            /// <returns>x が y より小さい場合は 0 より小さい値。x と y は等しい場合は 0。x が y より大きい場合は 0 より大きい値。</returns>
            public int Compare(T x, T y) => this.compare(x, y);
        }

        /// <summary>
        /// オブジェクトが等しいかどうかの比較をサポートするメソッドを定義します。
        /// </summary>
        /// <typeparam name="T">比較するオブジェクトの型。</typeparam>
        private class EqualityComparer<T> : IEqualityComparer<T>
        {
            /// <summary>Func&lt;T, T, bool&gt; デリゲートを表します。</summary>
            private readonly Func<T, T, bool> equals;

            /// <summary>Func&lt;T, int&gt; デリゲートを表します。</summary>
            private readonly Func<T, int> getHashCode;

            /// <summary>
            /// EqualityComparer クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="equals">Func&lt;T, T, bool&gt; デリゲート。</param>
            /// <param name="getHashCode">Func&lt;T, int&gt; デリゲート。</param>
            public EqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                this.equals = equals;
                this.getHashCode = getHashCode;
            }

            /// <summary>
            /// 指定したオブジェクトが等しいかどうかを判断します。
            /// </summary>
            /// <param name="x">比較対象の T 型の第 1 オブジェクト。</param>
            /// <param name="y">比較対象の T 型の第 2 オブジェクト。</param>
            /// <returns>指定したオブジェクトが等しい場合は true。それ以外の場合は false。</returns>
            public bool Equals(T x, T y) => this.equals(x, y);

            /// <summary>
            /// 指定したオブジェクトのハッシュ コードを返します。
            /// </summary>
            /// <param name="obj">ハッシュ コードが返される対象の Object。</param>
            /// <returns>指定したオブジェクトのハッシュ コード。</returns>
            public int GetHashCode(T obj) => this.getHashCode(obj);
        }
    }
}
