using System;

namespace IwAutoUpdater.CrossCutting.Base
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DIAsTransientAttribute : Attribute
    {
    }
}
