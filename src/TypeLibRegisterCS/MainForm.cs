// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using TypeLibRegisterCS.Configurations;
using TypeLibRegisterCS.Entities;
using TypeLibRegisterCS.Extensions;
using Timer = System.Timers.Timer;

#endregion

namespace TypeLibRegisterCS
{
    /// <summary>
    /// MainForm のユーザーインターフェイスを構成するウィンドウを表します。
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>TypeLibCollector インスタンスを表します。</summary>
        private readonly TypeLibCollector collector;

        /// <summary>時刻表示用のタイマを表します。</summary>
        private readonly Timer timer;

        /// <summary>リストボックスの最大の横幅を表します。</summary>
        private int maxOfLoggingItemWidth;

        /// <summary>
        /// MainForm クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            this.collector = new TypeLibCollector();
            this.collector.ActionToWriteLog = this.AppendLog;

            var config = SerializableConfiguration<TypeLibRegisterConfiguration>.GetInstance();
            var ctrl = this.filterComboBox;
            ctrl.UseWaitCursor = true;
            ctrl.Items.Clear();
            config.FilterItems.ForEach((item, index) => ctrl.Items.Add(item));
            ctrl.SelectedIndex = config.FilterItems.Count > 0 ? 0 : -1;
            ctrl.UseWaitCursor = false;

            this.loggingListBox.DrawMode = DrawMode.OwnerDrawFixed;

            this.AppendLog(LoggingCategory.Debug, "LoggingCategory.Debug");
            this.AppendLog(LoggingCategory.Info, "LoggingCategory.Info");
            this.AppendLog(LoggingCategory.Warning, "LoggingCategory.Warning");
            this.AppendLog(LoggingCategory.Error, "LoggingCategory.Error");

            var context = SynchronizationContext.Current;
            this.timer = new Timer
            {
                AutoReset = true,
                Interval = 1000,
            };
            this.timer.Elapsed += (sender, e) =>
            {
                context.Post(
                    stateOfContext =>
                    {
                        var timeText = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                        if (this.informationToolStripStatusLabel.Text != timeText)
                        {
                            this.informationToolStripStatusLabel.Text = timeText;
                        }
                    },
                    null);

                this.timer.Start();
            };
        }

        /// <summary>
        /// TypeLibIdentifier コレクション (全件) を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="IEnumerable{TypeLibIdentifier}" /> 型。
        /// <para>TypeLibIdentifier コレクション (全件)。既定値は要素数 0 です。</para>
        /// </value>
        internal IEnumerable<TypeLibIdentifier> AllTypeLibIdentifiers { get; private set; }

        /// <summary>
        /// 現在表示されている TypeLibIdentifier コレクションを取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="IEnumerable{TypeLibIdentifier}" /> 型。
        /// <para>現在表示されている TypeLibIdentifier コレクション。既定値は要素数 0 です。</para>
        /// </value>
        internal IEnumerable<TypeLibIdentifier> CurrentTypeLibIdentifiers
        {
            get
            {
                var identifiers = this.typeLibIdentifierDataGridView.DataSource as IEnumerable<TypeLibIdentifier>;
                return identifiers;
            }

            set
            {
                this.typeLibIdentifierDataGridView.AutoGenerateColumns = false;
                this.typeLibIdentifierDataGridView.DataSource = value != null ? value.ToList() : null;
                this.typeLibIdentifierDataGridView.ClearSelection();
                this.typeLibIdentifierPropertyGrid.SelectedObject = null;
            }
        }

        /// <summary>
        /// GetGrayscale
        /// </summary>
        /// <param name="original">original</param>
        /// <returns>Bitmap</returns>
        private static Bitmap GetGrayscale(Image original)
        {
            // Set up the drawing surface
            var grayscale = new Bitmap(original.Width, original.Height);
            using (var g = Graphics.FromImage(grayscale))
            {
                // Grayscale Color Matrix
                var colorMatrix = new ColorMatrix(
                    new[]
                    {
                        new[] { 0.3f, 0.3f, 0.3f, 0, 0 },
                        new[] { 0.59f, 0.59f, 0.59f, 0, 0 },
                        new[] { 0.11f, 0.11f, 0.11f, 0, 0 },
                        new float[] { 0, 0, 0, 1, 0 },
                        new float[] { 0, 0, 0, 0, 1 },
                    });
                // Create attributes
                var att = new ImageAttributes();
                att.SetColorMatrix(colorMatrix);

                // Draw the image with the new attributes
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width,
                    original.Height, GraphicsUnit.Pixel, att);
            }

            return grayscale;
        }

        /// <summary>
        /// ログを追加します。
        /// </summary>
        /// <param name="category">ログカテゴリ。</param>
        /// <param name="format">メッセージの複合書式指定文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ Object 配列。</param>
        private void AppendLog(LoggingCategory category, string format, params object[] args)
        {
            var message = string.Format(CultureInfo.InvariantCulture, format, args);
            this.AppendLog(category, message);
        }

        /// <summary>
        /// ログを追加します。
        /// </summary>
        /// <param name="category">ログカテゴリ。</param>
        /// <param name="message">追加されるメッセージ。</param>
        private void AppendLog(LoggingCategory category, string message)
        {
            var builder = new StringBuilder();
            builder.AppendFormat(
                CultureInfo.InvariantCulture,
                "{0:yyyy/MM/dd HH:mm:ss.fff} {1}",
                DateTime.Now,
                message);
            var ctrl = this.loggingListBox;
            ctrl.Items.Add(new LoggingItem(category, builder.ToString()));
            ctrl.SelectedIndex = ctrl.Items.Count - 1;
        }

        /// <summary>
        /// 検索処理中かの状態を設定します。
        /// </summary>
        /// <param name="isBusy">処理中の場合は true。それ以外は false。</param>
        private void SetScaningProgressState(bool isBusy)
        {
            this.scaningProgressImageLabel.Text = @"Scanning...";
            this.scaningProgressImageLabel.Visible = isBusy;
            this.scaningToolStripProgressBar.Visible = isBusy;

            this.filterComboBox.Enabled = !isBusy;
            this.commandButtonLayoutPanel.Enabled = !isBusy;
            this.logoPicturePanel.Enabled = !isBusy;
        }

        /// <summary>
        /// 保存処理中かの状態を設定します。
        /// </summary>
        /// <param name="isBusy">処理中の場合は true。それ以外は false。</param>
        private void SetSavingProgressState(bool isBusy)
        {
            this.scaningProgressImageLabel.Text = @"Saving...";
            this.scaningProgressImageLabel.Visible = isBusy;
            this.scaningToolStripProgressBar.Visible = isBusy;

            this.filterComboBox.Enabled = !isBusy;
            this.commandButtonLayoutPanel.Enabled = !isBusy;
            this.logoPicturePanel.Enabled = !isBusy;
        }

        /// <summary>
        /// 削除処理中かの状態を設定します。
        /// </summary>
        /// <param name="isBusy">処理中の場合は true。それ以外は false。</param>
        private void SetDeletingProgressState(bool isBusy)
        {
            this.deletingProgressImageLabel.Visible = isBusy;
            this.deletingToolStripProgressBar.Visible = isBusy;

            this.filterComboBox.Enabled = !isBusy;
            this.commandButtonLayoutPanel.Enabled = !isBusy;
            this.logoPicturePanel.Enabled = !isBusy;
        }

        /// <summary>
        /// 削除進捗率を設定します。
        /// </summary>
        /// <param name="percent">進捗率。</param>
        private void SetDeletingProgress(double percent) =>
            this.deletingToolStripProgressBar.Value = Convert.ToInt32(percent);

        /// <summary>
        /// フィルタラベルをクリアします。
        /// </summary>
        private void ClearFoundInformation() => this.filterLabel.Text = string.Empty;

        /// <summary>
        /// フィルタラベルの内容を設定します。
        /// </summary>
        /// <param name="filter">フィルタされる FilterItem。</param>
        /// <param name="foundItemsCount">検索結果件数。</param>
        private void SetFoundInformation(FilterItem filter, int foundItemsCount)
        {
            this.filterLabel.Text = string.Format(
                CultureInfo.InvariantCulture,
                @"Filter {0}: {1} items found.",
                filter != null ? "(" + filter.SearchPattern + ")" : string.Empty,
                foundItemsCount);
            this.AppendLog(LoggingCategory.Info, this.filterLabel.Text);
        }

        /// <summary>
        /// すべてを検索します。
        /// </summary>
        private void PopulateAll() => this.Populate(FilterItem.All);

        /// <summary>
        /// 検索します。
        /// </summary>
        /// <param name="filter">フィルタされる FilterItem。</param>
        private void Populate(FilterItem filter) =>
            this.ConcretePopulate(filter,
                identifier => TypeLibCollector.PathMatchSpec(identifier.FilePath, filter.SearchPattern));

        /// <summary>
        /// 検索します。
        /// </summary>
        /// <param name="filter">フィルタされる FilterItem。</param>
        /// <param name="predicate">TypeLibIdentifier を表示するかを取得するメソッドのデリゲート。</param>
        private void ConcretePopulate(FilterItem filter, Predicate<TypeLibIdentifier> predicate)
        {
            this.SetScaningProgressState(true);

            this.typeLibIdentifierDataGridView.DataSource = null;
            this.typeLibIdentifierPropertyGrid.SelectedObject = null;
            this.ClearFoundInformation();

            this.collector.PopulateAsync(
                foundItems =>
                {
                    var items = foundItems.ToList();
                    this.AllTypeLibIdentifiers = items;

                    var filteredItems = items
                        .Where(identifier => predicate(identifier))
                        .ToList();
                    this.CurrentTypeLibIdentifiers = filteredItems;
                    this.SetFoundInformation(filter, filteredItems.Count);
                    this.SetScaningProgressState(false);
                },
                ex =>
                {
                    this.HandleException(ex);
                    this.SetScaningProgressState(false);
                });
        }

        /// <summary>
        /// 指定された FilterItem でフィルタします。
        /// </summary>
        /// <param name="filter">フィルタされる FilterItem。</param>
        private void Filter(FilterItem filter)
        {
            if (filter == null)
            {
                return;
            }

            this.typeLibIdentifierDataGridView.DataSource = null;
            this.typeLibIdentifierPropertyGrid.SelectedObject = null;
            this.ClearFoundInformation();

            var filteredItems = this.AllTypeLibIdentifiers
                .Where(identifier => TypeLibCollector.PathMatchSpec(identifier.FilePath, filter.SearchPattern))
                .ToList();
            this.CurrentTypeLibIdentifiers = filteredItems;
            this.SetFoundInformation(filter, filteredItems.Count);
        }

        /// <summary>
        /// XML 形式で保存します。
        /// </summary>
        /// <param name="filepathToSave">保存されるファイルパス。</param>
        private void SaveAsXml(string filepathToSave)
        {
            this.SetSavingProgressState(true);

            this.collector.SaveAsXmlAsync(
                filepathToSave,
                this.filterComboBox.SelectedItem as FilterItem,
                this.CurrentTypeLibIdentifiers,
                () => this.SetSavingProgressState(false),
                ex =>
                {
                    this.HandleException(ex);
                    this.SetSavingProgressState(false);
                });
        }

        /// <summary>
        /// TypeLibIdentifier コレクションで構成される各レジストリを削除します。
        /// </summary>
        private void DeleteRegistryTree()
        {
            var filter = this.filterComboBox.SelectedItem as FilterItem;
            this.SetDeletingProgressState(true);
            this.typeLibIdentifierPropertyGrid.SelectedObject = null;

            var deletedItems = new List<TypeLibIdentifier>();
            var queryToDelete = this.typeLibIdentifierDataGridView.SelectedRows
                .OfType<DataGridViewRow>()
                .Select(row => row.DataBoundItem as TypeLibIdentifier)
                .Where(identifier => identifier != null);
            this.collector.DeleteRegistryAsync(
                queryToDelete,
                identifier => deletedItems.Add(identifier),
                this.SetDeletingProgress,
                () =>
                {
                    var query = this.AllTypeLibIdentifiers.Except(deletedItems);
                    this.AllTypeLibIdentifiers = query.ToList();
                    this.Filter(filter);
                    this.SetDeletingProgressState(false);
                },
                ex =>
                {
                    this.HandleException(ex);
                    this.SetDeletingProgressState(false);
                });
        }

        /// <summary>
        /// 例外をハンドルします。
        /// </summary>
        /// <param name="ex">扱う例外。</param>
        private void HandleException(Exception ex)
        {
            this.AppendLog(LoggingCategory.Error, "{0}", ex.Message);
            var dialog = new ExceptionMessageBox(
                ex,
                ExceptionMessageBoxButtons.OK,
                ExceptionMessageBoxSymbol.Error,
                ExceptionMessageBoxDefaultButton.Button1,
                ExceptionMessageBoxOptions.None)
            {
                ShowToolBar = true,
                UseOwnerFont = true,
            };
            dialog.Show(this);
        }

        /// <summary>
        /// フォームが初めて表示される直前に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.timer.Start();

            var infos = new List<string>();
            infos.Add(TypeLibCollector.CpuName);
            infos.Add(TypeLibCollector.CpuIdentifier);
            this.cpuInfoLabel.Text = string.Join(Environment.NewLine, infos.ToArray());

            this.SetScaningProgressState(false);
            this.SetDeletingProgressState(false);
            this.ClearFoundInformation();

            this.PopulateAll();
        }

        /// <summary>
        /// コントロールが再描画されると発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void logoPicturePanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var imagePanel = this.logoPicturePanel;
            var isEnabled = this.logoPicturePanel.Enabled;
            var image = this.logoPicturePanel.BackgroundImage;

            var imageRect = new Rectangle(0, 0, imagePanel.Width, imagePanel.Height);
            if (image != null)
            {
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                if (isEnabled)
                {
                    g.DrawImage(image, imageRect, new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel);
                }
                else
                {
                    // generate grayscale now
                    using (var grayImage = MainForm.GetGrayscale(image))
                    {
                        g.DrawImage(grayImage, imageRect);
                    }
                }
            }
        }

        /// <summary>
        /// オーナー描画 System.Windows.Forms.ListBox のビジュアルな部分を変更すると発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void loggingListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var ctrl = sender as ListBox;
            if (ctrl == null || e.Index == -1)
            {
                return;
            }

            var item = ctrl.Items[e.Index] as LoggingItem;
            var itemText = item != null ? item.ToString() : string.Empty;
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var foreColor = isSelected ? Color.White : e.ForeColor;
            var backColor = isSelected ? Color.Orange : e.BackColor;
            if (item != null)
            {
                foreColor = isSelected ? item.SelectedForeColor : item.ForeColor;
                backColor = isSelected ? item.SelectedBackColor : item.BackColor;
            }

            using (Brush foreBrush = new SolidBrush(foreColor))
            using (Brush backBrush = new SolidBrush(backColor))
            {
                e.DrawBackground();
                e.Graphics.FillRectangle(backBrush, e.Bounds);
                e.Graphics.DrawString(itemText, e.Font, foreBrush, e.Bounds);
            }

            if (isSelected)
            {
                e.DrawFocusRectangle();
            }

            var itemWidth = (int)e.Graphics.MeasureString(itemText, e.Font).Width;
            if (this.maxOfLoggingItemWidth < itemWidth)
            {
                this.maxOfLoggingItemWidth = itemWidth + SystemInformation.VerticalScrollBarWidth;
                ctrl.HorizontalExtent = this.maxOfLoggingItemWidth;
            }
        }

        /// <summary>
        /// SelectedIndex プロパティが変更された場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void filterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox ctrl && !ctrl.UseWaitCursor)
            {
                this.Filter(ctrl.SelectedItem as FilterItem);
            }
        }

        /// <summary>
        /// 入力フォーカスを失ったとき、または取得したときなど、行の状態が変化した場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void typeLibIdentifierDataGridView_RowStateChanged(object sender,
            DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                if (this.typeLibIdentifierPropertyGrid.SelectedObject != e.Row.DataBoundItem)
                {
                    this.typeLibIdentifierPropertyGrid.SelectedObject = e.Row.DataBoundItem;
                }
            }
        }

        /// <summary>
        /// 現在選択されている対象が変更されたときに発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void typeLibIdentifierDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (sender is DataGridView view)
            {
                var selectedCount = view.SelectedRows.Count;
                this.buttonToDelete.Enabled = selectedCount > 0;
            }
        }

        /// <summary>
        /// セルの内容を表示に合わせて変換する必要がある場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void typeLibIdentifierDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is DataGridView view && e.ColumnIndex == 1)
            {
                var row = view.Rows[e.RowIndex];
                var cellStyle = row.DefaultCellStyle;
                if (row.DataBoundItem is TypeLibIdentifier identifier)
                {
                    var foreColor = identifier.IsExistsFile ? cellStyle.ForeColor : Color.DarkRed;
                    cellStyle.ForeColor = foreColor;
                }
            }
        }

        /// <summary>
        /// [clearTheItems] ToolStripItem がクリックされたときに発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void clearTheItemsToolStripMenuItem_Click(object sender, EventArgs e) =>
            this.loggingListBox.Items.Clear();

        /// <summary>
        /// [Refresh] ボタンがクリックされた場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void buttonToRefresh_Click(object sender, EventArgs e) =>
            this.Populate(this.filterComboBox.SelectedItem as FilterItem);

        /// <summary>
        /// [SelectAll] ボタンがクリックされた場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void buttonToSelectAll_Click(object sender, EventArgs e) =>
            this.typeLibIdentifierDataGridView.SelectAll();

        /// <summary>
        /// [SaveAsXml] ボタンがクリックされた場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void buttonToSaveAsXml_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = @"Save as Xml.";
                dialog.Filter = @"xml files (*.xml)|*.xml|All files (*.*)|*.*";
                dialog.AutoUpgradeEnabled = true;
                dialog.RestoreDirectory = false;
                dialog.ValidateNames = true;
                dialog.FileName = string.Format(CultureInfo.InvariantCulture, "TypeLibRegister_{0:yyyyMMddHHmmss}.xml",
                    DateTime.Now);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    var filepathToSave = dialog.FileName;
                    this.SaveAsXml(filepathToSave);
                }
            }
        }

        /// <summary>
        /// [Delete] ボタンがクリックされた場合に発生するイベントのイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベントのソースを表す Object。</param>
        /// <param name="e">イベントデータを格納している EventArgs。</param>
        private void buttonToDelete_Click(object sender, EventArgs e)
        {
            var message = string.Format(
                CultureInfo.InvariantCulture,
                "{0} item(s) is selected. Are you sure you want to delete?",
                this.typeLibIdentifierDataGridView.SelectedRows.Count);
            var dialog = new ExceptionMessageBox(
                message,
                "confirmation...",
                ExceptionMessageBoxButtons.YesNo,
                ExceptionMessageBoxSymbol.Question,
                ExceptionMessageBoxDefaultButton.Button2,
                ExceptionMessageBoxOptions.None)
            {
                ShowToolBar = true,
                UseOwnerFont = true,
            };
            var dialogResult = dialog.Show(this);
            if (dialogResult == DialogResult.Yes)
            {
                this.DeleteRegistryTree();
            }
        }

        /// <summary>
        /// LoggingItem のオブジェクトを表します。
        /// </summary>
        private class LoggingItem
        {
            /// <summary>
            /// LoggingItem クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="category">ログカテゴリ。</param>
            /// <param name="text">ログメッセージ。</param>
            public LoggingItem(LoggingCategory category, string text)
            {
                if (this.Category != category)
                {
                    this.Category = category;
                }

                this.Text = text;
                this.BackColor = SystemColors.Info;
                this.SelectedForeColor = Color.White;
                if (category == LoggingCategory.None)
                {
                    this.ForeColor = SystemColors.InfoText;
                    this.SelectedBackColor = Color.Orange;
                }
                else if (category == LoggingCategory.Debug)
                {
                    this.ForeColor = Color.Gray;
                    this.SelectedBackColor = Color.Gray;
                }
                else if (category == LoggingCategory.Info)
                {
                    this.ForeColor = Color.Blue;
                    this.SelectedBackColor = Color.Blue;
                }
                else if (category == LoggingCategory.Warning)
                {
                    this.ForeColor = Color.DarkOrange;
                    this.SelectedBackColor = Color.DarkOrange;
                }
                else if (category == LoggingCategory.Error)
                {
                    this.ForeColor = Color.Red;
                    this.SelectedBackColor = Color.Red;
                }
            }

            /// <summary>
            /// ログメッセージを取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="string" /> 型。
            /// <para>ログメッセージ。既定値は string.Empty です。</para>
            /// </value>
            public string Text { get; }

            /// <summary>
            /// ログカテゴリを取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="LoggingCategory" /> 型。
            /// <para>ログカテゴリ。既定値は LoggingCategory.None です。</para>
            /// </value>
            internal LoggingCategory Category { get; }

            /// <summary>
            /// 前景色を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="Color" /> 型。
            /// <para>前景色。既定値は Color.Empty です。</para>
            /// </value>
            internal Color ForeColor { get; }

            /// <summary>
            /// 背景色を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="Color" /> 型。
            /// <para>背景色。既定値は Color.Empty です。</para>
            /// </value>
            internal Color BackColor { get; }

            /// <summary>
            /// 選択時の前景色を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="Color" /> 型。
            /// <para>選択時の前景色。既定値は Color.Empty です。</para>
            /// </value>
            internal Color SelectedForeColor { get; }

            /// <summary>
            /// 選択時の背景色を取得します。
            /// </summary>
            /// <value>
            /// 値を表す<see cref="Color" /> 型。
            /// <para>選択時の背景色。既定値は Color.Empty です。</para>
            /// </value>
            internal Color SelectedBackColor { get; }

            /// <summary>
            /// 現在の System.Object を表す System.String を返します。
            /// </summary>
            /// <returns>現在の System.Object を表す System.String。</returns>
            public override string ToString() => this.Text;
        }
    }
}
