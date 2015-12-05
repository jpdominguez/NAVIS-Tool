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
using System.Text.RegularExpressions;
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
    /// User control to show the Tools settings
    /// </summary>
    public partial class uTools : UserControl
    {
        public uTools()
        {
            InitializeComponent();

            foreach (EnumSize e in Enum.GetValues(typeof(EnumSize)))
            {
                CB_ImageSize.Items.Add(e.ToString());
            }
        }

        public void updateFrom(ToolsS sd)
        {
            CB_ImageSize.SelectedItem = sd.imgSize.ToString();
            TB_IntegrationPeriod.Text = Convert.ToString(sd.integrationPeriod);
            SB_NoiseThreshold.Value = sd.noiseThreshold;
            SB_NoiseTolerance.Value = sd.noiseTolerance;
            //L_noiseThreshold.Text = sd.noiseThreshold.ToString();
            //L_noiseTolerance.Text = sd.noiseTolerance.ToString();
            this.InvalidateVisual();
        }

        public ToolsS getFromForm()
        {
            ToolsS sd = new ToolsS();
            bool error = false;
            //RTB_errorShowDiagram.ForeColor = Colors.Red;
            //RTB_errorShowDiagram.Clear();
            if (CB_ImageSize.SelectedItem == null || (CB_ImageSize.SelectedItem.ToString() != EnumSize.LARGE.ToString() && CB_ImageSize.SelectedItem.ToString() != EnumSize.MEDIUM.ToString() && CB_ImageSize.SelectedItem.ToString() != EnumSize.SMALL.ToString() && CB_ImageSize.SelectedItem.ToString() != EnumSize.TINY.ToString()))
            {
                //RTB_errorShowDiagram.AppendText("- Image size is not valid\n");
                error = true;
            }
            if (TB_IntegrationPeriod.Text == "0" || TB_IntegrationPeriod.Text == "")
            {
                //RTB_errorShowDiagram.AppendText("- Integration Period value is not valid\n");
                error = true;
            }

            if (error == false)
            {
                sd = new ToolsS();
                //sd.imgFormat = (EnumFormat)CB_image_Format.SelectedItem;   //ELIMINAR
                sd.imgSize = (EnumSize)Enum.Parse(typeof(EnumSize), CB_ImageSize.SelectedItem.ToString(), true);
                sd.integrationPeriod = Convert.ToInt64(TB_IntegrationPeriod.Text);
                sd.noiseThreshold = SB_NoiseThreshold.Value;
                sd.noiseTolerance = SB_NoiseTolerance.Value;   //cambiar nombre SB por slider

                //RTB_errorShowDiagram.Clear();
                //RTB_errorShowDiagram.ForeColor = Colors.Green;
                //RTB_errorShowDiagram.AppendText("\n\nSettings has been succesfully changed");
                //RTB_errorShowDiagram.SelectAll();
                //RTB_errorShowDiagram.SelectionAlignment = HorizontalAlignment.Center;
            }

            return sd;
        }

        private void TB_IntegrationPeriod_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        /*private void SB_noiseThreshold_Scroll(object sender, ScrollEventArgs e)
        {
            L_noiseThreshold.Text = SB_NoiseThreshold.Value.ToString();
            L_noiseTolerance.Text = SB_NoiseTolerance.Value.ToString();
        }
        */
    }
}
