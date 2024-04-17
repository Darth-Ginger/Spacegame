using System;


namespace Utility
{
    /// <summary>
    /// A generic Singleton class for creating a single instance of a type T.
    /// </summary>
    /// <typeparam name="T">The type of the singleton instance. Must be a class with a parameterless constructor.</typeparam>
    public class Singleton<T> where T : class, new()
    {
        // A private static variable to hold the instance of the type T.
        private static T instance;

        /// <summary>
        /// The public accessor for the singleton instance.
        /// </summary>
        /// <value>The instance of type T.</value>
        public static T Instance => instance ?? (instance = new T());
    }
}