// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadExceptionDialogResultEventArgs.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// ThreadExceptionDialogResultEventArgs のオブジェクトを表します。
    /// </summary>
    [Serializable]
    public class ThreadExceptionDialogResultEventArgs : CancelEventArgs
    {
        /// <summary>
        /// ThreadExceptionDialogResultEventArgs クラスの新しいインスタンスを初期化します。
        /// </summary>
        private ThreadExceptionDialogResultEventArgs()
        {
            this.Result = DialogResult.None;
        }

        /// <summary>
        /// ThreadExceptionDialogResultEventArgs クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="result">DialogResult。</param>
        internal ThreadExceptionDialogResultEventArgs(DialogResult result)
            : this()
        {
            this.Result = result;
        }

        /// <summary>
        /// DialogResult を取得します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="DialogResult" /> 型。
        /// <para>DialogResult 。既定値は DialogResult.None です。</para>
        /// </value>
        public DialogResult Result { get; private set; }
    }
}
