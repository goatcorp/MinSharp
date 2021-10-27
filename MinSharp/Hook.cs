using System;
using System.Runtime.InteropServices;

namespace MinSharp
{
    /// <summary>
    /// Container class for MinHook hooks.
    /// </summary>
    /// <typeparam name="T">Delegate function for this hook.</typeparam>
    public unsafe class Hook<T> : IDisposable
        where T : Delegate
    {
        private readonly IntPtr targetFunctionPtr;
        private readonly IntPtr detourFunctionPtr;
        private readonly IntPtr originalFunctionPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hook{T}"/> class.
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

                this.Original = Marshal.GetDelegateForFunctionPointer<T>(*pOriginalFunctionPtr);
                this.Enabled = false;

                Glue.NumHooks++;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this hook is enabled.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Gets a delegate with which the original function can be called.
        /// </summary>
        public T Original { get; private set; }

        /// <summary>
        /// Enable this hook.
        /// </summary>
        public void Enable()
        {
            if (this.Enabled)
                return;

            var status = Glue.EnableHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            this.Enabled = true;
        }

        /// <summary>
        /// Disable this hook.
        /// </summary>
        public void Disable()
        {
            if (!this.Enabled)
                return;

            var status = Glue.DisableHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            this.Enabled = false;
        }

        /// <summary>
        /// Remove this hook.
        /// </summary>
        private void Remove()
        {
            var status = Glue.RemoveHook(this.targetFunctionPtr);

            if (status != MhStatus.MH_OK)
                throw new MinHookException(status);

            this.Enabled = false;
        }

        /// <summary>
        /// Disable and remove this hook, and, in case no hooks are left, uninitialize MinHook.
        /// </summary>
        public void Dispose()
        {
            this.Disable();
            this.Remove();

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
