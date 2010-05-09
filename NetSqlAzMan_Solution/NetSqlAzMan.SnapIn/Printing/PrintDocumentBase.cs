using System.Drawing;
using System.Drawing.Printing;

namespace NetSqlAzMan.SnapIn.Printing
{
    /// <summary>
    /// NetSqlAzMan Print Document Base class.
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public class PrintDocumentBase : PrintDocument
    {
        #region Fields
        protected Font font;
        protected Brush brush;
        protected Pen pen;
        protected float spaceBetweenLines;
        protected float currentX;
        protected float currentY;
        protected internal float top, left, right, bottom;
        protected Image topIcon;
        protected string title;
        protected readonly float margin;
        protected int pageIndex;
        protected float lineFrom;
        protected float lineTo;
        protected float lastX;
        protected float lastY;
        #endregion Fields
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PrintDocumentBase"/> class.
        /// </summary>
        public PrintDocumentBase()
        {
            this.currentX = 0.0F;
            this.font = new Font("Courier New", 8.0F);
            this.brush = Brushes.Black;
            this.pen = new Pen(this.brush);
            this.spaceBetweenLines = 0.0F;
            this.topIcon = Properties.Resources.NetSqlAzMan_16x16.ToBitmap();
            this.title = string.Empty;
            this.pageIndex = 0;
            this.margin = 15.0F;
            this.lineFrom = 0.0F;
            this.lineTo = 0.0F;
            this.lastX = 0.0F;
            this.lastY = 0.0F;
        }
        #endregion Constructors
        #region Properties
        /// <summary>
        /// Gets or sets the top icon.
        /// </summary>
        /// <value>The top icon.</value>
        public Image TopIcon
        {
            get
            {
                return this.topIcon;
            }
            set
            {
                this.topIcon = value;
            }
        }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [bold on].
        /// </summary>
        /// <value><c>true</c> if [bold on]; otherwise, <c>false</c>.</value>
        public bool BoldOn
        {
            get
            {
                return this.font.Style == FontStyle.Bold;
            }
            set
            {
                if (value)
                {
                    this.font = new Font(this.font, FontStyle.Bold);
                }
                else
                {
                    this.font = new Font(this.font, FontStyle.Regular);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [italic on].
        /// </summary>
        /// <value><c>true</c> if [italic on]; otherwise, <c>false</c>.</value>
        public bool ItalicOn
        {
            get
            {
                return this.font.Style == FontStyle.Italic;
            }
            set
            {
                if (value)
                {
                    this.font = new Font(this.font, FontStyle.Italic);
                }
                else
                {
                    this.font = new Font(this.font, FontStyle.Regular);
                }
            }
        }
        #endregion Properties
        #region Methods
        /// <summary>
        /// Writes the line string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        public void WriteLineString(string text, PrintPageEventArgs e)
        {
            SizeF size = e.Graphics.MeasureString(text, this.font);
            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoClip;
            sf.Trimming = StringTrimming.Word;
            RectangleF rect = new RectangleF(this.currentX, this.currentY, e.PageBounds.Width - this.currentX - e.PageBounds.Left, e.PageBounds.Height - e.PageBounds.Top);
            e.Graphics.DrawString(text, this.font, this.brush, rect,sf);
            this.currentY += this.meauseMultiLines(text, this.font, rect, sf, e) + this.spaceBetweenLines;
            this.lastX = this.currentX;
            this.lastY = this.currentY;
            this.currentX = this.left;
        }

        /// <summary>
        /// Writes the line string.
        /// </summary>
        /// <param name="beforeText">The before text.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="text">The text.</param>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        public void WriteLineString(string beforeText, Icon icon, string text, PrintPageEventArgs e)
        {
            Rectangle rect = new Rectangle(new Point((int)(e.Graphics.MeasureString(beforeText, this.font).Width), (int)this.currentY), icon.Size);
            this.lastX = e.Graphics.MeasureString(beforeText, this.font).Width;
            this.lastY = this.currentY;
            e.Graphics.DrawImageUnscaled(icon.ToBitmap(), rect);
            this.currentX = rect.Left + rect.Width + 3;
            this.currentY += rect.Height - e.Graphics.MeasureString(text, this.font).Height;
            SizeF size = e.Graphics.MeasureString(text, this.font);
            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoClip;
            sf.Trimming = StringTrimming.Word;
            RectangleF rect2 = new RectangleF(this.currentX, this.currentY, e.PageBounds.Width - this.currentX - e.PageBounds.Left - icon.Size.Width, e.PageBounds.Height - e.PageBounds.Top);
            e.Graphics.DrawString(text, this.font, this.brush, rect2, sf);
            this.currentY += this.meauseMultiLines(text, this.font, rect2, sf, e) + this.spaceBetweenLines;
            this.currentX = this.left;
        }

        /// <summary>
        /// Writes the line string.
        /// </summary>
        /// <param name="beforeText">The before text.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="text">The text.</param>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        public void WriteLineString(string beforeText, Image image, string text, PrintPageEventArgs e)
        {
            Rectangle rect = new Rectangle(new Point((int)(e.Graphics.MeasureString(beforeText, this.font).Width), (int)this.currentY), image.Size);
            this.lastX = e.Graphics.MeasureString(beforeText, this.font).Width;
            this.lastY = this.currentY;
            e.Graphics.DrawImageUnscaled(image, rect);
            this.currentX = rect.Left + rect.Width + 3;
            this.currentY += rect.Height - e.Graphics.MeasureString(text, this.font).Height;
            SizeF size = e.Graphics.MeasureString(text, this.font);
            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoClip;
            sf.Trimming = StringTrimming.Word;
            RectangleF rect2 = new RectangleF(this.currentX, this.currentY, e.PageBounds.Width - this.currentX - e.PageBounds.Left - image.Size.Width, e.PageBounds.Height - e.PageBounds.Top);
            e.Graphics.DrawString(text, this.font, this.brush, rect2, sf);
            this.currentY += this.meauseMultiLines(text, this.font, rect2, sf, e) + this.spaceBetweenLines;
            this.currentX = this.left;
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void WriteString(string text, PrintPageEventArgs e, float x, float y)
        {
            SizeF size = e.Graphics.MeasureString(text, this.font);
            e.Graphics.DrawString(text, this.font, this.brush, new PointF(x, y));
            this.lastX = x;
            this.lastY = y;
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        public void WriteString(string text, PrintPageEventArgs e)
        {
            string partialText = text;
            SizeF size = e.Graphics.MeasureString(text, this.font);
            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoClip;
            sf.Trimming = StringTrimming.Word;
            RectangleF rect = new RectangleF(this.currentX, this.currentY, e.PageBounds.Width - this.currentX - e.PageBounds.Left, e.PageBounds.Height - e.PageBounds.Top);
            e.Graphics.DrawString(text, this.font, this.brush, rect);
            this.currentY += this.meauseMultiLines(text, this.font, rect, sf, e) + this.spaceBetweenLines;
            this.lastX = this.currentX;
            this.lastY = this.currentY;
            this.currentX += size.Width;
        }

        protected float meauseMultiLines(string text, Font font, RectangleF rectF, StringFormat stringFormat, PrintPageEventArgs e)
        {
            return e.Graphics.MeasureString(text, this.font, rectF.Size, stringFormat).Height;
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Drawing.Printing.PrintDocument.PrintPage"></see> event. It is called before a page prints.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs"></see> that contains the event data.</param>
        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);
            this.pageIndex++;
            this.top = e.PageBounds.Top + this.margin;
            this.left = e.PageBounds.Left + this.margin;
            this.right = e.PageBounds.Right - this.margin;
            this.bottom = e.PageBounds.Bottom - this.margin;
            if (this.currentX==0.0F)
                this.currentX = this.left;
            this.currentY = this.top;
            this.PrintHeader(e);
            bool morePages = this.PrintPageBody(e);
            this.PrintFooter(e);
            if (morePages)
            {
                e.HasMorePages = true;
                this.currentY = this.top;
            }
        }

        /// <summary>
        /// Prints the header.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        public virtual void PrintHeader(PrintPageEventArgs e)
        {
            float fontSize = this.font.Size;
            this.font = new Font(this.font.FontFamily, fontSize * 2);
            this.BoldOn = true;
            e.Graphics.DrawLine(this.pen, this.left, this.top, this.right, this.top);
            float titleWidth = e.Graphics.MeasureString(this.title, this.font).Width;
            float l = this.left + (this.right - this.left) / 2 - this.topIcon.Size.Width / 2 - titleWidth / 2 - 1.5F;
            Rectangle rect = new Rectangle(new Point((int)l, (int)(this.top + 3)),  this.topIcon.Size);
            e.Graphics.DrawImageUnscaled(this.topIcon, rect);
            this.WriteString(this.title, e, l + rect.Width + 3, rect.Top + rect.Height + 3 - e.Graphics.MeasureString(this.title, this.font).Height);
            this.font = new Font(this.font.FontFamily, fontSize);
            this.WriteString("# "+this.pageIndex.ToString(), e, this.right - e.Graphics.MeasureString("# "+this.pageIndex.ToString(), this.font).Width, rect.Top + rect.Height + 3 - e.Graphics.MeasureString(this.title, this.font).Height);
            e.Graphics.DrawLine(this.pen, this.left, this.currentY + rect.Height + 6, this.right, this.currentY + rect.Height + 6);
            this.currentY += rect.Height + 6;
            this.BoldOn = false;
        }

        /// <summary>
        /// Prints the footer.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        public virtual void PrintFooter(PrintPageEventArgs e)
        {
            this.ItalicOn = true;
            string footerLeft = ".NET Sql Authorization Manager";
            string footerRight = "http://netsqlazman.codeplex.com";
            Rectangle rect = new Rectangle(new Point((int)this.left, (int)(this.bottom - Properties.Resources.NetSqlAzMan_16x16.Size.Height - 3)),  Properties.Resources.NetSqlAzMan_16x16.Size);
            e.Graphics.DrawLine(this.pen, this.left, this.bottom - rect.Height - 6, this.right, this.bottom - rect.Height - 6);
            e.Graphics.DrawImageUnscaled(Properties.Resources.NetSqlAzMan_16x16.ToBitmap(), rect);
            this.WriteString(footerLeft, e, this.left + rect.Width, this.bottom - rect.Height);
            this.WriteString(footerRight, e, this.right - e.Graphics.MeasureString(footerRight, this.font).Width, this.bottom - rect.Height);
            e.Graphics.DrawLine(this.pen, this.left, this.bottom, this.right, this.bottom);
            this.ItalicOn = false;
        }
        /// <summary>
        /// Skips the lines.
        /// </summary>
        /// <param name="lines">The lines.</param>
        public void SkipLines(PrintPageEventArgs e, int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                this.currentY += e.Graphics.MeasureString("\r\n", this.font).Height + this.spaceBetweenLines;
            }
        }

        /// <summary>
        /// Lines from.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        public void LineFrom(float x1, float y1)
        {
            this.lineFrom = x1;
            this.lineTo = y1;
        }

        /// <summary>
        /// Lines to.
        /// </summary>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        /// <param name="pen">The pen.</param>
        public void LineTo(float x2, float y2, PrintPageEventArgs e, Pen pen)
        {
            e.Graphics.DrawLine(pen, this.lineFrom, this.lineTo, x2, y2);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:PrintDocumentBase"/> is End Of Page.
        /// </summary>
        /// <value><c>true</c> if EOP; otherwise, <c>false</c>.</value>
        protected bool EOP
        {
            get
            {
                return (this.currentY > this.bottom - 70);
            }
        }
        /// <summary>
        /// Prints the page body.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        protected virtual bool PrintPageBody(PrintPageEventArgs e)
        {
            return false;
        }
        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
            this.pageIndex = 0;
        }
        #endregion Methods
    }
}
