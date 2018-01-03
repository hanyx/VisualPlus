namespace VisualPlus.Toolkit.Dialogs
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    #endregion

    public class ThemesEditor : UITypeEditor
    {
        #region Events

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context == null) || (provider == null))
            {
                return base.EditValue(context, provider, value);
            }

            IWindowsFormsEditorService _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (_editorService == null)
            {
                return base.EditValue(context, provider, value);
            }

            OpenFileDialog _openFileDialog = new OpenFileDialog
                {
                    FileName = string.Empty,
                    Filter = @"Theme|*.xml"
                };

            return _openFileDialog.ShowDialog() == DialogResult.OK ? _openFileDialog.FileName : base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion
    }
}