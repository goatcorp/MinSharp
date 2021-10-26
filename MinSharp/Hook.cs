using System;
using System.Runtime.InteropServices;

namespace MinSharp
{
    /// <summary>
    /// Container class for MinHook hooks.
    /// </summary>
    /// <typeparam name="T">Delegate function for this hook.</typeparam>
    public unsafe class Hook<T> : IDisposable where T : Delegate
    {
        /// <summary>
        /// Gets a value indicating whether or not this hook is enabled.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Gets a delegate with which the original function can be called.
        /// </summary>
        public T Original { get; private set; }

        private readonly IntPtr targetFunctionPtr;
        private readonly IntPtr detourFunctionPtr;
        private readonly IntPtr originalFunctionPtr;

        /// <summary>
        /// Create a new hook, but do not enable it.
        /// </summary>
        /// <param name="address">The address of the target function.</param>
        /// <param name="detour">The detour function.</param>
        /// <exception cref="MinHookException">Exception that may occurr when hooking has failed.</exception>
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

        /// <summary>
        /// Enable this hook.
        /// </summary>
        public void Enable()
        {
            if (Enabled)
                return;

            var status = Glue.EnableHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            Enabled = true;
        }

        /// <summary>
        /// Disable this hook.
        /// </summary>
        public void Disable()
        {
            if (!Enabled)
                return;

            var status = Glue.DisableHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            Enabled = false;
        }

        /// <summary>
        /// Remove this hook.
        /// </summary>
        private void Remove()
        {
            var status = Glue.RemoveHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            Enabled = false;
        }

        /// <summary>
        /// Disable and remove this hook, and, in case no hooks are left, uninitialize MinHook.
        /// </summary>
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

            GC.SuppressFinalize(this);
        }
    }
}
