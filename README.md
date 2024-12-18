# STAB - Simple They Are Billions Mod Loader

STAB is a simple mod loader for the game They Are Billions.

## Usage

1. Place your mods in the `Mods` directory alongside `STAB.Injector.exe`, with a filename matching `STAB.*.dll`.
2. Run `STAB.Injector.exe`.
3. Launch They Are Billions.

## Anti-Virus Warnings

STAB uses DLL injection to modify the game code. This may trigger warnings from some anti-virus programs. You may need to add an exception for `STAB.Injector.exe`.

## Directory Structure

```
STAB/
├── Mods/
│   └── STAB.YourMod.dll
├── STAB.Injector.exe
├── OtherFiles
```

## Building

1. Open `STAB.sln` in Visual Studio.
2. Build the solution.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Contributing

Contributions are welcome! Please submit a pull request.