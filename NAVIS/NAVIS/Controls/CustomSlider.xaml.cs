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

namespace NAVIS.Controls
{
    public partial class CustomSlider : UserControl
    {
        #region properties
        int _Value = 0;
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                sliderControl.Value = value;
            }
        }

        int _Maximum = 0;
        public int Maximum
        {
            get { return (int)sliderControl.Maximum; }
            set
            {
                _Maximum = value;
                sliderControl.Maximum = _Maximum;
            }
        }

        int _Minimum = 0;
        public int Minimum
        {
            get { return (int)sliderControl.Minimum; }
            set
            {
                _Minimum = value;
                sliderControl.Minimum = _Minimum;
            }
        }
        #endregion

        public CustomSlider()
        {
            InitializeComponent();
            sliderControl.Value = _Value;
            textBlockControl.Text = _Value.ToString();
        }

        #region events
        public event EventHandler dragStartedScroll;

        public event EventHandler dragCompletedScroll;

        public event EventHandler valueChanged;

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

            if (dragCompletedScroll != null)
            {
                dragCompletedScroll(this, EventArgs.Empty);
            }
        }

        private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

            if (dragStartedScroll != null)
            {
                dragStartedScroll(this, EventArgs.Empty);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (valueChanged != null)
            {
                valueChanged(this, EventArgs.Empty);
            }
            _Value = (int)sliderControl.Value;
            textBlockControl.Text = _Value.ToString();
        }
        #endregion
    }
}
