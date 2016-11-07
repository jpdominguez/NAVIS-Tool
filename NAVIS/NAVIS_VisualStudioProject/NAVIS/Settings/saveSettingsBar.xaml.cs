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


using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAVIS;

namespace NAVIS.Settings
{
    /// <summary>
    /// Save settings bar usercontrol for the Settings window
    /// </summary>
    public partial class saveSettingsBar : UserControl
    {
        public Settings settingsForm;

        Window _Owner;
        public Window Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        public saveSettingsBar()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save settings to the default path
        /// </summary>
        private void _btSave_Click(object sender, RoutedEventArgs e)
        {
            settingsForm.updateSettingsFromForm();
            MainWindow.settings.saveSettingsToFile();

            MainWindow.updateButtonList();  // Updates the buttons from the sidebar that will be shown
            for (int i = 0; i < MainWindow.buttonList.Count; i++)  
            {
                if (MainWindow.buttonListCollapsed[i] == false)
                {
                    MainWindow.buttonList[i].Visibility = Visibility.Collapsed;
                }
                else
                {
                    MainWindow.buttonList[i].Visibility = Visibility.Visible;
                }
            }
            MainWindow.updateScreenSize();
            InfoWindow iw = new InfoWindow("Success!", "Settings saved successfuly");
            iw.ShowDialog();
        }

        private void TBStatus_Loaded(object sender, RoutedEventArgs e)
        {
            _Owner = Window.GetWindow(this);
            if (_Owner != null)
            {
                this.FontSize = _Owner.FontSize;
            }
        }

        /// <summary>
        /// Save settings to a path chosen by the user
        /// </summary>
        private void _btSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML Files|*.xml";
            saveFileDialog1.Title = "Save a XML settings File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                settingsForm.updateSettingsFromForm();
                MainWindow.settings.saveSettingsToFile(saveFileDialog1.FileName);

                InfoWindow iw = new InfoWindow("Success!", "Settings saved successfuly");
                iw.ShowDialog();
            }
        }

        /// <summary>
        /// Restore settings to its latest saved values
        /// </summary>
        private void _btRestore_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.settings.loadSettingsFromFile();
            settingsForm.updateFormFromSettings();

            InfoWindow iw = new InfoWindow("Success!", "Settings restored to default");
            iw.ShowDialog();
        }

        /// <summary>
        /// Load settings from a path chosen by the user
        /// </summary>
        private void _btLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML Files|*.xml";
            openFileDialog1.Title = "Select a XML settings file";

            if (openFileDialog1.ShowDialog() == true)
            {
                MainWindow.settings.loadSettingsFromFile(openFileDialog1.FileName);
                settingsForm.updateFormFromSettings();

                InfoWindow iw = new InfoWindow("Success!", "Settings loaded successfuly");
                iw.ShowDialog();
            }
        }
    }
}
