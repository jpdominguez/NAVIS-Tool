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
using System.IO;
using System.Windows.Forms;

namespace NAVIS.Screens
{
    /// <summary>
    /// Automatic Aedat Splitter window. Lets the user change the values of Noise tolerance and Noise threshold before starting the process
    /// </summary>
    public partial class AedatSplitterAutoWindow : Window
    {
        /// <summary>
        /// Automatic Aedat Splitter empty constructor
        /// </summary>
        public AedatSplitterAutoWindow()
        {
            InitializeComponent();

            SB_noiseThreshold.Value = MainWindow.settings.ToolsS.noiseThreshold;  // Loads Noise Threshold and Noise Tolerance values from the settings
            SB_noiseTolerance.Value = MainWindow.settings.ToolsS.noiseTolerance;
        }

        /// <summary>
        /// Split click event. Splits the loaded aedat file using the values of Noise Tolerance and Noise Threshold selected by the user. After the process is completed, the user is asked if he wants to see the cochleogram for every split that was generated.
        /// </summary>
        private void Btn_Split_Click(object sender, RoutedEventArgs e)
        {
            String path = MainWindow.fileName.Split('.')[0];
            this.Cursor = System.Windows.Input.Cursors.Wait;
            if (MainWindow.settings.MainS.eventSize == 16)
            {
                MainWindow.aedatObject16.AERSplitter(SB_noiseTolerance.Value, SB_noiseThreshold.Value);
            }
            else if (MainWindow.settings.MainS.eventSize == 32)
            {
                MainWindow.aedatObject32.AedatSplitterAuto(SB_noiseTolerance.Value, SB_noiseThreshold.Value);
            }
            if (Directory.GetFiles("./" + path).Length != 0)
            {
                MessageBoxResult res = System.Windows.MessageBox.Show("The split process generated " + Directory.GetFiles("./" + path).Length + " files\nDo you want to display them individually as a cochleohgram", "AedatSplitter success", System.Windows.MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    foreach (String file in Directory.GetFiles("./" + path))
                    {
                        CochleogramSplit cs = new CochleogramSplit(file);
                        cs.Show();
                    }
                }
            }
            else
            {
                MessageBoxResult res = System.Windows.MessageBox.Show("The cutting process generated 0 files\nTry another configuration", "AedatSplitter success", System.Windows.MessageBoxButton.OK);
            }
            this.Cursor = System.Windows.Input.Cursors.Arrow;
        }
    }
}
