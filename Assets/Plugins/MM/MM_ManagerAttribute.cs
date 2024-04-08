using System;

namespace MM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false, Inherited = true)]
    public class Manager : Attribute
    {
        public string DisplayName = "";
        public string Path = "";

        public Manager()
        {
        }
    }
}
