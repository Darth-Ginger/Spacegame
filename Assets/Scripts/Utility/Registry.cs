using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Spacegame.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility
{
    /// <summary>
    /// Base class for registries
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// @ingroup utility
    /// @property descriptors The list of descriptors in the registry
    /// @property destructor Whether or not the registry should destroy descriptors on destroy. Defaults to false
    public abstract class Registry<T> : ScriptableObject where T : SerializableScriptableObject
    {

        [SerializeField] protected List<T>       _descriptors  = new();
        [SerializeField] protected readonly bool _destructor   = false;
        [SerializeField] public SortMethod       _sortedMethod = SortMethod.None;

        /// <summary>
        /// Finds a descriptor by its Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>A descriptor or null if not found</returns>
        public T FindByGuid (Guid guid)
        {
            foreach (var desc in _descriptors)
            {
                if (desc.Guid == guid)
                {
                    return desc;
                }
            }

            return null;
        }

        /// <summary>
        /// Registers a descriptor in the registry
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns>True if the descriptor was successfully registered</returns>
        public bool Register (T descriptor)
        {
            if (descriptor.Guid == null
                || FindByGuid(descriptor.Guid) == null) return false;

            _descriptors.Add(descriptor);
            return true;
        }

        /// <summary>
        /// Unregisters a descriptor from the registry
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns>True if the descriptor was successfully unregistered</returns>
        public bool Unregister(T descriptor, bool destroy = false)
        {
            bool unregistered;
            
            unregistered = _descriptors.Remove(descriptor);

            // Destroy the descriptor if the registry is set to be able to and the destroy flag is set
            if (destroy && _destructor) Destroy(descriptor);

            return unregistered;
        }

        /// <summary>
        /// Unregisters all descriptors with a given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>An integer representing the number of descriptors unregistered</returns>
        public int Unregister (string name, bool destroy = false) 
        {
            var count = 0;
            foreach (var desc in _descriptors)
            {
                if (desc.name == name) {
                    Unregister(desc);
                    count++;
                    if (destroy && _destructor) Destroy(desc);
                }
            }
            return count;
        }
    
        /// <summary>
        /// Returns 1 or more random descriptor
        /// </summary>
        /// <param name="count"></param>
        /// <returns>Returns 1 or more random descriptors</returns>
        public T GetRandom(int count = 1) => _descriptors.Take(count).ElementAt(Random.Range(0, _descriptors.Count));
        
        /// <summary>
        /// Returns the first descriptor
        /// </summary>
        /// <returns>Returns the first descriptor</returns>
        public T GetFirst() => _descriptors[0];
        /// <summary>
        /// Returns the last descriptor
        /// </summary>
        /// <returns>Returns the last descriptor</returns>
        public T GetLast()  => _descriptors.Last();

        public virtual void Sort<TParam>(SortMethod sortMethod, Func<TParam, IComparable> keySelector)
        {
            switch (sortMethod)
            {
                case SortMethod.Ascending:
                    SortAscending(keySelector);
                    break;
                case SortMethod.Descending:
                    SortDescending(keySelector);
                    break;
            }
        }

        public virtual void SortAscending<TParam>(Func<TParam, IComparable> keySelector)
        {
            _descriptors.Sort((x, y) => x.CompareTo(y));
            _sortedMethod = SortMethod.Ascending;
        }

        public virtual void SortDescending<TParam>(Func<TParam, IComparable> keySelector)
        {
            _descriptors.Sort((x, y) => y.CompareTo(x));
            _sortedMethod = SortMethod.Descending;
        }
    }
}