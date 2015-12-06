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

            System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
            textInfo.Text = "https://github.com/jpdominguez/NAVIS-Tool\nCopyright © 2015 Juan P. Dominguez-Morales\njpdominguez@atc.us.es\n\nRobotics and Techonology of Computers Lab. (RTC)\nwww.rtc.us.es\n\nDepartment of Architecture and Technology of Computers (ATC)\nwww.atc.us.es\n\nHigher Technical School of Computer Engineering\nUniversity of Seville, Spain\nwww.informatica.us.es";
            versionInfo.Text = "v" + version.ToString();
        }
    }
}