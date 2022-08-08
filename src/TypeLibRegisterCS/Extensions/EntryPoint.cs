// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntryPoint.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// アプリケーションのエントリポイントとしてアプリケーションの開始を行うメソッドを提供します。
    /// </summary>
    public static class EntryPoint
    {
        /// <summary>例外が検出されたときに発生します。</summary>
        public static event EventHandler<ThreadExceptionDetectedEventArgs> ThreadExceptionDetected;
        
        /// <summary>ThreadExceptionDialog のボタンが押されて消去されたときに発生します。</summary>
        public static event EventHandler<ThreadExceptionDialogResultEventArgs> ThreadExceptionDialogResult;

        /// <summary>
        /// エントリポイントを有するアセンブリのファイル名 を取得します。
        /// ファイル名には拡張子も含みます。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>エントリポイントを有するアセンブリのファイル名。</para>
        /// </value>
        public static string AssemblyFileName
        {
            get
            {
                Assembly executingAssembly = Assembly.GetEntryAssembly();
                return Path.GetFileName(new Uri(executingAssembly.Location).LocalPath);
            }
        }

        /// <summary>
        /// 多重起動を禁止としたアプリケーションを開始します。
        /// </summary>
        /// <param name="action">アプリケーションを開始するメソッドのデリゲート。</param>
        public static void RunSingle(Action action)
        {
            // Mutex の新しいインスタンスを生成します。
            //var mutexName = AppDomain.CurrentDomain.FriendlyName; // Mutex の名前にアプリケーションドメイン名を付けます。
            var mutexName = EntryPoint.AssemblyFileName;            // Mutex の名前にアセンブリファイル名を付けます。
            using (Mutex mutex = new Mutex(false, mutexName))
            {
                // Mutex のシグナルを受信できるかどうか判断します。
                if (mutex.WaitOne(0, false))
                {
                    EntryPoint.Run(action);
                }
            }
        }

        /// <summary>
        /// アプリケーションを開始します。
        /// </summary>
        /// <param name="action">アプリケーションを開始するメソッドのデリゲート。</param>
        public static void Run(Action action)
        {
            // アプリケーションで発生したハンドルされていない例外をキャッチするようにします。
            var domain = Thread.GetDomain();
            domain.UnhandledException += new UnhandledExceptionEventHandler(EntryPoint.AppDomainUnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(EntryPoint.ApplicationThreadException);
            try
            {
                // アプリケーションを実行します。
                action();
            }
            catch (Exception ex)
            {
                EntryPoint.OnDetected(domain, ex);
                EntryPoint.ShowThreadExceptionDialog(ex);
            }
            finally
            {
                Application.ThreadException -= new ThreadExceptionEventHandler(EntryPoint.ApplicationThreadException);
                domain.UnhandledException -= new UnhandledExceptionEventHandler(EntryPoint.AppDomainUnhandledException);
            }
        }

        /// <summary>
        /// 例外がキャッチされない場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト。</param>
        /// <param name="e">イベントデータを格納している UnhandledExceptionEventArgs。</param>
        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (e.ExceptionObject as Exception) ?? new InvalidProgramException("ハンドルされていない例外を検出しました。");
            EntryPoint.OnDetected(sender, ex);
            EntryPoint.ShowThreadExceptionDialog(ex);
        }

        /// <summary>
        /// トラップされないスレッドの例外がスローされると発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト。</param>
        /// <param name="e">イベントデータを格納している ThreadExceptionEventArgs。</param>
        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = (e.Exception as Exception) ?? new InvalidProgramException("トラップされていない例外を検出しました。");
            EntryPoint.OnDetected(sender, ex);
            EntryPoint.ShowThreadExceptionDialog(ex);
        }

        /// <summary>
        /// ThreadExceptionDetected イベントを発生させます。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト。</param>
        /// <param name="ex">発生した例外を表す Exception。</param>
        private static void OnDetected(object sender, Exception ex)
        {
            var eh = EntryPoint.ThreadExceptionDetected;
            if (eh != null)
            {
                eh(sender, new ThreadExceptionDetectedEventArgs(ex));
            }
        }

        /// <summary>
        /// 例外がスレッドで発生したときに表示されるダイアログボックスを表示します。
        /// </summary>
        /// <param name="ex">発生した例外を表す Exception。</param>
        private static void ShowThreadExceptionDialog(Exception ex)
        {
            using (var dialog = new ThreadExceptionDialog(ex))
            {
                var dialogResult = dialog.ShowDialog();
                var eh = EntryPoint.ThreadExceptionDialogResult;
                if (eh != null)
                {
                    var args = new ThreadExceptionDialogResultEventArgs(dialogResult);
                    eh(dialog, args);
                    if (args.Cancel)
                    {
                        // 継続をキャンセルしてアプリケーションを終了させます。
                        Application.Exit();
                    }
                }
            }
        }
    }
}
