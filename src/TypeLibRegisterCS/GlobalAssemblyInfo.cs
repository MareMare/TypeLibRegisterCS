// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalAssemblyInfo.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Reflection;
using System.Resources;

#endregion

// ソリューション共通のアセンブリに関する一般情報をここで設定します。
// アセンブリに関連付けられている情報を変更するには、これらの属性値を変更してください。
// したがって、各プロジェクトの AssemblyInfo.cs ファイルを編集して次の属性値のみを設定するようにします。
// 
//      個々のプロジェクトの以下の属性値を変更してください。
//      [assembly: AssemblyTitle("Feature.SubFeature")]
//      [assembly: AssemblyDescription("")]
//      [assembly: AssemblyConfiguration("")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyCompany("MareMare")]
[assembly: AssemblyProduct("TypeLibRegisterCS")]
[assembly: AssemblyCopyright("COPYRIGHT (C) 2010 MareMare ALL RIGHTS RESERVED")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: NeutralResourcesLanguageAttribute("ja-JP")]
