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
    public partial class uMain : UserControl
    {
        /// <summary>
        /// User control to show the Main settings
        /// </summary>
        public uMain()
        {
            InitializeComponent();
            List<string> list = new List<string>();

            foreach(EnumColor e in Enum.GetValues(typeof(EnumColor)))
            {
                CB_leftColor.Items.Add(e.ToString());
                CB_rightColor.Items.Add(e.ToString());
            }
            foreach (EnumAudio e in Enum.GetValues(typeof(EnumAudio)))
            {
                CB_mono_stereo.Items.Add(e.ToString());
            }
            foreach (EnumScreenSize e in Enum.GetValues(typeof(EnumScreenSize)))
            {
                CB_screenSize.Items.Add(e.ToString().Replace("s", string.Empty));
            }
            CB_channels.Items.Add(32);
            CB_channels.Items.Add(64);
            CB_channels.Items.Add(128);
            CB_channels.Items.Add(256);
            CB_eventSize.Items.Add(16);
            CB_eventSize.Items.Add(32);
        }

        public void updateFrom(MainS ms)
        {
            NUD_Dot_Size.NUDTextBox.Text = ms.dotSize.ToString();
            CB_leftColor.SelectedItem = ms.leftColor.ToString();
            CB_rightColor.SelectedItem = ms.rightColor.ToString();
            ChB_ShowCoch.IsChecked = ms.showChart;
            CB_eventSize.SelectedItem = ms.eventSize;
            NUD_Dots_Per_Event.NUDTextBox.Text = ms.dotsToPaint.ToString();
            CB_channels.SelectedItem = ms.channels;
            CB_mono_stereo.SelectedItem = ms.mono_stereo.ToString();
            CB_screenSize.SelectedItem = ms.screenSize.ToString().Replace("s", string.Empty);
            this.InvalidateVisual();
        }

        public MainS getFromForm()
        {
            MainS ms = new MainS();
            bool error = false;
            if (CB_leftColor.SelectedItem == null || (CB_leftColor.SelectedItem.ToString() != EnumColor.BLACK.ToString() && CB_leftColor.SelectedItem.ToString() != EnumColor.BLUE.ToString() && CB_leftColor.SelectedItem.ToString() != EnumColor.BROWN.ToString() && CB_leftColor.SelectedItem.ToString() != EnumColor.GREEN.ToString() && CB_leftColor.SelectedItem.ToString() != EnumColor.MAGENTA.ToString() && CB_leftColor.SelectedItem.ToString() != EnumColor.ORANGE.ToString() && CB_leftColor.SelectedItem.ToString() != EnumColor.RED.ToString() && CB_leftColor.SelectedItem.ToString() != EnumColor.YELLOW.ToString()))
            {
                error = true;
            }
            if (CB_rightColor.SelectedItem == null || (CB_rightColor.SelectedItem.ToString() != EnumColor.BLACK.ToString() && CB_rightColor.SelectedItem.ToString() != EnumColor.BLUE.ToString() && CB_rightColor.SelectedItem.ToString() != EnumColor.BROWN.ToString() && CB_rightColor.SelectedItem.ToString() != EnumColor.GREEN.ToString() && CB_rightColor.SelectedItem.ToString() != EnumColor.MAGENTA.ToString() && CB_rightColor.SelectedItem.ToString() != EnumColor.ORANGE.ToString() && CB_rightColor.SelectedItem.ToString() != EnumColor.RED.ToString() && CB_rightColor.SelectedItem.ToString() != EnumColor.YELLOW.ToString()))
            {
                error = true;
            }
            if (CB_eventSize.SelectedItem == null || (Convert.ToInt16(CB_eventSize.SelectedItem) != 16 && Convert.ToInt16(CB_eventSize.SelectedItem) != 32))
            {
                error = true;
            }
            if (CB_channels.SelectedItem == null || (Convert.ToInt16(CB_channels.SelectedItem) != 64 && Convert.ToInt16(CB_channels.SelectedItem) != 32 && Convert.ToInt16(CB_channels.SelectedItem) != 128 && Convert.ToInt16(CB_channels.SelectedItem) != 256))
            {
                error = true;
            }
            if (CB_mono_stereo.SelectedItem == null || (CB_mono_stereo.SelectedItem.ToString() != EnumAudio.MONO.ToString() && CB_mono_stereo.SelectedItem.ToString() != EnumAudio.STEREO.ToString()))
            {
                error = true;
            }
            if (error == false)
            {
                ms.dotSize = Convert.ToInt16(NUD_Dot_Size.NUDTextBox.Text);
                ms.leftColor = (EnumColor)Enum.Parse(typeof(EnumColor), CB_leftColor.SelectedItem.ToString(), true);
                ms.rightColor = (EnumColor)Enum.Parse(typeof(EnumColor), CB_rightColor.SelectedItem.ToString(), true);
                ms.showChart = ChB_ShowCoch.IsChecked.Value;
                ms.eventSize = Convert.ToInt16(CB_eventSize.SelectedItem);
                ms.dotsToPaint = Convert.ToInt16(NUD_Dots_Per_Event.NUDTextBox.Text);
                ms.channels = Convert.ToInt16(CB_channels.SelectedItem);
                ms.mono_stereo = (EnumAudio)Enum.Parse(typeof(EnumAudio), CB_mono_stereo.SelectedItem.ToString(), true);
                ms.screenSize = (EnumScreenSize)Enum.Parse(typeof(EnumScreenSize), "s" + CB_screenSize.SelectedItem.ToString());
            }
            return ms;
        }        
    }
}
