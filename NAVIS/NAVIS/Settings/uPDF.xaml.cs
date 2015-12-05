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
using NAVIS;

namespace NAVIS.Settings
{
    /// <summary>
    /// User control to show the PDF settings
    /// </summary>
    public partial class uPDF : UserControl
    {
        List<CheckBox> chbList;

        public uPDF()
        {
            InitializeComponent();

            chbList = new List<CheckBox> {chb_showAverage, chb_showCochleogram, chb_showDate, chb_showDiff, chb_showHistogram, chb_showSonogram };
        }

        public void updateFrom(PdfS pdf)
        {
            chb_showDate.IsChecked = pdf.showDate;
            chb_showCochleogram.IsChecked = pdf.showCochleogram;
            chb_showHistogram.IsChecked = pdf.showHistogram;
            chb_showSonogram.IsChecked = pdf.showSonogram;
            chb_showDiff.IsChecked = pdf.showDiff;
            chb_showAverage.IsChecked = pdf.showAverage;
            this.InvalidateVisual();
        }

        public PdfS getFromForm()
        {
            PdfS pdf = new PdfS();

            pdf.showDate = chb_showDate.IsChecked.Value;
            pdf.showCochleogram = chb_showCochleogram.IsChecked.Value;
            pdf.showHistogram = chb_showHistogram.IsChecked.Value;
            pdf.showSonogram = chb_showSonogram.IsChecked.Value;
            pdf.showDiff = chb_showDiff.IsChecked.Value;
            pdf.showAverage = chb_showAverage.IsChecked.Value;
            return pdf;
        }

        private void Btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox chb in chbList)
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
