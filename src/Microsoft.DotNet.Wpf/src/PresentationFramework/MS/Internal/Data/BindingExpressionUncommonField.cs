﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
// Description: Defines an UncommonField of type BindingExpression.
//

using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
    /// <summary>
    /// An UncommonField whose type is BindingExpression.
    /// </summary>
    internal class BindingExpressionUncommonField : UncommonField<BindingExpression>
    {
        internal new void SetValue(DependencyObject instance, BindingExpression bindingExpr)
        {
            base.SetValue(instance, bindingExpr);
            bindingExpr.Attach(instance);
        }

        internal new void ClearValue(DependencyObject instance)
        {
            BindingExpression bindingExpr = GetValue(instance);
            bindingExpr?.Detach();
            base.ClearValue(instance);
        }
    }
}
