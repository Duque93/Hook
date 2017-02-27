using System;
using System.Runtime.InteropServices;

namespace Utils
{
    class ControllerSystemInfo
    {

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int smIndex);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx
        /// <summary>
        /// Enum for available parameters to be pased to GetSystemMetrics
        /// </summary>
        public enum SystemMetric : int
        {

            /// <summary>
            /// The width of the screen of the primary display monitor, in pixels.
            /// This is the same value obtained by calling GetDeviceCaps as follows:
            /// GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
            /// </summary>
            SM_CXSCREEN = 0,
            /// <summary>
            /// The height of the screen of the primary display monitor, in pixels.
            /// This is the same value obtained by calling GetDeviceCaps as follows:
            /// GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
            /// </summary>
            SM_CYSCREEN = 1,

            /// <summary>
            /// The width of a vertical scroll bar, in pixels.
            /// </summary>
            SM_CXVSCROLL = 2,
            /// <summary>
            /// The height of a vertical scroll bar, in pixels.
            /// </summary>
            SM_CYVSCROLL = 20,

            /// <summary>
            /// Returns the width of a window border, in pixels
            /// </summary>
            SM_CXBORDER = 5,
            /// <summary>
            /// Returns the height of a window border, in pixels
            /// </summary>
            SM_CYBORDER = 6,

            /// <summary>
            /// Returns the height of the horizontal border of a frame around the perimeter of a window that has a caption but is not sizable, in pixels
            /// </summary>
            SM_CXFIXEDFRAME = 7,
            /// <summary>
            /// Returns the width of the vertical border of a frame around the perimeter of a window that has a caption but is not sizable, in pixels
            /// </summary>
            SM_CYFIXEDFRAME = 8,

            /// <summary>
            /// Returns the width of the thumb box in a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHTHUMB = 10,

            /// <summary>
            /// The default width of an icon, in pixels.
            /// </summary>
            SM_CXICON = 11,
            /// <summary>
            /// The default height of an icon, in pixels.
            /// </summary>
            SM_CYICON = 12,

            /// <summary>
            /// Returns the width of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CXCURSOR = 13,
            /// <summary>
            /// Returns the width of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CYCURSOR = 14,

            /// <summary>
            /// Returns the width for a full-screen window on the primary display monitor, in pixels.
            /// </summary>
            SM_CXFULLSCREEN = 16,
            /// <summary>
            ///  Returns the height for a full-screen window on the primary display monitor, in pixels.
            /// </summary>
            SM_CYFULLSCREEN = 17,

            /// <summary>
            /// Nonzero if a mouse is installed; otherwise, 0.
            /// This value is rarely zero, because of support for virtual mice and because some systems detect the presence of the 
            /// port instead of the presence of a mouse.
            /// </summary>
            SM_MOUSEPRESENT = 19,

            /// <summary>
            /// Returns the width of the arrow bitmap on a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHSCROLL = 21,
            /// <summary>
            /// Returns the height of the arrow bitmap on a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CYHSCROLL = 3,

            /// <summary>
            /// Nonzero if the debug version of User.exe is installed; otherwise, 0.
            /// </summary>
            SM_DEBUG = 22,

            /// <summary>
            /// Nonzero if the meanings of the left and right mouse buttons are swapped; otherwise, 0.
            /// </summary>
            SM_SWAPBUTTON = 23,

            /// <summary>
            /// The minimum width of a window, in pixels.
            /// </summary>
            SM_CXMIN = 28,
            /// <summary>
            /// The minimum height of a window, in pixels.
            /// </summary>
            SM_CYMIN = 29,

            /// <summary>
            /// The width of a button in a window caption or title bar, in pixels.
            /// </summary>
            SM_CXSIZE = 30,
            /// <summary>
            /// The height of a button in a window caption or title bar, in pixels.
            /// </summary>
            SM_CYSIZE = 31,

            /// <summary>
            /// The thickness(width) of the sizing border around the perimeter of a window that can be resized, in pixels. 
            /// </summary>
            SM_CXSIZEFRAME = 32,
            /// <summary>
            /// The thickness(height) of the sizing border around the perimeter of a window that can be resized, in pixels. 
            /// </summary>
            SM_CYSIZEFRAME = 33,

            /// <summary>
            /// The minimum tracking width of a window, in pixels.
            /// The user cannot drag the window frame to a size smaller than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMINTRACK = 34,
            /// <summary>
            /// The minimum tracking height of a window, in pixels.
            /// The user cannot drag the window frame to a size smaller than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CYMINTRACK = 35,

            /// <summary>
            /// Returns the width of the rectangle that received the clicks.
            /// To set the width of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKWIDTH.
            /// </summary>
            SM_CXDOUBLECLK = 36,
            /// <summary>
            /// Returns the height of the rectangle that received the clicks.
            /// </summary>
            SM_CYDOUBLECLK = 37,

            /// <summary>
            /// The width of a grid cell for items in large icon view, in pixels.This value is always greater than or equal to SM_CXICON
            /// </summary>
            SM_CXICONSPACING = 38,
            /// <summary>
            /// The height of a grid cell for items in large icon view, in pixels.This value is always greater than or equal to SM_CYICON
            /// </summary>
            SM_CYICONSPACING = 39,

            /// <summary>
            /// Nonzero if drop-down menus are right-aligned with the corresponding menu-bar item; 0 if the menus are left-aligned.
            /// </summary>
            SM_MENUDROPALIGNMENT = 40,

            /// <summary>
            /// Nonzero if the Microsoft Windows for Pen computing extensions are installed; zero otherwise.
            /// </summary>
            SM_PENWINDOWS = 41,

            /// <summary>
            /// Nonzero if User32.dll supports DBCS; otherwise, 0. 
            /// </summary>
            SM_DBCSENABLED = 42,

            /// <summary>
            /// Return the number of buttons on a mouse, or zero if no mouse is installed.
            /// </summary>
            SM_CMOUSEBUTTONS = 43,

            /// <summary>
            /// Returns the width of a 3D border. This metric is the 3D counterpart of SM_CXBORDER.
            /// </summary>
            SM_CXEDGE = 45,
            /// <summary>
            /// Returns the height of a 3D border. This metric is the 3D counterpart of SM_CXBORDER.
            /// </summary>
            SM_CYEDGE = 46,

            /// <summary>
            /// The width of a grid cell for a minimized window, in pixels.
            /// Each minimized window fits into a rectangle this size when arranged.
            /// This value is always greater than or equal to <seealso cref="SystemMetric.SM_CXMINIMIZED"/>
            /// </summary>
            SM_CXMINSPACING = 47,
            /// <summary>
            /// The height of a grid cell for a minimized window, in pixels.
            /// Each minimized window fits into a rectangle this size when arranged.
            /// This value is always greater than or equal to <seealso cref="SystemMetric.SM_CYMINIMIZED"/>
            /// </summary>
            SM_CYMINSPACING = 48,

            /// <summary>
            /// The recommended width of a small icon, in pixels.
            /// Small icons typically appear in window captions and in small icon view.
            /// </summary>
            SM_CXSMICON = 49,
            /// <summary>
            /// The recommended height of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
            /// </summary>
            SM_CYSMICON = 50,

            /// <summary>
            /// The width of small caption buttons, in pixels.
            /// </summary>
            SM_CXSMSIZE = 52,
            /// <summary>
            /// The height of small caption buttons, in pixels.
            /// </summary>
            SM_CYSMSIZE = 53,

            /// <summary>
            /// The width of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
            /// </summary>
            SM_CXMENUSIZE = 54,
            /// <summary>
            /// The height of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
            /// </summary>
            SM_CYMENUSIZE = 55,

            /// <summary>
            /// How the system minimize windows. 
            /// More info about how to handle value returned :
            /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms724500(v=vs.85).aspx
            /// </summary>
            SM_ARRANGE = 56,

            /// <summary>
            /// The width of a minimized window, in pixels.
            /// </summary>
            SM_CXMINIMIZED = 57,
            /// <summary>
            /// The height of a minimized window, in pixels.
            /// </summary>
            SM_CYMINIMIZED = 58,

            /// <summary>
            /// The default maximum width of a window that has a caption and sizing borders, in pixels.
            /// This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMAXTRACK = 59,
            /// <summary>
            /// The default maximum height of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop.
            /// The user cannot drag the window frame to a size larger than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CYMAXTRACK = 60,

            /// <summary>
            /// The default width, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CXMAXIMIZED = 61,
            /// <summary>
            /// The default height, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CYMAXIMIZED = 62,

            /// <summary>
            /// The least significant bit is set if a network is present; otherwise, it is cleared.
            /// The other bits are reserved for future use.
            /// </summary>
            SM_NETWORK = 63,

            /// <summary>
            /// How the system has started : 0 Normal boot | 1 Fail-safe boot | 2 Fail-safe with network boot
            /// </summary>
            SM_CLEANBOOT = 67,

            /// <summary>
            /// Nonzero if the user requires an application to present information visually in situations
            /// where it would otherwise present the information only in audible form; otherwise, 0.
            /// </summary>
            SM_SHOWSOUNDS = 70,

            /// <summary>
            /// The width of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CXMENUCHECK = 71,
            /// <summary>
            /// The height of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CYMENUCHECK = 72,

            /// <summary>
            /// Nonzero if the computer has a low-end (slow) processor; otherwise, 0.
            /// </summary>
            SM_SLOWMACHINE = 73,

            /// <summary>
            /// Nonzero if the system is enabled for Hebrew and Arabic languages, 0 if not.
            /// </summary>
            SM_MIDEASTENABLED = 74,

            /// <summary>
            /// Nonzero if a mouse with a vertical scroll wheel is installed; otherwise 0. 
            /// </summary>
            SM_MOUSEWHEELPRESENT = 75,

            /// <summary>
            /// The coordinates for the left side of the virtual screen.
            /// The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_CXVIRTUALSCREEN metric is the width of the virtual screen. 
            /// </summary>
            SM_XVIRTUALSCREEN = 76,
            /// <summary>
            /// The coordinates for the top of the virtual screen.
            /// The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_CYVIRTUALSCREEN metric is the height of the virtual screen. 
            /// </summary>
            SM_YVIRTUALSCREEN = 77,

            /// <summary>
            /// The width of the virtual screen, in pixels.
            /// The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_XVIRTUALSCREEN metric is the coordinates for the left side of the virtual screen. 
            /// </summary>
            SM_CXVIRTUALSCREEN = 78,
            /// <summary>
            /// The height of the virtual screen, in pixels.
            /// The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_YVIRTUALSCREEN metric is the coordinates for the top of the virtual screen.
            /// </summary>
            SM_CYVIRTUALSCREEN = 79,

            /// <summary>
            /// Return the number of visible display monitors on a desktop.
            /// </summary>
            SM_CMONITORS = 80,

            /// <summary>
            /// Nonzero if all the display monitors have the same color format, otherwise, 0. 
            /// Two displays can have the same bit depth, but different color formats.
            /// For example, the red, green, and blue pixels can be encoded with different numbers of bits,
            /// or those bits can be located in different places in a pixel color value. 
            /// </summary>
            SM_SAMEDISPLAYFORMAT = 81,

            /// <summary>
            /// Nonzero if Input Method Manager/Input Method Editor features are enabled; otherwise, 0.
            /// SM_IMMENABLED indicates whether the system is ready to use a Unicode-based IME on a Unicode application. 
            /// To ensure that a language-dependent IME works, check SM_DBCSENABLED and the system ANSI code page. 
            /// Otherwise the ANSI-to-Unicode conversion may not be performed correctly, or some components like fonts or registry settings may not be present.
            /// </summary>
            SM_IMMENABLED = 82,

            /// <summary>
            /// Returns the width from the left to the right edges of the rectangle drawn by DrawFocusRect 
            /// </summary>
            SM_CXFOCUSBORDER = 83,
            /// <summary>
            /// Returns the height from the top to the bottom edges of the rectangle drawn by DrawFocusRect 
            /// </summary>
            SM_CYFOCUSBORDER = 84,

            /// <summary>
            /// Nonzero if the current operating system is the Windows XP Tablet PC edition or if the current operating system is Windows Vista
            /// or Windows 7 and the Tablet PC Input service is started; otherwise, 0. 
            /// The SM_DIGITIZER setting indicates the type of digitizer input supported by a device running Windows 7 or Windows Server 2008 R2.
            /// For more information, see Remarks. 
            /// </summary>
            SM_TABLETPC = 86,

            /// <summary>
            /// Nonzero if the current operating system is the Windows XP, Media Center Edition, 0 if not.
            /// </summary>
            SM_MEDIACENTER = 87,

            /// <summary>
            /// Nonzero if the current operating system is Windows 7 Starter Edition, Windows Vista Starter, or Windows XP Starter Edition; otherwise, 0.
            /// </summary>
            SM_STARTER = 88,

            /// <summary>
            /// Nonzero if a mouse with a horizontal scroll wheel is installed; otherwise 0.
            /// </summary>
            SM_MOUSEHORIZONTALWHEELPRESENT = 91,

            /// <summary>
            /// The amount of border padding for captioned windows, in pixels.
            /// Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_CXPADDEDBORDER = 92,

            /// <summary>
            /// Nonzero if the current operating system is Windows 7 or Windows Server 2008 R2 and the Tablet PC Input service is started; otherwise, 0. 
            /// The return value is a bitmask that specifies the type of digitizer input supported by the device. For more information, see Remarks.
            /// Windows Server 2008, Windows Vista and Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_DIGITIZER = 94,

            /// <summary>
            /// Nonzero if there are digitizers in the system; otherwise, 0.
            /// SM_MAXIMUMTOUCHES returns the aggregate maximum of the maximum number of contacts supported by every digitizer in the system.
            /// If the system has only single-touch digitizers, the return value is 1. If the system has multi-touch digitizers,
            /// the return value is the number of simultaneous contacts the hardware can provide.
            /// Windows Server 2008, Windows Vista and Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_MAXIMUMTOUCHES = 95,

            /// <summary>
            /// his system metric is used in a Terminal Services environment.
            /// If the calling process is associated with a Terminal Services client session, the return value is nonzero.
            /// If the calling process is associated with the Terminal Services console session, the return value is 0.
            /// Windows Server 2003 and Windows XP:  The console session is not necessarily the physical console.
            /// For more information, see WTSGetActiveConsoleSessionId.
            /// </summary>
            SM_REMOTESESSION = 0x1000,

            /// <summary>
            /// Nonzero if the current session is shutting down; otherwise, 0.
            /// Windows 2000:  This value is not supported.
            /// </summary>
            SM_SHUTTINGDOWN = 0x2000,

            /// <summary>
            /// This system metric is used in a Terminal Services environment to determine if the current Terminal Server session is being remotely controlled.
            /// Its value is nonzero if the current session is remotely controlled; otherwise, 0. 
            /// You can use terminal services management tools such as Terminal Services Manager (tsadmin.msc) and shadow.exe to control a remote session.
            /// When a session is being remotely controlled, another user can view the contents of that session and potentially interact with it.
            /// </summary>
            SM_REMOTECONTROL = 0x2001,

            /// <summary>
            /// Return the state of the LAPTOP.
            /// 0 = Modo pizzara
            /// != 0 = Otro estado.
            /// Inutil para Sobremesas
            /// </summary>
            SM_CONVERTIBLESLATEMODE = 0x2003,

            /// <summary>
            /// Reflects the state of the docking mode, 0 for Undocked Mode and non-zero otherwise. 
            /// When this system metric changes, the system sends a broadcast message via WM_SETTINGCHANGE 
            /// with "SystemDockMode" in the LPARAM.
            /// </summary>
            SM_SYSTEMDOCKED = 0x2004,

        }

        public static int getSystemInfo(SystemMetric query)
        {
            return GetSystemMetrics( (int)query );
        }
    }
}
