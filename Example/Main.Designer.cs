namespace Example
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            VisualPlus.Structure.Theme theme1 = new VisualPlus.Structure.Theme();
            VisualPlus.Structure.BackgroundSettings backgroundSettings1 = new VisualPlus.Structure.BackgroundSettings();
            VisualPlus.Structure.BorderSettings borderSettings1 = new VisualPlus.Structure.BorderSettings();
            VisualPlus.Structure.ControlColorStateSettings controlColorStateSettings1 = new VisualPlus.Structure.ControlColorStateSettings();
            VisualPlus.Structure.ThemeInformation themeInformation1 = new VisualPlus.Structure.ThemeInformation();
            VisualPlus.Structure.ListItemSettings listItemSettings1 = new VisualPlus.Structure.ListItemSettings();
            VisualPlus.Structure.OtherSettings otherSettings1 = new VisualPlus.Structure.OtherSettings();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            VisualPlus.Structure.TextSettings textSettings1 = new VisualPlus.Structure.TextSettings();
            this.stylesManager1 = new VisualPlus.Toolkit.Components.StylesManager(this.components);
            this.SuspendLayout();
            // 
            // stylesManager1
            // 
            this.stylesManager1.CustomThemePath = "C:\\Users\\Daniel\\AppData\\Roaming\\Microsoft\\Windows\\Templates\\VisualPlus Themes\\Def" +
    "aultTheme.xml";
            backgroundSettings1.Type1 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            backgroundSettings1.Type2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            backgroundSettings1.Type3 = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(64)))), ((int)(((byte)(65)))));
            backgroundSettings1.Type4 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            theme1.BackgroundSettings = backgroundSettings1;
            borderSettings1.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(183)))), ((int)(((byte)(230)))));
            borderSettings1.Normal = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            theme1.BorderSettings = borderSettings1;
            controlColorStateSettings1.Disabled = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            controlColorStateSettings1.Enabled = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            controlColorStateSettings1.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            controlColorStateSettings1.Pressed = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            theme1.ColorStateSettings = controlColorStateSettings1;
            themeInformation1.Author = "DarkByte7";
            themeInformation1.Name = "Visual";
            theme1.InformationSettings = themeInformation1;
            listItemSettings1.Item = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            listItemSettings1.ItemAlternate = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(196)))), ((int)(((byte)(13)))));
            listItemSettings1.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(183)))), ((int)(((byte)(230)))));
            listItemSettings1.ItemSelected = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(183)))), ((int)(((byte)(230)))));
            theme1.ListItemSettings = listItemSettings1;
            otherSettings1.BackCircle = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            otherSettings1.BoxDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            otherSettings1.BoxEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            otherSettings1.ColumnHeader = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            otherSettings1.ColumnText = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            otherSettings1.ControlDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            otherSettings1.ControlEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            otherSettings1.FlatControlDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(118)))));
            otherSettings1.FlatControlEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(118)))));
            otherSettings1.ForeCircle = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            otherSettings1.HatchBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            otherSettings1.HatchForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            otherSettings1.LightText = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            otherSettings1.Line = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(222)))), ((int)(((byte)(220)))));
            otherSettings1.Progress = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(136)))), ((int)(((byte)(45)))));
            otherSettings1.ProgressBackground = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(222)))), ((int)(((byte)(220)))));
            otherSettings1.ProgressDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(129)))), ((int)(((byte)(129)))));
            otherSettings1.Shadow = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            otherSettings1.TabPageDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(73)))));
            otherSettings1.TabPageEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(73)))));
            otherSettings1.TabPageHover = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            otherSettings1.TabPageSelected = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(76)))), ((int)(((byte)(88)))));
            otherSettings1.WatermarkActive = System.Drawing.Color.Empty;
            otherSettings1.WatermarkInactive = System.Drawing.Color.Empty;
            theme1.OtherSettings = otherSettings1;
            theme1.RawTheme = resources.GetString("theme1.RawTheme");
            textSettings1.Disabled = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(129)))), ((int)(((byte)(129)))));
            textSettings1.Enabled = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            textSettings1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            textSettings1.Hover = System.Drawing.Color.Empty;
            textSettings1.Selected = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            theme1.TextSetting = textSettings1;
            this.stylesManager1.Theme = theme1;
            this.stylesManager1.ThemeType = VisualPlus.Enumerators.Themes.Visual;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private VisualPlus.Toolkit.Components.StylesManager stylesManager1;
    }
}

