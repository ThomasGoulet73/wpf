// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//

using System.Windows.Media.Media3D;

namespace System.Windows.Media.Animation
{
    /// <summary>
    ///
    /// </summary>
    public abstract class Int64AnimationBase : AnimationTimeline
    {
        #region Constructors

        /// <Summary>
        /// Creates a new Int64AnimationBase.
        /// </Summary>
        protected Int64AnimationBase()
            : base()
        {
        }

        #endregion

        #region Freezable

        /// <summary>
        /// Creates a copy of this Int64AnimationBase
        /// </summary>
        /// <returns>The copy</returns>
        public new Int64AnimationBase Clone()
        {
            return (Int64AnimationBase)base.Clone();
        }

        #endregion

        #region IAnimation

        /// <summary>
        /// Calculates the value this animation believes should be the current value for the property.
        /// </summary>
        /// <param name="defaultOriginValue">
        /// This value is the suggested origin value provided to the animation
        /// to be used if the animation does not have its own concept of a
        /// start value. If this animation is the first in a composition chain
        /// this value will be the snapshot value if one is available or the
        /// base property value if it is not; otherise this value will be the 
        /// value returned by the previous animation in the chain with an 
        /// animationClock that is not Stopped.
        /// </param>
        /// <param name="defaultDestinationValue">
        /// This value is the suggested destination value provided to the animation
        /// to be used if the animation does not have its own concept of an
        /// end value. This value will be the base value if the animation is
        /// in the first composition layer of animations on a property; 
        /// otherwise this value will be the output value from the previous 
        /// composition layer of animations for the property.
        /// </param>
        /// <param name="animationClock">
        /// This is the animationClock which can generate the CurrentTime or
        /// CurrentProgress value to be used by the animation to generate its
        /// output value.
        /// </param>
        /// <returns>
        /// The value this animation believes should be the current value for the property.
        /// </returns>
        public override sealed object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            // Verify that object arguments are non-null since we are a value type
            ArgumentNullException.ThrowIfNull(defaultOriginValue);
            ArgumentNullException.ThrowIfNull(defaultDestinationValue);
            return GetCurrentValue((Int64)defaultOriginValue, (Int64)defaultDestinationValue, animationClock);
        }

        /// <summary>
        /// Returns the type of the target property
        /// </summary>
        public override sealed Type TargetPropertyType
        {
            get
            {
                ReadPreamble();

                return typeof(Int64);
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Calculates the value this animation believes should be the current value for the property.
        /// </summary>
        /// <param name="defaultOriginValue">
        /// This value is the suggested origin value provided to the animation
        /// to be used if the animation does not have its own concept of a
        /// start value. If this animation is the first in a composition chain
        /// this value will be the snapshot value if one is available or the
        /// base property value if it is not; otherise this value will be the 
        /// value returned by the previous animation in the chain with an 
        /// animationClock that is not Stopped.
        /// </param>
        /// <param name="defaultDestinationValue">
        /// This value is the suggested destination value provided to the animation
        /// to be used if the animation does not have its own concept of an
        /// end value. This value will be the base value if the animation is
        /// in the first composition layer of animations on a property; 
        /// otherwise this value will be the output value from the previous 
        /// composition layer of animations for the property.
        /// </param>
        /// <param name="animationClock">
        /// This is the animationClock which can generate the CurrentTime or
        /// CurrentProgress value to be used by the animation to generate its
        /// output value.
        /// </param>
        /// <returns>
        /// The value this animation believes should be the current value for the property.
        /// </returns>
        public Int64 GetCurrentValue(Int64 defaultOriginValue, Int64 defaultDestinationValue, AnimationClock animationClock)
        {
            ReadPreamble();

            ArgumentNullException.ThrowIfNull(animationClock);

            if (animationClock.CurrentState == ClockState.Stopped)
            {
                return defaultDestinationValue;
            }

            /*
            if (!AnimatedTypeHelpers.IsValidAnimationValueInt64(defaultDestinationValue))
            {
                throw new ArgumentException(
                    SR.Format(
                        SR.Animation_InvalidBaseValue,
                        defaultDestinationValue, 
                        defaultDestinationValue.GetType(), 
                        GetType()),
                        "defaultDestinationValue");
            }
            */

            return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
        }


        /// <summary>
        /// Calculates the value this animation believes should be the current value for the property.
        /// </summary>
        /// <param name="defaultOriginValue">
        /// This value is the suggested origin value provided to the animation
        /// to be used if the animation does not have its own concept of a
        /// start value. If this animation is the first in a composition chain
        /// this value will be the snapshot value if one is available or the
        /// base property value if it is not; otherise this value will be the 
        /// value returned by the previous animation in the chain with an 
        /// animationClock that is not Stopped.
        /// </param>
        /// <param name="defaultDestinationValue">
        /// This value is the suggested destination value provided to the animation
        /// to be used if the animation does not have its own concept of an
        /// end value. This value will be the base value if the animation is
        /// in the first composition layer of animations on a property; 
        /// otherwise this value will be the output value from the previous 
        /// composition layer of animations for the property.
        /// </param>
        /// <param name="animationClock">
        /// This is the animationClock which can generate the CurrentTime or
        /// CurrentProgress value to be used by the animation to generate its
        /// output value.
        /// </param>
        /// <returns>
        /// The value this animation believes should be the current value for the property.
        /// </returns>
        protected abstract Int64 GetCurrentValueCore(Int64 defaultOriginValue, Int64 defaultDestinationValue, AnimationClock animationClock);

        #endregion
    }
}
