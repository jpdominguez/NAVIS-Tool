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

namespace NAVIS
{
    /// <summary>
    /// Information message window. Used to show some message to the user.
    /// </summary>
    public partial class InfoWindow : Window
    {
        /// <summary>
        /// Shows a message to the user with some information.
        /// </summary>
        public InfoWindow(String titleText, String contentText)
        {
            InitializeComponent();

            this.Title = titleText;
            textBlock_Content.Text = contentText;

            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        /// <summary>
        /// OK button click event. Closes the information window.
        /// </summary>
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
