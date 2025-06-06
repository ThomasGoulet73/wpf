// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
// Description: TextViewBase is a minimal base class, providing only 
//              the functionality common across TextViews.
// 

using System.Collections.ObjectModel;       // ReadOnlyCollection
using System.Windows;                       // Point, Rect, ...
using System.Windows.Controls.Primitives;   // DocumentPageView
using System.Windows.Documents;             // ITextView, ITextContainer
using System.Windows.Media;                 // VisualTreeHelper

namespace MS.Internal.Documents
{
    /// <summary>
    /// TextViewBase is a minimal base class, providing only the functionality 
    /// common across TextViews.
    /// </summary>
    internal abstract class TextViewBase : ITextView
    {
        //-------------------------------------------------------------------
        //
        //  Internal Methods
        //
        //-------------------------------------------------------------------

        #region Internal Methods

        /// <summary>
        /// <see cref="ITextView.GetTextPositionFromPoint"/>
        /// </summary>
        internal abstract ITextPointer GetTextPositionFromPoint(Point point, bool snapToText);

        /// <summary>
        /// <see cref="ITextView.GetRectangleFromTextPosition"/>
        /// </summary>
        /// <remarks>
        /// Calls GetRawRectangleFromTextPosition to get a rect and a transform, and applies the transform to the
        /// rect.
        /// </remarks>
        internal virtual Rect GetRectangleFromTextPosition(ITextPointer position)
        {
            Transform transform;
            Rect rect = GetRawRectangleFromTextPosition(position, out transform);
            // Transform must not be null. TextViews returning no transform should return identity
            Invariant.Assert(transform != null);
            if (rect != Rect.Empty)
            {
                rect = transform.TransformBounds(rect);
            }
            return rect;
        }

        /// <summary>
        /// <see cref="ITextView.GetRawRectangleFromTextPosition"/>
        /// </summary>
        internal abstract Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform);

        /// <summary>
        /// <see cref="ITextView.GetTightBoundingGeometryFromTextPositions"/>
        /// </summary>
        internal abstract Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition);

        /// <summary>
        /// <see cref="ITextView.GetPositionAtNextLine"/>
        /// </summary>
        internal abstract ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved);

        /// <summary>
        /// <see cref="ITextView.GetPositionAtNextLine"/>
        /// </summary>
        internal virtual ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
        {
            newSuggestedOffset = suggestedOffset;
            pagesMoved = 0;
            return position;
        }

        /// <summary>
        /// <see cref="ITextView.IsAtCaretUnitBoundary"/>
        /// </summary>
        internal abstract bool IsAtCaretUnitBoundary(ITextPointer position);

        /// <summary>
        /// <see cref="ITextView.GetNextCaretUnitPosition"/>
        /// </summary>
        internal abstract ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction);

        /// <summary>
        /// <see cref="ITextView.GetBackspaceCaretUnitPosition"/>
        /// </summary>
        internal abstract ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position);

        /// <summary>
        /// <see cref="ITextView.GetLineRange"/>
        /// </summary>
        internal abstract TextSegment GetLineRange(ITextPointer position);

        /// <summary>
        /// <see cref="ITextView.GetGlyphRuns"/>
        /// </summary>
        internal virtual ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
        {
            // Verify that layout information is valid. Cannot continue if not valid.
            if (!IsValid)
            {
                throw new InvalidOperationException(SR.TextViewInvalidLayout);
            }
            return ReadOnlyCollection<GlyphRun>.Empty;
        }

        /// <summary>
        /// <see cref="ITextView.Contains"/>
        /// </summary>
        internal abstract bool Contains(ITextPointer position);

        /// <summary>
        /// Scroll the given rectangle the minimum amount required to bring it entirely into view.
        /// </summary>
        /// <param name="textView">TextView doing the scrolling</param>
        /// <param name="rect">Rect to scroll</param>
        /// <remarks>
        /// # RECT POSITION       RECT SIZE        SCROLL      REMEDY
        /// 1 Above viewport      lte viewport     Down        Align top edge of rect and viewport
        /// 2 Above viewport      gt viewport      Down        Align bottom edge of rect and viewport
        /// 3 Below viewport      lte viewport     Up          Align bottom edge of rect and viewport
        /// 4 Below viewport      gt viewport      Up          Align top edge of rect and viewport
        /// 5 Entirely within viewport             NA          No scroll.
        /// 6 Spanning viewport                    NA          No scroll.
        /// </remarks>
        internal static void BringRectIntoViewMinimally(ITextView textView, Rect rect)
        {
            IScrollInfo isi = textView.RenderScope as IScrollInfo;
            if (isi != null)
            {
                // Initialize the viewport
                Rect viewport = new Rect(isi.HorizontalOffset, isi.VerticalOffset, isi.ViewportWidth, isi.ViewportHeight);
                rect.X += viewport.X;
                rect.Y += viewport.Y;

                // Compute the offsets required to minimally scroll the child maximally into view.
                double minX = System.Windows.Controls.ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(viewport.Left, viewport.Right, rect.Left, rect.Right);
                double minY = System.Windows.Controls.ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(viewport.Top, viewport.Bottom, rect.Top, rect.Bottom);

                // We have computed the scrolling offsets; scroll to them.
                isi.SetHorizontalOffset(minX);
                isi.SetVerticalOffset(minY);

                // Adjust rect to reflect changes and allow outer IScrollInfos to react
                FrameworkElement frameworkParent = FrameworkElement.GetFrameworkParent(textView.RenderScope) as FrameworkElement;
                if (frameworkParent != null)
                {
                    if (isi.ViewportWidth > 0)
                    {
                        rect.X -= minX;
                    }
                    if (isi.ViewportHeight > 0)
                    {
                        rect.Y -= minY;
                    }
                    frameworkParent.BringIntoView(rect);
                }
            }
            else
            {
                ((FrameworkElement)textView.RenderScope).BringIntoView(rect);
            }
        }

        /// <summary>
        /// <see cref="ITextView.BringPositionIntoViewAsync"/>
        /// </summary>
        internal virtual void BringPositionIntoViewAsync(ITextPointer position, object userState)
        {
            // Verify that layout information is valid. Cannot continue if not valid.
            if (!IsValid)
            {
                throw new InvalidOperationException(SR.TextViewInvalidLayout);
            }
            OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(position, Contains(position), null, false, userState));
        }

        /// <summary>
        /// <see cref="ITextView.BringPointIntoViewAsync"/>
        /// </summary>
        internal virtual void BringPointIntoViewAsync(Point point, object userState)
        {
            ITextPointer position;

            // Verify that layout information is valid. Cannot continue if not valid.
            if (!IsValid)
            {
                throw new InvalidOperationException(SR.TextViewInvalidLayout);
            }
            position = GetTextPositionFromPoint(point, true);
            OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(point, position, position != null, null, false, userState));
        }

        /// <summary>
        /// <see cref="ITextView.BringLineIntoViewAsync"/>
        /// </summary>
        internal virtual void BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
        {
            ITextPointer newPosition;
            double newSuggestedX;
            int linesMoved;

            // Verify that layout information is valid. Cannot continue if not valid.
            if (!IsValid)
            {
                throw new InvalidOperationException(SR.TextViewInvalidLayout);
            }
            newPosition = GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
            OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(position, suggestedX, count, newPosition, newSuggestedX, linesMoved, linesMoved == count, null, false, userState));
        }

        /// <summary>
        /// <see cref="ITextView.BringPageIntoViewAsync"/>
        /// </summary>
        internal virtual void BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
        {
            ITextPointer newPosition;
            Point newSuggestedOffset;
            int pagesMoved;

            // Verify that layout information is valid. Cannot continue if not valid.
            if (!IsValid)
            {
                throw new InvalidOperationException(SR.TextViewInvalidLayout);
            }
            newPosition = GetPositionAtNextPage(position, suggestedOffset, count, out newSuggestedOffset, out pagesMoved);
            OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(position, suggestedOffset, count, newPosition, newSuggestedOffset, pagesMoved, pagesMoved == count, null, false, userState));
        }

        /// <summary>
        /// <see cref="ITextView.CancelAsync"/>
        /// </summary>
        internal virtual void CancelAsync(object userState)
        {
        }

        /// <summary>
        /// <see cref="ITextView.Validate()"/>
        /// </summary>
        internal virtual bool Validate()
        {
            return this.IsValid;
        }

        /// <summary>
        /// <see cref="ITextView.Validate(Point)"/>
        /// </summary>
        internal virtual bool Validate(Point point)
        {
            return Validate();
        }

        /// <summary>
        /// <see cref="ITextView.Validate(ITextPointer)"/>
        /// </summary>
        internal virtual bool Validate(ITextPointer position)
        {
            Validate();
            return (this.IsValid && this.Contains(position));
        }

        /// <summary>
        /// <see cref="ITextView.ThrottleBackgroundTasksForUserInput"/>
        /// </summary>
        internal virtual void ThrottleBackgroundTasksForUserInput()
        {
        }

        #endregion Internal Methods

        //-------------------------------------------------------------------
        //
        //  Internal Properties
        //
        //-------------------------------------------------------------------

        #region Internal Properties

        /// <summary>
        /// <see cref="ITextView.RenderScope"/>
        /// </summary>
        internal abstract UIElement RenderScope { get; }

        /// <summary>
        /// <see cref="ITextView.TextContainer"/>
        /// </summary>
        internal abstract ITextContainer TextContainer { get; }

        /// <summary>
        /// <see cref="ITextView.IsValid"/>
        /// </summary>
        internal abstract bool IsValid { get; }

        /// <summary>
        /// <see cref="ITextView.RendersOwnSelection"/>
        /// </summary>
        internal virtual bool RendersOwnSelection
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// <see cref="ITextView.TextSegments"/>
        /// </summary>
        internal abstract ReadOnlyCollection<TextSegment> TextSegments { get; }

        #endregion Internal Properties

        //-------------------------------------------------------------------
        //
        //  Public Events
        //
        //-------------------------------------------------------------------

        #region Public Events

        /// <summary>
        /// <see cref="ITextView.BringPositionIntoViewCompleted"/>
        /// </summary>
        public event BringPositionIntoViewCompletedEventHandler BringPositionIntoViewCompleted;

        /// <summary>
        /// <see cref="ITextView.BringPointIntoViewCompleted"/>
        /// </summary>
        public event BringPointIntoViewCompletedEventHandler BringPointIntoViewCompleted;

        /// <summary>
        /// <see cref="ITextView.BringLineIntoViewCompleted"/>
        /// </summary>
        public event BringLineIntoViewCompletedEventHandler BringLineIntoViewCompleted;

        /// <summary>
        /// <see cref="ITextView.BringPageIntoViewCompleted"/>
        /// </summary>
        public event BringPageIntoViewCompletedEventHandler BringPageIntoViewCompleted;

        /// <summary>
        /// <see cref="ITextView.Updated"/>
        /// </summary>
        public event EventHandler Updated;

        #endregion Public Events

        //-------------------------------------------------------------------
        //
        //  Protected Methods
        //
        //-------------------------------------------------------------------

        #region Protected Methods

        /// <summary>
        /// Fires BringPositionIntoViewCompleted event.
        /// </summary>
        /// <param name="e">Event arguments for the BringPositionIntoViewCompleted event.</param>
        protected virtual void OnBringPositionIntoViewCompleted(BringPositionIntoViewCompletedEventArgs e)
        {
            if (this.BringPositionIntoViewCompleted != null)
            {
                this.BringPositionIntoViewCompleted(this, e);
            }
        }

        /// <summary>
        /// Fires BringPointIntoViewCompleted event.
        /// </summary>
        /// <param name="e">Event arguments for the BringPointIntoViewCompleted event.</param>
        protected virtual void OnBringPointIntoViewCompleted(BringPointIntoViewCompletedEventArgs e)
        {
            if (this.BringPointIntoViewCompleted != null)
            {
                this.BringPointIntoViewCompleted(this, e);
            }
        }

        /// <summary>
        /// Fires BringLineIntoViewCompleted event.
        /// </summary>
        /// <param name="e">Event arguments for the BringLineIntoViewCompleted event.</param>
        protected virtual void OnBringLineIntoViewCompleted(BringLineIntoViewCompletedEventArgs e)
        {
            if (this.BringLineIntoViewCompleted != null)
            {
                this.BringLineIntoViewCompleted(this, e);
            }
        }

        /// <summary>
        /// Fires BringPageIntoViewCompleted event.
        /// </summary>
        /// <param name="e">Event arguments for the BringPageIntoViewCompleted event.</param>
        protected virtual void OnBringPageIntoViewCompleted(BringPageIntoViewCompletedEventArgs e)
        {
            if (this.BringPageIntoViewCompleted != null)
            {
                this.BringPageIntoViewCompleted(this, e);
            }
        }

        /// <summary>
        /// Fires Updated event.
        /// </summary>
        /// <param name="e">Event arguments for the Updated event.</param>
        protected virtual void OnUpdated(EventArgs e)
        {
            if (this.Updated != null)
            {
                this.Updated(this, e);
            }
        }

        /// <summary>
        /// Returns aggregate of two transforms
        /// </summary>
        /// <remarks>
        /// If either transform is identity, aggregation is not needed and the other transform is returned.
        /// Otherwise returns a matrix transform whose value is the product of first and second transform values.
        /// </remarks>
        protected virtual Transform GetAggregateTransform(Transform firstTransform, Transform secondTransform)
        {
            Invariant.Assert(firstTransform != null);
            Invariant.Assert(secondTransform != null);

            if (firstTransform.IsIdentity)
            {
                // First transform is Identity. No aggregation needed. Return second transform.
                return secondTransform;
            }
            else if (secondTransform.IsIdentity)
            {
                // Second transform is Identity. No aggregation needed. Return first transform.
                return firstTransform;
            }

            // Both transforms are non-identity. Return matrix transform that is the product of firstTransform * secondTransform
            Transform transform = new MatrixTransform(firstTransform.Value * secondTransform.Value);
            return transform;
        }

        #endregion Protected Methods

        //-------------------------------------------------------------------
        //
        //  ITextView
        //
        //-------------------------------------------------------------------

        #region ITextView

        /// <summary>
        /// <see cref="ITextView.GetTextPositionFromPoint"/>
        /// </summary>
        ITextPointer ITextView.GetTextPositionFromPoint(Point point, bool snapToText)
        {
            return GetTextPositionFromPoint(point, snapToText);
        }

        /// <summary>
        /// <see cref="ITextView.GetRectangleFromTextPosition"/>
        /// </summary>
        Rect ITextView.GetRectangleFromTextPosition(ITextPointer position)
        {
            return GetRectangleFromTextPosition(position);
        }

        /// <summary>
        /// <see cref="ITextView.GetRawRectangleFromTextPosition"/>
        /// </summary>
        Rect ITextView.GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
        {
            return GetRawRectangleFromTextPosition(position, out transform);
        }

        /// <summary>
        /// <see cref="ITextView.GetTightBoundingGeometryFromTextPositions"/>
        /// </summary>
        Geometry ITextView.GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
        {
            return GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
        }

        /// <summary>
        /// <see cref="ITextView.GetPositionAtNextLine"/>
        /// </summary>
        ITextPointer ITextView.GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
        {
            return GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
        }

        /// <summary>
        /// <see cref="ITextView.GetPositionAtNextPage"/>
        /// </summary>
        ITextPointer ITextView.GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
        {
            return GetPositionAtNextPage(position, suggestedOffset, count, out newSuggestedOffset, out pagesMoved);
        }

        /// <summary>
        /// <see cref="ITextView.IsAtCaretUnitBoundary"/>
        /// </summary>
        bool ITextView.IsAtCaretUnitBoundary(ITextPointer position)
        {
            return IsAtCaretUnitBoundary(position);
        }

        /// <summary>
        /// <see cref="ITextView.GetNextCaretUnitPosition"/>
        /// </summary>
        ITextPointer ITextView.GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
        {
            return GetNextCaretUnitPosition(position, direction);
        }

        /// <summary>
        /// <see cref="ITextView.GetBackspaceCaretUnitPosition"/>
        /// </summary>
        ITextPointer ITextView.GetBackspaceCaretUnitPosition(ITextPointer position)
        {
            return GetBackspaceCaretUnitPosition(position);
        }

        /// <summary>
        /// <see cref="ITextView.GetLineRange"/>
        /// </summary>
        TextSegment ITextView.GetLineRange(ITextPointer position)
        {
            return GetLineRange(position);
        }

        /// <summary>
        /// <see cref="ITextView.GetGlyphRuns"/>
        /// </summary>
        ReadOnlyCollection<GlyphRun> ITextView.GetGlyphRuns(ITextPointer start, ITextPointer end)
        {
            return GetGlyphRuns(start, end);
        }

        /// <summary>
        /// <see cref="ITextView.Contains"/>
        /// </summary>
        bool ITextView.Contains(ITextPointer position)
        {
            return Contains(position);
        }

        /// <summary>
        /// <see cref="ITextView.BringPositionIntoViewAsync"/>
        /// </summary>
        void ITextView.BringPositionIntoViewAsync(ITextPointer position, object userState)
        {
            BringPositionIntoViewAsync(position, userState);
        }

        /// <summary>
        /// <see cref="ITextView.BringPointIntoViewAsync"/>
        /// </summary>
        void ITextView.BringPointIntoViewAsync(Point point, object userState)
        {
            BringPointIntoViewAsync(point, userState);
        }

        /// <summary>
        /// <see cref="ITextView.BringLineIntoViewAsync"/>
        /// </summary>
        void ITextView.BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
        {
            BringLineIntoViewAsync(position, suggestedX, count, userState);
        }

        /// <summary>
        /// <see cref="ITextView.BringLineIntoViewAsync"/>
        /// </summary>
        void ITextView.BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
        {
            BringPageIntoViewAsync(position, suggestedOffset, count, userState);
        }

        /// <summary>
        /// <see cref="ITextView.CancelAsync"/>
        /// </summary>
        void ITextView.CancelAsync(object userState)
        {
            CancelAsync(userState);
        }

        /// <summary>
        /// <see cref="ITextView.Validate()"/>
        /// </summary>
        bool ITextView.Validate()
        {
            return Validate();
        }

        /// <summary>
        /// <see cref="ITextView.Validate(Point)"/>
        /// </summary>
        bool ITextView.Validate(Point point)
        {
            return Validate(point);
        }

        /// <summary>
        /// <see cref="ITextView.Validate(ITextPointer)"/>
        /// </summary>
        bool ITextView.Validate(ITextPointer position)
        {
            return Validate(position);
        }

        /// <summary>
        /// <see cref="ITextView.ThrottleBackgroundTasksForUserInput"/>
        /// </summary>
        void ITextView.ThrottleBackgroundTasksForUserInput()
        {
            ThrottleBackgroundTasksForUserInput();
        }

        /// <summary>
        /// <see cref="ITextView.RenderScope"/>
        /// </summary>
        UIElement ITextView.RenderScope
        {
            get { return RenderScope; }
        }

        /// <summary>
        /// <see cref="ITextView.TextContainer"/>
        /// </summary>
        ITextContainer ITextView.TextContainer
        {
            get { return TextContainer; }
        }

        /// <summary>
        /// <see cref="ITextView.IsValid"/>
        /// </summary>
        bool ITextView.IsValid
        {
            get { return IsValid; }
        }

        /// <summary>
        /// <see cref="ITextView.RendersOwnSelection"/>
        /// </summary>
        bool ITextView.RendersOwnSelection
        {
            get { return RendersOwnSelection; }
        }

        /// <summary>
        /// <see cref="ITextView.TextSegments"/>
        /// </summary>
        ReadOnlyCollection<TextSegment> ITextView.TextSegments
        {
            get { return TextSegments; }
        }

        #endregion ITextView
    }
}


