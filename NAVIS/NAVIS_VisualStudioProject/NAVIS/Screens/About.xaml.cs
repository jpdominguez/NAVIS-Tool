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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace NAVIS
{
    public partial class About : Window
    {
        /// <summary>
        /// About window. Shows information about the author and the software
        /// </summary>
        public About()
        {
            InitializeComponent();

            //Get NAVIS version
            System.Version version = Assembly.GetExecutingAssembly().GetName().Version;

            #region textblock hyperlinks and text

            //Creating texts
            Run run1 = new Run("https://github.com/jpdominguez/NAVIS-Tool");
            Run run2 = new Run("Copyright © 2015 Juan P. Dominguez-Morales");
            Run run3 = new Run("jpdominguez@atc.us.es");
            Run run4 = new Run("Robotics and Techonology of Computers Lab. (RTC)");
            Run run5 = new Run("www.rtc.us.es");
            Run run6 = new Run("Department of Architecture and Technology of Computers (ATC)");
            Run run7 = new Run("www.atc.us.es");
            Run run8 = new Run("Higher Technical School of Computer Engineering");
            Run run9 = new Run("University of Seville, Spain");
            Run run10 = new Run("www.informatica.us.es");

            //Associating hyperlinks with text
            Hyperlink hyperlink1 = new Hyperlink(run1)
            {              
                NavigateUri = new Uri("https://github.com/jpdominguez/NAVIS-Tool")
            };
            Hyperlink hyperlink2 = new Hyperlink(run3)
            {
                NavigateUri = new Uri("mailto:jpdominguez@atc.us.es?subject=NAVIS_Support&body=")
            };
            Hyperlink hyperlink3 = new Hyperlink(run5)
            {
                NavigateUri = new Uri("http://www.rtc.us.es")
            };
            Hyperlink hyperlink4 = new Hyperlink(run7)
            {
                NavigateUri = new Uri("http://www.atc.us.es")
            };
            Hyperlink hyperlink5 = new Hyperlink(run10)
            {
                NavigateUri = new Uri("http://www.informatica.us.es")
            };

            //Creating hyperlinks events
            hyperlink1.RequestNavigate += new RequestNavigateEventHandler(delegate (object sender, RequestNavigateEventArgs e) {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            });
            hyperlink2.RequestNavigate += new RequestNavigateEventHandler(delegate (object sender, RequestNavigateEventArgs e) {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            });
            hyperlink3.RequestNavigate += new RequestNavigateEventHandler(delegate (object sender, RequestNavigateEventArgs e) {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            });
            hyperlink4.RequestNavigate += new RequestNavigateEventHandler(delegate (object sender, RequestNavigateEventArgs e) {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            });
            hyperlink5.RequestNavigate += new RequestNavigateEventHandler(delegate (object sender, RequestNavigateEventArgs e) {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            });

            //Filling the textblock with content
            textInfo.Inlines.Clear();
            textInfo.Inlines.Add(hyperlink1);
            textInfo.Inlines.Add("\n");
            textInfo.Inlines.Add(run2);
            textInfo.Inlines.Add("\n");
            textInfo.Inlines.Add(hyperlink2);
            textInfo.Inlines.Add("\n\n");
            textInfo.Inlines.Add(run4);
            textInfo.Inlines.Add("\n");
            textInfo.Inlines.Add(hyperlink3);
            textInfo.Inlines.Add("\n\n");
            textInfo.Inlines.Add(run6);
            textInfo.Inlines.Add("\n");
            textInfo.Inlines.Add(hyperlink4);
            textInfo.Inlines.Add("\n\n");
            textInfo.Inlines.Add(run8);
            textInfo.Inlines.Add("\n");
            textInfo.Inlines.Add(run9);
            textInfo.Inlines.Add("\n");
            textInfo.Inlines.Add(hyperlink5);
            #endregion

            //Show NAVIS version in a textblock
            versionInfo.Text = "v" + version.ToString();
        }
    }
}