//!
//! @file 		MovingAverage.cs
//! @author 	Geoffrey Hunter <gbmhunter@gmail.com>
//! @edited 	n/a
//! @date 		14/01/2013
//! @brief 		Calculates averages from data.
//! @details
//!		<b>Last Modified:			</b> 14/01/2013			        		\n
//!		<b>Version:					</b> v1.0.0				        		\n
//!		<b>Company:					</b> CladLabs   				        \n
//!		<b>Project:					</b> Signal Processing: Averaging   	\n
//!		<b>Language:				</b> C# (.NET)				        	\n
//!		<b>Computer Architecture:	</b> Platform independant	        	\n
//!		<b>Compiler:				</b> C# compiler			        	\n
//! 	<b>uC Model:				</b> n/a					        	\n
//! 	<b>Operating System:		</b> n/a					        	\n
//!		<b>Documentation Format:	</b> Doxygen				        	\n
//!		<b>License:					</b> GPLv3					        	\n
//!
//! Dependances: 
//!     - MathNet NeoDym library

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet;

namespace SignalProcessing
{
    /// <summary>
    /// Dependances: Requires the MathNet NeoDym library
    /// </summary>
    public class Averaging
    {
        /// <summary>
        /// Offline algorithm. Implements a FIR filter. Moving window incorporates windowSize previous elements up to the current element.
        /// Each element in the window has the same weighting (simple averaging).
        /// </summary>
        /// <param name="dataArray">Data to filter</param>
        /// <param name="windowSize">Number of elements to average</param>
        /// <returns>Filtered data</returns>
        public static double[] MovingAverage(double[] dataArray, int windowSize)
        {
            if (windowSize <= 0)
                throw new System.IO.InvalidDataException("Sliding average window size was less or equal to 0.");

            // Create coefficients
            IList<double> coefficients = new List<double>();
            for (int i = 0; i < windowSize; i++)
            {
                // coefficient = 1/searchWindowRadius
                coefficients.Add(1.0 / (double)windowSize);
            }

            // Create filter with required coefficients
            MathNet.SignalProcessing.Filter.FIR.OnlineFirFilter filter = new MathNet.SignalProcessing.Filter.FIR.OnlineFirFilter(coefficients);

            // Perform filtering and return result
            return filter.ProcessSamples(dataArray);
        }
    }
}
