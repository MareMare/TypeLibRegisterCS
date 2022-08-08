// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MachineInformation.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Net;
using System.Xml.Serialization;

#endregion

namespace TypeLibRegisterCS.Entities
{
    /// <summary>
    /// MachineInformation のオブジェクトを表します。
    /// </summary>
    [Serializable]
    public class MachineInformation
    {
        /// <summary>
        /// MachineInformation クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MachineInformation()
        {
            this.CreationDateTime = DateTime.Now;
            this.MachineName = Dns.GetHostName();
            this.CpuName = TypeLibCollector.CpuName;
            this.CpuIdentifier = TypeLibCollector.CpuIdentifier;
        }

        /// <summary>
        /// 生成日時を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="DateTime" /> 型。
        /// <para>生成日時。既定値は null です。</para>
        /// </value>
        [XmlAttribute("creation")]
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// マシン名を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>マシン名。既定値は string.Empty です。</para>
        /// </value>
        [XmlAttribute("machineName")]
        public string MachineName { get; set; }

        /// <summary>
        /// CPU 名を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>CPU 名。既定値は string.Empty です。</para>
        /// </value>
        [XmlAttribute("cpuName")]
        public string CpuName { get; set; }

        /// <summary>
        /// CPU Identifier を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string" /> 型。
        /// <para>CPU Identifier。既定値は string.Empty です。</para>
        /// </value>
        [XmlAttribute("cpuIdentifier")]
        public string CpuIdentifier { get; set; }
    }
}
