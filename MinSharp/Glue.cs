using System;
using System.Runtime.InteropServices;

namespace MinSharp
{
    internal static unsafe class Glue
    {
        public static bool Initialized { get; set; }

        public static uint NumHooks { get; set; }

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_Initialize")]
        public static extern MhStatus Initialize();

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_Uninitialize")]
        public static extern MhStatus Uninitialize();

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_CreateHook")]
        public static extern MhStatus CreateHook(IntPtr pTarget, IntPtr pDetour, IntPtr* ppOriginal);

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_RemoveHook")]
        public static extern MhStatus RemoveHook(IntPtr pTarget);

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_EnableHook")]
        public static extern MhStatus EnableHook(IntPtr pTarget);

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_DisableHook")]
        public static extern MhStatus DisableHook(IntPtr pTarget);

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_QueueEnableHook")]
        public static extern MhStatus QueueEnableHook(IntPtr pTarget);

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_QueueDisableHook")]
        public static extern MhStatus QueueDisableHook(IntPtr pTarget);

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_ApplyQueued")]
        public static extern MhStatus ApplyQueued();

        [DllImport("minhook.x64d", CallingConvention = CallingConvention.StdCall, EntryPoint = "MH_StatusToString")]
        public static extern IntPtr StatusToString(MhStatus status);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
    }
}
