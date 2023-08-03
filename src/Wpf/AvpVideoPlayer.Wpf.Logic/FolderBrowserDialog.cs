using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace AvpVideoPlayer.Wpf.Logic;

public delegate int BrowseCallbackProc(IntPtr hwnd, int uMsg, IntPtr lParam, IntPtr lpData);

[StructLayout(LayoutKind.Sequential)]
public struct BROWSEINFO
{
    public IntPtr hwndOwner;
    public int iImage;
    public IntPtr lParam;
    public BrowseCallbackProc lpfn;
    [MarshalAs(UnmanagedType.LPTStr)]
    public string lpszTitle;
    public IntPtr pidlRoot;
    public IntPtr pszDisplayName;
    public uint ulFlags;
}

[ExcludeFromCodeCoverage]
public class FolderBrowserDialog
{

    private string? _initialPath;
    private const int BFFM_INITIALIZED = 1;
    private const int BFFM_SELCHANGED = 2;
    private const int BFFM_SETSELECTIONW = WM_USER + 103;
    private const int BFFM_SETSTATUSTEXTW = WM_USER + 104;

    // Constants for sending and receiving messages in BrowseCallBackProc
    private const int WM_USER = 0x400;

    private FolderBrowserDialog() { }

    private int OnBrowseEvent(IntPtr hWnd, int msg, IntPtr lp, IntPtr lpData)
    {
        switch (msg)
        {
            case BFFM_INITIALIZED: // Required to set initialPath
                {
                    // Use BFFM_SETSELECTIONW if passing a Unicode string, i.e. native CLR Strings.
                    SendMessage(new HandleRef(null, hWnd), BFFM_SETSELECTIONW, 1, _initialPath);
                    break;
                }
            case BFFM_SELCHANGED:
                {
                    IntPtr pathPtr = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
                    if (SHGetPathFromIDList(lp, pathPtr))
                        SendMessage(new HandleRef(null, hWnd), BFFM_SETSTATUSTEXTW, 0, pathPtr);
                    Marshal.FreeHGlobal(pathPtr);
                    break;
                }
        }

        return 0;
    }

    [DllImport("user32.dll", PreserveSig = true)]
    private static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string? lParam);

    [DllImport("shell32.dll")]
    private static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

    // Note that the BROWSEINFO object's pszDisplayName only gives you the name of the folder.
    // To get the actual path, you need to parse the returned PIDL
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

    private string? Show(string caption, string? initialPath, IntPtr parentHandle)
    {
        _initialPath = initialPath;
        StringBuilder sb = new(256);
        IntPtr bufferAddress = Marshal.AllocHGlobal(256);
        IntPtr pidl = IntPtr.Zero;
        BROWSEINFO bi = new();
        bi.hwndOwner = parentHandle;
        bi.pidlRoot = IntPtr.Zero;
        bi.lpszTitle = caption;
        bi.ulFlags = (uint)(BrowseInfoFlags.BIF_NEWDIALOGSTYLE | BrowseInfoFlags.BIF_SHAREABLE);
        bi.lpfn = new BrowseCallbackProc(OnBrowseEvent);
        bi.lParam = IntPtr.Zero;
        bi.iImage = 0;

        try
        {
            pidl = SHBrowseForFolder(ref bi);
            if (!SHGetPathFromIDList(pidl, bufferAddress))
            {
                return null;
            }
            sb.Append(Marshal.PtrToStringAuto(bufferAddress));
        }
        finally
        {
            // Caller is responsible for freeing this memory.
            Marshal.FreeCoTaskMem(pidl);
        }

        return sb.ToString();
    }
    public static string? ShowDialog(string caption, string? initialPath, IntPtr parentHandle)
    {
        return new FolderBrowserDialog().Show(caption, initialPath, parentHandle);
    }
}

[Flags]
public enum BrowseInfoFlags : uint
{
    /// <summary>
    /// Only return file system directories.
    ///
    /// If the user selects folders that are not part of the file system,
    /// the OK button is grayed.
    /// </summary>
    BIF_RETURNONLYFSDIRS = 0x00000001,
    /// <summary>
    /// Do not include network folders below the domain level in the dialog box's tree view control.
    /// </summary>
    BIF_DONTGOBELOWDOMAIN = 0x00000002,
    /// <summary>
    /// Include a status area in the dialog box.
    /// The callback function can set the status text by sending messages to the dialog box.
    /// This flag is not supported when <bold>BIF_NEWDIALOGSTYLE</bold> is specified
    /// </summary>
    BIF_STATUSTEXT = 0x00000004,
    /// <summary>
    /// Only return file system ancestors.
    /// An ancestor is a subfolder that is beneath the root folder in the namespace hierarchy.
    /// If the user selects an ancestor of the root folder that is not part of the file system, the OK button is grayed
    /// </summary>
    BIF_RETURNFSANCESTORS = 0x00000008,
    /// <summary>
    /// Include an edit control in the browse dialog box that allows the user to type the name of an item.
    /// </summary>
    BIF_EDITBOX = 0x00000010,
    /// <summary>
    /// If the user types an invalid name into the edit box, the browse dialog box calls the application's BrowseCallbackProc with the BFFM_VALIDATEFAILED message.
    /// This flag is ignored if <bold>BIF_EDITBOX</bold> is not specified.
    /// </summary>
    BIF_VALIDATE = 0x00000020,
    /// <summary>
    /// Use the new user interface.
    /// Setting this flag provides the user with a larger dialog box that can be resized.
    /// The dialog box has several new capabilities, including: drag-and-drop capability within the
    /// dialog box, reordering, shortcut menus, new folders, delete, and other shortcut menu commands.
    /// </summary>
    BIF_NEWDIALOGSTYLE = 0x00000040,
    /// <summary>
    /// The browse dialog box can display URLs. The <bold>BIF_USENEWUI</bold> and <bold>BIF_BROWSEINCLUDEFILES</bold> flags must also be set.
    /// If any of these three flags are not set, the browser dialog box rejects URLs.
    /// </summary>
    BIF_BROWSEINCLUDEURLS = 0x00000080,
    /// <summary>
    /// Use the new user interface, including an edit box. This flag is equivalent to <bold>BIF_EDITBOX | BIF_NEWDIALOGSTYLE</bold>
    /// </summary>
    BIF_USENEWUI = BIF_EDITBOX | BIF_NEWDIALOGSTYLE,
    /// <summary>
    /// hen combined with <bold>BIF_NEWDIALOGSTYLE</bold>, adds a usage hint to the dialog box, in place of the edit box. <bold>BIF_EDITBOX</bold> overrides this flag.
    /// </summary>
    BIF_UAHINT = 0x00000100,
    /// <summary>
    /// Do not include the New Folder button in the browse dialog box.
    /// </summary>
    BIF_NONEWFOLDERBUTTON = 0x00000200,
    /// <summary>
    /// When the selected item is a shortcut, return the PIDL of the shortcut itself rather than its target.
    /// </summary>
    BIF_NOTRANSLATETARGETS = 0x00000400,
    /// <summary>
    /// Only return computers. If the user selects anything other than a computer, the OK button is grayed.
    /// </summary>
    BIF_BROWSEFORCOMPUTER = 0x00001000,
    /// <summary>
    /// Only allow the selection of printers. If the user selects anything other than a printer, the OK button is grayed
    /// </summary>
    BIF_BROWSEFORPRINTER = 0x00002000,
    /// <summary>
    /// The browse dialog box displays files as well as folders.
    /// </summary>
    BIF_BROWSEINCLUDEFILES = 0x00004000,
    /// <summary>
    /// The browse dialog box can display shareable resources on remote systems.
    /// </summary>
    BIF_SHAREABLE = 0x00008000,
    /// <summary>
    /// Allow folder junctions such as a library or a compressed file with a .zip file name extension to be browsed.
    /// </summary>
    BIF_BROWSEFILEJUNCTIONS = 0x00010000,
}
