// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using UnityEngine;

// Partially based on: http://wiki.unity3d.com/index.php/Singleton

namespace Kore.Utils
{
    /// <summary>
    /// Singleton through Inheritance. There's also a optional Init() method to be
    /// overrided (because as MonoBehaviour you can't use constructor and if you 
    /// access the instance from within an Awake() method the Awake of the instance 
    /// won't be called. I also provide a replacement OnDestroyCalled.
    /// </summary>
    public class SceneScopedSingletonI< T, I> : MonoBehaviour, IInitiable where T : MonoBehaviour, I, IInitiable
        where I: class
    {
        public virtual void Init()
        {

        }

        public virtual void OnDestroyCalled()
        {
            Instantiated = false;
            instanceInterface = null;
        }

        // since T should be subclass of MonoBehaviour and I, and MonoBehaviour is a class
        // therefore I can only be a interface
        private static I instanceInterface = null;

        // flag to avoid null comparation check of monobeahviours (which is very expensive)
        // this actually makes this class one of the fastest Singletons
        private static bool Instantiated = false;

        public static I Instance
        {
            get
            {
                if (Instantiated == false)
                {
                    Instantiated = true;
                    instanceInterface = SingletonInstance_SceneScoped.GO.AddComponent<T>();
                    (instanceInterface as IInitiable).Init();
                    return instanceInterface;
                }

                return instanceInterface;
            }
        }
    }

    public class SceneScopedSingleton< T> : MonoBehaviour, IInitiable  where T : MonoBehaviour, IInitiable
    {
        public virtual void Init()
        {

        }

        public virtual void OnDestroyCalled()
        {
            instanceConcrete = null;
            Instantiated = false;
        }

        private static T instanceConcrete = null;

        // flag to avoid null comparation check of monobeahviours (which is very expensive)
        // this actually makes this class one of the fastest Singletons
        private static bool Instantiated = false;

        public static T Instance
        {
            get
            {
                if (Instantiated == false)
                {
                    Instantiated = true;
                    instanceConcrete = SingletonInstance_SceneScoped.GO.AddComponent< T>();
                    instanceConcrete.Init();
                    return instanceConcrete;
                }

                return instanceConcrete;
            }
        }
    }

    /// <summary>
    /// Internal utility: there will be just 1 GameObject in the scene with
    /// all singletons attached.
    /// </summary>
    internal class SingletonInstance_SceneScoped
    {
        private static bool Instantiated = false;

        internal static GameObject TheOnlyGO = null;

        internal static GameObject GO
        {
            get
            {
                // Creates only 1 game object
                if (Instantiated == false)
                {
                    TheOnlyGO = new GameObject( "[Singletons:SceneScoped]");
                    TheOnlyGO.AddComponent< SceneScopedSingleton_Resetter>();
                    Instantiated = true;
                }
                
                return TheOnlyGO;
            }
        }

        internal static void Reset()
        {
            ResetSingletons();
            Instantiated = false;
            TheOnlyGO = null;
        }

        internal static void ResetSingletons()
        {
            var singletons = TheOnlyGO.GetComponents< IInitiable>();
            foreach (var single in singletons)
                single.OnDestroyCalled();
        }
    }

    // Reset Instance so that we will instantiate again the GO in the next scene
    internal class SceneScopedSingleton_Resetter: MonoBehaviour
    {
        private void OnDestroy()
        {
            SingletonInstance_SceneScoped.Reset();
        }
    }
}
