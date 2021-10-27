using System;

namespace MinSharp
{
    /// <summary>
    /// An exception thrown by MinHook.
    /// </summary>
    internal class MinHookException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinHookException"/> class.
        /// </summary>
        /// <param name="status">Error status.</param>
        public MinHookException(MhStatus status)
            : base($"MinHook could not complete this operation. Status: {status}")
        {
            this.Status = status;
        }

        /// <summary>
        /// Gets the exception status code.
        /// </summary>
        public MhStatus Status { get; private set; }
    }
}
