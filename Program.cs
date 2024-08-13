using FunkyFridayAutoPlay;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using WindowsInput.Native;

public class Program 
{
    public static int bindsDone = 1;
    public static bool settingBinds = false;

    public static int pollingMS = 5;

    public static int[] binds = new int[5];

    public static Color[] rgbs = new Color[4];

    public static Color grayButton = Color.FromArgb(255, 136, 165, 171);

    public static IntPtr robloxHandle = IntPtr.Zero;

    public static bool started = false;

    public static Point[] points = new Point[]
    {
        new Point(1144, 172), // FAR_LEFT
        new Point(1287, 172), // LEFT
        new Point(1440, 172), // RIGHT
        new Point(1589, 172) // FAR_RIGHT
    };

    [STAThread]
    public static void Main(string[] args)
    {
        if(args.Length != 0)
        {
            //TODO: soon
        }

        IntPtr hdc = GetDC(IntPtr.Zero);

        int width = GetDeviceCaps(hdc, DESKTOPHORZRES);
        int height = GetDeviceCaps(hdc, DESKTOPVERTRES);

        _ = ReleaseDC(IntPtr.Zero, hdc);

        if (width != 1920 || height != 1080)
        {    
            MessageBox.Show("Your screen resolution must be 1920x1080.", "Wrong resolution :p", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.Title = "Funky Friday AutoPlay Tool (prod trxsh 2.0)";

        Console.WriteLine("please set rgb values for detection. These can be found in your arrow settings.");
        Console.WriteLine("Your gray button color MUST be " + grayButton + "!");

        Console.WriteLine("The format to enter these values are:  R,G,B  DO NOT use spaces. After you're done entering a RGB value, press enter.");

        Console.Write("Enter FAR-LEFT rgb: ");
        Color farLeftColor = StringToColor(Console.ReadLine());

        Console.Write("Enter LEFT rgb: ");
        Color leftColor = StringToColor(Console.ReadLine());

        Console.Write("Enter FAR-RIGHT rgb: ");
        Color farRightColor = StringToColor(Console.ReadLine());

        Console.Write("Enter RIGHT rgb: ");
        Color rightColor = StringToColor(Console.ReadLine());

        rgbs[0] = farLeftColor;
        rgbs[1] = leftColor;
        rgbs[2] = farRightColor;
        rgbs[3] = rightColor;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Rgb values set!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("please set keybinds.");
        Console.WriteLine("please press FAR_LEFT key");

        KeyboardHook.CreateHook();
        Application.Run();
    }

    public static void Start()
    {
        // set binds.
        Keybinds.setBind(Bind.FAR_LEFT, binds[0]);
        Keybinds.setBind(Bind.LEFT, binds[1]);
        Keybinds.setBind(Bind.FAR_RIGHT, binds[2]);
        Keybinds.setBind(Bind.RIGHT, binds[3]);

        Console.WriteLine("getting window...");

        IntPtr gameClientHandle = FindWindow("WINDOWSCLIENT", "Roblox");

        if (gameClientHandle == IntPtr.Zero)
        {
            Fail("Roblox handle could not be found. Restart roblox");
            return;
        }

        robloxHandle = gameClientHandle;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Roblox handle is valid!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("AutoPlay will automatically detect if you are in a track. Roblox will maximize in 5 seconds...");

        Thread.Sleep(5000);

        SetForegroundWindow(gameClientHandle); 
        ShowWindow(gameClientHandle, 3);

        Pixels.PollGameCheck(points[0], grayButton);
    }

    public static void GameStarted()
    {
        new Thread(() => Pixels.PollPixelChangeToCallInput(Keybinds.getBind(Bind.FAR_LEFT), points[0], grayButton, rgbs[0])).Start();
        new Thread(() => Pixels.PollPixelChangeToCallInput(Keybinds.getBind(Bind.LEFT), points[1], grayButton, rgbs[1])).Start();
        new Thread(() => Pixels.PollPixelChangeToCallInput(Keybinds.getBind(Bind.RIGHT), points[2], grayButton, rgbs[3])).Start();
        new Thread(() => Pixels.PollPixelChangeToCallInput(Keybinds.getBind(Bind.FAR_RIGHT), points[3], grayButton, rgbs[2])).Start();

        started = true;

        //MessageBox.Show(NativeWindow.FromHandle(robloxHandle), "Buttons detected. AutoPlay turned on. Make sure you are PLAYER 2");
    }

    public static void GameEnded()
    {
        MessageBox.Show(NativeWindow.FromHandle(robloxHandle), "Track finished. AutoPlay turned off");

        started = false;

        Pixels.PollGameCheck(points[0], grayButton);
    }

    public static void Fail(string reason)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(reason);
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("press any key to continue...");
        Console.ReadKey();

        Application.Exit();
    }

    public static Color StringToColor(string colorString)
    {
        string[] rgb = colorString.Split(',');

        if (rgb.Length != 3)
        {
            throw new ArgumentException("Input string must be in the format 'R,G,B'");
        }

        int r = int.Parse(rgb[0]);
        int g = int.Parse(rgb[1]);
        int b = int.Parse(rgb[2]);

        return Color.FromArgb(255, r, g, b);
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpCLass, string lpWindow);
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    private const int DESKTOPHORZRES = 118;
    private const int DESKTOPVERTRES = 117;
}