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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NAVIS
{
    /// <summary>
    /// Class for working with aedat files with 16-bit address size
    /// </summary>
    public class aedat16
    {
        private List<aedatEvent16> aedatFileList = new List<aedatEvent16>();
        BinaryReader bReader;
        BinaryWriter bWriter;
        public long maxTimestamp = 0;
        public long minTimestamp = long.MaxValue;
        public static int maxValSonogram;
        private StringBuilder sb_csv = new StringBuilder();

        /// <summary>
        /// Loads a file and stores it in aedatFileRows. The file can either have .aedat or .csv extension
        /// </summary>
        public aedat16(String filePath)
        {
            #region .aedat
            if (MainWindow.fileName.Split('.')[MainWindow.fileName.Split('.').Length - 1] == "aedat")  // If the file is an aedat file
            {
                bReader = new BinaryReader(File.Open(filePath, FileMode.Open));
                int pos = 0;
                int length = (int)bReader.BaseStream.Length;
                int rowID = 0;

                UInt16 s = (UInt16)bReader.ReadByte();
                pos += sizeof(Byte);

                while (s == 35)
                {
                    while (s != '\n')
                    {
                        s = bReader.ReadByte();
                        pos += sizeof(Byte);
                    }
                    s = bReader.ReadByte();
                    pos += sizeof(Byte);
                }

                int last_pos = pos;

                UInt16 evt;
                UInt32 timestamp;
                while (pos < length)
                {
                    if (last_pos == pos)
                    {
                        evt = (UInt16)(s);
                        pos += sizeof(Byte);
                    }
                    else
                    {
                        evt = (UInt16)(bReader.ReadByte());
                        pos += sizeof(Byte);
                    }
                    evt = (UInt16)(evt << 8);
                    evt = (UInt16)(evt | (UInt16)(bReader.ReadByte()));
                    evt = (UInt16)(0x00FF & evt);
                    pos += sizeof(Byte);

                    timestamp = (UInt32)(bReader.ReadByte());
                    pos += sizeof(Byte);
                    timestamp = (UInt32)(timestamp << 8);
                    timestamp = (UInt32)(timestamp | (UInt32)(bReader.ReadByte()));
                    pos += sizeof(Byte);
                    timestamp = (UInt32)(timestamp << 8);
                    timestamp = (UInt32)(timestamp | (UInt32)(bReader.ReadByte()));
                    pos += sizeof(Byte);
                    timestamp = (UInt32)(timestamp << 8);
                    timestamp = (UInt32)(timestamp | (UInt32)(bReader.ReadByte()));
                    pos += sizeof(Byte);

                    if (maxTimestamp < timestamp)
                    {
                        maxTimestamp = Convert.ToInt64(timestamp);
                    }
                    if (minTimestamp > timestamp)
                    {
                        minTimestamp = Convert.ToInt64(timestamp);
                    }
                    aedatFileList.Add(new aedatEvent16(rowID, Convert.ToUInt16(evt), Convert.ToUInt32(timestamp)));
                    rowID++;
                }
                bReader.Close();
            }
            #endregion
            #region .csv
            else if (MainWindow.fileName.Split('.')[MainWindow.fileName.Split('.').Length - 1] == "csv")  // if the file is a csv file
            {
                List<String> csvLinesList = new List<String>();
                var reader = new StreamReader(File.OpenRead(filePath));
                String[] line = null;
                int rowID = 0;
                int cont = 0;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Split(';');
                    if (cont != 0)
                    {
                        aedatFileList.Add(new aedatEvent16(rowID, Convert.ToUInt16(line[1]), Convert.ToUInt32(line[0])));
                        rowID++;

                        if (maxTimestamp < Convert.ToUInt32(line[0]))
                        {
                            maxTimestamp = Convert.ToInt64(Convert.ToUInt32(line[0]));
                        }
                        if (minTimestamp > Convert.ToUInt32(line[0]))
                        {
                            minTimestamp = Convert.ToInt64(Convert.ToUInt32(line[0]));
                        }
                    }
                    else
                    {
                        cont++;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Save the loaded information to an aedat file.
        /// </summary>
        public void saveAedat(String fileName, List<aedatEvent16> aedat)
        {
            bWriter = new BinaryWriter(File.OpenWrite(fileName));
            UInt16 evt;
            UInt32 timestamp;

            foreach (aedatEvent16 row in aedat)
            {
                evt = (UInt16)((BitConverter.GetBytes(row.addr)[0] << 8) | BitConverter.GetBytes(row.addr)[1]);
                timestamp = (UInt32)((BitConverter.GetBytes(row.timestamp)[0] << 24) | BitConverter.GetBytes(row.timestamp)[1] << 16 | BitConverter.GetBytes(row.timestamp)[2] << 8 | BitConverter.GetBytes(row.timestamp)[3]);
                if (row.timestamp - aedat[0].timestamp < aedat[aedat.Count - 1].timestamp)
                {
                    bWriter.Write(evt);
                    bWriter.Write(timestamp);
                }
            }
            bWriter.Close();
        }

        /// <summary>
        /// Substracts the minimum timestamp on the file to every event, moving the entire information to the start of the chart.
        /// </summary>
        public void adaptAedat()
        {
            aedatFileList.ForEach(c => c.timestamp = (UInt32)(c.timestamp - minTimestamp));
            maxTimestamp = (long)((maxTimestamp - minTimestamp));
            minTimestamp = 0;
        }

        /// <summary>
        /// Return the event information of the file
        /// </summary>
        public List<aedatEvent16> getValues()
        {
            return aedatFileList;
        }


        /// <summary>
        /// Returns an event list with those who are in the range [rangeInit, rangeEnd]on the original file.
        /// </summary>
        public List<aedatEvent16> dataBetweenTimestamps(long rangeInit, long rangeEnd)
        {
            List<aedatEvent16> res = new List<aedatEvent16>();
            res = aedatFileList.FindAll(x => x.timestamp >= rangeInit && x.timestamp <= rangeEnd);
            return res;
        }

        /// <summary>
        /// Returns an array with the number of events fired for each address, based on the list that is used as a parameter.
        /// </summary>
        public int[] eventsFiredForEachChannel(List<aedatEvent16> pairList)
        {
            int[] eventsFired;
            bool mono = false;
            int limit;
            switch (MainWindow.cochleaInfo)
            {
                case EnumCochleaInfo.MONO32: eventsFired = new int[64]; limit = 64; mono = true; break;
                case EnumCochleaInfo.MONO64: eventsFired = new int[128]; limit = 128; mono = true; break;
                case EnumCochleaInfo.MONO128: eventsFired = new int[256]; limit = 256; mono = true; break;
                case EnumCochleaInfo.MONO256: eventsFired = new int[512]; limit = 512; mono = true; break;
                case EnumCochleaInfo.STEREO32: eventsFired = new int[128]; limit = 128; mono = false; break;
                case EnumCochleaInfo.STEREO64: eventsFired = new int[256]; limit = 256; mono = false; break;
                case EnumCochleaInfo.STEREO128: eventsFired = new int[512]; limit = 512; mono = false; break;
                case EnumCochleaInfo.STEREO256: eventsFired = new int[1024]; limit = 1024; mono = false; break;
                default: eventsFired = new int[256]; limit = 256; mono = false; break;
            }

            Array.Clear(eventsFired, 0, eventsFired.Length);

            foreach (aedatEvent16 pair in pairList)
            {
                if (pair.addr < limit)
                {
                    if (mono && ((pair.addr != limit - 2) && (pair.addr != limit - 1)))
                    {
                        eventsFired[pair.addr]++;
                    }
                    else if (mono == false && ((pair.addr != limit - 1) && (pair.addr != limit - 2) && (pair.addr != limit / 2 - 1) && (pair.addr != limit / 2 - 2)))
                    {
                        eventsFired[pair.addr]++;
                    }
                }
            }

            return eventsFired;
        }

        /// <summary>
        /// Returns the total number of events that are stored in the list used as a parameter.
        /// </summary>
        public int eventsFiredTotal(List<aedatEvent16> pairList)
        {
            return pairList.Count;
        }

        /// <summary>
        /// Returns the maximum value of the sonogram after calculating it.
        /// </summary>
        public int maxValueSonogram()
        {
            int[] eventsForEachChannelInRange;
            int eventos = 0;
            int max = 0;

            List<int> lista = new List<int>();

            for (long i = 0; i <= maxTimestamp; i += (int)MainWindow.settings.ToolsS.integrationPeriod)
            {
                eventsForEachChannelInRange = eventsFiredForEachChannel(dataBetweenTimestamps(i, i + MainWindow.settings.ToolsS.integrationPeriod));

                for (int indx = 0; indx < eventsForEachChannelInRange.Length; indx += 2)
                {
                    bool condition = true;
                    switch (MainWindow.cochleaInfo)
                    {
                        case EnumCochleaInfo.MONO32: condition = indx != 63 && indx != 62; break;
                        case EnumCochleaInfo.MONO64: condition = indx != 127 && indx != 126; break;
                        case EnumCochleaInfo.MONO128: condition = indx != 255 && indx != 254; break;
                        case EnumCochleaInfo.MONO256: condition = indx != 511 && indx != 510; break;
                        case EnumCochleaInfo.STEREO32: condition = indx != 127 && indx != 63 && indx != 126 && indx != 62; break;
                        case EnumCochleaInfo.STEREO64: condition = indx != 127 && indx != 255 && indx != 126 && indx != 254; break;
                        case EnumCochleaInfo.STEREO128: condition = indx != 255 && indx != 511 && indx != 126 && indx != 510; break;
                        case EnumCochleaInfo.STEREO256: condition = indx != 1023 && indx != 511 && indx != 1022 && indx != 510; break;
                        default: condition = indx != 127 && indx != 255 && indx != 126 && indx != 254; break;
                    }

                    if (condition)
                    {
                        eventos = eventsForEachChannelInRange[indx] + eventsForEachChannelInRange[indx + 1];
                        if (eventos > max)
                        {
                            max = eventos;
                        }
                    }
                }
            }
            return max;
        }

        /// <summary>
        /// Generates the sonogram for the loaded file.
        /// </summary>
        public bool generateSonogram(String file, int maxValueSonogram)
        {
            maxValSonogram = maxValueSonogram;
            sb_csv.Clear();

            int tam = (int)MainWindow.settings.ToolsS.imgSize;

            if (Convert.ToInt32((maxTimestamp * tam / MainWindow.settings.ToolsS.integrationPeriod) + 4) > System.Windows.SystemParameters.PrimaryScreenWidth) //If the calculated sonogram is greater than the resolution of the screen in which it will be displayed.
            {
                MessageBox.Show("This process would generate an image larger than the primary screen width. Try increasing the Integration Period in File->Settings->Tools tab or reducing the image size", "Maximum size exceeded", MessageBoxButton.OK);
                return false;
            }
            else
            {
                int[] eventsForEachChannelInRange;

                int frameSizeHeight = 128;
                switch (MainWindow.cochleaInfo)
                {
                    case EnumCochleaInfo.MONO32: frameSizeHeight = 32; break;
                    case EnumCochleaInfo.MONO64: frameSizeHeight = 64; break;
                    case EnumCochleaInfo.MONO128: frameSizeHeight = 128; break;
                    case EnumCochleaInfo.MONO256: frameSizeHeight = 256; break;
                    case EnumCochleaInfo.STEREO32: frameSizeHeight = 64; break;
                    case EnumCochleaInfo.STEREO64: frameSizeHeight = 128; break;
                    case EnumCochleaInfo.STEREO128: frameSizeHeight = 256; break;
                    case EnumCochleaInfo.STEREO256: frameSizeHeight = 512; break;
                    default: frameSizeHeight = 128; break;
                }

                Bitmap Bmp = new Bitmap(Convert.ToInt32(((maxTimestamp * tam) / MainWindow.settings.ToolsS.integrationPeriod) + 4), frameSizeHeight * tam);  // Creates an empty bitmap with the correct size, specified by the user.
                Color color;
                int contador = 0;
                int eventos = 0;

                for (long i = 0; i <= maxTimestamp; i += (int)MainWindow.settings.ToolsS.integrationPeriod)
                {
                    eventsForEachChannelInRange = eventsFiredForEachChannel(dataBetweenTimestamps(i, i + MainWindow.settings.ToolsS.integrationPeriod));  // Firing rate for each channel in a time period window

                    for (int indx = 0; indx < eventsForEachChannelInRange.Length; indx += 2)
                    {
                        eventos = eventsForEachChannelInRange[indx] + eventsForEachChannelInRange[indx + 1];  // Adds positive and negative events of the same channel

                        float max = (float)eventos / maxValSonogram;

                        #region color scale
                        Byte green, red, blue;
                        if (max >= 0.5)
                        {
                            if (max >= 1)
                            {
                                red = 255;
                                green = 0;
                                blue = 0;
                            }
                            else
                            {
                                red = (Byte)((255 * (max - 0.5) * 2));
                                green = (Byte)(255 - red);
                                blue = (Byte)(0);
                            }
                        }
                        else
                        {
                            red = (Byte)(0);
                            green = (Byte)((255 * (max) * 2));
                            blue = (Byte)(255 - green);
                        }

                        color = Color.FromArgb((Byte)red, (Byte)green, (Byte)blue);
                        #endregion

                        for (int rep = 0; rep < tam; rep++)
                        {
                            for (int rep2 = 0; rep2 < tam; rep2++)
                            {
                                Bmp.SetPixel((int)(i / (int)MainWindow.settings.ToolsS.integrationPeriod) + contador + rep, tam * frameSizeHeight - 1 - rep2 - indx * tam / 2, color); //Paint the pixel with the color, depending on the activity
                            }
                        }

                        sb_csv.Append(max + ";");
                    }
                    sb_csv.Append("\n");
                    contador += tam - 1;
                }
                Bmp.Save(file + ".png", ImageFormat.Png);
                return true;
            }
        }



        public void generateSonogramCSV(string fileName)
        {
            File.AppendAllText(fileName, sb_csv.Replace(",", ".").ToString());
        }

        public void generateDisparityCSV(string fileName)
        {
            File.AppendAllText(fileName, sb_csv.Replace(",", ".").ToString());
        }

        /// <summary>
        /// Generates the disparity between cochleae fo the loaded file
        /// </summary>
        public bool generateDisparity(String file, int maxValSon)
        {
            int tam = (int)MainWindow.settings.ToolsS.imgSize;
            sb_csv.Clear();

            int frameSizeHeight = 128;
            switch (MainWindow.cochleaInfo)
            {
                case EnumCochleaInfo.MONO32: frameSizeHeight = 32; break;
                case EnumCochleaInfo.MONO64: frameSizeHeight = 64; break;
                case EnumCochleaInfo.MONO128: frameSizeHeight = 128; break;
                case EnumCochleaInfo.MONO256: frameSizeHeight = 256; break;
                case EnumCochleaInfo.STEREO32: frameSizeHeight = 64; break;
                case EnumCochleaInfo.STEREO64: frameSizeHeight = 128; break;
                case EnumCochleaInfo.STEREO128: frameSizeHeight = 256; break;
                case EnumCochleaInfo.STEREO256: frameSizeHeight = 512; break;
                default: frameSizeHeight = 128; break;
            }

            Bitmap Bmp = new Bitmap(Convert.ToInt32((maxTimestamp * tam / MainWindow.settings.ToolsS.integrationPeriod) + 4), frameSizeHeight / 2 * tam);
            int[] eventsForEachChannelInRange;
            maxValSonogram = maxValSon;

            Color color;
            int contador = 0;
            int eventosR = 0;
            int eventosL = 0;

            if (Convert.ToInt32((maxTimestamp * tam / MainWindow.settings.ToolsS.integrationPeriod) + 4) > System.Windows.SystemParameters.PrimaryScreenWidth)
            {
                MessageBox.Show("This process would generate an image larger than the primary screen width. Try increasing the Integration Period in File->Settings->Tools tab or reducing the image size", "Maximum size exceeded", MessageBoxButton.OK); //If the calculated chart is greater than the resolution of the screen in which it will be displayed.
                return false;
            }
            else
            {
                for (long i = 0; i <= maxTimestamp; i += (int)MainWindow.settings.ToolsS.integrationPeriod)
                {
                    eventsForEachChannelInRange = eventsFiredForEachChannel(dataBetweenTimestamps(i, i + MainWindow.settings.ToolsS.integrationPeriod));

                    for (int indx = 0; indx < eventsForEachChannelInRange.Length / 2; indx += 2)
                    {
                        eventosL = eventsForEachChannelInRange[indx] + eventsForEachChannelInRange[indx + 1];
                        eventosR = eventsForEachChannelInRange[indx + frameSizeHeight] + eventsForEachChannelInRange[indx + 1 + frameSizeHeight];

                        float difference = Math.Abs(eventosR - eventosL);
                        #region color scale
                        Byte green, red, blue;

                        if (eventosL > eventosR)
                        {
                            red = (Byte)((255 * (difference / maxValSonogram)));
                            green = (Byte)(0);
                            blue = (Byte)(255 - red);
                        }
                        else if (eventosL < eventosR)
                        {
                            red = (Byte)(0);
                            green = (Byte)((255 * (difference / maxValSonogram)));
                            blue = (Byte)(255 - green);
                        }
                        else
                        {
                            red = (Byte)0;
                            green = (Byte)0;
                            blue = (Byte)255;
                        }

                        color = Color.FromArgb((Byte)red, (Byte)green, (Byte)blue);
                        #endregion

                        for (int rep = 0; rep < tam; rep++)
                        {
                            for (int rep2 = 0; rep2 < tam; rep2++)
                            {
                                Bmp.SetPixel((int)(i / (int)MainWindow.settings.ToolsS.integrationPeriod) + contador + rep, tam * frameSizeHeight / 2 - 1 - rep2 - indx * tam / 2, color); //Paint the pixel with the color, depending on the activity
                            }
                        }
                        sb_csv.Append(difference + ";");
                    }
                    sb_csv.Append("\n");
                    contador += tam - 1;
                }
                Bmp.Save(file + ".png", ImageFormat.Png);

                return true;
            }
        }

        public void closeAedat()
        {
            bReader.Close();
        }

        /// <summary>
        /// Generates splits using the noise tolerance and noise threshold values from the input parameters.
        /// </summary>
        public void AERSplitter(int noiseTolerance, int noiseThreshold)
        {
            int timesThatEnters = 0;
            int eventsFired;
            List<aedatEvent16> lista = new List<aedatEvent16>(aedatFileList);
            bool hasEntered = false;
            List<List<aedatEvent16>> splittedAedat = new List<List<aedatEvent16>>();
            for (int i = 0; i < 200; i++)
            {
                splittedAedat.Add(null);
            }
            List<aedatEvent16> aedatSplit = new List<aedatEvent16>();
            int splitIndex = 0;

            for (long i = 0; i <= maxTimestamp; i += (int)MainWindow.settings.ToolsS.integrationPeriod)
            {
                eventsFired = eventsFiredTotal(dataBetweenTimestamps(i, i + MainWindow.settings.ToolsS.integrationPeriod));

                if (eventsFired < noiseThreshold * 0.0001 * lista.Count)
                {
                    lista.RemoveAll(x => x.timestamp >= i + MainWindow.settings.ToolsS.integrationPeriod && x.timestamp <= i);
                    if (hasEntered == true)
                    {
                        hasEntered = false;
                        aedatSplit.Clear();
                        splitIndex++;

                        if (timesThatEnters < noiseTolerance)
                        {
                            splittedAedat[splitIndex - 1] = null;
                        }
                        timesThatEnters = 0;
                    }
                }
                else
                {
                    aedatSplit.AddRange(lista.FindAll(x => x.timestamp >= i && x.timestamp <= i + MainWindow.settings.ToolsS.integrationPeriod));
                    splittedAedat[splitIndex] = new List<aedatEvent16>(aedatSplit);
                    hasEntered = true;
                    timesThatEnters++;
                }
            }

            if (Directory.Exists(MainWindow.fileName.Split('.')[0]))
            {
                Directory.Delete(MainWindow.fileName.Split('.')[0], true);
                Directory.CreateDirectory(MainWindow.fileName.Split('.')[0]);
            }
            else
            {
                Directory.CreateDirectory(MainWindow.fileName.Split('.')[0]);
            }

            int a = 0;
            foreach (List<aedatEvent16> r in splittedAedat)
            {
                if (r != null)
                {
                    saveAedat("./" + MainWindow.fileName.Split('.')[0] + "/arch" + a + ".aedat", r);
                    a++;
                }
            }
        }

        /// <summary>
        /// Returns the average activity for both cochleae within the [init, end] timestamp range
        /// </summary>
        public double[] averageBetweenTimestamps(long init, long end)
        {
            double[] meanLR = new double[2];
            List<aedatEvent16> listaEventos = dataBetweenTimestamps(init, end);

            int addresses = 256;
            switch (MainWindow.cochleaInfo)
            {
                case EnumCochleaInfo.MONO32: addresses = 64; break;
                case EnumCochleaInfo.MONO64: addresses = 128; break;
                case EnumCochleaInfo.MONO128: addresses = 256; break;
                case EnumCochleaInfo.MONO256: addresses = 512; break;
                case EnumCochleaInfo.STEREO32: addresses = 128; break;
                case EnumCochleaInfo.STEREO64: addresses = 256; break;
                case EnumCochleaInfo.STEREO128: addresses = 512; break;
                case EnumCochleaInfo.STEREO256: addresses = 1024; break;
                default: addresses = 256; break;
            }

            int numElR = 0;
            int numElL = 0;

            for (int i = 0; i < listaEventos.Count; i++)
            {
                if ((MainWindow.cochleaInfo == EnumCochleaInfo.MONO64 || MainWindow.cochleaInfo == EnumCochleaInfo.MONO32 || MainWindow.cochleaInfo == EnumCochleaInfo.MONO128 || MainWindow.cochleaInfo == EnumCochleaInfo.MONO256) && listaEventos[i].timestamp >= init && listaEventos[i].timestamp < end)
                {
                    numElL++;
                }
                else if (MainWindow.cochleaInfo == EnumCochleaInfo.STEREO32 || MainWindow.cochleaInfo == EnumCochleaInfo.STEREO64 || MainWindow.cochleaInfo == EnumCochleaInfo.STEREO128 || MainWindow.cochleaInfo == EnumCochleaInfo.STEREO256)
                {
                    if (listaEventos[i].addr >= addresses / 2 && listaEventos[i].addr < addresses && listaEventos[i].timestamp >= init && listaEventos[i].timestamp < end)
                    {
                        numElR++;
                    }
                    else if (listaEventos[i].addr < addresses / 2 && listaEventos[i].addr >= 0 && listaEventos[i].timestamp >= init && listaEventos[i].timestamp < end)
                    {
                        numElL++;
                    }
                }
            }
            if (numElR != 0)
            {
                meanLR[1] = numElR / (float)(MainWindow.settings.ToolsS.integrationPeriod);
            }
            if (numElL != 0)
            {
                meanLR[0] = numElL / (float)(MainWindow.settings.ToolsS.integrationPeriod);
            }

            return meanLR;
        }

        /// <summary>
        /// Saves a new aedat file with the events within the [init, end] timestamp range to the path chosen by the user
        /// </summary>
        public void AERSplitterManual(int init, int end, String path)
        {
            List<aedatEvent16> res = aedatFileList.FindAll(x => x.timestamp >= init && x.timestamp <= end);
            saveAedat(path, res);
        }

        /// <summary>
        /// Saves the mono information taken from an stereo sound to an aedat file
        /// </summary>
        public void saveStereoToMono(String fileName, List<aedatEvent16> aedat)
        {
            bWriter = new BinaryWriter(File.OpenWrite(fileName));
            UInt16 evt;
            UInt32 timestamp;

            foreach (aedatEvent16 row in aedat)
            {
                if ((MainWindow.cochleaInfo == EnumCochleaInfo.STEREO32 && row.addr < 64) || (MainWindow.cochleaInfo == EnumCochleaInfo.STEREO64 && row.addr < 128) || (MainWindow.cochleaInfo == EnumCochleaInfo.STEREO128 && row.addr < 256) || (MainWindow.cochleaInfo == EnumCochleaInfo.STEREO256 && row.addr < 512))
                {
                    evt = (UInt16)((BitConverter.GetBytes(row.addr)[0] << 8) | BitConverter.GetBytes(row.addr)[1]);
                    timestamp = (UInt32)((BitConverter.GetBytes(row.timestamp)[0] << 24) | BitConverter.GetBytes(row.timestamp)[1] << 16 | BitConverter.GetBytes(row.timestamp)[2] << 8 | BitConverter.GetBytes(row.timestamp)[3]);

                    bWriter.Write(evt);
                    bWriter.Write(timestamp);
                }
            }
            bWriter.Close();
        }
    }
}