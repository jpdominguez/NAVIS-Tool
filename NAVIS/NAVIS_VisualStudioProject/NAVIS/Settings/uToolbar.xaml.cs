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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NAVIS.Settings
{
    /// <summary>
    /// User control to show the Toolbar settings
    /// </summary>
    public partial class uToolbar : UserControl
    {
        List<CheckBox> chbList;

        public uToolbar()
        {
            InitializeComponent();

            chbList = new List<CheckBox> { chb_LoadAedat, chb_Settings, chb_About, chb_Sonogram, chb_Histogram, chb_Average, chb_Diff, chb_ManualAedatSplitter, chb_AutomaticAedatSplitter, chb_GeneratePDF, chb_GenerateCSV, chb_StereoToMono, chb_MonoToStereo };
        }

        public void updateFrom(ToolbarS tb)
        {
            chb_LoadAedat.IsChecked = tb.showLoadAedat;
            chb_Settings.IsChecked = tb.showSettings;
            chb_About.IsChecked = tb.showAbout;
            chb_Sonogram.IsChecked = tb.showSonogram;
            chb_Histogram.IsChecked = tb.showHistogram;
            chb_Average.IsChecked = tb.showAverage;
            chb_Diff.IsChecked = tb.showDiff;
            chb_GenerateCSV.IsChecked = tb.showGenerateCSV;
            chb_GeneratePDF.IsChecked = tb.showGeneratePDF;
            chb_ManualAedatSplitter.IsChecked = tb.showManualAedatSplitter;
            chb_AutomaticAedatSplitter.IsChecked = tb.showAutomaticAedatSplitter;
            chb_StereoToMono.IsChecked = tb.showStereoToMono;
            chb_MonoToStereo.IsChecked = tb.showMonoToStereo;
            this.InvalidateVisual();
        }

        public ToolbarS getFromForm()
        {
            ToolbarS tb = new ToolbarS();

            tb.showLoadAedat = chb_LoadAedat.IsChecked.Value;
            tb.showSettings = chb_Settings.IsChecked.Value;
            tb.showAbout = chb_About.IsChecked.Value;
            tb.showSonogram = chb_Sonogram.IsChecked.Value;
            tb.showHistogram = chb_Histogram.IsChecked.Value;
            tb.showAverage = chb_Average.IsChecked.Value;
            tb.showDiff = chb_Diff.IsChecked.Value;
            tb.showGenerateCSV = chb_GenerateCSV.IsChecked.Value;
            tb.showGeneratePDF = chb_GeneratePDF.IsChecked.Value;
            tb.showManualAedatSplitter = chb_ManualAedatSplitter.IsChecked.Value;
            tb.showAutomaticAedatSplitter = chb_AutomaticAedatSplitter.IsChecked.Value;
            tb.showStereoToMono = chb_StereoToMono.IsChecked.Value;
            tb.showMonoToStereo = chb_MonoToStereo.IsChecked.Value;
            return tb;
        }

        private void Btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(CheckBox chb in chbList)
            {
                chb.IsChecked = true;
            }
        }

        private void Btn_DeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox chb in chbList)
            {
                chb.IsChecked = false;
            }
        }
    }
}
