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

namespace NAVIS.Screens
{
    /// <summary>
    /// Cochleogram split window. Shows the cochleogram of the splitted aedat file after the Automatic Aedat Splitter process.
    /// </summary>
    public partial class CochleogramSplit : Window
    {
        cAedat16 aedatObject16;
        cAedat32 aedatObject32;

        /// <summary>
        /// Loads an aedat file (split) from a path and displays its cochleogram
        /// </summary>
        public CochleogramSplit(String path)
        {
            InitializeComponent();

            this.Title = path.Split('\\')[path.Split('\\').Length - 1];

            if (MainWindow.settings.MainS.eventSize == 16)
            {
                aedatObject16 = new cAedat16(path);
                aedatObject16.adaptAedat();
            }
            else if (MainWindow.settings.MainS.eventSize == 32)
            {
                aedatObject32 = new cAedat32(path);
                aedatObject32.adaptAedat();
            }

            displayCochleogram();
        }

        /// <summary>
        /// Displays the cochleogram of the file in the chart
        /// </summary>
        public void displayCochleogram()
        {
            chart_CochleogramCut.ChartAreas["ChartArea"].AxisX.Minimum = 0;
            chart_CochleogramCut.ChartAreas["ChartArea"].AxisY.Minimum = 0;
            chart_CochleogramCut.ChartAreas["ChartArea"].AxisY.Maximum = 256;
            chart_CochleogramCut.ChartAreas["ChartArea"].AxisY.Interval = 32;

            int cont = 0;

            chart_CochleogramCut.Series.SuspendUpdates();
            if (MainWindow.settings.MainS.eventSize == 16)
            {
                
                foreach (cAedatRow16 fila in aedatObject16.getValues())
                {
                    if (cont == 0)
                    {
                        if (fila.evt < 127)
                        {
                            chart_CochleogramCut.Series["Left"].Points.AddXY(fila.timestamp, (double)fila.evt);
                        }
                        else if (fila.evt >= 128)
                        {
                            chart_CochleogramCut.Series["Right"].Points.AddXY(fila.timestamp, (double)fila.evt);
                        }
                    }
                    cont++;
                    if (cont == MainWindow.settings.MainS.dotsToPaint)
                    {
                        cont = 0;
                    }
                }                
            }
            else if (MainWindow.settings.MainS.eventSize == 32)
            {
                foreach (cAedatRow32 fila in aedatObject32.getValues())
                {
                    if (cont == 0)
                    {
                        if (fila.evt < 127)
                        {
                            chart_CochleogramCut.Series["Left"].Points.AddXY(fila.timestamp, (double)fila.evt);
                        }
                        else if (fila.evt >= 128)
                        {
                            chart_CochleogramCut.Series["Right"].Points.AddXY(fila.timestamp, (double)fila.evt);
                        }
                    }
                    cont++;
                    if (cont == MainWindow.settings.MainS.dotsToPaint)
                    {
                        cont = 0;
                    }
                }
            }
            chart_CochleogramCut.Series.ResumeUpdates();
            chart_CochleogramCut.Series["Left"].MarkerSize = MainWindow.settings.MainS.dotSize;
            chart_CochleogramCut.Series["Right"].MarkerSize = MainWindow.settings.MainS.dotSize;

            Color[] colores = MainWindow.colorFromSettings();

            chart_CochleogramCut.Series["Left"].Color = System.Drawing.Color.FromArgb(colores[0].A, colores[0].R, colores[0].G, colores[0].B);
            chart_CochleogramCut.Series["Right"].Color = System.Drawing.Color.FromArgb(colores[1].A, colores[1].R, colores[1].G, colores[1].B);
        }
    }
}
