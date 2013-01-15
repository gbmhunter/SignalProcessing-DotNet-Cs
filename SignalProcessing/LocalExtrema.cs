//!
//! @file 		LocalExtremaTest.cs
//! @author 	Geoffrey Hunter <gbmhunter@gmail.com>
//! @edited 	n/a
//! @date 		14/01/2013
//! @brief 		Testing code for the local extrema algorithem.
//! @details
//!		<b>Last Modified:			</b> 15/01/2013			        		\n
//!		<b>Version:					</b> v1.0.1				        		\n
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
//!     v1.0.0  14/01/2013  First verion
//!     v1.0.1  15/01/2013  Fixed alternate extrema rule bug (it was always enabled)

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
            this._thresholdWindowRadius = this._searchWindowRadius;
            this._thresholdingEnabled = true;
            this._thresholdValue = 20;
        }

        #endregion

        #region Enums

        /// <summary>
        /// Enumeration used to remeber the last discovered extrema when enforcing 
        /// alternate extrema (see <see cref="M:LocalExtremaDetection.EnforceAlternateExtrema(bool trueFalse)"/>).
        /// </summary>
        private enum ExtremaType
        {
            none,
            maxima,
            minima
        }

        /// <summary>
        /// Used as a input to <see cref="LocalExtremaDetection.SetWindowingMode(WindowMode windowMode)"/> to determine the windowing mode.
        /// </summary>
        public enum WindowMode
        {
            fixedWidth,
            dynamicWidth
        }

        #endregion

        #region Variables

        /// <summary>
        /// Determines the window size when looking for maxima/minima. Change by calling <see cref="SetSearchWindowRadius()"/>.
        /// </summary>
        private int _searchWindowRadius;

        /// <summary>
        /// Determines the window size when thresholding.
        /// </summary>
        private int _thresholdWindowRadius;
        private bool _thresholdingEnabled;
        private double _thresholdValue;
        private WindowMode _windowMode;
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
        /// Sets the window size for thresholding. Algorithm looks at the data points x-thresholdWindowRadius and x+thresholdWindowRadius which have to differ
        /// by x by the threshold or more to qualify for being a maxima or minimia. 
        /// The total threshold window width is 2*thresholdWindowRadius + 1.
        /// </summary>
        /// <param name="thresholdWindowRadius">The threshold window radius.</param>
        public void SetThresholdWindowRadius(int thresholdWindowRadius)
        {
            // Make sure input is valid
            if (thresholdWindowRadius <= 0)
                throw new System.IO.InvalidDataException("Threshold window radius was set to <= 0.");

            this._thresholdWindowRadius = thresholdWindowRadius;
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
        /// Sets the windowing mode.
        /// </summary>
        /// <param name="windowMode"></param>
        public void SetWindowingMode(WindowMode windowMode)
        {
            _windowMode = windowMode;
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
            double current;
            List<double> range;

            int leftSampleWindowIndex = 0;
            int rightSampleWindowIndex = 0;

            int leftThresholdIndex = 0;
            int rightThresholdIndex = 0;

            ExtremaType lastFeature = ExtremaType.none;
            int lastFeatureIndex = 0;

            // Clear the maxima fields
            _maxima.Clear();
            _minima.Clear();

            // Inspect each element in dataList to determine if its a local maxima/minima
            for (int i = 0; i < dataList.Count; i++)
            {
                // Calculate new window/threshold values
                switch (_windowMode)
                {
                    // Fixed width implementation. May miss some slow frequency extrema due to thresholding
                    case WindowMode.fixedWidth:
                        leftSampleWindowIndex = i - this._searchWindowRadius;
                        rightSampleWindowIndex = i + this._searchWindowRadius;

                        leftThresholdIndex = i - this._thresholdWindowRadius;
                        rightThresholdIndex = i + this._thresholdWindowRadius;
                        break;
                    // Smarter extrema finding algorithm. 
                    case WindowMode.dynamicWidth:
                        // Left side of window remains at the last feature
                        leftSampleWindowIndex = lastFeatureIndex;
                        // Right window stays the same
                        rightSampleWindowIndex = i + this._searchWindowRadius;

                        leftThresholdIndex = lastFeatureIndex;
                        rightThresholdIndex = i + this._thresholdWindowRadius;
                        break;
                }


                // Store the point to analyse
                current = dataList[i];
                range = dataList;

                // Remove unneeded elements of left-side of window
                range = range.Skip<double>(leftSampleWindowIndex).ToList<double>();

                // Remove unneeded elements of right-side of window. 
                // Need to add 1 because converting from index bounds to number of elements.
                range = range.Take<double>(rightSampleWindowIndex - leftSampleWindowIndex + 1).ToList<double>();

                // Make sure range is not empty and if current dataIList is the maximum/minimum then store index
                if ((range.Count() > 0) && (current == range.Max()))
                {
                    if ((_enforceAlternatingExtrema == true) && (lastFeature == ExtremaType.maxima))
                        continue;

                    // If thresholding is enabled, perform threshold checks
                    if (_thresholdingEnabled) 
                    {
                        // Check if index is too close to data edges for thresholding
                        if (leftThresholdIndex < 0)
                            leftThresholdIndex = 0;

                        // Check if index is too close to data edges for thresholding
                        if (rightThresholdIndex > dataList.Count - 1)
                            rightThresholdIndex = dataList.Count - 1;

                        // Check for one value on each side of index within threshold range that is lower than threshold index.
                        // if greater than positive threshold, if not, do not add to maximaMinimaList
                        List<double> leftThresholdRange = dataList.GetRange(leftThresholdIndex, i - leftThresholdIndex);
                        List<double> rightThresholdRange = dataList.GetRange(i, rightThresholdIndex - i);

                        // Make sure there are elements, if not, then skip this point
                        if ((leftThresholdRange.Count == 0) || (rightThresholdRange.Count == 0))
                            continue;

                        if (!((dataList[i] - leftThresholdRange.Min() >= _thresholdValue) && (dataList[i] - rightThresholdRange.Min() >= _thresholdValue)))
                            continue;
                    }
                   
                    // Maxima discovered, add to list
                    _maxima.Add(i);

                    // Remeber that last feature was maxima
                    lastFeature = ExtremaType.maxima;
                    lastFeatureIndex = i;
                }
                else if ((range.Count() > 0) && (current == range.Min()))
                {
                    if ((_enforceAlternatingExtrema == true) && (lastFeature == ExtremaType.minima))
                        continue;

                    // If thresholding is enabled, perform threshold checks
                    if (_thresholdingEnabled)
                    {
                        // Check if index is out of bounds, and limit if so
                        if (leftThresholdIndex < 0)
                            leftThresholdIndex = 0;

                        // Check if index is out of bounds, and limit if so
                        if (rightThresholdIndex > dataList.Count - 1)
                            rightThresholdIndex = dataList.Count - 1;


                        // Check for one value on each side of index within threshold range that is lower than threshold index.
                        // if greater than positive threshold, if not, do not add to maximaMinimaList
                        List<double> leftThresholdRange = dataList.GetRange(leftThresholdIndex, i - leftThresholdIndex);
                        List<double> rightThresholdRange = dataList.GetRange(i, rightThresholdIndex - i);

                        // Make sure there are elements, if not, then skip this point
                        if ((leftThresholdRange.Count == 0) || (rightThresholdRange.Count == 0))
                            continue;

                        if (!((dataList[i] - leftThresholdRange.Max() <= -_thresholdValue) && (dataList[i] - rightThresholdRange.Max() <= -_thresholdValue)))
                            continue;
                    }
     
                    // Minima discovered, so add to list
                    _minima.Add(i);

                    // Remeber that last feature was minima
                    lastFeature = ExtremaType.minima;
                    lastFeatureIndex = i;
                }
            }

            // Return total number of extrema found
            return (_maxima.Count() + _minima.Count());
        }

        /// <summary>
        /// In development! Do not expect this algorithm to work correctly!
        /// </summary>
        /// <param name="dataList">Data to operate on.</param>
        /// <returns>Number of local extrema found.</returns>
        public int FindLocalExtremaTest(List<double> dataList)
        {
            double current;
            List<double> range;

            int leftSampleWindowIndex = 0;
            int rightSampleWindowIndex = 0;

            int leftThresholdIndex = 0;
            int rightThresholdIndex = 0;

            ExtremaType lastFeature = ExtremaType.none;
            int lastFeatureIndex = 0;

            // Clear the maxima fields
            _maxima.Clear();
            _minima.Clear();

            // Inspect each element in dataList to determine if its a local maxima/minima
            for (int i = 0; i < dataList.Count; i++)
            {
                // Calculate new window/threshold values
                switch (_windowMode)
                {
                    // Fixed width implementation. May miss some slow frequency extrema due to thresholding
                    case WindowMode.fixedWidth:
                        leftSampleWindowIndex = i - this._searchWindowRadius;
                        rightSampleWindowIndex = i + this._searchWindowRadius;

                        leftThresholdIndex = i - this._thresholdWindowRadius;
                        rightThresholdIndex = i + this._thresholdWindowRadius;
                        break;
                    // Smarter extrema finding algorithm. 
                    case WindowMode.dynamicWidth:
                        // Left side of window remains at the last feature
                        leftSampleWindowIndex = lastFeatureIndex;
                        // Right window stays the same
                        rightSampleWindowIndex = i + this._searchWindowRadius;

                        leftThresholdIndex = lastFeatureIndex;
                        rightThresholdIndex = i + this._thresholdWindowRadius;
                        break;
                }


                // Store the point to analyse
                current = dataList[i];
                range = dataList;

                // Remove unneeded elements of left-side of window
                range = range.Skip<double>(leftSampleWindowIndex).ToList<double>();

                // Remove unneeded elements of right-side of window. 
                // Need to add 1 because converting from index bounds to number of elements.
                range = range.Take<double>(rightSampleWindowIndex - leftSampleWindowIndex + 1).ToList<double>();

                // Make sure range is not empty and if current dataIList is the maximum/minimum then store index
                if ((range.Count() > 0) && (current == range.Max()))
                {
                    if ((_enforceAlternatingExtrema == true) && (lastFeature == ExtremaType.maxima))
                        continue;

                    // If thresholding is enabled, perform threshold checks
                    if (_thresholdingEnabled) 
                    {
                        // Check if index is too close to data edges for thresholding
                        if (leftThresholdIndex < 0)
                            leftThresholdIndex = 0;

                        // Check for right threshold
                        for (int j = i + 1; j == dataList.Count - 1; j++)
                        {
                            // Check if data climbs back above the previously found maximum,
                            // and if so, quit to next loop iteration
                            if (dataList[j] > range.Max())
                                goto END;

                            // Check if data exceeds low threshold
                            if (dataList[j] <= range.Max() - _thresholdValue)
                            {
                                goto MAXIMA_FOUND;
                            }
                           
                        }

                        // Check if index is too close to data edges for thresholding
                        if (rightThresholdIndex > dataList.Count - 1)
                            rightThresholdIndex = dataList.Count - 1;

                        // Check for one value on each side of index within threshold range that is lower than threshold index.
                        // if greater than positive threshold, if not, do not add to maximaMinimaList
                        List<double> leftThresholdRange = dataList.GetRange(leftThresholdIndex, i - leftThresholdIndex);
                        List<double> rightThresholdRange = dataList.GetRange(i, (dataList.Count - 1) - i);

                        // Make sure there are elements, if not, then skip this point
                        if ((leftThresholdRange.Count == 0) || (rightThresholdRange.Count == 0))
                            continue;

                        if (!((dataList[i] - leftThresholdRange.Min() >= _thresholdValue) && (dataList[i] - rightThresholdRange.Min() >= _thresholdValue)))
                            continue;
                    }

                    MAXIMA_FOUND:
                   
                    // Maxima discovered, add to list
                    _maxima.Add(i);

                    // Remeber that last feature was maxima
                    lastFeature = ExtremaType.maxima;
                    lastFeatureIndex = i;
                }
                else if ((range.Count() > 0) && (current == range.Min()))
                {
                    if ((_enforceAlternatingExtrema == true) && (lastFeature == ExtremaType.minima))
                        continue;

                    // If thresholding is enabled, perform threshold checks
                    if (_thresholdingEnabled)
                    {
                        // Check if index is out of bounds, and limit if so
                        if (leftThresholdIndex < 0)
                            leftThresholdIndex = 0;

                        // Check if index is out of bounds, and limit if so
                        if (rightThresholdIndex > dataList.Count - 1)
                            rightThresholdIndex = dataList.Count - 1;


                        // Check for one value on each side of index within threshold range that is lower than threshold index.
                        // if greater than positive threshold, if not, do not add to maximaMinimaList
                        List<double> leftThresholdRange = dataList.GetRange(leftThresholdIndex, i - leftThresholdIndex);
                        List<double> rightThresholdRange = dataList.GetRange(i, rightThresholdIndex - i);

                        // Make sure there are elements, if not, then skip this point
                        if ((leftThresholdRange.Count == 0) || (rightThresholdRange.Count == 0))
                            continue;

                        if (!((dataList[i] - leftThresholdRange.Max() <= -_thresholdValue) && (dataList[i] - rightThresholdRange.Max() <= -_thresholdValue)))
                            continue;
                    }
     
                    // Minima discovered, so add to list
                    _minima.Add(i);

                    // Remeber that last feature was minima
                    lastFeature = ExtremaType.minima;
                    lastFeatureIndex = i;
                }
                END:
                    Console.WriteLine("test");
            }

            // Return total number of extrema found
            return (_maxima.Count() + _minima.Count());
        }


        #endregion

    }
}
