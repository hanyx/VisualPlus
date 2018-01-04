using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example
{
    using VisualPlus.Enumerators;
    using VisualPlus.Structure;

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Theme _t = new Theme(Themes.Visual);
            
            _t.BorderSettings.Normal = Color.Red;
        }

        private void visualButton1_Click(object sender, EventArgs e)
        {
           var _path = stylesManager1.CustomThemePath = @"C:\Theme.xml";
            stylesManager1.SaveTheme(_path);
        }
    }
}
