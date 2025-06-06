﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//

using System.Runtime.InteropServices;


namespace System.Windows.Media.Effects
{
    /// <summary>
    /// BlurBitmapEffectPrimitive
    /// </summary>
    public sealed partial class BlurBitmapEffect : BitmapEffect
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BlurBitmapEffect()
        {
        }

        /// <summary>
        /// Creates the unmanaged effect handle
        /// </summary>
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        protected override unsafe SafeHandle CreateUnmanagedEffect()
        {
            return null;
        }

        /// <summary>
        /// Update (propagetes) properties to the unmanaged effect
        /// </summary>    
        [Obsolete(MS.Internal.Media.VisualTreeUtils.BitmapEffectObsoleteMessage)]
        protected override void UpdateUnmanagedPropertyState(SafeHandle unmanagedEffect)
        {
        }

        /// <summary>
        /// An ImageEffect can be used to emulate a BlurBitmapEffect with certain restrictions. This
        /// method returns true when it is possible to emulate the BlurBitmapEffect using an ImageEffect.
        /// </summary>
        internal override bool CanBeEmulatedUsingEffectPipeline()
        {
            return (Radius <= 100.0);
        }

        /// <summary>
        /// Returns a Effect that emulates this BlurBitmapEffect.
        /// </summary>        
        internal override Effect GetEmulatingEffect()
        {
            if (_imageEffectEmulation != null && _imageEffectEmulation.IsFrozen)
            {
                return _imageEffectEmulation;
            }
            
            if (_imageEffectEmulation == null)
            {
                _imageEffectEmulation = new BlurEffect();
            }

            double radius = Radius;
            if (_imageEffectEmulation.Radius != radius)
            {
                _imageEffectEmulation.Radius = radius;
            }

            KernelType kernelType = KernelType;
            if (_imageEffectEmulation.KernelType != kernelType)
            {
                _imageEffectEmulation.KernelType = kernelType;
            }

            _imageEffectEmulation.RenderingBias = RenderingBias.Performance;

            if (this.IsFrozen)
            {
                _imageEffectEmulation.Freeze();
            }
            
            return _imageEffectEmulation;
        }

        private BlurEffect _imageEffectEmulation;
    }
}
