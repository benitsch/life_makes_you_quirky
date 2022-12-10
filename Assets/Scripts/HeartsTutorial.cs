using UnityEngine;

public class HeartsTutorial : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 8.0f;
    [SerializeField] private GameObject _canvasTutorial;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("esc");
            _canvasTutorial.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _canvasTutorial.SetActive(true);
            Invoke("HideCanvas", _destroyDelay);
            Destroy(gameObject, _destroyDelay);
        }
    }

    void HideCanvas()
    {
        _canvasTutorial.SetActive(false);
    }
}
