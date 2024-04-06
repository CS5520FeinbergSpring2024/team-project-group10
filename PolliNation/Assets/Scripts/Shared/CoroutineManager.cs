using UnityEngine;

//Use for used for handling tasks such as delays, timed operations, and animations.
public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;

    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var manager = new GameObject("CoroutineManager");
                _instance = manager.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(manager);
            }
            return _instance;
        }
    }

    public new Coroutine StartCoroutine(System.Collections.IEnumerator routine)
    {
        return base.StartCoroutine(routine);
    }
}
