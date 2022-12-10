using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerdownSpeed : MonoBehaviour
{
    private int _heartValue = 2;
    private bool _pickupButtonPressed; // 'E'
    private PlayerController _playerController;

    [SerializeField] private float _slowerPercentage = 0.5F;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<TextMeshProUGUI>().text = "";
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _pickupButtonPressed = Input.GetKey(KeyCode.E);
    }

     // Player Collision CODE

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.name);
        if (!isPlayer(other.gameObject.name)) {return;}
        GameObject.FindObjectOfType<TextMeshProUGUI>().text = "Speed";
              
    }

    bool isPlayer(string nameString) {
        return (nameString == "Player");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Hello");
        if (!isPlayer(other.gameObject.name)) {return;}
        applyPowerdown();

    }

    public void applyPowerdown()
    {
        if (_pickupButtonPressed)
        {
            _playerController.Speed = (_playerController.Speed * _slowerPercentage);
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(other.name);
        if (!isPlayer(other.gameObject.name)) {return;}
        GameObject.FindObjectOfType<TextMeshProUGUI>().text = "";
    }

}
