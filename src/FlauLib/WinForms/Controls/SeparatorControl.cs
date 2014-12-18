using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FlauLib.WinForms.Controls
{
    /// <summary>
    /// A visible separator control
    /// </summary>
    public class SeparatorControl : Control
    {
        private readonly Pen _penGray;
        private readonly Pen _penWhite;
        private LineMode _lineMode;

        // Enums
        public enum LineMode
        {
            Down,
            Up,
            Left,
            Right
        }

        public int LineLength { get; set; }

        [RefreshProperties(RefreshProperties.Repaint)]
        public LineMode Mode
        {
            get { return _lineMode; }
            set
            {
                _lineMode = value;
                Anchor = Anchor;
            }
        }

        [DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [DefaultValue(""), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        public new AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set
            {
                var oldTop = (base.Anchor & AnchorStyles.Top) == AnchorStyles.Top;
                var oldBottom = (base.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom;
                var oldLeft = (base.Anchor & AnchorStyles.Left) == AnchorStyles.Left;
                var oldRight = (base.Anchor & AnchorStyles.Right) == AnchorStyles.Right;

                var newTop = (value & AnchorStyles.Top) == AnchorStyles.Top;
                var newBottom = (value & AnchorStyles.Bottom) == AnchorStyles.Bottom;
                var newLeft = (value & AnchorStyles.Left) == AnchorStyles.Left;
                var newRight = (value & AnchorStyles.Right) == AnchorStyles.Right;

                if (_lineMode == LineMode.Down || _lineMode == LineMode.Up)
                {
                    if (newTop && newBottom)
                    {
                        if (oldTop)
                        {
                            value &= oldBottom ? ~AnchorStyles.Bottom : ~AnchorStyles.Top;
                        }
                        else
                        {
                            value &= ~AnchorStyles.Bottom;
                        }
                    }
                }

                if (_lineMode == LineMode.Left || _lineMode == LineMode.Right)
                {
                    if (newLeft && newRight)
                    {
                        if (oldLeft)
                        {
                            value &= oldRight ? ~AnchorStyles.Right : ~AnchorStyles.Left;
                        }
                        else
                        {
                            value &= ~AnchorStyles.Right;
                        }
                    }
                }

                base.Anchor = value;
            }
        }

        public SeparatorControl()
        {
            TabStop = false;
            LineLength = 40;
            _lineMode = LineMode.Down;
            _penGray = new Pen(SystemColors.ControlDark, 1);
            _penWhite = new Pen(Color.White, 1);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (_lineMode == LineMode.Down || _lineMode == LineMode.Up)
            {
                LineLength = Width;
            }
            else
            {
                LineLength = Height;
            }
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            if (Visible)
            {
                if (_lineMode == LineMode.Down)
                {
                    e.Graphics.DrawLine(_penGray, new Point(0, 0), new Point(LineLength, 0));
                    e.Graphics.DrawLine(_penWhite, new Point(0, 0 + 1), new Point(LineLength, 0 + 1));
                    Size = new Size(LineLength, 2);
                }
                else if (_lineMode == LineMode.Up)
                {
                    e.Graphics.DrawLine(_penGray, new Point(0, 0 + 1), new Point(LineLength, 0 + 1));
                    e.Graphics.DrawLine(_penWhite, new Point(0, 0), new Point(LineLength, 0));
                    Size = new Size(LineLength, 2);
                }
                else if (_lineMode == LineMode.Left)
                {
                    e.Graphics.DrawLine(_penGray, new Point(0, 0), new Point(0, LineLength));
                    e.Graphics.DrawLine(_penWhite, new Point(0 + 1, 0), new Point(0 + 1, LineLength));
                    Size = new Size(2, LineLength);
                }
                else if (_lineMode == LineMode.Right)
                {
                    e.Graphics.DrawLine(_penGray, new Point(0 + 1, 0), new Point(0 + 1, LineLength));
                    e.Graphics.DrawLine(_penWhite, new Point(0, 0), new Point(0, LineLength));
                    Size = new Size(2, LineLength);
                }
            }
            base.OnPaint(e);
        }
    }
}
