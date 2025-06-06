﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace System.Windows.Media.Effects
{
    /// <summary>
    /// BitmapEffect
    /// </summary>
    public abstract partial class BitmapEffect
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        protected BitmapEffect()
        {     
            // STA Requirement
            //
            // Avalon doesn't necessarily require STA, but many components do.  Examples
            // include Cicero, OLE, COM, etc.  So we throw an exception here if the
            // thread is not STA.
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new InvalidOperationException(SR.RequiresSTA);
            }
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// This method is called before calling GetOutput on an effect.
        /// It gives a chance for the managed effect to update the properties
        /// of the unmanaged object.
        /// </summary>
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        protected abstract void UpdateUnmanagedPropertyState(SafeHandle unmanagedEffect);


        /// <summary>
        /// Returns a safe handle to an unmanaged effect clone
        /// </summary>
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        protected abstract unsafe SafeHandle CreateUnmanagedEffect();

        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="effect"> SafeHandle to the unmanaged effect object</param>
        /// <param name="propertyName">Name of the unmanaged property to be set</param>
        /// <param name="value">Object value to set unmanaged property to</param>
        /// <returns></returns>
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        protected static unsafe void SetValue(SafeHandle effect, string propertyName, object value)
        {
        }

        /// <summary>
        /// Creates an IMILBitmapEffect object
        /// </summary>
        /// <returns>IMILBitmapEffect object</returns>
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        protected static unsafe SafeHandle /* IMILBitmapEffect */ CreateBitmapEffectOuter()
        {
            return null;
        }

        /// <summary>
        /// Initializes the IMILBitmapEffect object with the IMILBitmapEffectPrimitive object
        /// </summary>
        /// <param name="outerObject">The IMILBitmapEffect object</param>
        /// <param name="innerObject">The IMILBitmapEffectPrimitive object</param>
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        protected static unsafe void InitializeBitmapEffect(SafeHandle /*IMILBitmapEffect */ outerObject,
                 SafeHandle/* IMILBitmapEffectPrimitive */ innerObject)
        {
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// This returns the output at index 0
        /// </summary>
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        public BitmapSource GetOutput(BitmapEffectInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            // if we don't have the input set, we should not be calling the output property
            if (input.Input == null)
            {
                throw new ArgumentException(SR.Effect_No_InputSource, nameof(input));
            }

            if (input.Input == BitmapEffectInput.ContextInputSource)
            {
                throw new InvalidOperationException(SR.Format(SR.Effect_No_ContextInputSource, null));
            }

            return input.Input.Clone();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// True if the effect can be emulated by the Effect pipeline. Derived classes 
        /// can override this method to indicate that they can be emulated using the ImageEffect
        /// pipeline. If a derived class returns true it needs to also implement the GetEmulatingImageEffect
        /// property to provide an emulating ImageEffect.
        /// </summary>
        internal virtual bool CanBeEmulatedUsingEffectPipeline()
        {
            return false;
        }

        /// <summary>
        /// Derived classes need to return an emulating image effect if they return true from CanBeEmulatedUsingImageEffectPipeline.
        /// </summary>        
        internal virtual Effect GetEmulatingEffect()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

