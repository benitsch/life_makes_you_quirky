using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerdownDark : MonoBehaviour
{
    [SerializeField] private int _heartValue = 1;
    [SerializeField] private string _text = "Dark";
    private bool _pickupButtonPressed; // 'E'
    private PlayerController _playerController;
    //[SerializeField] private TextMeshProUGUI _textMesh; // reference
    private TextMeshProUGUI _textMesh; // reference

    // mesh animation
    private Mesh _mesh;
    private Vector3[] _meshVertices;

    // Start is called before the first frame update
    void Start()
    {
        _text += $"\n{_heartValue} ♥";
        _textMesh = gameObject.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        setText("");
    }

    // Update is called once per frame
    void Update()
    {
        _pickupButtonPressed = Input.GetKey(KeyCode.E);
        _textMesh.ForceMeshUpdate();
        updateTextmesh();
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
            //TODO: player effect
            _playerController.Hearts += _heartValue;
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

    void updateTextmesh()
    {
        if (_textMesh.text == "") { return; }
        _mesh = _textMesh.mesh;
        _meshVertices = _mesh.vertices;
        string s = _textMesh.text;
        for (int i = 0; i < _meshVertices.Length; i++)
        {
            Vector3 offset = Wobble(Time.time + i);
            _meshVertices[i] = _meshVertices[i] + offset;
        }
        _mesh.vertices = _meshVertices;
        _textMesh.canvasRenderer.SetMesh(_mesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f));
    }

}
