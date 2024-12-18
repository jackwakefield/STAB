using System.Runtime.InteropServices;

namespace STAB.Injector;

internal static partial class Native
{
    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint OpenProcess(
        uint dwDesiredAccess,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
        uint dwProcessId);

    [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", SetLastError = true)]
    public static partial nint GetModuleHandle(
        [MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint GetProcAddress(
        nint hModule,
        [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint VirtualAllocEx(
        nint hProcess,
        nint lpAddress,
        nuint dwSize,
        uint flAllocationType,
        uint flProtect);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WriteProcessMemory(
        nint hProcess,
        nint lpBaseAddress,
        byte[] lpBuffer,
        nuint nSize,
        out nuint lpNumberOfBytesWritten);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint CreateRemoteThread(
        nint hProcess,
        nint lpThreadAttributes,
        nuint dwStackSize,
        nint lpStartAddress,
        nint lpParameter,
        uint dwCreationFlags,
        out uint lpThreadId);
}
