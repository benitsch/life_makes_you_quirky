using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Awake()
    {
        PlayerPrefs.SetInt("Hearts", 0);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
