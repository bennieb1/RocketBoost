using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set this instance as the singleton instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
}
