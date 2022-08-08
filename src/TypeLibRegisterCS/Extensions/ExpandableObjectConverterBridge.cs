// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpandableObjectConverterBridge.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// ExpandableObjectConverterBridge のオブジェクトを表します。
    /// </summary>
    /// <typeparam name="T">ExpandableObjectConverter の対象となる型。</typeparam>
    internal sealed class ExpandableObjectConverterBridge<T> : ExpandableObjectConverter
    {
        /// <summary>
        /// ExpandableObjectConverterBridge クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ExpandableObjectConverterBridge()
        {
        }

        /// <summary>
        /// ExpandableObjectConverter の対象となる型を登録します。
        /// </summary>
        public static void Register()
        {
            ExpandableObjectConverterBridge<T>.Register(typeof(ExpandableObjectConverterBridge<T>));
        }

        /// <summary>
        /// ExpandableObjectConverter の対象となる型を登録します。
        /// </summary>
        /// <param name="converterType">ExpandableObjectConverterBridge{T} の型。</param>
        private static void Register(Type converterType)
        {
            var arrtibutes = TypeDescriptor.GetAttributes(typeof(T));
            var hasExpandableObjectConverterBridge = arrtibutes.OfType<TypeConverterAttribute>().Any((tca) => Type.GetType(tca.ConverterTypeName) == converterType);
            if (!hasExpandableObjectConverterBridge)
            {
                TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(converterType));
            }
        }

        /// <summary>
        /// value パラメータに指定されたオブジェクト型のプロパティのコレクションを取得します。
        /// </summary>
        /// <param name="context">書式指定コンテキストを提供する ITypeDescriptorContext。</param>
        /// <param name="value">プロパティを取得する対象となるオブジェクトの型を指定する Object。</param>
        /// <param name="attributes">フィルタとして使用される Attribute 型の配列。</param>
        /// <returns>指定されたコンポーネントに対して公開されているプロパティを格納している PropertyDescriptorCollection。コレクションにプロパティが格納されていない場合は null。</returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            if (context == null || value == null)
            {
                return base.GetProperties(context, value, attributes);
            }

            var properties = new List<PropertyDescriptor>();

            IList list = value as IList;
            if (list != null)
            {
                for (int index = 0, count = list.Count; index < count; index++)
                {
                    properties.Add(new ListPropertyDescriptor(list, index));
                }

                return new PropertyDescriptorCollection(properties.ToArray());
            }

            IDictionary dictionary = value as IDictionary;
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    properties.Add(new DictionaryPropertyDescriptor(dictionary, key));
                }

                return new PropertyDescriptorCollection(properties.ToArray());
            }

            properties.AddRange(base.GetProperties(context, value, attributes).OfType<PropertyDescriptor>());
            return new PropertyDescriptorCollection(properties.ToArray());
        }

        /// <summary>
        /// PropertyDescriptorBridge のオブジェクトを表します。
        /// </summary>
        /// <typeparam name="TBridge">Bridge の型。</typeparam>
        /// <typeparam name="TKey">Key の型。</typeparam>
        private abstract class PropertyDescriptorBridge<TBridge, TKey> : PropertyDescriptor
        {
            /// <summary>
            /// PropertyDescriptorBridge クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="items">TBridge。</param>
            /// <param name="key">TKey。</param>
            protected PropertyDescriptorBridge(TBridge items, TKey key)
                : base(string.Format(CultureInfo.InvariantCulture, "[{0}]", key), null)
            {
                this.Items = items;
                this.Key = key;
            }

            /// <summary>
            /// TBridge を取得します。
            /// </summary>
            /// <value>
            /// 値を表す TBridge 型。
            /// <para>TBridge 。既定値は null です。</para>
            /// </value>
            protected TBridge Items
            {
                get;
                private set;
            }

            /// <summary>
            /// TKey を取得します。
            /// </summary>
            /// <value>
            /// 値を表す TKey 型。
            /// <para>Key。既定値は null です。</para>
            /// </value>
            protected TKey Key
            {
                get;
                private set;
            }

            /// <summary>
            /// Value を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="object"/> 型。
            /// <para>Value。既定値は null です。</para>
            /// </value>
            protected abstract object Value
            {
                get;
            }

            /// <summary>
            /// メンバの属性のコレクションを取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="AttributeCollection"/> 型。
            /// <para>メンバの属性のコレクション。既定値は要素数 0 です。</para>
            /// </value>
            public override AttributeCollection Attributes
            {
                get
                {
                    var hasExpandebleTypeConverter = false;
                    var baseAttributes = base.Attributes;
                    var attributes = new List<Attribute>(baseAttributes.Count);
                    foreach (Attribute baseAttribute in baseAttributes)
                    {
                        TypeConverterAttribute tca = baseAttribute as TypeConverterAttribute;
                        if (tca != null && Type.GetType(tca.ConverterTypeName).IsSubclassOf(typeof(ExpandableObjectConverter)))
                        {
                            attributes.Add(baseAttribute);
                            hasExpandebleTypeConverter = true;
                        }
                        else
                        {
                            attributes.Add(baseAttribute);
                        }
                    }

                    attributes.Add(new CategoryAttribute(this.Category));
                    attributes.Add(new DescriptionAttribute(this.Description));
                    if (this.Value != null)
                    {
                        Type valueType = this.Value.GetType();
                        if (!hasExpandebleTypeConverter && !valueType.IsValueType && valueType != typeof(string))
                        {
                            attributes.Add(new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
                        }
                    }

                    return new AttributeCollection(attributes.ToArray());
                }
            }

            /// <summary>
            /// プロパティが関連付けられているコンポーネントの型を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="Type"/> 型。
            /// <para>プロパティが関連付けられているコンポーネントの型。既定値は null です。</para>
            /// </value>
            public override Type ComponentType
            {
                get
                {
                    return typeof(TBridge);
                }
            }

            /// <summary>
            /// プロパティが読み取り専用かどうかを示す値を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="bool"/> 型。
            /// <para>プロパティが読み取り専用の場合は true。既定値は false です。</para>
            /// </value>
            public override bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// プロパティの型を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="Type"/> 型。
            /// <para>プロパティの型。既定値は null です。</para>
            /// </value>
            public override Type PropertyType
            {
                get
                {
                    object value = this.Value;
                    return (value != null) ? value.GetType() : typeof(object);
                }
            }

            /// <summary>
            /// オブジェクトをリセットしたときに、そのオブジェクトの値が変化するかどうかを示す値を返します。
            /// </summary>
            /// <param name="component">リセット機能について調べる対象のコンポーネント。</param>
            /// <returns>コンポーネントをリセットするとコンポーネントの値が変化する場合は true。それ以外の場合は false。</returns>
            public override bool CanResetValue(object component)
            {
                return false;
            }

            /// <summary>
            /// コンポーネントのプロパティの現在の値を取得します。
            /// </summary>
            /// <param name="component">値の取得対象であるプロパティを持つコンポーネント。</param>
            /// <returns>指定したコンポーネントのプロパティの値。</returns>
            public override object GetValue(object component)
            {
                return this.Value;
            }

            /// <summary>
            /// コンポーネントのプロパティの値を既定値にリセットします。
            /// </summary>
            /// <param name="component">既定値にリセットする対象のプロパティ値を持つコンポーネント。</param>
            public override void ResetValue(object component)
            {
            }

            /// <summary>
            /// コンポーネントの値を別の値に設定します。
            /// </summary>
            /// <param name="component">設定する対象のプロパティ値を持つコンポーネント。</param>
            /// <param name="value">新しい値。</param>
            public override void SetValue(object component, object value)
            {
            }

            /// <summary>
            /// プロパティの値を永続化する必要があるかどうかを示す値を決定します。
            /// </summary>
            /// <param name="component">永続性について調べる対象のプロパティを持つコンポーネント。</param>
            /// <returns>プロパティを永続化する必要がある場合は true。それ以外の場合は false。</returns>
            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }

        /// <summary>
        /// ListPropertyDescriptor のオブジェクトを表します。
        /// </summary>
        private class ListPropertyDescriptor : PropertyDescriptorBridge<IList, int>
        {
            /// <summary>
            /// ListPropertyDescriptor クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="list">IList</param>
            /// <param name="index">index</param>
            internal ListPropertyDescriptor(IList list, int index)
                : base(list, index)
            {
            }

            /// <summary>
            /// メンバが属するカテゴリの名前を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>メンバが属するカテゴリの名前。既定値は "IList" です。</para>
            /// </value>
            public override string Category
            {
                get
                {
                    return "IList";
                }
            }

            /// <summary>
            /// メンバの説明を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>メンバの説明。既定値は string.Empty です。</para>
            /// </value>
            public override string Description
            {
                get
                {
                    return string.Format(CultureInfo.InvariantCulture, "List item at position {0}", this.Key);
                }
            }

            /// <summary>
            /// メンバの値を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="object"/> 型。
            /// <para>メンバの値。既定値は null です。</para>
            /// </value>
            protected override object Value
            {
                get
                {
                    return this.Items[this.Key];
                }
            }
        }

        /// <summary>
        /// DictionaryPropertyDescriptor のオブジェクトを表します。
        /// </summary>
        private class DictionaryPropertyDescriptor : PropertyDescriptorBridge<IDictionary, object>
        {
            /// <summary>
            /// DictionaryPropertyDescriptor クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="dictionary">IDictionary</param>
            /// <param name="key">key</param>
            internal DictionaryPropertyDescriptor(IDictionary dictionary, object key)
                : base(dictionary, key)
            {
            }

            /// <summary>
            /// メンバが属するカテゴリの名前を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>メンバが属するカテゴリの名前。既定値は "IDictionary" です。</para>
            /// </value>
            public override string Category
            {
                get
                {
                    return "IDictionary";
                }
            }

            /// <summary>
            /// メンバの説明を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string"/> 型。
            /// <para>メンバの説明。既定値は string.Empty です。</para>
            /// </value>
            public override string Description
            {
                get
                {
                    return string.Format(CultureInfo.InvariantCulture, "Dictionary item with key '{0}'", this.Key);
                }
            }

            /// <summary>
            /// メンバの値を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="object"/> 型。
            /// <para>メンバの値。既定値は null です。</para>
            /// </value>
            protected override object Value
            {
                get
                {
                    return this.Items[this.Key];
                }
            }
        }
    }
}
