//!
//! @file 		LocalExtremaTest.cs
//! @author 	Geoffrey Hunter <gbmhunter@gmail.com> (www.cladlab.com)
//! @edited 	n/a
//! @date 		14/01/2013
//! @brief 		Testing code for the local extrema algorithem.
//! @details
//!		<b>Last Modified:			</b> 16/01/2013			        		\n
//!		<b>Version:					</b> v1.1.0				        		\n
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
//! Version History:
//!     v1.0.0  14/01/2013. First verion
//!     v1.0.1  15/01/2013. Fixed alternate extrema rule bug (it was always enabled)
//!     v1.1.0  16/01/2013. Changed the way the algorithm performs thresholding. Removed
//!                 the windowing mode (now always dynamic. Algorithm works much better for extrema
//!                 detection on low frequency signals.
//!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalProcessing
{
    /// <summary>
    /// Finds local extrems (maxima/minima) of a data set.
    /// 
    /// Features:
    /// - Changeable width sample window
    /// - Adjustable thresholding: Good for preventing noise from triggering many local maxima/minima when the signal
    ///     is relatively constant/unchanging.
    /// - Constant or dynamic windowing modes: Uses the fact that a local maxima must occur after a local minima 
    ///     and vise versa, to increase the window size dynamically. 
    /// - Ability to enforce alternate extrema detection.
    /// 
    /// Todo:
    /// - Fnish writing public int FindLocalExtremaTest(List<double> dataList), which
    /// should fix thresholding window issue.
    /// - Write code to deal with maxima or minima which have 2 or more points of equal value
    /// </summary>
    public class LocalExtremaDetection
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalExtremaDetection()
        {
            // Setup defaults
            this._searchWindowRadius = 5;
            this._thresholdingEnabled = true;
            this._thresholdValue = 20;
        }

        #endregion

        #region Enums

        /// <summary>
        /// Enumeration used to remeber the last discovered extrema when enforcing 
        /// alternate extrema (see <see cref="M:LocalExtremaDetection.EnforceAlternateExtrema(bool trueFalse)"/>).
        /// </summary>
        public enum ExtremaType
        {
            none,
            Maxima,
            Minima
        }

        /// <summary>
        /// Used as a input to <see cref="LocalExtremaDetection.SetWindowingMode(WindowMode windowMode)"/> to determine the windowing mode.
        /// </summary>
        public enum WindowMode
        {
            fixedWidth,
            dynamicWidth
        }

        private enum AnalyseStates
        {
            CheckIsMostExtremeInSampleWindow,
            CheckLeftThreshold,
            CheckRightThreshold,
            IsQualifingExtrema
        }

        #endregion

        #region Variables

        /// <summary>
        /// Determines the window size when looking for maxima/minima. Change by calling <see cref="SetSearchWindowRadius()"/>.
        /// </summary>
        private int _searchWindowRadius;


        private bool _thresholdingEnabled;
        private double _thresholdValue;
        private bool _enforceAlternatingExtrema = false;
        private List<int> _minima = new List<int>();
        private List<int> _maxima = new List<int>();

        #endregion

        #region Methods

        /// <summary>
        /// Sets the window size for checking for maxima/minima.
        /// </summary>
        /// <param name="searchWindowRadius"></param>
        public void SetSearchWindowRadius(int searchWindowRadius)
        {
            // Make sure input is valid
            if (searchWindowRadius <= 0)
                throw new System.IO.InvalidDataException("Search window radius was set to <= 0.");

            this._searchWindowRadius = searchWindowRadius;
        }

        /// <summary>
        /// Enable/disable thresholding.
        /// </summary>
        public void EnableThresholding(bool trueFalse)
        {
            this._thresholdingEnabled = trueFalse;
        }

        /// <summary>
        /// Sets the threshold value. <see cref="EnableThresholding()"/> has to be called for this to have any effect.
        /// </summary>
        /// <param name="thresholdValue">Value to set as the threshold.</param>
        public void SetThreshold(double thresholdValue)
        {
            this._thresholdValue = thresholdValue;
        }

        /// <summary>
        /// Enforce or de-enforce the alternate extrema law. Enforcing this makes sure that
        /// a local maxima is always found after a local minima and vise versa.
        /// </summary>
        /// <param name="trueFalse"></param>
        public void EnforceAlternateExtrema(bool trueFalse)
        {
            this._enforceAlternatingExtrema = trueFalse;
        }

        /// <summary>
        /// Returns the maxima found by calling <see cref="LocalExtremaDetection.FindLocalExtrema(List{double} dataList)"/>.
        /// </summary>
        /// <returns></returns>
        public List<int> GetMaxima()
        {
            return _maxima;
        }

        /// <summary>
        /// Returns the minima found by calling <see cref="LocalExtremaDetection.FindLocalExtrema(List{double} dataList)"/>.
        /// </summary>
        /// <returns></returns>
        public List<int> GetMinima()
        {
            return _minima;
        }

        /// <summary>
        /// Finds local extrema of a given data set.
        /// Performs search windowing and thresholding (if enabled). Offline algorithm. 
        /// Clears any previously found extrema from memory before processing data.
        /// </summary>
        /// <param name="dataList">Data to operate on.</param>
        /// <returns>Number of local extrema found.</returns>
        public int FindLocalExtrema(List<double> dataList)
        {
            ExtremaPoint lastExtremaPoint = new ExtremaPoint();

            // Clear the maxima fields
            _maxima.Clear();
            _minima.Clear();

            // Inspect each element in dataList to determine if its a local maxima/minima
            for (int i = 0; i < dataList.Count; i++)
            {
                ExtremaPoint extremaPoint = this.AnalyseDataPoint(dataList, i, lastExtremaPoint);

                if (extremaPoint != null)
                {
                    if (extremaPoint.extremaType == ExtremaPoint.ExtremaType.Maxima)
                    {
                        _maxima.Add(extremaPoint.index);
                        lastExtremaPoint = extremaPoint;
                    }

                    if (extremaPoint.extremaType == ExtremaPoint.ExtremaType.Minima)
                    {
                        _minima.Add(extremaPoint.index);
                        lastExtremaPoint = extremaPoint;
                    }
                }
            }

            // Return total number of extrema found
            return (_maxima.Count() + _minima.Count());
        }

        

        private ExtremaPoint AnalyseDataPoint(List<double> dataList, int index, ExtremaPoint lastExtremaPoint)
        {

            List<double> range = dataList;

            ExtremaPoint currExtremaPoint = new ExtremaPoint(index, ExtremaPoint.ExtremaType.none);

            // Remove unneeded elements of left-side of window
            range = range.Skip<double>(lastExtremaPoint.index).ToList<double>();

            // Remove unneeded elements of right-side of window. 
            // Need to add 1 because converting from index bounds to number of elements.
            range = range.Take<double>(index + this._searchWindowRadius - lastExtremaPoint.index + 1).ToList<double>();

            AnalyseStates currState = AnalyseStates.CheckIsMostExtremeInSampleWindow;

            while(true)
            {

                switch (currState)
                {
                    case AnalyseStates.CheckIsMostExtremeInSampleWindow:
                         // Make sure range is not empty and if current dataIList is the maximum/minimum then store index
                        if ((range.Count() > 0) && (dataList[index] == range.Max()))
                        {
                            // If enforcing alternate extrema, quit if this is another maxima
                            if ((_enforceAlternatingExtrema == true) && (lastExtremaPoint.extremaType == ExtremaPoint.ExtremaType.Maxima))
                                return null;

                            currExtremaPoint.extremaType = ExtremaPoint.ExtremaType.Maxima;

                            currState = AnalyseStates.CheckLeftThreshold;
                            break;
                        }

                        if ((range.Count() > 0) && (dataList[index] == range.Min()))
                        {
                            // If enforcing alternate extrema, quit if this is another minima
                            if ((_enforceAlternatingExtrema == true) && (lastExtremaPoint.extremaType == ExtremaPoint.ExtremaType.Minima))
                                return null;

                            currExtremaPoint.extremaType = ExtremaPoint.ExtremaType.Minima;

                            currState = AnalyseStates.CheckLeftThreshold;
                            break;
                        }

                        return null;
                    case AnalyseStates.CheckLeftThreshold:
                        // If thresholding is not enabled, data point has already
                        // met qualifying criteria and return true
                        if(_thresholdingEnabled == false)
                            return currExtremaPoint;

                        // Check for special case. If index is 0, there are no
                        // left elements to check for threshold, therefore
                        // it fails this check automatically
                        if (index == 0)
                            return null;

                        // Check for left threshold. This has to be an iterative process
                        for (int j = index - 1; j >= 0; j--)
                        {
                            if (currExtremaPoint.extremaType == ExtremaPoint.ExtremaType.Maxima)
                            {
                                // Check if data climbs back above the previously found maximum,
                                // and if so, quit to next loop iteration
                                if (dataList[j] > range.Max())
                                    return null;

                                // Check if data exceeds low threshold
                                if (dataList[j] - dataList[index] <= -_thresholdValue)
                                {
                                    currState = AnalyseStates.CheckRightThreshold;
                                    break;
                                }
                            }
                            else if (currExtremaPoint.extremaType == ExtremaPoint.ExtremaType.Minima)
                            {
                                // Check if data climbs back above the previously found maximum,
                                // and if so, quit to next loop iteration
                                if (dataList[j] < range.Min())
                                    return null;

                                if (dataList[j] - dataList[index] >= _thresholdValue)
                                {
                                    currState = AnalyseStates.CheckRightThreshold;
                                    break;
                                }
                            }

                            // Check if reached the last data point, and if so, quit
                            if (j == 0)
                                return null;
                        }

                        break;

                    case AnalyseStates.CheckRightThreshold:
                        // Check for special case. If index is 0, there are no
                        // left elements to check for threshold, therefore
                        // it fails this check automatically
                        if (index == dataList.Count - 1)
                            return null;

                         // Check for right threshold. This has to be an iterative process
                        for (int j = index + 1; j <= dataList.Count - 1; j++)
                        {
                            if(currExtremaPoint.extremaType == ExtremaPoint.ExtremaType.Maxima)
                            {
                                // Check if data climbs back above the previously found maximum,
                                // and if so, quit to next loop iteration
                                if (dataList[j] > range.Max())
                                    return null;

                                // Check if data exceeds low threshold
                                if (dataList[j] - dataList[index] <= -_thresholdValue)
                                {
                                    currState = AnalyseStates.IsQualifingExtrema;
                                    break;
                                }
                            }
                            else if(currExtremaPoint.extremaType == ExtremaPoint.ExtremaType.Minima)
                            {
                                // Check if data climbs back above the previously found maximum,
                                // and if so, quit to next loop iteration
                                if (dataList[j] < range.Min())
                                    return null;

                                if (dataList[j] - dataList[index] >= _thresholdValue)
                                {
                                    currState = AnalyseStates.IsQualifingExtrema;
                                    break;
                                }
                            }

                            // Check if reached the last data point, and if so, quit
                            if (j == dataList.Count - 1)
                                return null;
                        }
                        break;

                    case AnalyseStates.IsQualifingExtrema:
                        return currExtremaPoint;
                }

            }
        }


        #endregion

    }

    /// <summary>
    /// Object for one extrema point. Store information such as whether it
    /// is a maxima or minima, and it's index in a data array.
    /// </summary>
    public class ExtremaPoint
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ExtremaPoint()
        {
            this.index = -1;
            this.extremaType = ExtremaType.none;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Data index of extrema.</param>
        /// <param name="extremaType">The type of extrema (maxima or minima).</param>
        public ExtremaPoint(int index, ExtremaType extremaType)
        {
            this.index = index;
            this.extremaType = extremaType;
        }


        public ExtremaType extremaType = ExtremaType.none;
        public int index = -1;

        /// <summary>
        /// Enumeration used to remeber the last discovered extrema when enforcing 
        /// alternate extrema (see <see cref="M:LocalExtremaDetection.EnforceAlternateExtrema(bool trueFalse)"/>).
        /// </summary>
        public enum ExtremaType
        {
            none,
            Maxima,
            Minima
        }

    }

}
