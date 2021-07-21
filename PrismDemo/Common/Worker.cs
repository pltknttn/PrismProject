using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace PrismDemo.Common
{
    public static class Worker
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    WindowInteropHelper helper = new WindowInteropHelper(window);
        //    SetWindowLong(helper.Handle, GWL_EXSTYLE, GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        //}

        //запустить и дождаться окончания процесса
        public static Task ExecuteProcess(string path)
        {
            var p = new Process() { EnableRaisingEvents = true, StartInfo = { FileName = path } };
            var tcs = new TaskCompletionSource<bool>();
            p.Exited += (sender, args) => { tcs.SetResult(true); p.Dispose(); };
            // запуск выгружаем на пул потоков, потому что он медленный
            Task.Run(() => p.Start());
            return tcs.Task;
        }

        public static Task RunProcessAsync(string processPath)
        {
            var tcs = new TaskCompletionSource<object>();
            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(processPath)
                {
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            process.Exited += (sender, args) =>
            {
                if (process.ExitCode != 0)
                {
                    var errorMessage = process.StandardError.ReadToEnd();
                    tcs.SetException(new InvalidOperationException("The process did not exit correctly. " +
                        "The corresponding error message was: " + errorMessage));
                }
                else
                {
                    tcs.SetResult(null);
                }
                process.Dispose();
            };
            process.Start();
            return tcs.Task;
        }

        //public async Task<char> WaitInput()
        //{
        //    var tcs = new TaskCompletionSource<char>();
        //    SourceInputHandler handler = (o, args) => tcs.SetResult(args.Input);
        //    source.InputReceived += handler;
        //    try
        //    {
        //        return await tcs.Task;
        //    }
        //    finally
        //    {
        //        source.InputReceived -= handler;
        //    }
        //}

        //нужно дождаться, пока поток стартует, и придёт в «рабочее» состояние.Обычно для этого используют AutoResetEvent,
        //но блокироваться в ожидании его неохота, и намного проще использовать TaskCompletionSource:

        //public static Task<DispatcherThread> CreateAsync()
        //{
        //    var waitCompletionSource = new TaskCompletionSource<DispatcherThread>();
        //    var thread = new Thread(() =>
        //    {
        //        // тут могут быть любые настройки
        //        waitCompletionSource.SetResult(new DispatcherThread());
        //        Dispatcher.Run();
        //    });

        //    thread.SetApartmentState(ApartmentState.STA);
        //    thread.Start();

        //    return waitCompletionSource.Task;
        //}

        public static void ToFront(string programName)
        {             
            IntPtr handle = FindWindowByCaption(IntPtr.Zero, programName);
            if (handle == IntPtr.Zero) return;

            SetForegroundWindow(handle);
        }
    }


    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, ShowDesktop.WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        internal static extern int GetClassName(IntPtr hwnd, StringBuilder name, int count);
    }

    public static class ShowDesktop
    {
        private const uint WINEVENT_OUTOFCONTEXT = 0u;
        private const uint EVENT_SYSTEM_FOREGROUND = 3u;

        private const string WORKERW = "WorkerW";
        private const string PROGMAN = "Progman";

        public static void AddHook(Window window)
        {
            if (IsHooked)
            {
                return;
            }

            IsHooked = true;

            _delegate = new WinEventDelegate(WinEventHook);
            _hookIntPtr = NativeMethods.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _delegate, 0, 0, WINEVENT_OUTOFCONTEXT);
            _window = window;
        }

        public static void RemoveHook()
        {
            if (!IsHooked)
            {
                return;
            }

            IsHooked = false;

            NativeMethods.UnhookWinEvent(_hookIntPtr.Value);

            _delegate = null;
            _hookIntPtr = null;
            _window = null;
        }

        private static string GetWindowClass(IntPtr hwnd)
        {
            StringBuilder _sb = new StringBuilder(32);
            NativeMethods.GetClassName(hwnd, _sb, _sb.Capacity);
            return _sb.ToString();
        }

        internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private static void WinEventHook(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType == EVENT_SYSTEM_FOREGROUND)
            {
                string _class = GetWindowClass(hwnd);

                if (string.Equals(_class, WORKERW, StringComparison.Ordinal) /*|| string.Equals(_class, PROGMAN, StringComparison.Ordinal)*/ )
                {
                    _window.Topmost = true;
                }
                else
                {
                    _window.Topmost = false;
                }
            }
        }

        public static bool IsHooked { get; private set; } = false;

        private static IntPtr? _hookIntPtr { get; set; }

        private static WinEventDelegate _delegate { get; set; }

        private static Window _window { get; set; }

        //private async void Window_StateChanged_1(object sender, EventArgs e)
        //{
        //    await MaximizeWindow(this);
        //}

        //public Task MaximizeWindow(Window window)
        //{
        //    return Task.Factory.StartNew(() =>
        //    {
        //        this.Dispatcher.Invoke((Action)(() =>
        //        {
        //            Thread.Sleep(100);
        //            window.WindowState = System.Windows.WindowState.Maximized;
        //        }));
        //    });
        //}
    }
}
