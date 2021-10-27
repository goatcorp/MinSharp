namespace MinSharp
{
    /// <summary>
    /// Error codes returned by MinHook.
    /// </summary>
    internal enum MhStatus
    {
        /// <summary>
        /// Unknown error. Should not be returned.
        /// </summary>
        MH_UNKNOWN = -1,

        /// <summary>
        /// Successful.
        /// </summary>
        MH_OK = 0,

        /// <summary>
        /// MinHook is already initialized.
        /// </summary>
        MH_ERROR_ALREADY_INITIALIZED,

        /// <summary>
        /// MinHook is not initialized yet, or already uninitialized.
        /// </summary>
        MH_ERROR_NOT_INITIALIZED,

        /// <summary>
        /// The hook for the specified target function is already created.
        /// </summary>
        MH_ERROR_ALREADY_CREATED,

        /// <summary>
        /// The hook for the specified target function is not created yet.
        /// </summary>
        MH_ERROR_NOT_CREATED,

        /// <summary>
        /// The hook for the specified target function is already enabled.
        /// </summary>
        MH_ERROR_ENABLED,

        /// <summary>
        /// The hook for the specified target function is not enabled yet, or already
        /// disabled.
        /// </summary>
        MH_ERROR_DISABLED,

        /// <summary>
        /// The specified pointer is invalid. It points the address of non-allocated
        /// and/or non-executable region.
        /// </summary>
        MH_ERROR_NOT_EXECUTABLE,

        /// <summary>
        /// The specified target function cannot be hooked.
        /// </summary>
        MH_ERROR_UNSUPPORTED_FUNCTION,

        /// <summary>
        /// Failed to allocate memory.
        /// </summary>
        MH_ERROR_MEMORY_ALLOC,

        /// <summary>
        /// Failed to change the memory protection.
        /// </summary>
        MH_ERROR_MEMORY_PROTECT,

        /// <summary>
        /// The specified module is not loaded.
        /// </summary>
        MH_ERROR_MODULE_NOT_FOUND,

        /// <summary>
        /// The specified function is not found.
        /// </summary>
        MH_ERROR_FUNCTION_NOT_FOUND,

        /// <summary>
        /// The synchronization mutex object failed.
        /// </summary>
        MH_ERROR_MUTEX_FAILURE,
    }
}
