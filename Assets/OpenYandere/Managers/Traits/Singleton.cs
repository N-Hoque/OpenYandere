using UnityEngine;

namespace OpenYandere.Managers.Traits
{
    internal class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static          T      m_Instance;
        private static readonly object Lock = new object();

        public static T Instance
        {
            get
            {
                lock(Lock)
                {
                    if(m_Instance != null)
                    {
                        return m_Instance;
                    }

                    m_Instance = (T) FindObjectOfType(typeof(T));

                    if(m_Instance != null)
                    {
                        return m_Instance;
                    }

                    Debug.LogErrorFormat("[Singleton]: Failed to find an instance of '{0}', please create an instance in the scene.",
                                         typeof(T));

                    return null;
                }
            }
        }
    }
}