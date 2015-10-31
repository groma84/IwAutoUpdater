using System;

namespace IwAutoUpdater.CrossCutting.Base
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DIIgnoreAttribute : Attribute
    {
    }
}
