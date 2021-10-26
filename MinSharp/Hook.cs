using System;
using System.Runtime.InteropServices;

namespace MinSharp
{
    public unsafe class Hook<T> : IDisposable
    {
        public bool Enabled { get; private set; }

        public T Original { get; private set; }

        private readonly IntPtr targetFunctionPtr;
        private readonly IntPtr detourFunctionPtr;
        private readonly IntPtr originalFunctionPtr;

        public Hook(IntPtr address, T detour)
        {
            if (!Glue.Initialized)
                Glue.Initialize();

            Glue.Initialized = true;

            this.detourFunctionPtr = Marshal.GetFunctionPointerForDelegate(detour);
            this.targetFunctionPtr = address;

            fixed (IntPtr* pOriginalFunctionPtr = &this.originalFunctionPtr)
            {
                var status = Glue.CreateHook(address, this.detourFunctionPtr, pOriginalFunctionPtr);

                if (status != MhStatus.MH_OK)
                    throw new MinHookException(status);

                Original = Marshal.GetDelegateForFunctionPointer<T>(*pOriginalFunctionPtr);
                Enabled = false;

                Glue.NumHooks++;
            }
        }

        public void Enable()
        {
            var status = Glue.EnableHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            Enabled = true;
        }

        public void Disable()
        {
            var status = Glue.DisableHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            Enabled = false;
        }

        private void Remove()
        {
            var status = Glue.RemoveHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            Enabled = false;
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        public void Dispose()
        {
            Disable();
            Remove();

            Glue.NumHooks--;

            if (Glue.NumHooks <= 0)
            {
                Glue.Uninitialize();
                Glue.Initialized = false;
            }

            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Hook()
        {
            ReleaseUnmanagedResources();
        }
    }
}
