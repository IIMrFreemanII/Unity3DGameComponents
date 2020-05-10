using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Only one instance of Player is allowed!");
        }
    }
}
