using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerdownAlwaysJump : MonoBehaviour
{
    [SerializeField] private int _heartValue = 10;
    [SerializeField] private string _text = "Always Jump";
    private bool _pickupButtonPressed; // 'E'
    private PlayerController _playerController;
    //[SerializeField] private TextMeshProUGUI _textMesh; // reference
    private TextMeshProUGUI _textMesh; // reference

    // Start is called before the first frame update
    void Start()
    {
        _textMesh = gameObject.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        setText("");
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
        setText(_text);

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
            _playerController.IsAlwaysJumpingActive = !_playerController.IsAlwaysJumpingActive;
            _playerController.AddHearts(_heartValue);
            Destroy(gameObject);
        }
    }

    void setText(string txt)
    {
        _textMesh.SetText(txt);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(other.name);
        if (!isPlayer(other.gameObject.name)) {return;}
        setText("");
    }

}
