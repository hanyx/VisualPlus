﻿namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.Enumerators;
    using VisualPlus.Localization;
    using VisualPlus.Managers;
    using VisualPlus.Properties;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    /// <summary>The <see cref="ControlBoxButton" />.</summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Description("The Control Box Button")]
    [Designer(typeof(VisualControlBoxButtonDesigner))]
    [DesignerCategory("code")]
    [ToolboxItem(false)]
    public class ControlBoxButton : VisualStyleBase
    {
        #region Variables

        private ControlColorState _backColorState;
        private ControlBoxType _boxType;
        private ControlColorState _foreColorState;
        private Image _image;
        private Point _offsetLocation;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ControlBoxButton" /> class.</summary>
        public ControlBoxButton()
        {
            _backColorState = new ControlColorState();
            _boxType = ControlBoxType.Default;
            _foreColorState = new ControlColorState();
            _image = Resources.VisualPlus;
            MouseState = MouseStates.Normal;
            _offsetLocation = new Point(0, 0);
        }

        public enum ControlBoxType
        {
            /// <summary>The default.</summary>
            Default,

            /// <summary>The image.</summary>
            Image,

            /// <summary>The text.</summary>
            Text
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState BackColorState
        {
            get
            {
                return _backColorState;
            }

            set
            {
                if (value == _backColorState)
                {
                    return;
                }

                _backColorState = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Type)]
        public ControlBoxType BoxType
        {
            get
            {
                return _boxType;
            }

            set
            {
                _boxType = value;
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState ForeColorState
        {
            get
            {
                return _foreColorState;
            }

            set
            {
                if (value == _foreColorState)
                {
                    return;
                }

                _foreColorState = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Image)]
        public Image Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Point)]
        public Point OffsetLocation
        {
            get
            {
                return _offsetLocation;
            }

            set
            {
                _offsetLocation = value;
            }
        }

        #endregion

        #region Events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseState = MouseStates.Down;
            Focus();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor = Cursors.Default;
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Enabled)
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Default;
            }

            MouseState = MouseStates.Hover;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics _graphics = e.Graphics;
            _graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            try
            {
                Size _stringSize;

                Color _backColor = ControlColorState.BackColorState(_backColorState, Enabled, MouseState);
                Color _foreColor = ControlColorState.BackColorState(_foreColorState, Enabled, MouseState);

                _graphics.FillRectangle(new SolidBrush(_backColor), ClientRectangle);

                switch (_boxType)
                {
                    case ControlBoxType.Default:
                        {
                            Font _specialFont = new Font("Marlett", 12);
                            _stringSize = GraphicsManager.MeasureText(_graphics, Text, _specialFont);
                            Point _location = new Point(((Width / 2) - (_stringSize.Width / 2)) + _offsetLocation.X, ((Height / 2) - (_stringSize.Height / 2)) + _offsetLocation.Y);

                            _graphics.DrawString(Text, _specialFont, new SolidBrush(_foreColor), _location);
                            break;
                        }

                    case ControlBoxType.Image:
                        {
                            Point _location = new Point(((Width / 2) - (_image.Width / 2)) + _offsetLocation.X, ((Height / 2) - (_image.Height / 2)) + _offsetLocation.Y);
                            _graphics.DrawImage(_image, _location);
                            break;
                        }

                    case ControlBoxType.Text:
                        {
                            _stringSize = GraphicsManager.MeasureText(_graphics, Text, Font);
                            Point _location = new Point(((Width / 2) - (_stringSize.Width / 2)) + _offsetLocation.X, ((Height / 2) - (_stringSize.Height / 2)) + _offsetLocation.Y);

                            _graphics.DrawString(Text, Font, new SolidBrush(_foreColor), _location);
                            break;
                        }

                    default:
                        {
                            throw new ArgumentOutOfRangeException(nameof(_boxType), _boxType, null);
                        }
                }
            }
            catch (Exception exception)
            {
                VisualExceptionDialog.Show(exception);
            }
        }

        #endregion
    }
}