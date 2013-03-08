using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SignalProcessing
{
    /// <summary>
    /// This class is used for wrap-around detection (also called overflow detection). 
    /// Algorithm is online, call Run() everytime you measure the variable. Finds the shortest 
    /// distance between two successive measurements and determines whether wrap-around would make
    /// this occur.
    /// </summary>
    public class WrapAroundDetector
    {
        #region Public Enums

        /// <summary>
        /// Type returned when Run() is called. Descriptions added so an extension method can
        /// be added to return text (e.g. myString = myEnum.Description();)
        /// </summary>
        public enum WrapType
        {
            [Description("None")]
            None,
            [Description("Forward Wrap")]
            ForwardWrap,
            [Description("Reverse Wrap")]
            ReverseWrap
        }

        #endregion

        #region Parameters

        /// <summary>
        /// Backing field
        /// </summary>
        private double _maxValue;
        /// <summary>
        /// The maximum value the variable can take on before it wraps back to 0. 
        /// This is also set when calling the constructor.
        /// </summary>
        public double MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }

        /// <summary>
        /// Backing field
        /// </summary>
        private WrapType _result;
        /// <summary>
        /// The last wrap-around result. This is calcultaed when Run() is called.
        /// Also returned by run.
        /// </summary>
        public WrapType Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        #endregion

        #region Private Variables

        /// <summary>
        /// Used to remember the previous value sent to Run(). The difference between this and
        /// the current value is compared.
        /// </summary>
        double _prevValue = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of WrapAroundDetector. Forces you to provide a maximum value,
        /// so that you don't forget to set it. maxValue can also be set via the property MaxValue.
        /// </summary>
        /// <param name="maxValue">The maximum value the variable can take on before it wraps back to 0.</param>
        public WrapAroundDetector(double maxValue)
        {
            _maxValue = maxValue;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs wrap around detection on the current value. 
        /// Algorithm is online, call this function everytime you measure the variable.
        /// </summary>
        /// <param name="value">Value to perform wrap detection on.</param>
        /// <returns>The result of the algorithm. The parameter Result also holds this value.</returns>
        public WrapType Run(double value)
        {

            double distance1;
            double distance2;

            if (value > _prevValue)
            {
                distance1 = value - _prevValue;
                distance2 = _prevValue + (_maxValue - value);

                if (distance2 < distance1)
                    // Data most likely negatively overflowed back to the end
                    Result = WrapType.ReverseWrap;
                else
                    Result = WrapType.None;
            }
            else
            {
                distance1 = _prevValue - value;
                distance2 = value + (_maxValue - _prevValue);

                if (distance2 < distance1)
                    // Data most likely positively overflowed back to begining
                    Result = WrapType.ForwardWrap;
                else
                    Result = WrapType.None;
            }

            // Remember value for next time Run() is called
            _prevValue = value;
            return Result;

        }

        #endregion

    }

}
