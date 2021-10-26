using System;
using System.Collections.Generic;
using System.Text;

namespace MinSharp
{
    class MinHookException : Exception
    {
        public MhStatus Status { get; private set; }

        public MinHookException(MhStatus status) : base($"MinHook could not complete this operation. Status: {status}")
        {
            Status = status;
        }
    }
}
