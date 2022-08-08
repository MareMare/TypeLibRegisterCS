// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerFactory.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;

#endregion

namespace TypeLibRegisterCS.Serialization
{
    /// <summary>
    /// ドキュメントにシリアル化または逆シリアル化するオブジェクトを作成する オブジェクトを表します。
    /// </summary>
    public static class SerializerFactory
    {
        /// <summary>
        /// 指定されたシリアル化を行う方法に応じたシリアライザを生成します。
        /// </summary>
        /// <param name="serializerMode">シリアル化を行う方法を表す SerializerMode。</param>
        /// <returns>生成されたシルアル化を行うインスタンス。</returns>
        public static ISerializerHelper CreateSerializer(SerializerMode serializerMode)
        {
            switch (serializerMode)
            {
                case SerializerMode.XmlSerializer:
                    return new XmlSerializerHelper();
                //case SerializerMode.DataContractSerializer:
                //    return new DataContractSerializerHelper();
            }

            throw new NotSupportedException();
        }
    }
}
