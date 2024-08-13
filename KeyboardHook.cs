using FunkyFridayAutoPlay;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public delegate IntPtr KeyboardProcess(int nCode, IntPtr wParam, IntPtr lParam);

// Credit to https://medium.com/@davho/c-keyloggers-using-windows-api-d53eafcd48b for keyboard hook to read keys to set as keybinds! ^_^

public sealed class KeyboardHook
{
    private const int WH_KEYBOARD = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static KeyboardProcess keyboardProc = HookCallback;
    private static IntPtr hookID = IntPtr.Zero;

    public static void CreateHook()
    {
        hookID = SetHook(keyboardProc);
    }

    public static void DisposeHook()
    {
        UnhookWindowsHookEx(hookID);
    }

    private static IntPtr SetHook(KeyboardProcess keyboardProc)
    {
        using (Process currentProcess = Process.GetCurrentProcess())
        using (ProcessModule currentProcessModule = currentProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD, keyboardProc, GetModuleHandle(currentProcessModule.ModuleName), 0);
        }
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            Program.settingBinds = true;

            if (Program.settingBinds)
            {
                if (Program.bindsDone >= 5)
                {
                    Program.settingBinds = false;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Binds set!");
                    Console.ForegroundColor = ConsoleColor.White;

                    DisposeHook();

                    Program.Start();

                    return CallNextHookEx(hookID, nCode, wParam, lParam);
                }

                switch(Program.bindsDone)
                {
                    case 0:
                        Console.WriteLine("please press FAR_LEFT key");
                        break;
                    case 1:
                        Console.WriteLine("please press LEFT key");
                        break;
                    case 2:
                        Console.WriteLine("please press FAR_RIGHT key");
                        break;
                    case 3:
                        Console.WriteLine("please press RIGHT key");
                        break;
                    case 4:
                        Console.WriteLine("please press RIGHT key again");
                        break;
                }

                Program.binds[Program.bindsDone - 1] = vkCode;
                Program.bindsDone++;
            }
        }
        return CallNextHookEx(hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProcess lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

}