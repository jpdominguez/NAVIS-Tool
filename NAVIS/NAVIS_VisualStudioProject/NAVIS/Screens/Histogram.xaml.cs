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
using System.Globalization;
using System.IO;
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
    /// Histogram window
    /// </summary>
    public partial class Histogram : Window
    {
        StringBuilder sb;
        int[] resultados;
        double max = 0;
        double yInterval = 0;
        CultureInfo ci = new CultureInfo("en-us");

        /// <summary>
        /// Histogram empty constructor
        /// </summary>
        public Histogram()
        {
            InitializeComponent();

            #region chart settings
            chart_Histogram.ChartAreas["ChartArea"].AxisX.Minimum = 0;
            chart_Histogram.ChartAreas["ChartArea"].AxisX.Interval = 16;
            chart_Histogram.ChartAreas["ChartArea"].AxisY.Interval = 1000;
            #endregion

            sb = new StringBuilder();

            resultados = null;

            chart_Histogram.Series.SuspendUpdates();


            if (MainWindow.settings.MainS.eventSize == 16) //Calculates the histogram
            {
                resultados = MainWindow.aedatObject16.eventsFiredForEachChannel(MainWindow.aedatObject16.dataBetweenTimestamps(0, MainWindow.aedatObject16.maxTimestamp));
            }

            else if (MainWindow.settings.MainS.eventSize == 32)
            {
                resultados = MainWindow.aedatObject32.eventsFiredForEachChannel(MainWindow.aedatObject32.dataBetweenTimestamps(0, MainWindow.aedatObject32.maxTimestamp));
            }


            for (int i = 0; i < resultados.Length; i++)
            {
                bool condition = true;
                switch (MainWindow.cochleaInfo)
                {
                    case EnumCochleaInfo.MONO32: condition = i != 63 && i != 62; break;
                    case EnumCochleaInfo.MONO64: condition = i != 127 && i != 126; break;
                    case EnumCochleaInfo.MONO128: condition = i != 255 && i != 254; break;
                    case EnumCochleaInfo.MONO256: condition = i != 511 && i != 510; break;
                    case EnumCochleaInfo.STEREO32: condition = i != 127 && i != 63 && i != 126 && i != 62; break;
                    case EnumCochleaInfo.STEREO64: condition = i != 127 && i != 255 && i != 126 && i != 254; break;
                    case EnumCochleaInfo.STEREO128: condition = i != 511 && i != 255 && i != 510 && i != 254; break;
                    case EnumCochleaInfo.STEREO256: condition = i != 511 && i != 1023 && i != 510 && i != 1022; break;
                    default: condition = i != 127 && i != 255 && i != 126 && i != 254; break;
                }

                if (condition)
                {
                    chart_Histogram.Series["Events"].Points.AddXY(i, resultados[i]); // Displays the histogram information in a chart
                    sb.Append(i + ";" + resultados[i] + "\n");
                    if (max < resultados[i])
                    {
                        max = resultados[i];
                    }
                    yInterval += resultados[i];
                }
            }

            #region chart settings
            yInterval = (int)((yInterval / resultados.Length) / 5);
            chart_Histogram.ChartAreas["ChartArea"].AxisY.Interval = yInterval;

            chart_Histogram.ChartAreas["ChartArea"].Axes[0].Title = "Address";
            chart_Histogram.ChartAreas["ChartArea"].Axes[1].Title = "Number of events";

            chart_Histogram.ChartAreas["ChartArea"].Axes[0].TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 11);
            chart_Histogram.ChartAreas["ChartArea"].Axes[1].TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 11);
            #endregion

            chart_Histogram.Series.ResumeUpdates();
        }

        /// <summary>
        /// Normalized checkbox checkedChange event. Swaps between showing the normalized histogram or not.
        /// </summary>
        private void CB_normalized_CheckedChanged(object sender, EventArgs e)
        {
            chart_Histogram.Series.SuspendUpdates();
            for (int i = 0; i < chart_Histogram.Series.Count; i++)
            {
                while (chart_Histogram.Series[i].Points.Count > 0)
                {
                    chart_Histogram.Series[i].Points.RemoveAt(chart_Histogram.Series[i].Points.Count - 1);
                }
            }

            if (CB_normalized.IsChecked == true)
            {
                for (int i = 0; i < resultados.Length; i++)
                {
                    bool condition = true;
                    switch (MainWindow.cochleaInfo)
                    {
                        case EnumCochleaInfo.MONO32: condition = i != 63 && i != 62; break;
                        case EnumCochleaInfo.MONO64: condition = i != 127 && i != 126; break;
                        case EnumCochleaInfo.MONO128: condition = i != 255 && i != 254; break;
                        case EnumCochleaInfo.MONO256: condition = i != 511 && i != 510; break;
                        case EnumCochleaInfo.STEREO32: condition = i != 127 && i != 63 && i != 126 && i != 62; break;
                        case EnumCochleaInfo.STEREO64: condition = i != 127 && i != 255 && i != 126 && i != 254; break;
                        case EnumCochleaInfo.STEREO128: condition = i != 511 && i != 255 && i != 510 && i != 254; break;
                        case EnumCochleaInfo.STEREO256: condition = i != 511 && i != 1023 && i != 510 && i != 1022; break;
                        default: condition = i != 127 && i != 255 && i != 126 && i != 254; break;
                    }
                    if (condition)
                    {
                        chart_Histogram.Series["Events"].Points.AddXY(i, resultados[i] / max);
                    }
                }
                chart_Histogram.ChartAreas["ChartArea"].AxisY.Maximum = 1;
                chart_Histogram.ChartAreas["ChartArea"].AxisY.Minimum = 0;
                chart_Histogram.ChartAreas["ChartArea"].AxisY.Interval = 0.10;
            }
            else
            {
                for (int i = 0; i < resultados.Length; i++)
                {
                    bool condition = true;
                    switch (MainWindow.cochleaInfo)
                    {
                        case EnumCochleaInfo.MONO32: condition = i != 63 && i != 62; break;
                        case EnumCochleaInfo.MONO64: condition = i != 127 && i != 126; break;
                        case EnumCochleaInfo.MONO128: condition = i != 255 && i != 254; break;
                        case EnumCochleaInfo.MONO256: condition = i != 511 && i != 510; break;
                        case EnumCochleaInfo.STEREO32: condition = i != 127 && i != 63 && i != 126 && i != 62; break;
                        case EnumCochleaInfo.STEREO64: condition = i != 127 && i != 255 && i != 126 && i != 254; break;
                        case EnumCochleaInfo.STEREO128: condition = i != 511 && i != 255 && i != 510 && i != 254; break;
                        case EnumCochleaInfo.STEREO256: condition = i != 511 && i != 1023 && i != 510 && i != 1022; break;
                        default: condition = i != 127 && i != 255 && i != 126 && i != 254; break;
                    }
                    if (condition)
                    {
                        chart_Histogram.Series["Events"].Points.AddXY(i, resultados[i]);
                    }
                }
                chart_Histogram.ChartAreas["ChartArea"].AxisY.Maximum = Double.NaN;
                chart_Histogram.ChartAreas["ChartArea"].AxisY.Minimum = Double.NaN;
                chart_Histogram.ChartAreas["ChartArea"].AxisY.Interval = yInterval;
                chart_Histogram.ChartAreas["ChartArea"].RecalculateAxesScale();
            }
            chart_Histogram.Series.ResumeUpdates();
        }

        /// <summary>
        /// Save image click event. Saves an image with the information shown in the histogram chart.
        /// </summary>
        private void B_saveImage_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveImageDialog = new SaveFileDialog();
            saveImageDialog.Title = "Select a name and a path for the .png file";
            saveImageDialog.Filter = "png files|*.png";
            if (saveImageDialog.ShowDialog() == true)
            {
                chart_Histogram.SaveImage(saveImageDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);

                InfoWindow iw = new InfoWindow("Success!", "Image saved successfuly");
                iw.ShowDialog();
            }
        }

        /// <summary>
        /// Export click event. Creates a CSV file with the information from the histogram.
        /// </summary>
        private void B_exportCSV_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveCSVDialog = new SaveFileDialog();
            saveCSVDialog.Title = "Select a name and a path for the .csv file";
            saveCSVDialog.Filter = "csv files|*.csv";
            if (saveCSVDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveCSVDialog.FileName, "Address;Number of events\n", Encoding.UTF8);
                File.AppendAllText(saveCSVDialog.FileName, sb.ToString());

                InfoWindow iw = new InfoWindow("Success!", "CSV saved successfuly");
                iw.ShowDialog();
            }
        }
    }
}
