// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// EnumerableExtensions のオブジェクトを表します。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 指定されたコレクション内の各要素に対して指定された処理を実行します。
        /// </summary>
        /// <typeparam name="T">要素の型。</typeparam>
        /// <param name="instance">指定のコレクション。</param>
        /// <param name="action">コレクションの各要素に対して実行する Action&lt;T, int&gt; デリゲート。</param>
        public static void ForEach<T>(this IEnumerable<T> instance, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in instance)
            {
                action(item, index++);
            }
        }
    }
}
