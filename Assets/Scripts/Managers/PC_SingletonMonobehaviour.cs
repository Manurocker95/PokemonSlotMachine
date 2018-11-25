/*===============================================================*
 *                                                               *
 *       Script made by Manuel Rodríguez Matesanz                *
 *            Using UNITY Examples as base                       *
 *          Free to use if credits are given                     *
 *                                                               *
 *===============================================================*/

using UnityEngine;

namespace PokemonCasino
{
    /// <summary>
    /// A singleton implementation for MonoBehaviours
    /// Just heritate from any manager and voilà! 
    /// Singleton instance!
    /// </summary>
    /// <typeparam name="T">Any type we want</typeparam>
    public class PC_SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// The actual instance of this type.
        /// </summary>
        private static MonoBehaviour m_instance;
        /// <summary>
        /// Do we wanna destroy this GO across levels
        /// </summary>
        [SerializeField] protected bool m_destroyOnLoad = false;
        /// <summary>
        /// Called when an instance is initialized due to no previous instance found.  Use this to
		/// initialize any resources this singleton requires (eg, if this is a gui item or prefab,
        /// build out the hierarchy in here or instantiate stuff).
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Get an instance to this MonoBehaviour.Always returns a valid object.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    // first search the scene for an instance
                    T[] scene = FindObjectsOfType<T>();

                    if (scene != null && scene.Length > 0)
                    {
                        m_instance = scene[0];

                        for (int i = 1; i < scene.Length; i++)
                        {
                            Destroy(scene[i]);
                        }
                    }
                    else
                    {
                        GameObject go = new GameObject();
                        string type_name = typeof(T).ToString();
                        int i = type_name.LastIndexOf('.') + 1;
                        go.name = (i > 0 ? type_name.Substring(i) : type_name) + " Singleton";
                        T inst = go.AddComponent<T>();
                        PC_SingletonMonobehaviour<T> cast = inst as PC_SingletonMonobehaviour<T>;
                        if (cast != null) cast.Initialize();
                        m_instance = (MonoBehaviour)inst;
                    }

                    if (!((PC_SingletonMonobehaviour<T>)m_instance).m_destroyOnLoad)
                        Object.DontDestroyOnLoad(m_instance.gameObject);
                }

                return (T)m_instance;
            }
        }

        /// <summary>
        /// Return the instance if it has been initialized, null otherwise.
        /// </summary>
        public static T nullableInstance
        {
            get { return (T)m_instance; }
        }

        /// <summary>
        /// If overriding, be sure to call base.Awake().
        /// </summary>
        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;

                if (!((PC_SingletonMonobehaviour<T>)m_instance).m_destroyOnLoad)
                    Object.DontDestroyOnLoad(m_instance.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}