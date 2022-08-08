// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadExceptionDetectedEventArgs.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// ThreadExceptionDetectedEventArgs のオブジェクトを表します。
    /// </summary>
    [Serializable]
    public class ThreadExceptionDetectedEventArgs : EventArgs
    {
        /// <summary>
        /// ThreadExceptionDetectedEventArgs クラスの新しいインスタンスを初期化します。
        /// </summary>
        private ThreadExceptionDetectedEventArgs()
        {
            this.Error = null;
        }

        /// <summary>
        /// ThreadExceptionDetectedEventArgs クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="ex">検出された例外。</param>
        internal ThreadExceptionDetectedEventArgs(Exception ex)
            : this()
        {
            this.Error = ex;
        }

        /// <summary>
        /// 検出された例外を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Exception"/> 型。
        /// <para>検出された例外。既定値は null です。</para>
        /// </value>
        public Exception Error
        {
            get;
            private set;
        }
    }
}
