// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLink.cs" company="MareMare">
// Copyright © 2010 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region References

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

#endregion

namespace TypeLibRegisterCS.Extensions
{
    /// <summary>
    /// CommandLink のオブジェクトを表します。
    /// </summary>
    [DefaultEvent("Click")]
    public partial class CommandLink : UserControl
    {
        /// <summary>state</summary>
        private State state = State.Normal;

        /// <summary>offset</summary>
        private int offset;

        /// <summary>headerText</summary>
        private string headerText = "Header Text";

        /// <summary>descriptionText</summary>
        private string descriptionText = "Description";

        /// <summary>image</summary>
        private Bitmap image;

        /// <summary>grayImage</summary>
        private Bitmap grayImage;

        /// <summary>imageSize</summary>
        private Size imageSize = new Size(24, 24);

        /// <summary>imageAlign</summary>
        private VerticalAlign imageAlign = VerticalAlign.Top;

        /// <summary>textAlign</summary>
        private VerticalAlign textAlign = VerticalAlign.Middle;

        /// <summary>descriptFont</summary>
        private Font descriptFont;

        /// <summary>diagResult</summary>
        private DialogResult diagResult = DialogResult.None;

        /// <summary>
        /// CommandLink クラスの新しいインスタンスを初期化します。
        /// </summary>
        public CommandLink()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true; //Smooth redrawing
        }

        /// <summary>
        /// State
        /// </summary>
        private enum State
        {
            /// <summary>Normal</summary>
            Normal,

            /// <summary>Hover</summary>
            Hover,

            /// <summary>Pushed</summary>
            Pushed,

            /// <summary>Disabled</summary>
            Disabled,
        }

        /// <summary>
        /// HeaderText を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>HeaderText。既定値は string.Empty です。</para>
        /// </value>
        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue("Header Text")]
        public string HeaderText
        {
            get
            {
                return this.headerText;
            }

            set
            {
                this.headerText = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// DescriptionText を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="string"/> 型。
        /// <para>DescriptionText。既定値は string.Empty です。</para>
        /// </value>
        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue("Description")]
        public string DescriptionText
        {
            get
            {
                return this.descriptionText;
            }

            set
            {
                this.descriptionText = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Image を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Bitmap"/> 型。
        /// <para>Image。既定値は null です。</para>
        /// </value>
        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue(null)]
        public Bitmap Image
        {
            get
            {
                return this.image;
            }

            set
            {
                //Clean up
                if (this.image != null)
                {
                    this.image.Dispose();
                }

                if (this.grayImage != null)
                {
                    this.grayImage.Dispose();
                }

                this.image = value;
                if (this.image != null)
                {
                    this.grayImage = CommandLink.GetGrayscale(this.image); //generate image for disabled state
                }
                else
                {
                    this.grayImage = null;
                }

                this.Refresh();
            }
        }

        /// <summary>
        /// ImageScalingSize を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Size"/> 型。
        /// <para>ImageScalingSize。既定値は 24,24 です。</para>
        /// </value>
        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue(typeof(Size), "24,24")]
        public Size ImageScalingSize
        {
            get
            {
                return this.imageSize;
            }

            set
            {
                this.imageSize = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// ImageVerticalAlign を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="VerticalAlign"/> 型。
        /// <para>ImageVerticalAlign。既定値は VerticalAlign.Top です。</para>
        /// </value>
        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue(VerticalAlign.Top)]
        public VerticalAlign ImageVerticalAlign
        {
            get
            {
                return this.imageAlign;
            }

            set
            {
                this.imageAlign = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// TextVerticalAlign を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="VerticalAlign"/> 型。
        /// <para>TextVerticalAlign。既定値は VerticalAlign.Middle です。</para>
        /// </value>
        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue(VerticalAlign.Middle)]
        public VerticalAlign TextVerticalAlign
        {
            get
            {
                return this.textAlign;
            }

            set
            {
                this.textAlign = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Font を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="Font"/> 型。
        /// <para>Font。既定値は null です。</para>
        /// </value>
        [Category("Command Appearance")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }

            set
            {
                base.Font = value;

                //Clean up
                if (this.descriptFont != null)
                {
                    this.descriptFont.Dispose();
                }

                //Update the description font, which is the same just 3 sizes smaller
                this.descriptFont = new Font(this.Font.FontFamily, this.Font.Size - 3);
            }
        }

        /// <summary>
        /// DialogResult を取得または設定します。
        /// </summary>
        /// <value>
        /// 値を表す<see cref="DialogResult"/> 型。
        /// <para>DialogResult。既定値は DialogResult.None です。</para>
        /// </value>
        [Category("Behavior"),
         DefaultValue(DialogResult.None)]
        public DialogResult DialogResult
        {
            get
            {
                return this.diagResult;
            }

            set
            {
                this.diagResult = value;
            }
        }

        /// <summary>
        /// RoundedRect
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="radius">radius</param>
        /// <returns>GraphicsPath</returns>
        private static GraphicsPath RoundedRect(int width, int height, int radius)
        {
            RectangleF baseRect = new RectangleF(0, 0, width, height);
            float diameter = radius * 2.0f;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// GetGrayscale
        /// </summary>
        /// <param name="original">original</param>
        /// <returns>Bitmap</returns>
        private static Bitmap GetGrayscale(Image original)
        {
            //Set up the drawing surface
            Bitmap grayscale = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(grayscale);

            //Grayscale Color Matrix
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] { 0.3f, 0.3f, 0.3f, 0, 0 },
                    new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
                    new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 },
                });

            //Create attributes
            ImageAttributes att = new ImageAttributes();
            att.SetColorMatrix(colorMatrix);

            //Draw the image with the new attributes
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, att);

            //Clean up
            g.Dispose();

            return grayscale;
        }

        /// <summary>
        /// WordWrap
        /// </summary>
        /// <param name="originalString">originalString</param>
        /// <param name="font">font</param>
        /// <param name="targetWidth">targetWidth</param>
        /// <returns>string</returns>
        private static string WordWrap(string originalString, Font font, int targetWidth)
        {
            string[] words = originalString.Split(' ');
            string wrappedString = words[0];

            //Add one word at a time, making sure it doesn't go over
            for (int i = 1; i < words.Length; i++)
            {
                //Use TextRenderer since it has a static measure function
                if (TextRenderer.MeasureText(wrappedString + " " + words[i], font).Width <= targetWidth)
                {
                    wrappedString += " " + words[i]; //next word fits on the same line
                }
                else
                {
                    wrappedString += Environment.NewLine + words[i]; //start it on the next line
                }
            }

            return wrappedString;
        }

        /// <summary>
        /// CommandLink クラスのインスタンスによって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.image.Dispose();
                this.grayImage.Dispose();
                this.descriptFont.Dispose();
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// System.Windows.Forms.Control.Paint イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); //draws the regular background stuff

            if (this.Focused && this.state == State.Normal)
            {
                this.DrawHighlight(e.Graphics);
            }

            switch (this.state)
            {
                case State.Normal:
                    this.DrawNormalState(e.Graphics);
                    break;
                case State.Hover:
                    this.DrawHoverState(e.Graphics);
                    break;
                case State.Pushed:
                    this.DrawPushedState(e.Graphics);
                    break;
                case State.Disabled:
                    this.DrawNormalState(e.Graphics); //DrawForeground takes care of drawing the disabled state
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// System.Windows.Forms.Control.Click イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.EventArgs。</param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (this.diagResult != DialogResult.None)
            {
                this.ParentForm.DialogResult = this.diagResult;
                this.ParentForm.Close();
            }
        }

        /// <summary>
        /// System.Windows.Forms.Control.KeyPress イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.Windows.Forms.KeyPressEventArgs。</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter, CultureInfo.InvariantCulture))
            {
                this.PerformClick();
            }

            base.OnKeyPress(e);
        }

        /// <summary>
        /// System.Windows.Forms.Control.GotFocus イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.EventArgs。</param>
        protected override void OnGotFocus(EventArgs e)
        {
            this.Refresh();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// System.Windows.Forms.Control.LostFocus イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.EventArgs。</param>
        protected override void OnLostFocus(EventArgs e)
        {
            this.Refresh();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// System.Windows.Forms.Control.MouseEnter イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.EventArgs。</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.Enabled)
            {
                this.state = State.Hover;
            }

            this.Refresh();

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// System.Windows.Forms.Control.MouseLeave イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.Enabled)
            {
                this.state = State.Normal;
            }

            this.Refresh();

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// System.Windows.Forms.Control.MouseDown イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                this.state = State.Pushed;
            }

            this.Refresh();

            base.OnMouseDown(e);
        }

        /// <summary>
        /// System.Windows.Forms.Control.MouseUp イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                if (this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
                {
                    this.state = State.Hover;
                }
                else
                {
                    this.state = State.Normal;
                }
            }

            this.Refresh();

            base.OnMouseUp(e);
        }

        /// <summary>
        /// System.Windows.Forms.Control.EnabledChanged イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.EventArgs。</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this.Enabled)
            {
                this.state = State.Normal;
            }
            else
            {
                this.state = State.Disabled;
            }

            this.Refresh();

            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Draws the light-blue rectangle around the button when it is focused (by Tab for example)
        /// </summary>
        /// <param name="g">Graphics</param>
        private void DrawHighlight(Graphics g)
        {
            //The outline is drawn inside the button
            GraphicsPath innerRegion = CommandLink.RoundedRect(this.Width - 3, this.Height - 3, 3);

            //----Shift the inner region inwards
            Matrix translate = new Matrix();
            translate.Translate(1, 1);
            innerRegion.Transform(translate);
            translate.Dispose();
            
            //-----
            Pen inlinePen = new Pen(Color.FromArgb(192, 233, 243)); //Light-blue

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawPath(inlinePen, innerRegion);

            //Clean-up
            inlinePen.Dispose();
            innerRegion.Dispose();
        }

        /// <summary>
        /// Draws the button when the mouse is over it
        /// </summary>
        /// <param name="g">Graphics</param>
        private void DrawHoverState(Graphics g)
        {
            GraphicsPath outerRegion = CommandLink.RoundedRect(this.Width - 1, this.Height - 1, 3);
            GraphicsPath innerRegion = CommandLink.RoundedRect(this.Width - 3, this.Height - 3, 2);
            //----Shift the inner region inwards
            Matrix translate = new Matrix();
            translate.Translate(1, 1);
            innerRegion.Transform(translate);
            translate.Dispose();
            //-----
            Rectangle backgroundRect = new Rectangle(1, 1, this.Width - 2, (int)(this.Height * 0.75f) - 2);

            Pen outlinePen = new Pen(Color.FromArgb(189, 189, 189)); //SystemColors.ControlDark
            Pen inlinePen = new Pen(Color.FromArgb(245, 255, 255, 255)); //Slightly transparent white

            //Gradient brush for the background, goes from white to transparent 75% of the way down
            LinearGradientBrush backBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, backgroundRect.Height), Color.White, Color.Transparent);
            backBrush.WrapMode = WrapMode.TileFlipX; //keeps the gradient smooth incase of the glitch where there's an extra gradient line

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillRectangle(backBrush, backgroundRect);
            g.DrawPath(inlinePen, innerRegion);
            g.DrawPath(outlinePen, outerRegion);

            //Text/Image
            this.offset = 0; //Text/Image doesn't move
            this.DrawForeground(g);

            //Clean up
            outlinePen.Dispose();
            inlinePen.Dispose();
            outerRegion.Dispose();
            innerRegion.Dispose();
        }

        /// <summary>
        /// Draws the button when it's clicked down
        /// </summary>
        /// <param name="g">Graphics</param>
        private void DrawPushedState(Graphics g)
        {
            GraphicsPath outerRegion = CommandLink.RoundedRect(this.Width - 1, this.Height - 1, 3);
            GraphicsPath innerRegion = CommandLink.RoundedRect(this.Width - 3, this.Height - 3, 2);
            //----Shift the inner region inwards
            Matrix translate = new Matrix();
            translate.Translate(1, 1);
            innerRegion.Transform(translate);
            translate.Dispose();
            //-----
            Rectangle backgroundRect = new Rectangle(1, 1, this.Width - 3, this.Height - 3);

            Pen outlinePen = new Pen(Color.FromArgb(167, 167, 167)); //Outline is darker than normal
            Pen inlinePen = new Pen(Color.FromArgb(227, 227, 227)); //Darker white
            SolidBrush backBrush = new SolidBrush(Color.FromArgb(234, 234, 234)); //SystemColors.ControlLight

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillRectangle(backBrush, backgroundRect);
            g.DrawPath(inlinePen, innerRegion);
            g.DrawPath(outlinePen, outerRegion);

            //Text/Image
            this.offset = 1; //moves image inwards 1 pixel (x and y) to create the illusion that the button was pushed
            this.DrawForeground(g);

            //Clean up
            outlinePen.Dispose();
            inlinePen.Dispose();
            outerRegion.Dispose();
            innerRegion.Dispose();
        }

        /// <summary>
        /// Draws the button in it's regular state
        /// </summary>
        /// <param name="g">Graphics</param>
        private void DrawNormalState(Graphics g)
        {
            //Nothing needs to be drawn but the text and image

            //Text/Image
            this.offset = 0; //Text/Image doesn't move
            this.DrawForeground(g);
        }

        /// <summary>
        /// Draws Text and Image
        /// </summary>
        /// <param name="g">Graphics</param>
        private void DrawForeground(Graphics g)
        {
            //Make sure drawing is of good quality
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //Coordinates-------------------------------
            int imageLeft = 9;
            int imageTop = 0;
            int textLeft = 20;
            if (this.image != null)
            {
                textLeft = imageLeft + this.imageSize.Width + 5; //adjust the text left coordinate to accomodate the image
            }

            //
            //Text Layout-------------------------------
            string wrappedDescriptText = CommandLink.WordWrap(this.descriptionText, this.descriptFont, this.Width - (textLeft + this.offset) - 5);

            //Gets the width/height of the text once it's drawn out
            SizeF headerLayout = g.MeasureString(this.headerText, this.Font);
            SizeF descriptLayout = g.MeasureString(wrappedDescriptText, this.descriptFont);

            //Merge the two sizes into one big rectangle
            Rectangle totalRect = new Rectangle(0, 0, (int)Math.Max(headerLayout.Width, descriptLayout.Width), (int)(headerLayout.Height + descriptLayout.Height) - 4);

            //Align the text rectangle
            totalRect.X = textLeft;

            switch (this.textAlign)
            {
                case VerticalAlign.Top:
                    totalRect.Y = 4;
                    break;
                case VerticalAlign.Middle:
                    totalRect.Y = (this.Height / 2) - (totalRect.Height / 2);
                    break;
                case VerticalAlign.Bottom:
                    totalRect.Y = this.Height - totalRect.Height;
                    break;
                default:
                    break;
            }
            //---------------------------------------------------

            //Align the top of the image---------------------
            if (this.image != null)
            {
                switch (this.imageAlign)
                {
                    case VerticalAlign.Top:
                        imageTop = 4;
                        break;
                    case VerticalAlign.Middle:
                        imageTop = this.imageSize.Height / 2;
                        break;
                    case VerticalAlign.Bottom:
                        imageTop = this.Height - this.imageSize.Height;
                        break;
                    default:
                        break;
                }
            }
            //-----------------------------------------------

            //Brushes--------------------------------
            // Determine text color depending on whether the control is enabled or not
            Color textColor = this.ForeColor;
            if (!this.Enabled)
            {
                textColor = SystemColors.GrayText;
            }

            SolidBrush textBrush = new SolidBrush(textColor);
            
            //------------------------------------------
            g.DrawString(this.headerText, this.Font, textBrush, totalRect.Left + this.offset, totalRect.Top + this.offset);
            //Note: the + 1 in "totalRect.Left + 1 + offset" compensates for GDI+ inconsistency
            g.DrawString(
                wrappedDescriptText,
                this.descriptFont,
                textBrush,
                totalRect.Left + 1 + this.offset,
                totalRect.Bottom - (int)descriptLayout.Height + this.offset);

            if (this.image != null)
            {
                if (this.Enabled)
                {
                    g.DrawImage(this.image, new Rectangle(imageLeft + this.offset, imageTop + this.offset, this.imageSize.Width, this.imageSize.Height));
                }
                else
                {
                    //make sure there is a gray-image
                    if (this.grayImage == null)
                    {
                        this.grayImage = CommandLink.GetGrayscale(this.image); //generate grayscale now
                    }

                    g.DrawImage(this.grayImage, new Rectangle(imageLeft + this.offset, imageTop + this.offset, this.imageSize.Width, this.imageSize.Height));
                }
            }

            //Clean-up
            textBrush.Dispose();
        }

        /// <summary>
        /// Click イベントを発生させます。
        /// </summary>
        public void PerformClick()
        {
            this.OnClick(null);
        }
    }
}
