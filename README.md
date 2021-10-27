# MinSharp ![Nuget](https://img.shields.io/nuget/v/MinSharp)
Minimalistic .NET API wrapper for the minimalistic x86/x64 API Hooking Library for Windows by TsudaKageyu.

### Example

```csharp
private delegate int MessageBoxWDelegate(
  IntPtr hWnd,
  [MarshalAs(UnmanagedType.LPWStr)] string text,
  [MarshalAs(UnmanagedType.LPWStr)] string caption,
  NativeFunctions.MessageBoxType type);

IntPtr pTarget = [...]; // Find address of target function

var hook = new Hook<MessageBoxWDelegate>(pTarget, MessageBoxWDetour);
hook.Enable();

private int MessageBoxWDetour(IntPtr hwnd, string text, string caption, NativeFunctions.MessageBoxType type)
{
    Console.WriteLine($"Hook triggered: {hwnd:X} {text} {caption} {type}");
    return this.messageBoxMinHook.Original(hwnd, text, caption, type);
}
```

### Attribution
[MinHook by TsudaKageyu](https://github.com/TsudaKageyu/minhook) under [BSD 2-Clause](https://opensource.org/licenses/BSD-2-Clause), with modifications by m417z
<br>
**Note:** The MinHook binary that ships with MinSharp is based on [m417z's `multihook` branch](https://github.com/m417z/minhook/tree/multihook).
