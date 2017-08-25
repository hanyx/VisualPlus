#region Namespace

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VisualPlus.Delegates;
using VisualPlus.EventArgs;
using VisualPlus.Styles;
using VisualPlus.Toolkit.Controls.DataManagement;
using VisualPlus.Toolkit.Controls.DataVisualization;
using VisualPlus.Toolkit.Controls.Editors;
using VisualPlus.Toolkit.Controls.Interactivity;
using VisualPlus.Toolkit.Controls.Layout;
using VisualPlus.Toolkit.VisualBase;

#endregion

namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    #endregion

    [ToolboxItem(true)]
    [Description("The VisualPlus style manager component enables you to change the control themes.")]
    public class VisualStyleManager : Component
    {
        #region Variables

        private List<Form> _formCollection;
        private Enumerators.Styles _style;

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
        public VisualStyleManager(Enumerators.Styles style) : this()
        {
            UpdateStyle(style);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualStyleManager" /> class.</summary>
        /// <param name="form">The form.</param>
        public VisualStyleManager(Form form) : this()
        {
            AddFormToManage(form);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualStyleManager" /> class.</summary>
        /// <param name="form">The form.</param>
        /// <param name="style">The style.</param>
        public VisualStyleManager(Form form, Enumerators.Styles style) : this()
        {
            UpdateStyle(style);
            AddFormToManage(form);
        }

        /// <summary>Prevents a default instance of the <see cref="VisualStyleManager" /> class from being created.</summary>
        private VisualStyleManager()
        {
            DefaultStyle = Settings.DefaultValue.DefaultStyle;
            _formCollection = new List<Form>();
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
        public Styles.IControlState ControlStatesStyle { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IControl ControlStyle { get; set; }

        public Enumerators.Styles DefaultStyle { get; }

        public Font Font { get; private set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IFont FontStyle { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IProgress ProgressStyle { get; set; }

        public Enumerators.Styles Style
        {
            get { return _style; }

            set
            {
                _style = value;
                UpdateStyle(_style);
                UpdateCollectionStyle();
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

        /// <summary>Adds a form to the collection to manage.</summary>
        /// <param name="form">The form.</param>
        public void AddFormToManage(Form form)
        {
            if (!_formCollection.Contains(form))
            {
                _formCollection.Add(form);
            }

            UpdateCollectionStyle();
        }

        /// <summary>Sets the style of the control.</summary>
        /// <param name="control">The control to style.</param>
        /// <param name="style">The style.</param>
        public void SetStyle(VisualControlBase control, Enumerators.Styles style)
        {
            UpdateControl(control, style);
        }

        /// <summary>Gets the style object.</summary>
        /// <param name="styles">The Style.</param>
        /// <returns>The interface style.</returns>
        protected virtual object GetStyleObject(Enumerators.Styles styles)
        {
            object interfaceObject;

            switch (styles)
            {
                case Enumerators.Styles.Visual:
                {
                    interfaceObject = new Visual();
                    break;
                }

                case Enumerators.Styles.Enigma:
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

        protected virtual void OnThemeChanged(ThemeEventArgs e)
        {
            ThemeChanged?.Invoke(e);
        }

        /// <summary>Updates the controls style.</summary>
        /// <param name="_control">The control.</param>
        /// <param name="style">The style to update the controls with.</param>
        protected virtual void UpdateControl(Control _control, Enumerators.Styles style)
        {
            (_control as VisualListBox)?.UpdateTheme(style);

            (_control as VisualListView)?.UpdateTheme(style);

            (_control as VisualCircleProgressBar)?.UpdateTheme(style);

            (_control as VisualGauge)?.UpdateTheme(style);

            (_control as VisualProgressBar)?.UpdateTheme(style);

            (_control as VisualRichTextBox)?.UpdateTheme(style);

            (_control as VisualTextBox)?.UpdateTheme(style);

            (_control as VisualButton)?.UpdateTheme(style);

            (_control as VisualCheckBox)?.UpdateTheme(style);

            (_control as VisualComboBox)?.UpdateTheme(style);

            (_control as VisualLabel)?.UpdateTheme(style);

            (_control as VisualNumericUpDown)?.UpdateTheme(style);

            (_control as VisualRadioButton)?.UpdateTheme(style);

            (_control as VisualShape)?.UpdateTheme(style);

            (_control as VisualToggle)?.UpdateTheme(style);

            (_control as VisualTrackBar)?.UpdateTheme(style);

            (_control as VisualGroupBox)?.UpdateTheme(style);

            (_control as VisualPanel)?.UpdateTheme(style);

            (_control as VisualSeparator)?.UpdateTheme(style);
        }

        /// <summary>Update the form collection controls style.</summary>
        private void UpdateCollectionStyle()
        {
            if (_formCollection.Count == 0)
            {
                return;
            }

            foreach (Form _form in _formCollection)
            {
                UpdateForm(_form);
            }
        }

        /// <summary>Updates the form controls style.</summary>
        /// <param name="form">The form to search for controls.</param>
        private void UpdateForm(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            if (form.Controls.Count == 0)
            {
                return;
            }

            foreach (Control _control in form.Controls)
            {
                UpdateControl(_control, _style);
            }
        }

        /// <summary>Updates the style.</summary>
        /// <param name="style">The style.</param>
        private void UpdateStyle(Enumerators.Styles style)
        {
            BorderStyle = (IBorder) GetStyleObject(style);
            CheckmarkStyle = (ICheckmark) GetStyleObject(style);
            ControlStatesStyle = (Styles.IControlState) GetStyleObject(style);
            ControlStyle = (IControl) GetStyleObject(style);
            FontStyle = (IFont) GetStyleObject(style);
            ProgressStyle = (IProgress) GetStyleObject(style);
            TabStyle = (ITab) GetStyleObject(style);
            WatermarkStyle = (IWatermark) GetStyleObject(style);

            Font = new Font(FontStyle.FontFamily, FontStyle.FontSize, FontStyle.FontStyle);

            OnThemeChanged(new ThemeEventArgs(_style));
        }

        #endregion
    }
}