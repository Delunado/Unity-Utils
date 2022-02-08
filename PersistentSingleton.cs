using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delu
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        public static bool HasInstance => instance != null;
        
        protected static T instance;

        /// <summary>
        /// Gets the singleton instance. Creates it if needed.
        /// </summary>
        /// <value>The singleton instance.</value>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T) + " Persistent Singleton");
                        instance = obj.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// On awake, it checks if there's already a copy of the object in the scene. If there's one, it destroys itself.
        /// </summary>
        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (instance == null) 
            {
                //Makes the current instance singleton if it's the first one, and mark it to not destroy on load.

                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this) 
            {
                //Destroy this instance if it's not the first one existing or the current singleton.
                
                Destroy(gameObject);
            } 
        }
    }
}