using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CatLove : MonoBehaviour
{
    private int _heartCost = 50;
    private string _text = "Love me";
    private bool _pickupButtonPressed; // 'E'
    private PlayerController _playerController;
    private TextMeshProUGUI _textMesh;

    // mesh animation
    private Mesh _mesh;
    private Vector3[] _meshVertices;

    [SerializeField] private ParticleSystem _lightParticleSystem;
    [SerializeField] private ParticleSystem _heartParticleSystem;

    private bool _isPlayerHere = false;
    private bool _isLoveTaken = false;

    private AudioSource _audioSource;

    void Start()
    {
        _text += $"\n{_heartCost} ♥";
        _textMesh = gameObject.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        setText("");

        _lightParticleSystem = _lightParticleSystem.GetComponent<ParticleSystem>();
        _heartParticleSystem = _heartParticleSystem.GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _textMesh.ForceMeshUpdate();
        updateTextmesh();
        chooseLove();
    }


    bool isPlayer(string nameString)
    {
        return (nameString == "Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPlayer(other.gameObject.name)) { return; }
        setText(_text);
        _isPlayerHere = true;

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isPlayer(other.gameObject.name)) { return; }
        setText("");
        _isPlayerHere = false;
    }

    public void chooseLove()
    {
        if (_isLoveTaken && _heartParticleSystem.particleCount == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("EndScene");
            return;
        }

        if (!_isPlayerHere) return;

        _pickupButtonPressed = Input.GetKey(KeyCode.E);

        int playerHearts = PlayerPrefs.HasKey("Hearts") ? PlayerPrefs.GetInt("Hearts") : 0;

        if (_pickupButtonPressed && !_isLoveTaken && playerHearts >= _heartCost)
        {
            //TODO: effect of picking character
            _lightParticleSystem.Emit(1);
            _lightParticleSystem.Play();
            _heartParticleSystem.Emit(1);
            _heartParticleSystem.Play();
            _audioSource.Play();
            _isLoveTaken = true;
        }
    }

    void setText(string txt)
    {
        _textMesh.SetText(txt);
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
