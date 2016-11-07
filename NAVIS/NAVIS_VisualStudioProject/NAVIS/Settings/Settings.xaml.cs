/////////////////////////////////////////////////////////////////////////////////
//                                                                             //
//    Copyright © 2015  Juan P. Dominguez-Morales                              //
//                                                                             //        
//    This file is part of Neuromorphic Auditory Visualizer Tool (NAVIS Tool). //
//                                                                             //
//    NAVIS Tool is free software: you can redistribute it and/or modify       //
//    it under the terms of the GNU General Public License as published by     //
//    the Free Software Foundation, either version 3 of the License, or        //
//    (at your option) any later version.                                      //
//                                                                             //
//    NAVIS Tool is distributed in the hope that it will be useful,            //
//    but WITHOUT ANY WARRANTY; without even the implied warranty of           //
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the              //
//    GNU General Public License for more details.                             //
//                                                                             //
//    You should have received a copy of the GNU General Public License        //
//    along with NAVIS Tool.  If not, see <http://www.gnu.org/licenses/>.      //
//                                                                             //
/////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NAVIS;

namespace NAVIS.Settings
{
    /// <summary>
    /// Settings window. Allows the user to navigate trough the different settings and load, modify and save them.
    /// </summary>
    public partial class Settings : Window
    {
        public MainWindow mw;
        cSettings backupSettings;

        public Settings(MainWindow mainW)
        {
            InitializeComponent();
            mw = mainW;

            MainWindow.settings = new cSettings();

            updateFormFromSettings();

            bottomBar.settingsForm = this;
        }

        /// <summary>
        /// Update the Settings form with the values taken from the output file Settings.xml.
        /// </summary> 
        public void updateFormFromSettings()
        {
            mainSettings.updateFrom(MainWindow.settings.MainS);
            toolsSettings.updateFrom(MainWindow.settings.ToolsS);
            pdfSettings.updateFrom(MainWindow.settings.PdfS);
            toolbarSettings.updateFrom(MainWindow.settings.ToolbarS);

            backupSettings = (cSettings)MainWindow.settings.Clone();
        }

        /// <summary>
        /// Updates Settings.xml output file with the values taken from the Settings form.
        /// </summary> 
        public void updateSettingsFromForm()
        {
            MainWindow.settings.MainS = mainSettings.getFromForm();
            MainWindow.settings.ToolsS = toolsSettings.getFromForm();
            MainWindow.settings.PdfS = pdfSettings.getFromForm();
            MainWindow.settings.ToolbarS = toolbarSettings.getFromForm();

            if (backupSettings.MainS.dotSize != MainWindow.settings.MainS.dotSize || backupSettings.MainS.leftColor != MainWindow.settings.MainS.leftColor || backupSettings.MainS.rightColor != MainWindow.settings.MainS.rightColor || backupSettings.MainS.showChart != MainWindow.settings.MainS.showChart || backupSettings.MainS.dotsToPaint != MainWindow.settings.MainS.dotsToPaint || backupSettings.MainS.channels != MainWindow.settings.MainS.channels || backupSettings.MainS.mono_stereo != MainWindow.settings.MainS.mono_stereo)
            {
                mw.reloadCochcleogram();
                backupSettings = (cSettings)MainWindow.settings.Clone();
            }
        }

        /// <summary>
        /// Loads Config click event. Loads a Settings file from a path chosen by the user
        /// </summary>
        private void B_LoadConfig_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog selectFileDialog = new Microsoft.Win32.OpenFileDialog();
            selectFileDialog.Filter = "XML Files|*.xml";
            selectFileDialog.Title = "Select a XML settings file";

            if (selectFileDialog.ShowDialog() == true)
            {
                MainWindow.settings.loadSettingsFromFile(selectFileDialog.FileName);
                updateFormFromSettings();
            }
        }

        /// <summary>
        /// Reload to default click event. Restores settings values from the last time that they were saved.
        /// </summary>
        private void B_Reload_Default_Click(object sender, EventArgs e)
        {
            MainWindow.settings.loadSettingsFromFile();
            updateFormFromSettings();
        }

        /// <summary>
        /// Save as click event. Saves a settings xml file in a path chosen by the user
        /// </summary>
        private void B_Save_As_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "XML Files|*.xml";
            saveFileDialog.Title = "Save a XML settings File";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                updateSettingsFromForm();
                MainWindow.settings.saveSettingsToFile(saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// Save as default click event. Saves the current state of the settings in the default path.
        /// </summary>
        private void B_Save_As_Default_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Wait;
            updateSettingsFromForm();
            MainWindow.settings.saveSettingsToFile();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Changes the content of the tooltip that the Help button shows depending of which tab is selected.
        /// </summary> 
        private void tabControlConfig_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabItem_General.IsSelected)
            {
                bottomBar.popup_text.Text = "";
                bottomBar.popup_text.Inlines.Add("If ");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Show Cochleogram\"")));
                bottomBar.popup_text.Inlines.Add(" is checked, the cochleogram for the chosen aedat file is generated on load.\r");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Dot Size\"")));
                bottomBar.popup_text.Inlines.Add(" and ");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Events\"")));
                bottomBar.popup_text.Inlines.Add(" add some tweaks to the cochleogram representation. Lesser values on the first one and greater on the second will help reducing load times.\rWith ");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Address Length\"")));
                bottomBar.popup_text.Inlines.Add(" you can select if the aedat file that you want to load has 16 bits or 32 bits address length.\r");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Screen size\"")));
                bottomBar.popup_text.Inlines.Add(": width x height (pixels)");
                bottomBar.popup_text.TextWrapping = TextWrapping.Wrap;
            }
            else if (tabItem_Tools.IsSelected)
            {
                bottomBar.popup_text.Text = "";
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Image Size\"")));
                bottomBar.popup_text.Inlines.Add(" let you choose between four different sizes, which will be used to generate both \"Sonogram\" and \"Disparity between both cochleae\" tools.\r");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Integration Period\"")));
                bottomBar.popup_text.Inlines.Add(" is the smallest time period that this software will work with.\r");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Noise Threshold\"")));
                bottomBar.popup_text.Inlines.Add(" and ");
                bottomBar.popup_text.Inlines.Add(new Bold(new Run("\"Noise Tolerance\"")));
                bottomBar.popup_text.Inlines.Add(" are used for the Automatic Aedat Splitter. More info about them can be found in that window.");
            }
            else if (tabItem_PDF.IsSelected)
            {
                bottomBar.popup_text.Text = "";
                bottomBar.popup_text.Inlines.Add("Check or uncheck the elements that you want to see in the generated PDF using the \"Generate PDF\" report tool.");
                bottomBar.popup_text.TextWrapping = TextWrapping.Wrap;
            }
            else if (tabItem_Toolbar.IsSelected)
            {
                bottomBar.popup_text.Text = "";
                bottomBar.popup_text.Inlines.Add("Check or uncheck the quick buttons of the vertical toolbar that you want to see.");
                bottomBar.popup_text.TextWrapping = TextWrapping.Wrap;
            }
        }

        private void Service_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.settingsOpenned = false;
        }
    }
}
