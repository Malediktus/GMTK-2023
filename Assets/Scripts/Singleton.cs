using UnityEngine;

namespace LuckiusDev.Utils.Types
{
    /// <summary>
    /// A static instance is similar to a singleton, but instead of destroying any new
    /// instances, it overrides the current instance. This is handy for resetting the state
    /// and saves you doing it manually.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        protected virtual void Awake() {
            Debug.Log("Creating static instance...", this);
            Instance = this as T;
        }

        protected virtual void OnApplicationQuit() {
            Debug.Log("Destroying static instance...", this);
            Instance = null;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This transforms the static instance into a basic singleton. This will destroy any new
    /// versions created, leaving the original instance intact.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake() {
            Debug.Log("Creating singleton...", this);
            if (Instance != null) Destroy(gameObject);
            base.Awake();
        }
    }

    /// <summary>
    /// Finally, we have a persistent version of the singleton. This will survive through scene
    /// loads. Perfect for system classes which require stateful, persistent data. Or audio sources
    /// where music plays through loading screens, etc...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake() {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}