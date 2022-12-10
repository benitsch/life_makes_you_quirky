using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsTutorial : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 8.0f;
    [SerializeField] private GameObject _canvasTutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _canvasTutorial.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _canvasTutorial.SetActive(true);
            Destroy(gameObject, _destroyDelay);
        }
    }
}
