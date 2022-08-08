// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeLibCollector.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using TypeLibRegisterCS.Configurations;
using TypeLibRegisterCS.Entities;
using TypeLibRegisterCS.Extensions;
using TypeLibRegisterCS.Serialization;

#endregion

namespace TypeLibRegisterCS
{
    /// <summary>
    /// 列挙に成功したときの処理を行うデリゲートを表します。
    /// </summary>
    /// <typeparam name="TPayload">コレクションの要素の型。</typeparam>
    /// <param name="payloads">コレクション。</param>
    internal delegate void EnumerableAction<TPayload>(IEnumerable<TPayload> payloads);

    /// <summary>
    /// 列挙に成功したときの処理を行うデリゲートを表します。
    /// </summary>
    /// <typeparam name="TPayload">コレクションの要素の型。</typeparam>
    /// <returns>コレクション。</returns>
    internal delegate IEnumerable<TPayload> EnumerableFunc<TPayload>();

    /// <summary>
    /// 失敗したときの処理を行うデリゲートを表します。
    /// </summary>
    /// <param name="ex">例外の原因である内部例外への参照。</param>
    internal delegate void FailureAction(Exception ex);

    /// <summary>
    /// ログカテゴリの列挙体を表します。
    /// </summary>
    internal enum LoggingCategory
    {
        /// <summary>不明なカテゴリを表します。</summary>
        None,

        /// <summary>Debug カテゴリを表します。</summary>
        Debug,

        /// <summary>Info カテゴリを表します。</summary>
        Info,

        /// <summary>Warning カテゴリを表します。</summary>
        Warning,

        /// <summary>Error カテゴリを表します。</summary>
        Error,
    }

    /// <summary>
    /// TypeLibCollector のオブジェクトを表します。
    /// </summary>
    internal class TypeLibCollector : IDisposable
    {
        /// <summary>既に Dispose メソッドが呼び出されているかどうかを表します。</summary>
        private bool disposed;

        /// <summary>現在のインスタンスのロックオブジェクトを表します。</summary>
        private readonly object thisLock = new object();

        /// <summary>SynchronizationContext.</summary>
        private readonly SynchronizationContext context;

        /// <summary>HKEY_CLASSES_ROOT\TypeLib へのレジストリキー。</summary>
        private readonly RegistryKey regkeyToFindOfTLBID;

        /// <summary>HKEY_CLASSES_ROOT\CLSID へのレジストリキー。</summary>
        private readonly RegistryKey regkeyToFindOfCLSID;

        /// <summary>HKEY_CLASSES_ROOT\Interface へのレジストリキー。</summary>
        private readonly RegistryKey regkeyToFindOfIID;

        /// <summary>
        /// TypeLibCollector クラスの新しいインスタンスを初期化します。
        /// </summary>
        public TypeLibCollector()
        {
            this.context = WindowsFormsSynchronizationContext.Current;
            this.regkeyToFindOfTLBID = Registry.ClassesRoot.OpenSubKey("TypeLib");
            this.regkeyToFindOfCLSID = Registry.ClassesRoot.OpenSubKey("CLSID");
            this.regkeyToFindOfIID = Registry.ClassesRoot.OpenSubKey("Interface");
        }

        /// <summary>
        /// TypeLibCollector クラスのインスタンスが GC に回収される時に呼び出されます。
        /// </summary>
        ~TypeLibCollector()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// CPU 名を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>CPU 名。既定値は string.Empty です。</para>
        /// </value>
        public static string CpuName
        {
            get
            {
                string value = string.Empty;
                value = Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "ProcessorNameString", null) as string;
                return value ?? string.Empty;
            }
        }

        /// <summary>
        /// CPU Identifier を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>CPU Identifier。既定値は string.Empty です。</para>
        /// </value>
        public static string CpuIdentifier
        {
            get
            {
                string value = string.Empty;
                value = Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "Identifier", null) as string;
                return value ?? string.Empty;
            }
        }

        /// <summary>
        /// ログを出力するメソッドのデリゲートを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Action{T1, T2}"/> 型。
        /// <para>ログを出力するメソッドのデリゲート。既定値は null です。</para>
        /// </value>
        public Action<LoggingCategory, string> ActionToWriteLog
        {
            get;
            set;
        }

        /// <summary>
        /// Searches a string using a Microsoft MS-DOS wild card match type.
        /// </summary>
        /// <param name="filepath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path to be searched.</param>
        /// <param name="searchPattern">A pointer to a null-terminated string of maximum length MAX_PATH that contains the file type for which to search.</param>
        /// <returns>Returns TRUE if the string matches, or FALSE otherwise.</returns>
        public static bool PathMatchSpec(string filepath, string searchPattern)
        {
            var spec = !string.IsNullOrEmpty(searchPattern) ? searchPattern : "*.*";
            return NativeMethods.PathMatchSpec(filepath, spec);
        }

        /// <summary>
        /// サブキーを検索します。
        /// </summary>
        /// <param name="keyToFind">検索を開始する RegistryKey。</param>
        /// <returns>RegistryKeyNode のコレクション。</returns>
        private static IEnumerable<RegistryKeyNode> FindSubKeyNodes(RegistryKey keyToFind)
        {
            return TypeLibCollector.RecursiveFindSubKeyNodes(null, keyToFind);
        }

        /// <summary>
        /// 再帰的にサブキーを検索します。
        /// </summary>
        /// <param name="rootKey">検索を開始したルート情報の RegistryKey。親ルートの場合は null を指定します。</param>
        /// <param name="parentKey">現在の検索対象となるカレント情報の RegistryKey。</param>
        /// <returns>RegistryKeyNode のコレクション。</returns>
        private static IEnumerable<RegistryKeyNode> RecursiveFindSubKeyNodes(RegistryKey rootKey, RegistryKey parentKey)
        {
            var foundNodes = new List<RegistryKeyNode>();
            if (parentKey.SubKeyCount > 0)
            {
                foreach (string childKeyName in parentKey.GetSubKeyNames())
                {
                    RegistryKey childKey = parentKey.OpenSubKey(childKeyName);
                    if (childKey != null)
                    {
                        var currentRootKey = (rootKey != null) ? rootKey : childKey;
                        foundNodes.Add(new RegistryKeyNode(currentRootKey, childKey));

                        // Recursive call back into this method
                        var keyGrandChildren = TypeLibCollector.RecursiveFindSubKeyNodes(currentRootKey, childKey);
                        foundNodes.AddRange(keyGrandChildren);
                    }
                }
            }

            return foundNodes;
        }

        /// <summary>
        /// TypeLibIdentifier コレクションを非同期で検索します。
        /// </summary>
        /// <param name="success">成功したときの処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">例外検出したときの処理を行うメソッドのデリゲート。</param>
        public void PopulateAsync(EnumerableAction<TypeLibIdentifier> success, FailureAction failure)
        {
            this.SafeQueueUserWorkItem(
                () =>
                {
                    Stopwatch sw1 = new Stopwatch();
                    sw1.Start();
                    var foundItems = this.PopulateTypeLibIdentifiers();
                    sw1.Stop();
                    this.WriteLog(LoggingCategory.Debug, "Scan by TLBID completed. {0} item(s) {1} [msec]", foundItems.Count(), sw1.ElapsedMilliseconds);
                    return foundItems;
                },
                success,
                failure);
        }

        /// <summary>
        /// ExportedTypeLibInformation インスタンスを非同期で XML 形式として保存します。
        /// </summary>
        /// <param name="filepathToSave">保存されるファイルパス。</param>
        /// <param name="filterItem">保存されるフィルタ項目。</param>
        /// <param name="typeLibIdentifiers">保存される TypeLibIdentifier コレクション。</param>
        /// <param name="success">成功したときの処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">例外検出したときの処理を行うメソッドのデリゲート。</param>
        public void SaveAsXmlAsync(string filepathToSave, FilterItem filterItem, IEnumerable<TypeLibIdentifier> typeLibIdentifiers, Action success, FailureAction failure)
        {
            var items = typeLibIdentifiers.ToList();
            this.SafeQueueUserWorkItem(
                () =>
                {
                    var information = new ExportedTypeLibInformation()
                    {
                        Filter = filterItem,
                    };
                    information.Add(items);

                    // 各要素がまだ構築されていない TypeLibIdentifier インスタンスを構築します。
                    this.ConstructIfNotYet(items);

                    var serializer = SerializerFactory.CreateSerializer(SerializerMode.XmlSerializer);
                    serializer.Save(filepathToSave, information);
                },
                success,
                failure);
        }

        /// <summary>
        /// TypeLibIdentifier コレクションで構成される各レジストリを削除します。
        /// </summary>
        /// <param name="typeLibIdentifiers">削除される TypeLibIdentifier コレクション。</param>
        /// <param name="partialCompleted">部分的に完了したときの処理を行うメソッドのデリゲート。</param>
        /// <param name="partialReportProgress">部分的に完了したときの進捗報告を行うメソッドのデリゲート。</param>
        /// <param name="success">成功したときの処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">例外検出したときの処理を行うメソッドのデリゲート。</param>
        public void DeleteRegistryAsync(IEnumerable<TypeLibIdentifier> typeLibIdentifiers, Action<TypeLibIdentifier> partialCompleted, Action<double> partialReportProgress, Action success, FailureAction failure)
        {
            Action<TypeLibIdentifier> partialCompletedInternal = (identifier) =>
            {
                this.context.Send((stateOfContext) => partialCompleted(identifier), null);
            };
            Action<double> partialReportProgressInternal = (percentComplete) =>
            {
                this.context.Send((stateOfContext) => partialReportProgress(percentComplete), null);
            };

            var items = typeLibIdentifiers.ToList();
            this.SafeQueueUserWorkItem(
                () =>
                {
                    // 各要素がまだ構築されていない TypeLibIdentifier インスタンスを構築します。
                    this.ConstructIfNotYet(items);

                    Stopwatch sw1 = new Stopwatch();
                    sw1.Start();
                    this.DeleteRegistry(items, partialCompletedInternal, partialReportProgressInternal);
                    sw1.Stop();
                    this.WriteLog(LoggingCategory.Debug, "Deletion from registry completed. {0} item(s) {1} [msec]", items.Count, sw1.ElapsedMilliseconds);
                    Thread.Sleep(2 * 1000);
                },
                success,
                failure);
        }

        /// <summary>
        /// ログを出力します。
        /// </summary>
        /// <param name="category">ログのカテゴリ。</param>
        /// <param name="format">メッセージの複合書式指定文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ Object 配列。</param>
        private void WriteLog(LoggingCategory category, string format, params object[] args)
        {
            Debug.Print(format, args);
            if (this.ActionToWriteLog != null)
            {
                this.context.Post(
                    (stateOfContext) =>
                    {
                        var builder = new StringBuilder();
                        builder.AppendFormat(CultureInfo.InvariantCulture, format, args);
                        this.ActionToWriteLog(category, builder.ToString());
                    },
                    null);
            }
        }

        /// <summary>
        /// HKEY_CLASSES_ROOT\TypeLib のすべてを検索し TypeLibIdentifier コレクションを生成します。
        /// </summary>
        /// <returns>検索結果の TypeLibIdentifier コレクション。</returns>
        private IEnumerable<TypeLibIdentifier> PopulateTypeLibIdentifiers()
        {
            // filepath:
            //      registry key = HKEY_CLASSES_ROOT\TypeLib\{03837500-098B-11D8-9414-505054503030}\1.0\0\win32
            //      value name   = string.Empty
            // version:
            //      registry key = HKEY_CLASSES_ROOT\TypeLib\{03837500-098B-11D8-9414-505054503030}\1.0
            // name:
            //      registry key = HKEY_CLASSES_ROOT\TypeLib\{03837500-098B-11D8-9414-505054503030}\1.0
            //      value name   = string.Empty
            var nodesOfTLBID = TypeLibCollector.FindSubKeyNodes(this.regkeyToFindOfTLBID);
            var queryOfTLBID = from node in nodesOfTLBID
                               let value = node.GetValueText()
                               where node.EndsWith(@"\win32")
                               select new { Node = node, FilePath = value, };

            var query = queryOfTLBID
                .Select((item) =>
                {
                    var elements = item.Node.KeyElementsOfCurrent.ToList();
                    var elementsOfVersion = elements.Take(elements.Count - 2);
                    var keyOfVersion = elementsOfVersion.Aggregate((working, next) => working + '\\' + next);
                    var name = Registry.GetValue(keyOfVersion, string.Empty, string.Empty) as string;
                    return new TypeLibIdentifier()
                    {
                        RegistryPath = item.Node.RootPath,
                        Tlbid = item.Node.LastSubkeyNameOfRoot,
                        FilePath = item.FilePath,
                        Name = name,
                        Version = elementsOfVersion.LastOrDefault() ?? string.Empty,
                    };
                })
                .OrderBy((identifier) => identifier.DisplayName)
                .ThenBy((identifier) => identifier.Version);

            return query.ToList();
        }

        /// <summary>
        /// 各要素がまだ構築されていない TypeLibIdentifier インスタンスを構築します。
        /// </summary>
        /// <param name="typeLibIdentifiers">構築される TypeLibIdentifier コレクション。</param>
        private void ConstructIfNotYet(IEnumerable<TypeLibIdentifier> typeLibIdentifiers)
        {
            var itemsOfNotYetCLSID = typeLibIdentifiers
                .Where((item) => !item.IsConstructedClassIdentifiers)
                .ToList();
            var itemsOfNotYetIID = typeLibIdentifiers
                .Where((item) => !item.IsConstructedInterfaceIdentifiers)
                .ToList();

            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            this.ConcreteConstructFromCLSID(itemsOfNotYetCLSID);
            sw1.Stop();
            this.WriteLog(LoggingCategory.Debug, "Scan by CLSID completed. {0} item(s) {1} [msec]", itemsOfNotYetCLSID.Count, sw1.ElapsedMilliseconds);

            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            this.ConcreteConstructFromIID(itemsOfNotYetIID);
            sw2.Stop();
            this.WriteLog(LoggingCategory.Debug, "Scan by IID completed. {0} item(s) {1} [msec]", itemsOfNotYetIID.Count, sw2.ElapsedMilliseconds);
        }

        /// <summary>
        /// HKEY_CLASSES_ROOT\CLSID より指定された TypeLibIdentifier に関連する内容を構築します。
        /// </summary>
        /// <param name="typeLibIdentifiers">構築される TypeLibIdentifier コレクション。</param>
        private void ConcreteConstructFromCLSID(IEnumerable<TypeLibIdentifier> typeLibIdentifiers)
        {
            // TLBID:
            //      registry key    = HKEY_CLASSES_ROOT\CLSID\{03837511-098B-11D8-9414-505054503030}\TypeLib
            //      value name      = string.Empty
            // CLSID:
            //      registry key    = HKEY_CLASSES_ROOT\CLSID\{03837511-098B-11D8-9414-505054503030}
            // ProgID:
            //      registry key    = HKEY_CLASSES_ROOT\CLSID\{03837511-098B-11D8-9414-505054503030}\ProgID
            //      value name      = string.Empty
            // VersionIndependentProgID:
            //      registry key    = HKEY_CLASSES_ROOT\CLSID\{03837511-098B-11D8-9414-505054503030}\VersionIndependentProgID
            //      value name      = string.Empty
            var nodesOfCLSID = TypeLibCollector.FindSubKeyNodes(this.regkeyToFindOfCLSID);
            var queryOfCLSID = from node in nodesOfCLSID
                               let value = node.GetValueText()
                               where node.EndsWith(@"\typelib")
                               select new { Node = node, TLBID = value, };

            var lookupOfCLSID = queryOfCLSID.ToLookup((item) => item.TLBID.ToUpperInvariant());
            foreach (var identifierToConstruct in typeLibIdentifiers)
            {
                var tlbid = identifierToConstruct.Tlbid.ToUpperInvariant();
                if (lookupOfCLSID.Contains(tlbid))
                {
                    var query = lookupOfCLSID[tlbid]
                        .Select((item) =>
                        {
                            var keyOfCLSID = item.Node.RootPath;
                            var progID = Registry.GetValue(keyOfCLSID + @"\ProgID", string.Empty, string.Empty) as string;
                            var versionIndependentProgID = Registry.GetValue(keyOfCLSID + @"\VersionIndependent", string.Empty, string.Empty) as string;
                            return new ClassIdentifier()
                            {
                                RegistryPath = item.Node.RootPath,
                                Clsid = item.Node.LastSubkeyNameOfRoot,
                                ProgId = progID,
                                VersionIndependentProgId = versionIndependentProgID,
                                Tlbid = item.TLBID,
                            };
                        })
                        .ToList();
                    query.ForEach((identifier, index) =>
                        identifierToConstruct.ClassIdentifiers.Add(identifier));
                }

                // 構築されたことを設定します。
                identifierToConstruct.IsConstructedClassIdentifiers = true;
            }
        }

        /// <summary>
        /// HKEY_CLASSES_ROOT\Interface より指定された TypeLibIdentifier に関連する内容を構築します。
        /// </summary>
        /// <param name="typeLibIdentifiers">構築される TypeLibIdentifier コレクション。</param>
        private void ConcreteConstructFromIID(IEnumerable<TypeLibIdentifier> typeLibIdentifiers)
        {
            // TLBID:
            //      registry key    = HKEY_CLASSES_ROOT\Interface\{038374FF-098B-11D8-9414-505054503030}\TypeLib
            //      value name      = string.Empty
            // IID:
            //      registry key    = HKEY_CLASSES_ROOT\Interface\{038374FF-098B-11D8-9414-505054503030}
            var nodesOfIID = TypeLibCollector.FindSubKeyNodes(this.regkeyToFindOfIID);
            var queryOfIID = from node in nodesOfIID
                               let value = node.GetValueText()
                               where node.EndsWith(@"\typelib")
                               select new { Node = node, TLBID = value, };

            var lookupOfIID = queryOfIID.ToLookup((item) => item.TLBID.ToUpperInvariant());
            foreach (var identifierToConstruct in typeLibIdentifiers)
            {
                var tlbid = identifierToConstruct.Tlbid.ToUpperInvariant();
                if (lookupOfIID.Contains(tlbid))
                {
                    var query = lookupOfIID[tlbid]
                        .Select((item) =>
                        {
                            return new InterfaceIdentifier()
                            {
                                RegistryPath = item.Node.RootPath,
                                Iid = item.Node.LastSubkeyNameOfRoot,
                                Tlbid = item.TLBID,
                            };
                        })
                        .ToList();
                    query.ForEach((identifier, index) =>
                        identifierToConstruct.InterfaceIdentifiers.Add(identifier));
                }

                // 構築されたことを設定します。
                identifierToConstruct.IsConstructedInterfaceIdentifiers = true;
            }
        }

        /// <summary>
        /// TypeLibIdentifier コレクションで構成される各レジストリを削除します。
        /// </summary>
        /// <param name="typeLibIdentifiers">削除される TypeLibIdentifier コレクション。</param>
        /// <param name="partialCompleted">部分的に完了したときの処理を行うメソッドのデリゲート。</param>
        /// <param name="partialReportProgress">部分的に完了したときの進捗報告を行うメソッドのデリゲート。</param>
        private void DeleteRegistry(IEnumerable<TypeLibIdentifier> typeLibIdentifiers, Action<TypeLibIdentifier> partialCompleted, Action<double> partialReportProgress)
        {
            Action<int, int> partialReportProgressInternal = (partial, total) =>
            {
                double percent = 100 * partial / total;
                partialReportProgress(percent);
            };

            var partialCount = 0;
            var totalCount = typeLibIdentifiers.Count();
            foreach (var typeLibIdentifier in typeLibIdentifiers)
            {
                this.DeleteRegistry(typeLibIdentifier);
                partialCount++;
                partialReportProgressInternal(partialCount, totalCount);
                partialCompleted(typeLibIdentifier);
            }
        }

        /// <summary>
        /// TypeLibIdentifier インスタンスで構成される各レジストリを削除します。
        /// </summary>
        /// <param name="typeLibIdentifier">削除される TypeLibIdentifier インスタンス。</param>
        private void DeleteRegistry(TypeLibIdentifier typeLibIdentifier)
        {
            // VersionIndependentProgID: HKEY_CLASSES_ROOT\PLA.TraceDataProviderCollection
            var queryOfVersionIndependentProgID = typeLibIdentifier
                .ClassIdentifiers
                .Where((identifier) => !string.IsNullOrEmpty(identifier.VersionIndependentProgId))
                .Select((identifier) => string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", Registry.ClassesRoot.Name, identifier.VersionIndependentProgId));

            // ProgID: HKEY_CLASSES_ROOT\PLA.TraceDataProviderCollection.1
            var queryOfProgID = typeLibIdentifier
                .ClassIdentifiers
                .Where((identifier) => !string.IsNullOrEmpty(identifier.ProgId))
                .Select((identifier) => string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", Registry.ClassesRoot.Name, identifier.ProgId));

            // CLSID: HKEY_CLASSES_ROOT\CLSID\{03837511-098B-11D8-9414-505054503030}
            var queryOfCLSID = typeLibIdentifier
                .ClassIdentifiers
                .Where((identifier) => !string.IsNullOrEmpty(identifier.RegistryPath))
                .Select((identifier) => identifier.RegistryPath);

            // IID: HKEY_CLASSES_ROOT\Interface\{038374FF-098B-11D8-9414-505054503030}
            var queryOfIID = typeLibIdentifier
                .InterfaceIdentifiers
                .Where((identifier) => !string.IsNullOrEmpty(identifier.RegistryPath))
                .Select((identifier) => identifier.RegistryPath);

            // TLBID: HKEY_CLASSES_ROOT\TypeLib\{03837500-098B-11D8-9414-505054503030}
            var queryOfTLBID = new List<string>() { typeLibIdentifier.RegistryPath };

            var registryPathsToDelete = new List<string>();
            registryPathsToDelete.AddRange(queryOfVersionIndependentProgID);
            registryPathsToDelete.AddRange(queryOfProgID);
            registryPathsToDelete.AddRange(queryOfCLSID);
            registryPathsToDelete.AddRange(queryOfIID);
            registryPathsToDelete.AddRange(queryOfTLBID);
            foreach (var registryPath in registryPathsToDelete)
            {
                this.DeleteRegistrySubKeyTree(typeLibIdentifier, registryPath);
            }
        }

        /// <summary>
        /// サブキーとその子サブキーを再帰的に削除します。文字列 subkey では、大文字と小文字は区別されません。
        /// </summary>
        /// <param name="typeLibIdentifier">削除される TypeLibIdentifier インスタンス。</param>
        /// <param name="registryPath">削除するレジストリのパス。</param>
        private void DeleteRegistrySubKeyTree(TypeLibIdentifier typeLibIdentifier, string registryPath)
        {
            string[] elements = registryPath.Split('\\');
            string hiveText = elements.First().ToUpperInvariant();
            string subKey = string.Join(@"\", elements, 1, elements.Length - 1);
            RegistryKey rootRegistry = null;
            switch (hiveText)
            {
                case "HKEY_CLASSES_ROOT":
                case "HKCR":
                    rootRegistry = Registry.ClassesRoot;
                    break;
                case "HKEY_CURRENT_USER":
                case "HKCU":
                    rootRegistry = Registry.CurrentUser;
                    break;
                case "HKEY_LOCAL_MACHINE":
                case "HKLM":
                    rootRegistry = Registry.LocalMachine;
                    break;
                case "HKEY_USERS":
                    rootRegistry = Registry.Users;
                    break;
                case "HKEY_CURRENT_CONFIG":
                    rootRegistry = Registry.CurrentConfig;
                    break;
                default:
                    throw new NotSupportedException("The registry hive '" + hiveText + "' is not supported.");
            }

            try
            {
                // ほんまに削除します。
                rootRegistry.DeleteSubKeyTree(subKey);
                //this.WriteLog(LoggingCategory.Debug, @"'{0}\{1}' of '{2}' successfully deleted from the registry.", rootRegistry.Name, subKey, typeLibIdentifier.DisplayName);    // 遅すぎ
                Debug.Print(@"'{0}\{1}' of '{2}' successfully deleted from the registry.", rootRegistry.Name, subKey, typeLibIdentifier.DisplayName);
            }
            catch (Exception ex)
            {
                this.WriteLog(LoggingCategory.Error, @"'{0}\{1}' of '{2}' failed to delete from the registry. {3}", rootRegistry.Name, subKey, typeLibIdentifier.DisplayName, ex.Message);
            }
        }

        /// <summary>
        /// 指定されたメソッドのデリゲートを非同期呼び出しします。
        /// これは ThreadPool.QueueUserWorkItem() を使用して success または failure の各デリゲートはメインスレッド上で実行されます。
        /// </summary>
        /// <typeparam name="TPayload">列挙されるコレクションの要素の型。</typeparam>
        /// <param name="action">非同期操作としてコレクションを取得するメソッドのデリゲート。</param>
        /// <param name="success">成功したときの処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">例外検出したときの処理を行うメソッドのデリゲート。</param>
        private void SafeQueueUserWorkItem<TPayload>(EnumerableFunc<TPayload> action, EnumerableAction<TPayload> success, FailureAction failure)
        {
            ThreadPool.QueueUserWorkItem(
                (stateOfPool) =>
                {
                    try
                    {
                        var payloads = action();
                        this.context.Post((stateOfContext) => success(payloads), null);
                    }
                    catch (Exception ex)
                    {
                        this.context.Post((stateOfContext) => failure(ex), null);
                    }
                },
                null);
        }

        /// <summary>
        /// 指定されたメソッドのデリゲートを非同期呼び出しします。
        /// これは ThreadPool.QueueUserWorkItem() を使用して success または failure の各デリゲートはメインスレッド上で実行されます。
        /// </summary>
        /// <param name="action">非同期操作としてコレクションを取得するメソッドのデリゲート。</param>
        /// <param name="success">成功したときの処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">例外検出したときの処理を行うメソッドのデリゲート。</param>
        private void SafeQueueUserWorkItem(Action action, Action success, FailureAction failure)
        {
            ThreadPool.QueueUserWorkItem(
                (stateOfPool) =>
                {
                    try
                    {
                        action();
                        this.context.Post((stateOfContext) => success(), null);
                    }
                    catch (Exception ex)
                    {
                        this.context.Post((stateOfContext) => failure(ex), null);
                    }
                },
                null);
        }

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// TypeLibCollector クラスのインスタンスによって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this.thisLock)
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                if (disposing)
                {
                    // マネージ リソース (IDisposable の実装インスタンス) の解放処理をこの位置に記述します。
                    // 例）
                    // ((IDisposable)this.disposableSomeField1).Close();
                    // ((IDisposable)this.disposableSomeField2).Dispose();
                    this.regkeyToFindOfTLBID.Close();
                    this.regkeyToFindOfCLSID.Close();
                    this.regkeyToFindOfIID.Close();
                }

                // アンマネージ リソース (IDisposable の非実装インスタンス) の解放処理をこの位置に記述します。
                // 例）
                // NativeMethod.CloseHandle(this.otherSomeField1);
                // this.otherSomeField1 = IntPtr.Zero;
            }
        }

        /// <summary>
        /// API 宣言のオブジェクトを表します。
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            /// Searches a string using a Microsoft MS-DOS wild card match type.
            /// </summary>
            /// <param name="pszFileParam">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path to be searched.</param>
            /// <param name="pszSpec">A pointer to a null-terminated string of maximum length MAX_PATH that contains the file type for which to search.</param>
            /// <returns>Returns TRUE if the string matches, or FALSE otherwise.</returns>
            [DllImport("shlwapi.dll", CharSet = CharSet.Ansi, ExactSpelling = false, BestFitMapping = false, ThrowOnUnmappableChar = false, SetLastError = false)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PathMatchSpec([In] string pszFileParam, [In] string pszSpec);
        }

        /// <summary>
        /// RegistryKeyNode のオブジェクトを表します。
        /// </summary>
        private class RegistryKeyNode
        {
            /// <summary>
            /// RegistryKeyNode クラスの新しいインスタンスを初期化します。
            /// </summary>
            private RegistryKeyNode()
            {
            }

            /// <summary>
            /// RegistryKeyNode クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="rootKey">ルート情報を示す RegistryKey。</param>
            /// <param name="currentKey">カレント情報を示す RegistryKey。</param>
            public RegistryKeyNode(RegistryKey rootKey, RegistryKey currentKey)
                : this()
            {
                this.RootPath = rootKey.Name;
                this.CurrentPath = currentKey.Name;
            }

            /// <summary>
            /// ルート情報へのパスを取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>ルート情報へのパス。既定値は null です。</para>
            /// </value>
            public string RootPath { get; private set; }

            /// <summary>
            /// カレント情報へのパスを取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>カレント情報へのパス。既定値は null です。</para>
            /// </value>
            public string CurrentPath { get; private set; }

            /// <summary>
            /// ルート情報の最後のサブキーを取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>ルート情報の最後のサブキー。既定値は string.Empty です。</para>
            /// </value>
            public string LastSubkeyNameOfRoot
            {
                get
                {
                    var elements = this.RootPath.Split('\\');
                    return elements.LastOrDefault() ?? string.Empty;
                }
            }

            /// <summary>
            /// カレント情報へのパスを構成するキーのコレクション取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>カレント情報へのパスを構成するキーのコレクション。既定値は要素数 0 です。</para>
            /// </value>
            public IEnumerable<string> KeyElementsOfCurrent
            {
                get
                {
                    var elements = this.CurrentPath.Split('\\');
                    return elements.ToList();
                }
            }

            /// <summary>
            /// カレント情報のキー名の末尾が指定した文字列と一致するかどうかを判断します。
            /// </summary>
            /// <param name="sufix">比較対象の文字列。</param>
            /// <returns>末尾が value と一致する場合は true。それ以外の場合は false。</returns>
            public bool EndsWith(string sufix)
            {
                return this.CurrentPath.ToUpperInvariant().EndsWith(sufix, StringComparison.OrdinalIgnoreCase);
            }

            /// <summary>
            /// カレント情報のレジストリ値の既定値に関連付けられている値を取得します。
            /// </summary>
            /// <returns>カレント情報のレジストリ値の既定値に関連付けられている値。見つからない場合は string.Empty。</returns>
            public string GetValueText()
            {
                return (Registry.GetValue(this.CurrentPath, string.Empty, null) as string) ?? string.Empty;
            }
        }
    }
}
