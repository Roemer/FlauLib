using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FlauLib.WinForms.Controls
{
    /// <summary>
    /// An extended split container with nicer graphics
    /// </summary>
    public class SplitContainerEx : SplitContainer
    {
        // Enums
        public enum LineMode
        {
            Hidden = 0,
            Normal = 1,
            Inverted = 2
        }

        public enum Panels
        {
            None = 0,
            Panel1 = 1,
            Panel2 = 2
        }

        private readonly Pen _penGray = new Pen(SystemColors.ControlDark, 1);
        private readonly Pen _penWhite = new Pen(Color.White, 1);
        private int _mouseDelta;
        private int _splitterDistanceBeforeCollapse;
        private int _panelSizeBeforeCollapse;
        private bool _alternativeCollapsed;

        // Properties
        [Browsable(true), Category("Extended Design"),
        Description("Maximum Size of the first Panel (0 for unlimited)")]
        public int Panel1MaxSize { get; set; }

        [Browsable(true), Category("Extended Design"),
       Description("Maximum Size of the second Panel (0 for unlimited)")]
        public int Panel2MaxSize { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("The Length of the Drag Lines (in Pixels)")]
        public int DragLineWidth { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("An Offset for the Drag Lines (in Pixels)")]
        public int DragLineOffset { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Drag Lines")]
        public LineMode DragLines { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Line on Top (Horizonal) or Left (Vertical)")]
        public LineMode TopLeftLine { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Line on Bottom (Horizonal) or Right (Vertical)")]
        public LineMode BottomRightLine { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Center Line")]
        public LineMode CenterLine { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("Alternative collapse panel")]
        public Panels AlternativeCollapsePanel { get; set; }

        [Browsable(true), Category("Extended Design"),
        Description("Alternatively collapsed by default")]
        public bool AlternativeCollapseDefault { get; set; }

        public SplitContainerEx()
        {
            CenterLine = LineMode.Hidden;
            BottomRightLine = LineMode.Normal;
            TopLeftLine = LineMode.Normal;
            DragLines = LineMode.Normal;
            DragLineWidth = 40;
            SplitterWidth = 20;

            SplitterMoved += SplitterMovedHandler;
            DoubleClick += SplitContainerEx_DoubleClick;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ContainerControl, true);
        }

        protected override void OnCreateControl()
        {
            if (AlternativeCollapseDefault)
            {
                AlternativeCollapse();
            }
            base.OnCreateControl();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_alternativeCollapsed)
            {
                UpdateAlternativeCollapseSplitter();
            }
        }

        public void AlternativeCollapseToggle()
        {
            if (_alternativeCollapsed)
            {
                AlternativeCollapseRestore();
            }
            else
            {
                AlternativeCollapse();
            }
        }

        public void AlternativeCollapseRestore()
        {
            if (!DesignMode)
            {
                SplitterDistance = _splitterDistanceBeforeCollapse;
                if (AlternativeCollapsePanel == Panels.Panel1)
                {
                    Panel1MinSize = _panelSizeBeforeCollapse;
                }
                else
                {
                    Panel2MinSize = _panelSizeBeforeCollapse;
                }
                _alternativeCollapsed = false;
            }
        }

        public void AlternativeCollapse()
        {
            if (!DesignMode)
            {
                _splitterDistanceBeforeCollapse = SplitterDistance;

                if (AlternativeCollapsePanel == Panels.Panel1)
                {
                    _panelSizeBeforeCollapse = Panel1MinSize;
                    Panel1MinSize = 0;
                }
                else
                {
                    _panelSizeBeforeCollapse = Panel2MinSize;
                    Panel2MinSize = 0;
                }

                UpdateAlternativeCollapseSplitter();

                _alternativeCollapsed = true;
            }
        }

        private void UpdateAlternativeCollapseSplitter()
        {
            if (!DesignMode)
            {
                if (AlternativeCollapsePanel == Panels.Panel1)
                {
                    SplitterDistance = 0;
                }
                else
                {
                    SplitterDistance = Orientation == Orientation.Horizontal ? Height : Width;
                }
            }
        }

        private void SplitContainerEx_DoubleClick(object sender, EventArgs e)
        {
            if (AlternativeCollapsePanel != Panels.None)
            {
                AlternativeCollapseToggle();
            }
        }

        private void SplitterMovedHandler(Object sender, SplitterEventArgs e)
        {
            SplitterDistance = SplitterDistance;
        }

        #region Drawing Stuff
        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.Clear(BackColor);

            // Get the current Splitter Rectangle
            var splitRect = SplitterRectangle;

            // Check Orientation
            if (Orientation == Orientation.Horizontal)
            {
                var centerStart = splitRect.Top + splitRect.Height / 2 - 1;
                // Center Line
                if (CenterLine != LineMode.Hidden)
                {
                    DrawLine(e.Graphics, splitRect.Left, centerStart, splitRect.Width, CenterLine);
                }

                // Top Left Line           
                if ((TopLeftLine != LineMode.Hidden) && (_alternativeCollapsed == false || AlternativeCollapsePanel == Panels.Panel2))
                {
                    DrawLine(e.Graphics, splitRect.Left, splitRect.Top, splitRect.Width, TopLeftLine);
                }

                // Bottom Right Line
                if ((BottomRightLine != LineMode.Hidden) && (_alternativeCollapsed == false || AlternativeCollapsePanel == Panels.Panel1))
                {
                    DrawLine(e.Graphics, splitRect.Left, splitRect.Bottom - 2, splitRect.Width, BottomRightLine);
                }

                // Drag Lines
                if (DragLines != LineMode.Hidden)
                {
                    // Calculate x-Position where to start
                    var xPos = splitRect.Width / 2 - DragLineWidth / 2;
                    for (var i = -1; i <= 1; i++)
                    {
                        DrawLine(e.Graphics, xPos, centerStart + DragLineOffset + i * 3, DragLineWidth, DragLines);
                    }
                }
            }
            else
            {
                var centerStart = splitRect.Left + splitRect.Width / 2 - 1;
                // Center Line
                if (CenterLine != LineMode.Hidden)
                {
                    DrawLine(e.Graphics, centerStart, splitRect.Top, splitRect.Height, CenterLine);
                }

                // Top Left Line
                if ((TopLeftLine != LineMode.Hidden) && (_alternativeCollapsed == false || AlternativeCollapsePanel == Panels.Panel2))
                {
                    DrawLine(e.Graphics, splitRect.Left, splitRect.Top, splitRect.Height, TopLeftLine);
                }

                // Bottom Right Line
                if ((BottomRightLine != LineMode.Hidden) && (_alternativeCollapsed == false || AlternativeCollapsePanel == Panels.Panel1))
                {
                    DrawLine(e.Graphics, splitRect.Right - 2, splitRect.Top, splitRect.Height, BottomRightLine);
                }

                // Drag Lines
                if (DragLines != LineMode.Hidden)
                {
                    // Calculate y-Position where to start
                    var yPos = splitRect.Height / 2 - DragLineWidth / 2;
                    for (var i = -1; i <= 1; i++)
                    {
                        DrawLine(e.Graphics, centerStart + DragLineOffset + i * 3, yPos, DragLineWidth, DragLines);
                    }
                }
            }

            if (Focused)
            {
                DrawFocus();
            }
        }
        private void DrawLine(Graphics g, int xFrom, int yFrom, int size, LineMode lm)
        {
            Pen pen1;
            Pen pen2;

            if (lm == LineMode.Normal)
            {
                pen1 = _penGray;
                pen2 = _penWhite;
            }
            else
            {
                pen1 = _penWhite;
                pen2 = _penGray;
            }

            if (Orientation == Orientation.Horizontal)
            {
                g.DrawLine(pen1, new Point(xFrom, yFrom), new Point(xFrom + size, yFrom));
                g.DrawLine(pen2, new Point(xFrom, yFrom + 1), new Point(xFrom + size, yFrom + 1));
            }
            else
            {
                g.DrawLine(pen1, new Point(xFrom, yFrom), new Point(xFrom, yFrom + size));
                g.DrawLine(pen2, new Point(xFrom + 1, yFrom), new Point(xFrom + 1, yFrom + size));
            }
        }
        #endregion

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!IsSplitterFixed)
            {
                if (e.KeyData == Keys.Right || e.KeyData == Keys.Down)
                {
                    SplitterDistance += 1;
                }
                else if (e.KeyData == Keys.Left || e.KeyData == Keys.Up)
                {
                    SplitterDistance -= 1;
                }
                Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            DrawFocus();
        }

        private void DrawFocus()
        {
            var r = SplitterRectangle;
            var g = Graphics.FromHwndInternal(Handle);
            r.Inflate(-1, -1);
            ControlPaint.DrawFocusRectangle(g, r, ForeColor, BackColor);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Orientation.Equals(Orientation.Vertical))
            {
                _mouseDelta = SplitterDistance - e.X;
                //Cursor.Current = System.Windows.Forms.Cursors.NoMoveHoriz;
                //Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                Cursor.Current = Cursors.VSplit;
            }
            else
            {
                _mouseDelta = SplitterDistance - e.Y;
                //Cursor.Current = System.Windows.Forms.Cursors.NoMoveVert;
                //Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                Cursor.Current = Cursors.HSplit;
            }

            // This disables the normal move behavior
            IsSplitterFixed = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _mouseDelta = 0;
            // This allows the splitter to be moved normally again
            IsSplitterFixed = false;

            Cursor.Current = Cursors.Default;

            if (Focused)
            {
                DrawFocus();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Check to make sure the splitter won't be updated by the normal move behavior also
            if (IsSplitterFixed)
            {
                // Make sure that the button used to move the splitter is the left mouse button
                if (e.Button.Equals(MouseButtons.Left))
                {
                    // Checks to see if the splitter is aligned Vertically
                    if (Orientation.Equals(Orientation.Vertical))
                    {
                        // Only move the splitter if the mouse is within the appropriate bounds
                        if (e.X > 0 && e.X < Width)
                        {
                            if (_alternativeCollapsed)
                            {
                                AlternativeCollapseRestore();
                            }

                            // Move the splitter
                            var newDistance = e.X + _mouseDelta;
                            SplitterDistance = (newDistance <= 0 ? 0 : newDistance);
                        }
                    }
                    // If it isn't aligned verically then it must be horizontal
                    else
                    {
                        // Only move the splitter if the mouse is within the appropriate bounds
                        if (e.Y > 0 && e.Y < Height)
                        {
                            if (_alternativeCollapsed)
                            {
                                AlternativeCollapseRestore();
                            }

                            // Move the splitter
                            var newDistance = e.Y + _mouseDelta;
                            SplitterDistance = (newDistance <= 0 ? 0 : newDistance);
                        }
                    }
                }
                // If a button other than left is pressed or no button at all
                else
                {
                    // This allows the splitter to be moved normally again
                    IsSplitterFixed = false;
                }

                Refresh();
            }
        }

        // Override SplitterDistance and addidionally Handle the MaxSize
        public new int SplitterDistance
        {
            get { return base.SplitterDistance; }
            set
            {
                if (Orientation == Orientation.Vertical)
                {
                    if (value > Panel1MaxSize && Panel1MaxSize != 0)
                    {
                        value = Panel1MaxSize;
                    }
                    if ((value + SplitterWidth) < (Width - Panel2MaxSize) && Panel2MaxSize != 0)
                    {
                        value = (Width - Panel2MaxSize) - SplitterWidth;
                    }

                }
                else
                {
                    if (value > Panel1MaxSize && Panel1MaxSize != 0)
                    {
                        value = Panel1MaxSize;
                    }
                    if ((value + SplitterWidth) < (Height - Panel2MaxSize) && Panel2MaxSize != 0)
                    {
                        value = (Height - Panel2MaxSize) - SplitterWidth;
                    }
                }
                base.SplitterDistance = value;
            }
        }
    }
}
