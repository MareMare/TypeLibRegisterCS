namespace TypeLibRegisterCS
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing && (this.timer != null))
            {
                this.timer.Dispose();
            }

            if (disposing && (this.collector != null))
            {
                this.collector.Dispose();
            }
            
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.contentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.informationToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.deletingProgressImageLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.deletingToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.scaningProgressImageLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.scaningToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.typeLibIdentifierDataGridView = new System.Windows.Forms.DataGridView();
            this.tLBIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isExistsFileDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.typeLibIdentifierPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.loggingListBox = new System.Windows.Forms.ListBox();
            this.contextMenuStripForLogging = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearTheItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightTopPanel = new System.Windows.Forms.Panel();
            this.filterLabel = new System.Windows.Forms.Label();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.commandButtonLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonToRefresh = new TypeLibRegisterCS.Extensions.CommandLink();
            this.buttonToSelectAll = new TypeLibRegisterCS.Extensions.CommandLink();
            this.buttonToSaveAsXml = new TypeLibRegisterCS.Extensions.CommandLink();
            this.buttonToDelete = new TypeLibRegisterCS.Extensions.CommandLink();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.logoPicturePanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.logoInfoPanel = new System.Windows.Forms.Panel();
            this.cpuInfoLabel = new System.Windows.Forms.Label();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.typeLibIdentifierDataGridView)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStripForLogging.SuspendLayout();
            this.rightTopPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.commandButtonLayoutPanel.SuspendLayout();
            this.logoPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.logoInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // contentPanel
            // 
            this.contentPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.contentPanel.Size = new System.Drawing.Size(810, 629);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            this.toolStripContainer1.BottomToolStripPanel.Font = new System.Drawing.Font("Tahoma", 10F);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.rightTopPanel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.leftPanel);
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.toolStripContainer1.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.toolStripContainer1.ContentPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1100, 587);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1100, 609);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informationToolStripStatusLabel,
            this.deletingProgressImageLabel,
            this.deletingToolStripProgressBar,
            this.scaningProgressImageLabel,
            this.scaningToolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(1100, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // informationToolStripStatusLabel
            // 
            this.informationToolStripStatusLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.informationToolStripStatusLabel.Font = new System.Drawing.Font("Tahoma", 10F);
            this.informationToolStripStatusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.informationToolStripStatusLabel.Name = "informationToolStripStatusLabel";
            this.informationToolStripStatusLabel.Size = new System.Drawing.Size(681, 17);
            this.informationToolStripStatusLabel.Spring = true;
            this.informationToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // deletingProgressImageLabel
            // 
            this.deletingProgressImageLabel.Font = new System.Drawing.Font("Tahoma", 10F);
            this.deletingProgressImageLabel.Image = global::TypeLibRegisterCS.Properties.Resources.Progress100;
            this.deletingProgressImageLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deletingProgressImageLabel.Name = "deletingProgressImageLabel";
            this.deletingProgressImageLabel.Size = new System.Drawing.Size(85, 17);
            this.deletingProgressImageLabel.Text = "Deleting...";
            this.deletingProgressImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // deletingToolStripProgressBar
            // 
            this.deletingToolStripProgressBar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.deletingToolStripProgressBar.Name = "deletingToolStripProgressBar";
            this.deletingToolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.deletingToolStripProgressBar.Step = 1;
            this.deletingToolStripProgressBar.Value = 100;
            // 
            // scaningProgressImageLabel
            // 
            this.scaningProgressImageLabel.Font = new System.Drawing.Font("Tahoma", 10F);
            this.scaningProgressImageLabel.Image = global::TypeLibRegisterCS.Properties.Resources.Progress100;
            this.scaningProgressImageLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.scaningProgressImageLabel.Name = "scaningProgressImageLabel";
            this.scaningProgressImageLabel.Size = new System.Drawing.Size(84, 17);
            this.scaningProgressImageLabel.Text = "Scaning...";
            this.scaningProgressImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scaningToolStripProgressBar
            // 
            this.scaningToolStripProgressBar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.scaningToolStripProgressBar.Name = "scaningToolStripProgressBar";
            this.scaningToolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.scaningToolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(243, 64);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.typeLibIdentifierDataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(847, 513);
            this.splitContainer1.SplitterDistance = 273;
            this.splitContainer1.SplitterWidth = 12;
            this.splitContainer1.TabIndex = 5;
            // 
            // typeLibIdentifierDataGridView
            // 
            this.typeLibIdentifierDataGridView.AllowUserToAddRows = false;
            this.typeLibIdentifierDataGridView.AllowUserToDeleteRows = false;
            this.typeLibIdentifierDataGridView.AllowUserToResizeColumns = false;
            this.typeLibIdentifierDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Orange;
            this.typeLibIdentifierDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.typeLibIdentifierDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.typeLibIdentifierDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tLBIDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.versionDataGridViewTextBoxColumn,
            this.filePathDataGridViewTextBoxColumn,
            this.fileNameDataGridViewTextBoxColumn,
            this.isExistsFileDataGridViewCheckBoxColumn});
            this.typeLibIdentifierDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeLibIdentifierDataGridView.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.typeLibIdentifierDataGridView.Location = new System.Drawing.Point(0, 0);
            this.typeLibIdentifierDataGridView.Name = "typeLibIdentifierDataGridView";
            this.typeLibIdentifierDataGridView.ReadOnly = true;
            this.typeLibIdentifierDataGridView.RowHeadersVisible = false;
            this.typeLibIdentifierDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Orange;
            this.typeLibIdentifierDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.typeLibIdentifierDataGridView.RowTemplate.Height = 25;
            this.typeLibIdentifierDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.typeLibIdentifierDataGridView.Size = new System.Drawing.Size(847, 273);
            this.typeLibIdentifierDataGridView.StandardTab = true;
            this.typeLibIdentifierDataGridView.TabIndex = 0;
            this.typeLibIdentifierDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.typeLibIdentifierDataGridView_CellFormatting);
            this.typeLibIdentifierDataGridView.SelectionChanged += new System.EventHandler(this.typeLibIdentifierDataGridView_SelectionChanged);
            this.typeLibIdentifierDataGridView.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.typeLibIdentifierDataGridView_RowStateChanged);
            // 
            // tLBIDDataGridViewTextBoxColumn
            // 
            this.tLBIDDataGridViewTextBoxColumn.DataPropertyName = "TLBID";
            this.tLBIDDataGridViewTextBoxColumn.HeaderText = "TLBID";
            this.tLBIDDataGridViewTextBoxColumn.Name = "tLBIDDataGridViewTextBoxColumn";
            this.tLBIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.tLBIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.nameDataGridViewTextBoxColumn.FillWeight = 60F;
            this.nameDataGridViewTextBoxColumn.HeaderText = "DisplayName";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // versionDataGridViewTextBoxColumn
            // 
            this.versionDataGridViewTextBoxColumn.DataPropertyName = "Version";
            this.versionDataGridViewTextBoxColumn.FillWeight = 10F;
            this.versionDataGridViewTextBoxColumn.HeaderText = "Version";
            this.versionDataGridViewTextBoxColumn.Name = "versionDataGridViewTextBoxColumn";
            this.versionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // filePathDataGridViewTextBoxColumn
            // 
            this.filePathDataGridViewTextBoxColumn.DataPropertyName = "FilePath";
            this.filePathDataGridViewTextBoxColumn.FillWeight = 30F;
            this.filePathDataGridViewTextBoxColumn.HeaderText = "FilePath";
            this.filePathDataGridViewTextBoxColumn.Name = "filePathDataGridViewTextBoxColumn";
            this.filePathDataGridViewTextBoxColumn.ReadOnly = true;
            this.filePathDataGridViewTextBoxColumn.Visible = false;
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            this.fileNameDataGridViewTextBoxColumn.FillWeight = 40F;
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "FileName";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isExistsFileDataGridViewCheckBoxColumn
            // 
            this.isExistsFileDataGridViewCheckBoxColumn.DataPropertyName = "IsExistsFile";
            this.isExistsFileDataGridViewCheckBoxColumn.HeaderText = "IsExistsFile";
            this.isExistsFileDataGridViewCheckBoxColumn.Name = "isExistsFileDataGridViewCheckBoxColumn";
            this.isExistsFileDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isExistsFileDataGridViewCheckBoxColumn.Visible = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.typeLibIdentifierPropertyGrid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.loggingListBox);
            this.splitContainer2.Size = new System.Drawing.Size(847, 228);
            this.splitContainer2.SplitterDistance = 102;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 8;
            // 
            // typeLibIdentifierPropertyGrid
            // 
            this.typeLibIdentifierPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeLibIdentifierPropertyGrid.Font = new System.Drawing.Font("Tahoma", 10F);
            this.typeLibIdentifierPropertyGrid.HelpVisible = false;
            this.typeLibIdentifierPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.typeLibIdentifierPropertyGrid.Name = "typeLibIdentifierPropertyGrid";
            this.typeLibIdentifierPropertyGrid.Size = new System.Drawing.Size(847, 102);
            this.typeLibIdentifierPropertyGrid.TabIndex = 0;
            this.typeLibIdentifierPropertyGrid.ToolbarVisible = false;
            this.typeLibIdentifierPropertyGrid.ViewBackColor = System.Drawing.SystemColors.Info;
            // 
            // loggingListBox
            // 
            this.loggingListBox.BackColor = System.Drawing.SystemColors.Info;
            this.loggingListBox.ContextMenuStrip = this.contextMenuStripForLogging;
            this.loggingListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loggingListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.loggingListBox.Font = new System.Drawing.Font("Tahoma", 10F);
            this.loggingListBox.FormattingEnabled = true;
            this.loggingListBox.HorizontalScrollbar = true;
            this.loggingListBox.IntegralHeight = false;
            this.loggingListBox.ItemHeight = 16;
            this.loggingListBox.Location = new System.Drawing.Point(0, 0);
            this.loggingListBox.Name = "loggingListBox";
            this.loggingListBox.ScrollAlwaysVisible = true;
            this.loggingListBox.Size = new System.Drawing.Size(847, 116);
            this.loggingListBox.TabIndex = 0;
            this.loggingListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.loggingListBox_DrawItem);
            // 
            // contextMenuStripForLogging
            // 
            this.contextMenuStripForLogging.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearTheItemsToolStripMenuItem});
            this.contextMenuStripForLogging.Name = "contextMenuStripForLogging";
            this.contextMenuStripForLogging.Size = new System.Drawing.Size(181, 26);
            // 
            // clearTheItemsToolStripMenuItem
            // 
            this.clearTheItemsToolStripMenuItem.Image = global::TypeLibRegisterCS.Properties.Resources.delete;
            this.clearTheItemsToolStripMenuItem.Name = "clearTheItemsToolStripMenuItem";
            this.clearTheItemsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearTheItemsToolStripMenuItem.Text = "&Clear the item(s).";
            this.clearTheItemsToolStripMenuItem.Click += new System.EventHandler(this.clearTheItemsToolStripMenuItem_Click);
            // 
            // rightTopPanel
            // 
            this.rightTopPanel.BackColor = System.Drawing.Color.Transparent;
            this.rightTopPanel.Controls.Add(this.filterLabel);
            this.rightTopPanel.Controls.Add(this.filterComboBox);
            this.rightTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.rightTopPanel.Location = new System.Drawing.Point(243, 10);
            this.rightTopPanel.Name = "rightTopPanel";
            this.rightTopPanel.Size = new System.Drawing.Size(847, 54);
            this.rightTopPanel.TabIndex = 6;
            // 
            // filterLabel
            // 
            this.filterLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterLabel.Location = new System.Drawing.Point(0, 27);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(847, 27);
            this.filterLabel.TabIndex = 1;
            this.filterLabel.Text = "Filter: 0 items found.";
            this.filterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // filterComboBox
            // 
            this.filterComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.Location = new System.Drawing.Point(0, 0);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(847, 27);
            this.filterComboBox.TabIndex = 0;
            this.filterComboBox.SelectedIndexChanged += new System.EventHandler(this.filterComboBox_SelectedIndexChanged);
            // 
            // leftPanel
            // 
            this.leftPanel.BackColor = System.Drawing.Color.Transparent;
            this.leftPanel.Controls.Add(this.commandButtonLayoutPanel);
            this.leftPanel.Controls.Add(this.logoPanel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 10);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.leftPanel.Size = new System.Drawing.Size(243, 567);
            this.leftPanel.TabIndex = 8;
            // 
            // commandButtonLayoutPanel
            // 
            this.commandButtonLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.commandButtonLayoutPanel.Controls.Add(this.buttonToRefresh);
            this.commandButtonLayoutPanel.Controls.Add(this.buttonToSelectAll);
            this.commandButtonLayoutPanel.Controls.Add(this.buttonToSaveAsXml);
            this.commandButtonLayoutPanel.Controls.Add(this.buttonToDelete);
            this.commandButtonLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandButtonLayoutPanel.Location = new System.Drawing.Point(3, 0);
            this.commandButtonLayoutPanel.Name = "commandButtonLayoutPanel";
            this.commandButtonLayoutPanel.Size = new System.Drawing.Size(237, 423);
            this.commandButtonLayoutPanel.TabIndex = 1;
            // 
            // buttonToRefresh
            // 
            this.buttonToRefresh.BackColor = System.Drawing.Color.Transparent;
            this.buttonToRefresh.DescriptionText = "Rescan the items from registry.";
            this.buttonToRefresh.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonToRefresh.HeaderText = "Rescan";
            this.buttonToRefresh.Image = global::TypeLibRegisterCS.Properties.Resources.refresh;
            this.buttonToRefresh.ImageVerticalAlign = TypeLibRegisterCS.Extensions.VerticalAlign.Middle;
            this.buttonToRefresh.Location = new System.Drawing.Point(3, 3);
            this.buttonToRefresh.Name = "buttonToRefresh";
            this.buttonToRefresh.Size = new System.Drawing.Size(233, 51);
            this.buttonToRefresh.TabIndex = 0;
            this.buttonToRefresh.Click += new System.EventHandler(this.buttonToRefresh_Click);
            // 
            // buttonToSelectAll
            // 
            this.buttonToSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.buttonToSelectAll.DescriptionText = "Select all items.";
            this.buttonToSelectAll.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonToSelectAll.HeaderText = "Select All";
            this.buttonToSelectAll.Image = global::TypeLibRegisterCS.Properties.Resources.arrow_right_green;
            this.buttonToSelectAll.ImageVerticalAlign = TypeLibRegisterCS.Extensions.VerticalAlign.Middle;
            this.buttonToSelectAll.Location = new System.Drawing.Point(3, 60);
            this.buttonToSelectAll.Name = "buttonToSelectAll";
            this.buttonToSelectAll.Size = new System.Drawing.Size(233, 51);
            this.buttonToSelectAll.TabIndex = 1;
            this.buttonToSelectAll.Click += new System.EventHandler(this.buttonToSelectAll_Click);
            // 
            // buttonToSaveAsXml
            // 
            this.buttonToSaveAsXml.BackColor = System.Drawing.Color.Transparent;
            this.buttonToSaveAsXml.DescriptionText = "Save as the XML format.";
            this.buttonToSaveAsXml.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonToSaveAsXml.HeaderText = "Save";
            this.buttonToSaveAsXml.Image = global::TypeLibRegisterCS.Properties.Resources.FloppyDisk;
            this.buttonToSaveAsXml.ImageVerticalAlign = TypeLibRegisterCS.Extensions.VerticalAlign.Middle;
            this.buttonToSaveAsXml.Location = new System.Drawing.Point(3, 117);
            this.buttonToSaveAsXml.Name = "buttonToSaveAsXml";
            this.buttonToSaveAsXml.Size = new System.Drawing.Size(233, 51);
            this.buttonToSaveAsXml.TabIndex = 2;
            this.buttonToSaveAsXml.Click += new System.EventHandler(this.buttonToSaveAsXml_Click);
            // 
            // buttonToDelete
            // 
            this.buttonToDelete.BackColor = System.Drawing.Color.Transparent;
            this.buttonToDelete.DescriptionText = "Removes the selected item(s).";
            this.buttonToDelete.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonToDelete.HeaderText = "Delete";
            this.buttonToDelete.Image = global::TypeLibRegisterCS.Properties.Resources.delete;
            this.buttonToDelete.ImageVerticalAlign = TypeLibRegisterCS.Extensions.VerticalAlign.Middle;
            this.buttonToDelete.Location = new System.Drawing.Point(3, 174);
            this.buttonToDelete.Name = "buttonToDelete";
            this.buttonToDelete.Size = new System.Drawing.Size(233, 51);
            this.buttonToDelete.TabIndex = 3;
            this.buttonToDelete.Click += new System.EventHandler(this.buttonToDelete_Click);
            // 
            // logoPanel
            // 
            this.logoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logoPanel.Controls.Add(this.logoPicturePanel);
            this.logoPanel.Controls.Add(this.panel2);
            this.logoPanel.Controls.Add(this.logoInfoPanel);
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logoPanel.Location = new System.Drawing.Point(3, 423);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(237, 144);
            this.logoPanel.TabIndex = 2;
            // 
            // logoPicturePanel
            // 
            this.logoPicturePanel.BackgroundImage = global::TypeLibRegisterCS.Properties.Resources.TypeLibRegister;
            this.logoPicturePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logoPicturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPicturePanel.Location = new System.Drawing.Point(0, 0);
            this.logoPicturePanel.Name = "logoPicturePanel";
            this.logoPicturePanel.Size = new System.Drawing.Size(89, 89);
            this.logoPicturePanel.TabIndex = 2;
            this.logoPicturePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.logoPicturePanel_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(89, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(148, 89);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 89);
            this.label1.TabIndex = 3;
            this.label1.Text = "1234567890123456789012345678\r\n2234567890123456789012345678\r\n323456789012345678901" +
                "2345678\r\n4234567890123456789012345678\r\n5234567890123456789012345678\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // logoInfoPanel
            // 
            this.logoInfoPanel.Controls.Add(this.cpuInfoLabel);
            this.logoInfoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logoInfoPanel.Location = new System.Drawing.Point(0, 89);
            this.logoInfoPanel.Name = "logoInfoPanel";
            this.logoInfoPanel.Size = new System.Drawing.Size(237, 55);
            this.logoInfoPanel.TabIndex = 1;
            // 
            // cpuInfoLabel
            // 
            this.cpuInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cpuInfoLabel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpuInfoLabel.Location = new System.Drawing.Point(0, 0);
            this.cpuInfoLabel.Name = "cpuInfoLabel";
            this.cpuInfoLabel.Size = new System.Drawing.Size(237, 55);
            this.cpuInfoLabel.TabIndex = 2;
            this.cpuInfoLabel.Text = "123456789012345678901234567890\r\n223456789012345678901234567890\r\n32345678901234567" +
                "8901234567890\r\n423456789012345678901234567890\r\n523456789012345678901234567890\r\n";
            this.cpuInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 609);
            this.Controls.Add(this.toolStripContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(709, 485);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TypeLibRegisterCS";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.typeLibIdentifierDataGridView)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStripForLogging.ResumeLayout(false);
            this.rightTopPanel.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.commandButtonLayoutPanel.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.logoInfoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel contentPanel;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel informationToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel scaningProgressImageLabel;
        private System.Windows.Forms.ToolStripProgressBar scaningToolStripProgressBar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView typeLibIdentifierDataGridView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripForLogging;
        private System.Windows.Forms.ToolStripMenuItem clearTheItemsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid typeLibIdentifierPropertyGrid;
        private System.Windows.Forms.ListBox loggingListBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn tLBIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isExistsFileDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.FlowLayoutPanel commandButtonLayoutPanel;
        private TypeLibRegisterCS.Extensions.CommandLink buttonToRefresh;
        private TypeLibRegisterCS.Extensions.CommandLink buttonToSelectAll;
        private TypeLibRegisterCS.Extensions.CommandLink buttonToSaveAsXml;
        private TypeLibRegisterCS.Extensions.CommandLink buttonToDelete;
        private System.Windows.Forms.Panel rightTopPanel;
        private System.Windows.Forms.Label filterLabel;
        private System.Windows.Forms.ComboBox filterComboBox;
        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.Panel logoInfoPanel;
        private System.Windows.Forms.Panel logoPicturePanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label cpuInfoLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel deletingProgressImageLabel;
        private System.Windows.Forms.ToolStripProgressBar deletingToolStripProgressBar;
    }
}

