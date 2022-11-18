// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Input.Preview.Injection;

#if WINAPPSDK
using MainWindow = Microsoft.UI.Xaml.Window;
#else
using MainWindow = Windows.UI.Xaml.Window;
#endif

namespace CommunityToolkit.Labs.Tests;

public partial class InputSimulator
{
    private static InputInjector _input = InputInjector.TryCreate();
    private static uint _currentPointerId = 0;

    private WeakReference<MainWindow> _currentWindowRef;

    /// <summary>
    /// Create a new <see cref="InputSimulator"/> helper class for the current window. All positions provided to this API will use the client space of the provided window's top-left as an origin point.
    /// </summary>
    /// <param name="currentWindow">Window to simulate input on, used as a reference for client-space coordinates.</param>
    public InputSimulator(MainWindow currentWindow)
    {
        _currentWindowRef = new(currentWindow);
    }

    public void StartTouch()
    {
        Assert.IsNotNull(_input);

        _input.InitializeTouchInjection(
                InjectedInputVisualizationMode.Default);
    }

    /// <summary>
    /// Simulates a touch press on screen at the coordinates provided, in app-local coordinates. For instance use <code>App.ContentRoot.CoordinatesTo(element)</code>.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public uint TouchDown(Point point)
    {
        // Create a unique pointer ID for the injected touch pointer.
        // Multiple input pointers would require more robust handling.
        uint pointerId = _currentPointerId++;

        var injectionPoint = TranslatePointForWindow(point);

        // Create a touch data point for pointer down.
        // Each element in the touch data list represents a single touch contact. 
        // For this example, we're mirroring a single mouse pointer.
        List<InjectedInputTouchInfo> touchData = new()
            {
                new()
                {
                    Contact = new InjectedInputRectangle
                    {
                        Left = 30, Top = 30, Bottom = 30, Right = 30
                    },
                    PointerInfo = new InjectedInputPointerInfo
                    {
                        PointerId = pointerId,
                        PointerOptions =
                        InjectedInputPointerOptions.PointerDown |
                        InjectedInputPointerOptions.InContact |
                        InjectedInputPointerOptions.New,
                        TimeOffsetInMilliseconds = 0,
                        PixelLocation = new InjectedInputPoint
                        {
                            PositionX = (int)injectionPoint.X ,
                            PositionY = (int)injectionPoint.Y
                        }
                },
                Pressure = 1.0,
                TouchParameters =
                    InjectedInputTouchParameters.Pressure |
                    InjectedInputTouchParameters.Contact
                }
            };

        // Inject the touch input. 
        _input.InjectTouchInput(touchData);

        return pointerId;
    }

    public void TouchMove(uint pointerId, int cX, int cY)
    {
        // Create a touch data point for pointer up.
        List<InjectedInputTouchInfo> touchData = new()
            {
                new()
                {
                    Contact = new InjectedInputRectangle
                    {
                        Left = 30, Top = 30, Bottom = 30, Right = 30
                    },
                    PointerInfo = new InjectedInputPointerInfo
                    {
                        PointerId = pointerId,
                        PointerOptions =
                        InjectedInputPointerOptions.InRange |
                        InjectedInputPointerOptions.InContact,
                        TimeOffsetInMilliseconds = 0,
                        PixelLocation = new InjectedInputPoint
                        {
                            PositionX = (int)cX ,
                            PositionY = (int)cY
                        }
                    },
                    Pressure = 1.0,
                    TouchParameters =
                        InjectedInputTouchParameters.Pressure |
                        InjectedInputTouchParameters.Contact
                }
            };

        // Inject the touch input. 
        _input.InjectTouchInput(touchData);
    }

    public void TouchUp(uint pointerId)
    {
        Assert.IsNotNull(_input);

        // Create a touch data point for pointer up.
        List<InjectedInputTouchInfo> touchData = new()
        {
            new()
            {
                PointerInfo = new InjectedInputPointerInfo
                {
                    PointerId = pointerId,
                    PointerOptions = InjectedInputPointerOptions.PointerUp
                }
            }
        };

        // Inject the touch input. 
        _input.InjectTouchInput(touchData);
    }

    public void StopTouch()
    {
        // Shut down the virtual input device.
        _input.UninitializeTouchInjection();
    }
}