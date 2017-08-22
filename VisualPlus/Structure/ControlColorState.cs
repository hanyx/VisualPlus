namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Components;

    #endregion

    [TypeConverter(typeof(ControlColorStateConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The control color state.")]
    public class ControlColorState
    {
        #region Variables

        private Color _color;
        private Color _disabled;
        private Color _hover;
        private Color _pressed;

        #endregion

        #region Constructors

        public ControlColorState()
        {
            VisualStyleManager _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);

            _color = _styleManager.ControlStyle.Background(0);
            _disabled = _styleManager.FontStyle.ForeColorDisabled;
            _hover = ControlPaint.Light(_styleManager.ControlStyle.Background(0));
            _pressed = ControlPaint.Light(_styleManager.ControlStyle.Background(0));
        }

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public Color Disabled
        {
            get
            {
                return _disabled;
            }

            set
            {
                _disabled = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public Color Hover
        {
            get
            {
                return _hover;
            }

            set
            {
                _hover = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public Color Pressed
        {
            get
            {
                return _pressed;
            }

            set
            {
                _pressed = value;
            }
        }

        #endregion
    }

    public class ControlColorStateConverter : ExpandableObjectConverter
    {
        #region Events

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;

            if (stringValue != null)
            {
                return new ControlColorStateWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            ControlColorState controlColorState;
            object result;

            result = null;
            controlColorState = value as ControlColorState;

            if (controlColorState != null && destinationType == typeof(string))
            {
                // result = borderStyle.ToString();
                result = "Color Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(ControlColorStateConverter))]
    public class ControlColorStateWrapper
    {
        #region Constructors

        public ControlColorStateWrapper()
        {
        }

        public ControlColorStateWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}