// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamAsyncHelper.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml;

#endregion

namespace TypeLibRegisterCS.Serialization
{
    /// <summary>
    /// ストリームに対して非同期で操作するオブジェクトを表します。
    /// <para>このクラスは継承できません。</para>
    /// </summary>
    internal sealed class StreamAsyncHelper
    {
        /// <summary>バッファサイズ。</summary>
        private const int BufferSize = 0x1000;

        /// <summary>
        /// StreamHelper クラスの新しいインスタンスを初期化します。
        /// </summary>
        private StreamAsyncHelper()
        {
        }

        /// <summary>
        /// 指定されたファイルパス (オリジナル、作業パス、バックアップパス) でファイルの生成を行います。
        /// </summary>
        /// <param name="path">最終的なパス。</param>
        /// <param name="tempFilePath">作業用のパス。</param>
        /// <param name="backFilePath">バックアップ用のパス。</param>
        internal static void GenerateFile(string path, string tempFilePath, string backFilePath)
        {
            if (File.Exists(backFilePath))
            {
                // 読み取り専用属性があるかを調べます。
                var attribute = File.GetAttributes(backFilePath);
                if ((attribute & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    // 読み取り専用属性を削除します。
                    File.SetAttributes(backFilePath, attribute & ~FileAttributes.ReadOnly);
                }

                File.Delete(backFilePath);
            }

            if (File.Exists(path))
            {
                File.Move(path, backFilePath);
            }

            File.Move(tempFilePath, path);
        }

        /// <summary>
        /// 指定したファイルに格納されている XML ドキュメントを非同期で逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="path">逆シリアル化する XML ドキュメントを格納しているファイルのパス。</param>
        /// <param name="deserialize">
        /// 逆シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;XmlDocument, T&gt; である必要があります。
        /// </param>
        /// <param name="success">非同期読み込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期読み込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        internal static void LoadAndXmlDeserializeAsync<T>(string path, Func<XmlDocument, T> deserialize,
            Action<T> success, Action<Exception> failure)
        {
            var asyncOp = AsyncOperationManager.CreateOperation(null);
            StreamAsyncHelper.ReadAllBytesAsyncInternal(
                path,
                bytes =>
                {
                    T deserialized;
                    using (var stream = new MemoryStream(bytes))
                    {
                        var document = new XmlDocument();
                        document.Load(stream);
                        deserialized = deserialize(document);
                    }

                    asyncOp.Post(delegate { success(deserialized); }, null);
                },
                exception => { asyncOp.Post(delegate { failure(exception); }, null); });
        }

        /// <summary>
        /// 指定したファイルに格納されているバイナリデータを非同期で逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">逆シリアル化する型。</typeparam>
        /// <param name="path">逆シリアル化するオブジェクトを格納しているファイルのパス。</param>
        /// <param name="deserialize">
        /// 逆シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;byte[], T&gt; である必要があります。
        /// </param>
        /// <param name="success">非同期読み込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期読み込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        private static void LoadAndBinaryDeserializeAsync<T>(string path, Func<byte[], T> deserialize,
            Action<T> success, Action<Exception> failure)
        {
            var asyncOp = AsyncOperationManager.CreateOperation(null);
            StreamAsyncHelper.ReadAllBytesAsyncInternal(
                path,
                bytes =>
                {
                    var deserialized = deserialize(bytes);
                    asyncOp.Post(delegate { success(deserialized); }, null);
                },
                exception => { asyncOp.Post(delegate { failure(exception); }, null); });
        }

        /// <summary>
        /// 指定した Object をシリアル化し、指定したファイルに XML ドキュメントとして非同期で書き込みます。
        /// </summary>
        /// <param name="path">保存するファイルのパス。</param>
        /// <param name="obj">シリアル化する Object。</param>
        /// <param name="serialize">
        /// シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;object, XmlDocument&gt; である必要があります。
        /// </param>
        /// <param name="success">非同期書き込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期書き込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        internal static void SaveAndXmlSerializeAsync(string path, object obj, Func<object, XmlDocument> serialize,
            Action success, Action<Exception> failure)
        {
            var asyncOp = AsyncOperationManager.CreateOperation(null);
            ThreadPool.QueueUserWorkItem(delegate
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    var document = serialize(obj);
                    document.Save(stream);
                    stream.Position = 0;
                    bytes = stream.GetBuffer();
                }

                StreamAsyncHelper.WriteAllBytesAsyncInternal(
                    path,
                    bytes,
                    delegate // DevPartner 静的解析対応：ラムダ式をデリゲートへ変更します。
                    {
                        asyncOp.Post(delegate { success(); }, null);
                    },
                    delegate(Exception exception) // DevPartner 静的解析対応：ラムダ式をデリゲートへ変更します。
                    {
                        asyncOp.Post(delegate { failure(exception); }, null);
                    });
            });
        }

        /// <summary>
        /// 指定した Object をシリアル化し、指定したファイルにバイナリデータとして非同期で書き込みます。
        /// </summary>
        /// <param name="path">保存するファイルパス。</param>
        /// <param name="obj">シリアル化するオブジェクト。</param>
        /// <param name="serialize">
        /// シリアル化を行うメソッドのデリゲート。
        /// これは Func&lt;object, byte[]&gt; である必要があります。
        /// </param>
        /// <param name="success">非同期書き込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期書き込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        internal static void SaveAndBinarySerializeAsync(string path, object obj, Func<object, byte[]> serialize,
            Action success, Action<Exception> failure)
        {
            var asyncOp = AsyncOperationManager.CreateOperation(null);
            ThreadPool.QueueUserWorkItem(delegate
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    bytes = serialize(obj);
                }

                StreamAsyncHelper.WriteAllBytesAsyncInternal(
                    path,
                    bytes,
                    delegate // DevPartner 静的解析対応：ラムダ式をデリゲートへ変更します。
                    {
                        asyncOp.Post(delegate { success(); }, null);
                    },
                    delegate(Exception exception) // DevPartner 静的解析対応：ラムダ式をデリゲートへ変更します。
                    {
                        asyncOp.Post(delegate { failure(exception); }, null);
                    });
            });
        }

        #region 基本 (サンプル)

#if SAMPLE
        private static void ReadAllBytesAsync(string path, Action<byte[]> success, Action<Exception> failure)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            StreamAsyncHelper.ReadAllBytesAsyncInternal(
                path,
                (bytes) =>
                {
                    asyncOp.Post(delegate { success(bytes); }, null);
                },
                (exception) =>
                {
                    asyncOp.Post(delegate { failure(exception); }, null);
                });
        }

        private static void WriteAllBytesAsync(string path, byte[] bytes, Action success, Action<Exception> failure)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            StreamAsyncHelper.WriteAllBytesAsyncInternal(
                path,
                bytes,
                () =>
                {
                    asyncOp.Post(delegate { success(); }, null);
                },
                (exception) =>
                {
                    asyncOp.Post(delegate { failure(exception); }, null);
                });
        }
#endif

        #endregion

        /// <summary>
        /// ファイルパスを指定して、ファイルからバイト配列を非同期で読み込みます。
        /// </summary>
        /// <param name="path">読み込むファイルのパス。</param>
        /// <param name="success">非同期読み込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期読み込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        private static void ReadAllBytesAsyncInternal(string path, Action<byte[]> success, Action<Exception> failure)
        {
            try
            {
                var input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read,
                    StreamAsyncHelper.BufferSize, true);
                var output = new MemoryStream((int)input.Length);
                StreamAsyncHelper.CopyStreamToStreamAsync(
                    input,
                    output,
                    error =>
                    {
                        var bytes = error == null ? output.GetBuffer() : null;
                        output.Close();
                        input.Close();

                        if (error != null)
                        {
                            failure(error);
                        }
                        else
                        {
                            success(bytes);
                        }
                    });
            }
            catch (Exception ex)
            {
                failure(ex);
            }
        }

        /// <summary>
        /// ファイルパスとバイト配列を指定して、ファイルを非同期で書き込みます。
        /// </summary>
        /// <param name="path">書き込むファイルのパス。</param>
        /// <param name="bytes">書き込まれるバイト配列。</param>
        /// <param name="success">非同期書き込みに成功した処理を行うメソッドのデリゲート。</param>
        /// <param name="failure">非同期書き込みで例外が発生した時の処理を行うメソッドのデリゲート。</param>
        private static void WriteAllBytesAsyncInternal(string path, byte[] bytes, Action success,
            Action<Exception> failure)
        {
            try
            {
                var input = new MemoryStream(bytes);
                var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None,
                    StreamAsyncHelper.BufferSize, true);
                StreamAsyncHelper.CopyStreamToStreamAsync(
                    input,
                    output,
                    error =>
                    {
                        input.Close();
                        output.Close();

                        if (error != null)
                        {
                            failure(error);
                        }
                        else
                        {
                            success();
                        }
                    });
            }
            catch (Exception ex)
            {
                failure(ex);
            }
        }

        /// <summary>
        /// 指定されたコピー元のストリームをコピー先のストリームへ非同期でコピーします。
        /// </summary>
        /// <param name="source">コピー元のストリーム。</param>
        /// <param name="destination">コピー先のストリーム。</param>
        /// <param name="completed">
        /// 非同期操作が完了した時の処理を行うメソッドのデリゲート。
        /// これは Action&lt;Exception&gt; である必要があります。
        /// 正常に完了した場合は null を、例外が発生した場合はその例外を引数へ渡されます。
        /// </param>
        private static void CopyStreamToStreamAsync(Stream source, Stream destination, Action<Exception> completed)
        {
            var buffer = new byte[StreamAsyncHelper.BufferSize];
            if (completed == null)
            {
                completed = delegate { };
            }

            AsyncCallback rc = null;
            rc = readResult =>
            {
                try
                {
                    var read = source.EndRead(readResult);
                    if (read > 0)
                    {
                        destination.BeginWrite(
                            buffer,
                            0,
                            read,
                            writeResult =>
                            {
                                try
                                {
                                    destination.EndWrite(writeResult);
                                    source.BeginRead(buffer, 0, buffer.Length, rc, null);
                                }
                                catch (Exception ex)
                                {
                                    completed(ex);
                                }
                            },
                            null);
                    }
                    else
                    {
                        completed(null);
                    }
                }
                catch (Exception ex)
                {
                    completed(ex);
                }
            };

            source.BeginRead(buffer, 0, buffer.Length, rc, null);
        }
    }
}
