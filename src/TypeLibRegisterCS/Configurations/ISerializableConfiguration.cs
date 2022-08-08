// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializableConfiguration.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;

#endregion

namespace TypeLibRegisterCS.Configurations
{
    /// <summary>
    /// 構成定義データコントラクトのインターフェイスを表します。
    /// </summary>
    public interface ISerializableConfiguration
    {
        /// <summary>
        /// 空の定義として構築します。
        /// </summary>
        void ConstructAsEmpty();

        /// <summary>
        /// 現在のインスタンスを検証します。
        /// </summary>
        void Validate();
    }
}
