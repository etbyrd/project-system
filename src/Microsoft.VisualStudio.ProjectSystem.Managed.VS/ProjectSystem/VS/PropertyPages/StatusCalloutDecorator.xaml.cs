using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;


namespace Microsoft.VisualStudio.ProjectSystem.VS.PropertyPages
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    internal partial class StatusCalloutDecorator : Decorator
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            name: nameof(Background),
            propertyType: typeof(Brush),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            name: nameof(BorderBrush),
            propertyType: typeof(Brush),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );

        public static readonly DependencyProperty BorderWidthProperty = DependencyProperty.Register(
            name: nameof(BorderWidth),
            propertyType: typeof(double),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: 0.0,
                flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );

        public static readonly DependencyProperty CalloutHeightProperty = DependencyProperty.Register(
            name: nameof(CalloutHeight),
            propertyType: typeof(double),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: 16.0,
                flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );

        public static readonly DependencyProperty CalloutWidthProperty = DependencyProperty.Register(
            name: nameof(CalloutWidth),
            propertyType: typeof(double),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: 16.0,
                flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );

        public static readonly DependencyProperty CalloutTargetProperty = DependencyProperty.Register(
            name: nameof(CalloutTarget),
            propertyType: typeof(FrameworkElement),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: OnCalloutTargetChanged,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );

        public static readonly DependencyProperty MinSquishHeightProperty = DependencyProperty.Register(
            name: nameof(MinSquishHeight),
            propertyType: typeof(double),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: 0.0,
                flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            name: nameof(Padding),
            propertyType: typeof(Thickness),
            ownerType: typeof(StatusCalloutDecorator),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: new Thickness(),
                flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: IsThicknessValid
        );

        private StreamGeometry _cachedBorderGeometry;
        private GuidelineSet _cachedGuidelineSet;
        private int _cachedMonitorTop;
        private Point _cachedPopupScreenLocation;
        private Point? _cachedTargetScreenLocation;
        private HwndSource _cachedWindowSource;

        public StatusCalloutDecorator()
        {
            InitializeComponent();
            PresentationSource.AddSourceChangedHandler(this, OnPresentationSourceChanged);
        }

        public Brush Background
        {
            get
            {
                return (Brush)GetValue(BackgroundProperty);
            }

            set
            {
                SetValue(BackgroundProperty, value);
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return (Brush)GetValue(BorderBrushProperty);
            }

            set
            {
                SetValue(BorderBrushProperty, value);
            }
        }

        public double BorderWidth
        {
            get
            {
                return (double)GetValue(BorderWidthProperty);
            }

            set
            {
                SetValue(BorderWidthProperty, value);
            }
        }

        public double CalloutHeight
        {
            get
            {
                return (double)GetValue(CalloutHeightProperty);
            }

            set
            {
                SetValue(CalloutHeightProperty, value);
            }
        }

        public double CalloutWidth
        {
            get
            {
                return (double)GetValue(CalloutWidthProperty);
            }

            set
            {
                SetValue(CalloutWidthProperty, value);
            }
        }

        public FrameworkElement CalloutTarget
        {
            get
            {
                return (FrameworkElement)GetValue(CalloutTargetProperty);
            }

            set
            {
                SetValue(CalloutTargetProperty, value);
            }
        }

        public double MinSquishHeight
        {
            get
            {
                return (double)GetValue(MinSquishHeightProperty);
            }

            set
            {
                SetValue(MinSquishHeightProperty, value);
            }
        }

        public Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }

            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var borderWidth = BorderWidth;
            var border = new Thickness(borderWidth);

            var offset = 0.0;
            var calloutHeight = 0.0;

            var presentationSource = PresentationSource.FromVisual(this);
            var popupIsAbove = true;

            var calloutTarget = CalloutTarget;

            if ((calloutTarget?.IsVisible).GetValueOrDefault() && IsVisible && (presentationSource != null))
            {
                // Determine where the center of the target is (in screen coordinates)
                var targetLocation = new Point(calloutTarget.ActualWidth / 2.0, 0.0);
                var targetScreenLocation = calloutTarget.PointToScreen(targetLocation);

                var popupScreenLocation = _cachedPopupScreenLocation;
                popupIsAbove = popupScreenLocation.Y < targetScreenLocation.Y;

                calloutHeight = CalloutHeight;

                // Offset = the distance between the target and popup locations (in window coordinates) - any margin
                targetLocation.X = Math.Abs(targetScreenLocation.X - popupScreenLocation.X);
                targetLocation = presentationSource.CompositionTarget.TransformFromDevice.Transform(targetLocation);
                offset = targetLocation.X - Margin.Left;

                border = new Thickness(
                    left: borderWidth,
                    top: borderWidth + (popupIsAbove ? 0.0 : calloutHeight),
                    right: borderWidth,
                    bottom: borderWidth + (popupIsAbove ? calloutHeight : 0.0)
                );
            }

            var bounds = new Rect(arrangeSize);
            var child = Child;

            if (child != null)
            {
                var padding = Padding;

                var innerRect = new Rect(
                    x: bounds.Left + border.Left,
                    y: bounds.Top + border.Top,
                    width: Math.Max(0.0, bounds.Width - border.Left - border.Right),
                    height: Math.Max(0.0, bounds.Height - border.Top - border.Bottom)
                );

                var childRect = new Rect(
                    x: innerRect.Left + padding.Left,
                    y: innerRect.Top + padding.Top,
                    width: Math.Max(0.0, innerRect.Width - padding.Left - padding.Right),
                    height: Math.Max(0.0, innerRect.Height - padding.Top - padding.Bottom)
                );

                child.Arrange(childRect);
            }

            if ((bounds.Width != 0) && (bounds.Height != 0))
            {
                var borderGeometry = new StreamGeometry();

                using (var streamGeometryContext = borderGeometry.Open())
                {
                    _cachedGuidelineSet = GenerateCalloutGeometry(streamGeometryContext, bounds, CalloutWidth, CalloutHeight, offset, popupIsAbove, borderWidth);
                }
                borderGeometry.Freeze();

                _cachedBorderGeometry = borderGeometry;
            }
            else
            {
                _cachedBorderGeometry = null;
                _cachedGuidelineSet = null;
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            // These represent the sizes of various bits of UI chrome
            var doubleBorderWidth = BorderWidth * 2.0;
            var calloutHeight = ((CalloutTarget?.IsVisible).GetValueOrDefault() && IsVisible) ? CalloutHeight : 0.0;

            var padding = Padding;
            var paddingWidth = padding.Left + padding.Right;
            var paddingHeight = padding.Top + padding.Bottom;

            var child = Child;

            if (child == null)
            {
                return new Size(doubleBorderWidth + paddingWidth, doubleBorderWidth + paddingHeight + calloutHeight);
            }

            var decoratorWidth = doubleBorderWidth + paddingWidth;
            var decoratorHeight = doubleBorderWidth + paddingHeight + calloutHeight;

            var minSquishHeight = MinSquishHeight;
            var squishHeight = _cachedTargetScreenLocation.HasValue ? (_cachedTargetScreenLocation.Value.Y - _cachedMonitorTop - decoratorHeight) : 0.0;
            var isSquished = false;

            var childConstraint = new Size(
                width: Math.Max(0.0, constraint.Width - decoratorWidth),
                height: Math.Max(0.0, constraint.Height - decoratorHeight)
            );

            // Determine if we should squish things to make the popup appear upwards, but only if we are not 'too close' to the top of the screen
            if ((minSquishHeight > 0.0) && (squishHeight >= minSquishHeight) && (squishHeight <= childConstraint.Height))
            {
                childConstraint.Height = squishHeight;
                isSquished = true;
            }

            child.Measure(childConstraint);
            var childDesiredSize = child.DesiredSize;

            // If we got squished, but are smaller than the child element's minimum size, we need to remeasure
            if (isSquished && (childDesiredSize.Height > constraint.Height))
            {
                childConstraint.Height = Math.Max(0.0, constraint.Height - decoratorHeight);
                child.Measure(childConstraint);
                childDesiredSize = child.DesiredSize;
            }

            return new Size(childDesiredSize.Width + decoratorWidth, childDesiredSize.Height + decoratorHeight);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var background = Background;
            var borderGeometry = _cachedBorderGeometry;

            if (background == null || borderGeometry == null)
            {
                return;
            }

            var borderBrush = BorderBrush;
            var borderWidth = BorderWidth;

            var pen = (borderBrush == null || borderWidth == 0.0) ? null : new Pen(borderBrush, borderWidth);
            var guidelineSet = _cachedGuidelineSet;

            var addGuidelines = (pen != null) && (guidelineSet != null);

            if (addGuidelines)
            {
                drawingContext.PushGuidelineSet(guidelineSet);
            }

            drawingContext.DrawGeometry(background, pen, borderGeometry);

            if (addGuidelines)
            {
                drawingContext.Pop();
            }
        }

        private static GuidelineSet GenerateCalloutGeometry(StreamGeometryContext streamGeometryContext, Rect calloutBounds, double calloutWidth, double calloutHeight, double calloutOffset, bool calloutIsBelow, double borderWidth)
        {
            // The coordinates of the content
            var topLeft = new Point(0.0, calloutIsBelow ? 0.0 : calloutHeight);
            var topRight = new Point(calloutBounds.Width - 1, topLeft.Y);
            var bottomRight = new Point(topRight.X, calloutBounds.Height - (calloutIsBelow ? calloutHeight : 0) - 1);
            var bottomLeft = new Point(topLeft.X, bottomRight.Y);

            // Determine the number of points we need
            var calloutHalfWidth = calloutWidth / 2.0;
            var calloutLeft = calloutOffset - calloutHalfWidth;
            var calloutRight = calloutOffset + calloutHalfWidth;

            // and swap some values depending on if the callout is above or below the target
            var calloutLineLeft = bottomLeft;

            if (!calloutIsBelow)
            {
                calloutLineLeft = topLeft;
                calloutHeight *= (-1.0);
            }

            var calloutPoints = Array.Empty<Point>();

            if (calloutLeft <= topLeft.X)
            {
                // We need a shape similar to |/
                calloutPoints = new[] {
                    new Point(calloutLineLeft.X + calloutWidth, calloutLineLeft.Y),
                    new Point(calloutLineLeft.X, calloutLineLeft.Y + calloutHeight)
                };
            }
            else if (calloutRight >= topRight.X)
            {
                var calloutLineRight = calloutIsBelow ? bottomRight : topRight;

                // We need a shape similar to \|
                calloutPoints = new[] {
                        new Point(calloutLineRight.X, calloutLineRight.Y + calloutHeight),
                        new Point(calloutLineRight.X - calloutWidth, calloutLineRight.Y),
                    };
            }
            else
            {
                // We need a shape similar to \/
                calloutPoints = new[] {
                        new Point(calloutLineLeft.X + calloutRight, calloutLineLeft.Y),
                        new Point(calloutLineLeft.X + calloutOffset, calloutLineLeft.Y + calloutHeight),
                        new Point(calloutLineLeft.X + calloutLeft, calloutLineLeft.Y),
                    };
            }

            //  Adjust the content bounds and callout points
            var offset = new Vector(calloutBounds.TopLeft.X, calloutBounds.TopLeft.Y);

            topLeft += offset;
            topRight += offset;
            bottomRight += offset;
            bottomLeft += offset;

            for (var index = 0; index < calloutPoints.Length; index++)
            {
                calloutPoints[index] += offset;
            }

            var guidelines = new GuidelineSet();

            // Add guidelines to prevent anti-aliasing on the border line
            var halfWidth = borderWidth / 2;

            guidelines.GuidelinesX.Add(topLeft.X + halfWidth);
            guidelines.GuidelinesX.Add(topRight.X + halfWidth);

            if (!calloutIsBelow)
            {
                guidelines.GuidelinesY.Add(topLeft.Y + calloutHeight + halfWidth);
            }

            guidelines.GuidelinesY.Add(topLeft.Y + halfWidth);
            guidelines.GuidelinesY.Add(bottomLeft.Y + halfWidth);

            if (calloutIsBelow)
            {
                guidelines.GuidelinesY.Add(bottomLeft.Y + calloutHeight + halfWidth);
            }

            // Generate the figure, starting at the top left and working clockwise
            streamGeometryContext.BeginFigure(topLeft, isFilled: true, isClosed: true);

            if (!calloutIsBelow)
            {
                for (var index = calloutPoints.Length - 1; index >= 0; index--)
                {
                    streamGeometryContext.LineTo(calloutPoints[index], isStroked: true, isSmoothJoin: false);
                }
            }

            streamGeometryContext.LineTo(topRight, isStroked: true, isSmoothJoin: false);
            streamGeometryContext.LineTo(bottomRight, isStroked: true, isSmoothJoin: false);

            if (calloutIsBelow)
            {
                foreach (var point in calloutPoints)
                {
                    streamGeometryContext.LineTo(point, isStroked: true, isSmoothJoin: false);
                }
            }

            streamGeometryContext.LineTo(bottomLeft, isStroked: true, isSmoothJoin: false);
            streamGeometryContext.LineTo(topLeft, isStroked: true, isSmoothJoin: false);

            return guidelines;
        }

        private static bool IsThicknessValid(object value)
        {
            var thickness = (Thickness)(value);

            if (thickness.Left < 0.0 || thickness.Right < 0.0 || thickness.Top < 0.0 || thickness.Bottom < 0.0)
            {
                return false;
            }

            if (double.IsNaN(thickness.Left) || double.IsNaN(thickness.Right) || double.IsNaN(thickness.Top) || double.IsNaN(thickness.Bottom))
            {
                return false;
            }

            if (double.IsInfinity(thickness.Left) || double.IsInfinity(thickness.Right) || double.IsInfinity(thickness.Top) || double.IsInfinity(thickness.Bottom))
            {
                return false;
            }

            return true;
        }

        private static void OnCalloutTargetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            var statusCalloutBorder = sender as StatusCalloutDecorator;

            if (statusCalloutBorder == null)
            {
                return;
            }

            var calloutTarget = eventArgs.NewValue as FrameworkElement;

            if (calloutTarget != null)
            {
                calloutTarget.LayoutUpdated += statusCalloutBorder.OnCalloutTargetLayoutUpdated;
            }

            var oldCalloutTarget = eventArgs.OldValue as FrameworkElement;

            if (oldCalloutTarget != null)
            {
                oldCalloutTarget.LayoutUpdated -= statusCalloutBorder.OnCalloutTargetLayoutUpdated;
            }
        }

        private void Invalidate()
        {
            InvalidateMeasure();
            InvalidateVisual();
        }

        private void OnCalloutTargetLayoutUpdated(object sender, EventArgs eventArgs)
        {
            var calloutTarget = CalloutTarget;

            if ((calloutTarget?.IsVisible).GetValueOrDefault() && IsVisible)
            {
                var targetLocation = new Point(calloutTarget.ActualWidth / 2.0, 0.0);
                var targetScreenLocation = calloutTarget.PointToScreen(targetLocation);

                if (targetScreenLocation != _cachedTargetScreenLocation)
                {
                    _cachedTargetScreenLocation = targetScreenLocation;

                    var nativeTargetLocation = new POINT()
                    {
                        x = (int)targetLocation.X,
                        y = (int)targetLocation.Y
                    };

                    var hMonitor = User32.MonitorFromPoint(nativeTargetLocation, MONITORINFOEX.DEFAULTTONEAREST);

                    var monitorInfo = new MONITORINFOEX()
                    {
                        cbSize = Marshal.SizeOf<MONITORINFOEX>()
                    };
                    User32.GetMonitorInfo(hMonitor, ref monitorInfo);

                    _cachedMonitorTop = monitorInfo.rcWork.top;

                    Invalidate();
                }
            }
        }

        private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs eventArgs)
        {
            base.OnGotKeyboardFocus(eventArgs);

            if (eventArgs.NewFocus == this)
            {
                var traversalRequest = new TraversalRequest(FocusNavigationDirection.Next);
                MoveFocus(traversalRequest);
            }
        }

        private void OnPresentationSourceChanged(object sender, SourceChangedEventArgs eventArgs)
        {
            var presentationSource = PresentationSource.FromVisual(this);
            var windowSource = _cachedWindowSource;

            if (presentationSource != windowSource)
            {
                windowSource?.RemoveHook(WindowProcHook);
            }

            windowSource = (HwndSource)presentationSource;

            if (windowSource != null)
            {
                windowSource.AddHook(WindowProcHook);
            }
            else
            {
                _cachedMonitorTop = 0;
            }

            _cachedWindowSource = windowSource;
        }

        private IntPtr WindowProcHook(IntPtr windowHandle, int msg, IntPtr wPara, IntPtr lParam, ref bool handled)
        {
            const int WM_WINDOWPOSCHANGED = 0x0047;

            if (msg == WM_WINDOWPOSCHANGED)
            {
                var lastPopupScreenLocation = _cachedPopupScreenLocation;
                var popupScreenLocation = Marshal.PtrToStructure<WINDOWPOS>(lParam);

                if ((lastPopupScreenLocation.X != popupScreenLocation.x) || (lastPopupScreenLocation.Y != popupScreenLocation.y))
                {
                    _cachedPopupScreenLocation = new Point(popupScreenLocation.x, popupScreenLocation.y);
                    Invalidate();
                }
            }

            return IntPtr.Zero;
        }
    }
}
