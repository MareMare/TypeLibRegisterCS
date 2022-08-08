// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TypeLibRegisterCS.Entities;
using TypeLibRegisterCS.Extensions;

#endregion

namespace TypeLibRegisterCS
{
    /// <summary>
    /// アプリケーションのエントリポイントオブジェクトを表します。
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ExpandableObjectConverterBridge<IList<ClassIdentifier>>.Register();
            ExpandableObjectConverterBridge<IList<InterfaceIdentifier>>.Register();

            EntryPoint.RunSingle(() =>
            {
                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            });
        }
    }
}
