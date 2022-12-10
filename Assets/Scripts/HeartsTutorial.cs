using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsTutorial : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(true);
            Destroy(gameObject, _destroyDelay);
        }
    }
}
