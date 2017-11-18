namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    #endregion

    [ToolboxItem(false)]
    [Description("The Visual Container Component")]
    public class VisualContainer : ToolStripDropDown
    {
        #region Variables

        private bool _fade;
        private int _frames;
        private int _totalDuration;
        private Control _userControl;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualContainer" /> class.</summary>
        /// <param name="contextControl">The context control to display.</param>
        public VisualContainer(Control contextControl) : this()
        {
            if (contextControl != null)
            {
                _userControl = contextControl;
            }
            else
            {
                throw new ArgumentNullException("No context control to load." + nameof(contextControl));
            }

            ToolStripControlHost controlHost = new ToolStripControlHost(contextControl)
                {
                    AutoSize = false
                };

            Padding = Margin = controlHost.Padding = controlHost.Margin = Padding.Empty;
            contextControl.Location = Point.Empty;
            Items.Add(controlHost);
            contextControl.Disposed += delegate
                {
                    contextControl = null;
                    Dispose(true);
                };
        }

        /// <summary>Prevents a default instance of the <see cref="VisualContainer" /> class from being created.</summary>
        private VisualContainer()
        {
            _fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;
            _frames = 5;
            _totalDuration = 100;
        }

        #endregion

        #region Properties

        public int Frames
        {
            get
            {
                return _frames;
            }

            set
            {
                _frames = value;
            }
        }

        public int TotalDuration
        {
            get
            {
                return _totalDuration;
            }

            set
            {
                _totalDuration = value;
            }
        }

        #endregion

        #region Events

        /// <summary>Displays a VisualContainer as a context menu of the control.</summary>
        /// <param name="control">The control.</param>
        public void Show(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            Show(control, control.ClientRectangle);
        }

        /// <summary>Displays a VisualContainer as a context menu of the control.</summary>
        /// <param name="form">The form.</param>
        /// <param name="point">The point.</param>
        public void Show(Form form, Point point)
        {
            Show(form, new Rectangle(point, new Size(0, 0)));
        }

        protected override void OnOpened(EventArgs e)
        {
            _userControl.Focus();
            base.OnOpened(e);
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if (_userControl.IsDisposed || _userControl.Disposing)
            {
                e.Cancel = true;
                return;
            }

            base.OnOpening(e);
        }

        /// <summary>Prevent ALT from closing it and allow ALT + MNEMONIC to work.</summary>
        /// <param name="keyData">The key data.</param>
        /// <returns>The <see cref="bool" />.</returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            return ((keyData & Keys.Alt) != Keys.Alt) && base.ProcessDialogKey(keyData);
        }

        /// <summary>Set the visible core toggle.</summary>
        /// <param name="visible">The visible.</param>
        protected override void SetVisibleCore(bool visible)
        {
            double opacity = Opacity;
            if (visible && _fade)
            {
                Opacity = 0;
            }

            base.SetVisibleCore(visible);
            if (!visible || !_fade)
            {
                return;
            }

            for (var i = 1; i <= _frames; i++)
            {
                if (i > 1)
                {
                    // The frame duration to sleep.
                    Thread.Sleep(_totalDuration / _frames);
                }

                Opacity = (opacity * i) / _frames;
            }

            Opacity = opacity;
        }

        /// <summary>Displays a VisualContainer as a context menu of the control.</summary>
        /// <param name="control">The control.</param>
        /// <param name="area">The area.</param>
        private void Show(Control control, Rectangle area)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
            Rectangle screen = Screen.FromControl(control).WorkingArea;

            if (location.X + Size.Width > screen.Left + screen.Width)
            {
                location.X = (screen.Left + screen.Width) - Size.Width;
            }

            if (location.Y + Size.Height > screen.Top + screen.Height)
            {
                location.Y -= Size.Height + area.Height;
            }

            location = control.PointToClient(location);

            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        #endregion
    }
}