namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Styles;

    #endregion

    [ToolboxItem(true)]
    [Description("The VisualPlus style manager component enables you to change the control themes.")]
    public class VisualStyleManager : Component
    {
        #region Variables

        private Styles _style;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualStyleManager" /> class.</summary>
        /// <param name="container">The container.</param>
        public VisualStyleManager(IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualStyleManager" /> class.</summary>
        /// <param name="style">The style.</param>
        public VisualStyleManager(Styles style) : this()
        {
            UpdateStyle(style);
        }

        /// <summary>Prevents a default instance of the <see cref="VisualStyleManager" /> class from being created.</summary>
        private VisualStyleManager()
        {
            DefaultStyle = Settings.DefaultValue.DefaultStyle;
            _style = DefaultStyle;

            UpdateStyle(_style);
        }

        public event ThemeChangedEventHandler ThemeChanged;

        #endregion

        #region Properties

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IBorder BorderStyle { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ICheckmark CheckmarkStyle { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IControlState ControlStatesStyle { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IControl ControlStyle { get; set; }

        public Styles DefaultStyle { get; }

        public Font Font { get; private set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IFont FontStyle { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IProgress ProgressStyle { get; set; }

        public Styles Style
        {
            get
            {
                return _style;
            }

            set
            {
                _style = value;
                UpdateStyle(_style);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ITab TabStyle { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IWatermark WatermarkStyle { get; set; }

        #endregion

        #region Events

        protected virtual void OnThemeChanged(ThemeEventArgs e)
        {
            ThemeChanged?.Invoke(e);
        }

        /// <summary>RGets the style object.</summary>
        /// <param name="styles">The Style.</param>
        /// <returns>The interface style.</returns>
        private static object GetStyleObject(Styles styles)
        {
            object interfaceObject;

            switch (styles)
            {
                case Styles.Visual:
                    {
                        interfaceObject = new Visual();
                        break;
                    }

                case Styles.Enigma:
                    {
                        interfaceObject = new Enigma();
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            return interfaceObject;
        }

        /// <summary>Updates the style.</summary>
        /// <param name="style">The style.</param>
        private void UpdateStyle(Styles style)
        {
            BorderStyle = (IBorder)GetStyleObject(style);
            CheckmarkStyle = (ICheckmark)GetStyleObject(style);
            ControlStatesStyle = (IControlState)GetStyleObject(style);
            ControlStyle = (IControl)GetStyleObject(style);
            FontStyle = (IFont)GetStyleObject(style);
            ProgressStyle = (IProgress)GetStyleObject(style);
            TabStyle = (ITab)GetStyleObject(style);
            WatermarkStyle = (IWatermark)GetStyleObject(style);

            Font = new Font(FontStyle.FontFamily, FontStyle.FontSize, FontStyle.FontStyle);

            OnThemeChanged(new ThemeEventArgs(_style));
        }

        #endregion
    }
}