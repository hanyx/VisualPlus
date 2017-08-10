namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;

    using VisualPlus.Delegates;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization.Category;
    using VisualPlus.Styles;
    using VisualPlus.Toolkit.Components;

    using Property = VisualPlus.Localization.Descriptions.Property;

    #endregion

    [TypeConverter(typeof(BorderConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The border.")]
    public class Border : Shape, IBorder
    {
        #region Variables

        private Color hoverColor;
        private bool hoverVisible;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Border" /> class.</summary>
        public Border()
        {
            StyleManager styleManager = new StyleManager(Settings.DefaultValue.DefaultStyle);

            hoverColor = styleManager.BorderStyle.HoverColor;
            hoverVisible = true;
        }

        [Category(Event.PropertyChanged)]
        [Description("Occours when the hvoer color has been changed.")]
        public event BorderHoverColorChangedEventHandler HoverColorChanged;

        [Category(Event.PropertyChanged)]
        [Description("Occours when the hover visible has been changed.")]
        public event BorderHoverVisibleChangedEventHandler HoverVisibleChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Color)]
        public Color HoverColor
        {
            get
            {
                return hoverColor;
            }

            set
            {
                hoverColor = value;
                HoverColorChanged?.Invoke(new ColorEventArgs(hoverColor));
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Visible)]
        public bool HoverVisible
        {
            get
            {
                return hoverVisible;
            }

            set
            {
                hoverVisible = value;
                HoverVisibleChanged?.Invoke();
            }
        }

        #endregion
    }

    public class BorderConverter : ExpandableObjectConverter
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
                return new ObjectShape2Wrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Border border;
            object result;

            result = null;
            border = value as Border;

            if (border != null && destinationType == typeof(string))
            {
                // result = borderStyle.ToString();
                result = "Border Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(BorderConverter))]
    public class ObjectShape2Wrapper
    {
        #region Constructors

        public ObjectShape2Wrapper()
        {
        }

        public ObjectShape2Wrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}