//!
//! @file 		LocalExtremaTest.cs
//! @author 	Geoffrey Hunter <gbmhunter@gmail.com>
//! @edited 	n/a
//! @date 		14/01/2013
//! @brief 		Testing code for the local extrema algorithem.
//! @details
//!		<b>Last Modified:			</b> 14/01/2013			        		\n
//!		<b>Version:					</b> v1.0.0				        		\n
//!		<b>Company:					</b> CladLabs   				        \n
//!		<b>Project:					</b> Signal Processing: Local Extrema	\n
//!		<b>Language:				</b> C# (.NET)				        	\n
//!		<b>Computer Architecture:	</b> Platform independant	        	\n
//!		<b>Compiler:				</b> C# compiler			        	\n
//! 	<b>uC Model:				</b> n/a					        	\n
//! 	<b>Operating System:		</b> n/a					        	\n
//!		<b>Documentation Format:	</b> Doxygen				        	\n
//!		<b>License:					</b> GPLv3					        	\n
//!
//! Change the values in dataList to change what the algorithm performs extrema checking on.
//!

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlgorithmTestWF
{

    public partial class LocalExtremaTest : Form
    {
        /// <summary>
        /// Data to text extrema algorithm on.
        /// </summary>
        List<double> dataList = new List<double> { 18, 20, 21, 23, 26, 28, 30, 31, 32, 31, 29, 27, 25, 24, 22, 21, 
            16, 19, 20, 21, 22, 23, 24, 24, 26, 27, 27, 27, 28, 29, 30, 33, 29, 29, 29, 34, 35, 37, 31, 20, 19 };

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalExtremaTest()
        {
            InitializeComponent();

            comboBoxWindowingMode.SelectedIndex = 0;
            zedGraph.GraphPane.Title.Text = "Extrema Algorithm Test";
        }

        /// <summary>
        /// Sets up graph colours
        /// </summary>
        /// <param name="zedGraph"></param>
        /// <param name="backgroundColour"></param>
        /// <param name="foreColour"></param>
        private void SetBasicGraphColours(ZedGraph.ZedGraphControl zedGraph, Color backgroundColour, Color foreColour)
        {
            zedGraph.GraphPane.Title.FontSpec.FontColor = foreColour;

            zedGraph.GraphPane.Chart.Fill = new ZedGraph.Fill(backgroundColour);
            zedGraph.GraphPane.Fill = new ZedGraph.Fill(backgroundColour);
            zedGraph.GraphPane.XAxis.Color = foreColour;
            zedGraph.GraphPane.XAxis.Title.FontSpec.FontColor = foreColour;
            zedGraph.GraphPane.XAxis.MajorGrid.Color = foreColour;
            zedGraph.GraphPane.XAxis.MajorTic.Color = foreColour;
            zedGraph.GraphPane.XAxis.MinorTic.Color = foreColour;
            zedGraph.GraphPane.XAxis.MinorGrid.Color = foreColour;
            zedGraph.GraphPane.XAxis.Scale.FontSpec.FontColor = foreColour;

            zedGraph.GraphPane.YAxis.Color = foreColour;
            zedGraph.GraphPane.YAxis.Title.FontSpec.FontColor = foreColour;
            zedGraph.GraphPane.YAxis.MajorGrid.Color = foreColour;
            zedGraph.GraphPane.YAxis.MajorTic.Color = foreColour;
            zedGraph.GraphPane.YAxis.MinorTic.Color = foreColour;
            zedGraph.GraphPane.YAxis.MinorGrid.Color = foreColour;
            zedGraph.GraphPane.YAxis.Scale.FontSpec.FontColor = foreColour;

            zedGraph.GraphPane.Chart.Border.IsVisible = false;
        }

        /// <summary>
        /// Main loop for testing extrema algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            zedGraph.GraphPane.CurveList.Clear();

            // Create maxima/minima finder
            SignalProcessing.LocalExtremaDetection maxMinDetect = new SignalProcessing.LocalExtremaDetection();

            // Set search window size based on UI
            try
            {
                maxMinDetect.SetSearchWindowRadius(Convert.ToInt32(textBoxSearchWindowRadius.Text));
            }
            catch
            {
                maxMinDetect.SetSearchWindowRadius(0);
            }

            // Enable thresholding
            if(checkBoxEnableThresholding.Checked == true)
                maxMinDetect.EnableThresholding(true);
            else
                maxMinDetect.EnableThresholding(false);

            // Set threshold window size based on UI
            try
            {
                maxMinDetect.SetThresholdWindowRadius(Convert.ToInt32(textBoxThresholdWindowRadius.Text));
            }
            catch
            {
                maxMinDetect.SetThresholdWindowRadius(0);
            }

            // Set threshold dataIList based on UI
            try
            {
                maxMinDetect.SetThreshold(Convert.ToDouble(textBoxThresholdValue.Text));
            }
            catch
            {
                maxMinDetect.SetThreshold(0.0);
            }
         
            // Set windowing mode
            if (comboBoxWindowingMode.SelectedItem == "Fixed")
                maxMinDetect.SetWindowingMode(SignalProcessing.LocalExtremaDetection.WindowMode.fixedWidth);
            else if (comboBoxWindowingMode.SelectedItem == "Dynamic")
                maxMinDetect.SetWindowingMode(SignalProcessing.LocalExtremaDetection.WindowMode.dynamicWidth);
            else
                throw new System.IO.InvalidDataException("Invalid windowing mode");

            if(checkBoxEnableAlternateExtremaRule.Checked)
                maxMinDetect.EnforceAlternateExtrema(true);
            else
                maxMinDetect.EnforceAlternateExtrema(false);

            int maxMinCounter = 0;
            int maxMinResetIndex = 0;

            // Loop through each data point for a particular capacitance channel, adding series
            ZedGraph.PointPairList pointPairList = new ZedGraph.PointPairList();

            

            int filterLength;

            try
            {
                filterLength = Convert.ToInt32(textBoxFirFilterLength.Text);
            }
            catch
            {
                filterLength = 1;
            }

            // Filter
            List<double> filteredDataList = SignalProcessing.Averaging.MovingAverage(dataList.ToArray(), filterLength).ToList<double>();

            // Get most recent numberOfPointsToPlot amount of data from the table
            for (int x = 0; x < dataList.Count; x++)
            {
                ZedGraph.PointPair pointPair = new ZedGraph.PointPair();
                pointPair.X = (double)x;
                // Get capacitance
                pointPair.Y = filteredDataList[x];
                //pointPair.Y = dataTable.Rows[x].Field<int>(dataNames[y-1]);
                pointPairList.Add(pointPair);
            }

            // Add line to graph with legend name
            ZedGraph.LineItem lineItem = zedGraph.GraphPane.AddCurve("Test data", pointPairList, Color.Blue);

            // Find local extrema
            labelNumExtrema.Text = Convert.ToString(maxMinDetect.FindLocalExtrema(dataList));

            // Retrieve indecies of extrema
            List<int> maxima = maxMinDetect.GetMaxima();
            List<int> minima = maxMinDetect.GetMinima();

            // Add  maxima markers
            for (int i = 0; i < maxima.Count(); i++)
            {
                ZedGraph.LineItem line = new ZedGraph.LineItem("Point", new double[] { maxima[i] }, new double[] { filteredDataList[maxima[i]] }, Color.Black, ZedGraph.SymbolType.TriangleDown);
                line.Symbol.Size = 20;
                // Colour the same as the line
                line.Symbol.Fill = new ZedGraph.Fill(Color.Green);
                // Don't draw label on legend
                line.Label.IsVisible = false;
                // Add marker to graph
                zedGraph.GraphPane.CurveList.Add(line);
            }
            // Add minima markers
            for (int i = 0; i < minima.Count(); i++)
            {
                ZedGraph.LineItem line = new ZedGraph.LineItem("Point", new double[] { minima[i] }, new double[] { filteredDataList[minima[i]] }, Color.Black, ZedGraph.SymbolType.Triangle);
                line.Symbol.Size = 20;
                // Colour the same as the line
                line.Symbol.Fill = new ZedGraph.Fill(Color.Green);
                // Don't draw label on legend
                line.Label.IsVisible = false;
                // Add marker to graph
                zedGraph.GraphPane.CurveList.Add(line);
            }

            zedGraph.AxisChange();

            this.SetBasicGraphColours(zedGraph, Color.Black, Color.White);

            // Re-draw graph
            zedGraph.Invalidate();
        }

    }
}
