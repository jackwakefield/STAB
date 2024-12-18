using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using STAB.Injector;

var dllPath = @"STAB.Bootstrap.dll";

Console.WriteLine("Waiting for They Are Billions to start...");

Process? gameProcess = null;

// wait for the game to start
while (gameProcess == null)
{
    var processes = Process.GetProcessesByName("TheyAreBillions");
    if (processes.Length > 0)
    {
        gameProcess = processes[0];
    }
    else
    {
        Thread.Sleep(1000);
    }
}

Console.WriteLine("They Are Billions found with PID: " + gameProcess.Id);

// ensure STAB hasn't already been injected
foreach (ProcessModule module in gameProcess.Modules)
{
    if (module.FileName == dllPath)
    {
        Console.WriteLine("STAB already injected, exiting...");
        return;
    }
}

// open the process with the necessary access rights
var processHandle = Native.OpenProcess(0x001F0FFF, false, (uint)gameProcess.Id);
if (processHandle == nint.Zero)
{
    Console.WriteLine("Failed to open process, error code: " + Marshal.GetLastWin32Error());
    return;
}

// allocate memory in the target process to store the DLL path
var allocatedMemoryAddress = Native.VirtualAllocEx(
    processHandle,
    nint.Zero,
    (nuint)((dllPath.Length + 1) * 2),
    0x3000,
    0x40);
if (allocatedMemoryAddress == nint.Zero)
{
    Console.WriteLine("Failed to allocate memory, error code: " + Marshal.GetLastWin32Error());
    return;
}

// write the DLL path to the allocated memory in the target process
var dllPathBytes = Encoding.Unicode.GetBytes(dllPath + "\0"); // Ensure null-terminated
if (!Native.WriteProcessMemory(
    processHandle,
    allocatedMemoryAddress,
    dllPathBytes,
    (nuint)dllPathBytes.Length,
    out _))
{
    Console.WriteLine("Failed to write memory, error code: " + Marshal.GetLastWin32Error());
    return;
}

// get the address of LoadLibraryW to load the DLL into the target process
var loadLibraryAddress = Native.GetProcAddress(
    Native.GetModuleHandle("kernel32.dll"),
    "LoadLibraryW");
if (loadLibraryAddress == nint.Zero)
{
    Console.WriteLine("Failed to get LoadLibraryW address, error code: " + Marshal.GetLastWin32Error());
    return;
}

// create a remote thread that calls LoadLibraryW with the DLL path to inject the DLL
var threadHandle = Native.CreateRemoteThread(
    processHandle,
    nint.Zero,
    0,
    loadLibraryAddress,
    allocatedMemoryAddress,
    0,
    out _);
if (threadHandle == nint.Zero)
{
    Console.WriteLine("Failed to create remote thread, error code: " + Marshal.GetLastWin32Error());
    return;
}

Console.WriteLine("STAB injected successfully!");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();