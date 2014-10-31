// Guids.cs
// MUST match guids.h
using System;

namespace Leboeuf.SnakeExtension
{
    static class GuidList
    {
        public const string guidSnakeExtensionPkgString = "fd8c8562-4160-4551-8262-4154ac75d91f";
        public const string guidSnakeExtensionCmdSetString = "f63fa11d-2d3d-4f2b-840d-4ce6503b8214";
        public const string guidToolWindowPersistanceString = "cb5df970-9418-4df2-9add-47a7f6bbe992";

        public static readonly Guid guidSnakeExtensionCmdSet = new Guid(guidSnakeExtensionCmdSetString);
    };
}