﻿namespace VisualPlus.Toolkit.Controls.Layout
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Orientation")]
    [Description("The Visual Separator")]
    [Designer(typeof(VisualProgressBarDesigner))]
    [ToolboxBitmap(typeof(VisualSeparator), "Resources.ToolboxBitmaps.VisualSeparator.bmp")]
    [ToolboxItem(true)]
    public class VisualSeparator : VisualStyleBase
    {
        #region Variables

        private Color _line;
        private Orientation _orientation;
        private Color _shadow;
        private bool _shadowVisible;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualSeparator" /> class.</summary>
        public VisualSeparator()
        {
            _orientation = Orientation.Horizontal;
            UpdateTheme(ThemeManager.Theme);
        }

        #endregion

        #region Properties

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color Line
        {
            get
            {
                return _line;
            }

            set
            {
                if (value == _line)
                {
                    return;
                }

                _line = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Orientation)]
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }

            set
            {
                _orientation = value;

                if (_orientation == Orientation.Horizontal)
                {
                    if (Width < Height)
                    {
                        int temp = Width;
                        Width = Height;
                        Height = temp;
                    }
                }
                else
                {
                    // Vertical
                    if (Width > Height)
                    {
                        int temp = Width;
                        Width = Height;
                        Height = temp;
                    }
                }

                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color Shadow
        {
            get
            {
                return _shadow;
            }

            set
            {
                if (value == _shadow)
                {
                    return;
                }

                _shadow = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool ShadowVisible
        {
            get
            {
                return _shadowVisible;
            }

            set
            {
                _shadowVisible = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Theme theme)
        {
            try
            {
                ForeColor = theme.TextSetting.Enabled;
                TextStyle.Enabled = theme.TextSetting.Enabled;
                TextStyle.Disabled = theme.TextSetting.Disabled;

                Font = theme.TextSetting.Font;

                _line = theme.OtherSettings.Line;
                _shadow = theme.OtherSettings.Shadow;
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            Invalidate();
            OnThemeChanged(new ThemeEventArgs(theme));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.TextRenderingHint = TextStyle.TextRenderingHint;

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1);
            _graphics.FillRectangle(new SolidBrush(BackColor), _clientRectangle);

            Point _linePosition;
            Size _lineSize;
            Point _shadowPosition;
            Size _shadowSize;

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    {
                        _linePosition = new Point(0, 1);
                        _lineSize = new Size(Width, 1);

                        _shadowPosition = new Point(0, 2);
                        _shadowSize = new Size(Width, 2);
                        break;
                    }

                case Orientation.Vertical:
                    {
                        _linePosition = new Point(1, 0);
                        _lineSize = new Size(1, Height);

                        _shadowPosition = new Point(2, 0);
                        _shadowSize = new Size(2, Height);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Rectangle _lineRectangle = new Rectangle(_linePosition, _lineSize);
            _graphics.DrawRectangle(new Pen(_line), _lineRectangle);

            if (_shadowVisible)
            {
                Rectangle _shadowRectangle = new Rectangle(_shadowPosition, _shadowSize);
                _graphics.DrawRectangle(new Pen(_shadow), _shadowRectangle);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_orientation == Orientation.Horizontal)
            {
                Height = 4;
            }
            else
            {
                Width = 4;
            }
        }

        #endregion
    }
}