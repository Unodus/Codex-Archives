using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Owner: Kyle Hatch
/// Created: 24/02/2017
/// 
/// Descirption: Windows Utils
/// </summary>

public static class WindowsUtilities 
{
	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern short GetKeyState(int keyCode);
    
    [DllImport("user32.dll")]
    private static extern int GetKeyboardState(byte [] lpKeyState);
    
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
	
	[DllImport("Kernel32")]
    public static extern bool GetDiskFreeSpaceEx(string directory, ref long avalibleFree, ref long total, ref long totalFree);

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public extern static bool GetVolumeInformation( string rootPathName, StringBuilder volumeNameBuffer, int volumeNameSize, out uint volumeSerialNumber, out uint maximumComponentLength, out FileSystemFeature fileSystemFlags, StringBuilder fileSystemNameBuffer, int nFileSystemNameSize);

    [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
    private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);
    private const int MAX_PATH = 260;
    private const int CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019;

    [DllImport("shell32.dll")]
    static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);

    #region Const Declares
    private const byte VK_NUMLOCK = 0x90;
    private const uint KEYEVENTF_EXTENDEDKEY = 1;
    private const int KEYEVENTF_KEYUP = 0x2;
    private const int KEYEVENTF_KEYDOWN = 0x0;
	#endregion
	
	#region Windows C++ function
	public static float GetFreeDiskSpace(string directory)
    {
        long avalibleFree = 0;
        long total = 0;
        long totalFree = 0;
        if(false == GetDiskFreeSpaceEx(directory, ref avalibleFree, ref total, ref totalFree))
        {
            return 0.0f;
        }
        float fFree = avalibleFree / 1024.0f / 1024.0f;// megabytes / 1024.0f; //gigabytes
        return fFree;
    }

    public static float GetPercentageOfFreeSpace(string directory)
    {
        long avalibleFree = 0;
        long total = 0;
        long totalFree = 0;
        if (false == GetDiskFreeSpaceEx(directory, ref avalibleFree, ref total, ref totalFree))
        {
            return 0.0f;
        }

        float fPercentage = ((float)avalibleFree / (float)total) * 100f;

        return fPercentage;
    }

    public static void GetDriveInfo(string sDrivePath, out HardDriveInfo hdInfo) //"c:\\"
    {
        StringBuilder volName = new StringBuilder(261);
        StringBuilder fileSystemName = new StringBuilder(261);
        if (false == GetVolumeInformation(sDrivePath, volName, volName.Capacity, out uint sernum, out uint maxlen, out FileSystemFeature flags, fileSystemName, fileSystemName.Capacity))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        hdInfo = new HardDriveInfo();
        hdInfo.sRootPathName = sDrivePath;
        hdInfo.sVolumeNameBuffer = volName.ToString();
        hdInfo.iVolumeNameSize = volName.Capacity;
        hdInfo.volumeSerialNumber = sernum;
        hdInfo.maximumComponentLength = maxlen;
        hdInfo.fileSystemFlags = flags;
        hdInfo.sFileSystemNameBuffer = fileSystemName.ToString();
        hdInfo.iFileSystemNameSize = fileSystemName.Capacity;
    }

    public static string GetAllUsersDesktopFolderPath()
    {
        StringBuilder sbPath = new StringBuilder(MAX_PATH);
        SHGetFolderPath(IntPtr.Zero, CSIDL_COMMON_DESKTOPDIRECTORY, IntPtr.Zero, 0, sbPath);
        return sbPath.ToString();
    }
    #endregion

    private static string s_sScriptTempFilename;

    /// <summary>
    /// Creates a shortcut at the specified path with the given target and
    /// arguments.
    /// </summary>
    /// <param name="sPath">The path where the shortcut will be created. This should
    ///     be a file with the LNK extension.</param>
    /// <param name="sTarget">The target of the shortcut, e.g. the program or file
    ///     or folder which will be opened.</param>
    /// <param name="sArguments">The additional command line arguments passed to the
    ///     target.</param>
    /// Must pass in path and target with bracket format: ie: "W:\\NET0022 Network Rail switching simulator\\Network Rail Share",
    public static void CreateShortcut(string sPath, string sTarget, string sDescription, string sArguments = "")
    {
        //delete old copy
        if (true == File.Exists(sPath))
        {
            File.Delete(sPath);
        }

        // Check if link path ends with LNK or URL
        string sExtension = Path.GetExtension(sPath).ToUpper();
        if ((sExtension != ".LNK") && (sExtension != ".URL"))
        {
            throw new ArgumentException("The path of the shortcut must have the extension .lnk or .url.");
        }

        // Get temporary file name with correct extension
        s_sScriptTempFilename = Path.GetTempFileName();
        File.Move(s_sScriptTempFilename, s_sScriptTempFilename += ".vbs");

        // Generate script and write it in the temporary file
        File.WriteAllText(s_sScriptTempFilename, String.Format(@"Dim WSHShell
            Set WSHShell = WScript.CreateObject({0}WScript.Shell{0})
            Dim Shortcut
            Set Shortcut = WSHShell.CreateShortcut({0}{1}{0})
            Shortcut.TargetPath = {0}{2}{0}   
            Shortcut.WorkingDirectory = {0}{3}{0}      
            Shortcut.Arguments = {0}{4}{0}           
            Shortcut.Description = {0}{5}{0}
            Shortcut.IconLocation = {0}{6}{0}     
            Shortcut.Save",
            "\"", sPath, sTarget, Path.GetDirectoryName(sTarget), sArguments, sDescription, @"C:\Windows\System32\SHELL32.dll, 3"),
            Encoding.Unicode);

        // Run the script and delete it after it has finished
        Process process = new Process();
        process.StartInfo.FileName = s_sScriptTempFilename;
        process.Start();
        process.WaitForExit();
        File.Delete(s_sScriptTempFilename);
    }

    public static string GetLocalLow()
    {
        Guid localLowId = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
        return GetKnownFolderPath(localLowId);
    }

    public static string GetKnownFolderPath(Guid knownFolderId)
    {
        IntPtr pszPath = IntPtr.Zero;
        try
        {
            int hr = SHGetKnownFolderPath(knownFolderId, 0, IntPtr.Zero, out pszPath);
            if (hr >= 0)
                return Marshal.PtrToStringAuto(pszPath);
            throw Marshal.GetExceptionForHR(hr);
        }
        finally
        {
            if (pszPath != IntPtr.Zero)
                Marshal.FreeCoTaskMem(pszPath);
        }
    }

    #region Keyboard Overrides   
    public static bool GetNumLock()
    {
        return (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
    }
    
    public static void SetNumLock( bool bState )
    {
        if (GetNumLock() != bState)
        {
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0);
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
	#endregion
}

public class HardDriveInfo
{
    public string sRootPathName;
    public string sVolumeNameBuffer;
    public int iVolumeNameSize;
    public uint volumeSerialNumber;
    public uint maximumComponentLength;
    public FileSystemFeature fileSystemFlags;
    public string sFileSystemNameBuffer;
    public int iFileSystemNameSize;
}

[Flags]
public enum FileSystemFeature : uint
{
    /// <summary>
    /// The file system preserves the case of file names when it places a name on disk.
    /// </summary>
    CasePreservedNames = 2,

    /// <summary>
    /// The file system supports case-sensitive file names.
    /// </summary>
    CaseSensitiveSearch = 1,

    /// <summary>
    /// The specified volume is a direct access (DAX) volume. This flag was introduced in Windows 10, version 1607.
    /// </summary>
    DaxVolume = 0x20000000,

    /// <summary>
    /// The file system supports file-based compression.
    /// </summary>
    FileCompression = 0x10,

    /// <summary>
    /// The file system supports named streams.
    /// </summary>
    NamedStreams = 0x40000,

    /// <summary>
    /// The file system preserves and enforces access control lists (ACL).
    /// </summary>
    PersistentACLS = 8,

    /// <summary>
    /// The specified volume is read-only.
    /// </summary>
    ReadOnlyVolume = 0x80000,

    /// <summary>
    /// The volume supports a single sequential write.
    /// </summary>
    SequentialWriteOnce = 0x100000,

    /// <summary>
    /// The file system supports the Encrypted File System (EFS).
    /// </summary>
    SupportsEncryption = 0x20000,

    /// <summary>
    /// The specified volume supports extended attributes. An extended attribute is a piece of
    /// application-specific metadata that an application can associate with a file and is not part
    /// of the file's data.
    /// </summary>
    SupportsExtendedAttributes = 0x00800000,

    /// <summary>
    /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
    /// </summary>
    SupportsHardLinks = 0x00400000,

    /// <summary>
    /// The file system supports object identifiers.
    /// </summary>
    SupportsObjectIDs = 0x10000,

    /// <summary>
    /// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
    /// </summary>
    SupportsOpenByFileId = 0x01000000,

    /// <summary>
    /// The file system supports re-parse points.
    /// </summary>
    SupportsReparsePoints = 0x80,

    /// <summary>
    /// The file system supports sparse files.
    /// </summary>
    SupportsSparseFiles = 0x40,

    /// <summary>
    /// The volume supports transactions.
    /// </summary>
    SupportsTransactions = 0x200000,

    /// <summary>
    /// The specified volume supports update sequence number (USN) journals. For more information,
    /// see Change Journal Records.
    /// </summary>
    SupportsUsnJournal = 0x02000000,

    /// <summary>
    /// The file system supports Unicode in file names as they appear on disk.
    /// </summary>
    UnicodeOnDisk = 4,

    /// <summary>
    /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
    /// </summary>
    VolumeIsCompressed = 0x8000,

    /// <summary>
    /// The file system supports disk quotas.
    /// </summary>
    VolumeQuotas = 0x20
}