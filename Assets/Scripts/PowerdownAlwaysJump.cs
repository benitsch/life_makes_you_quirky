using UnityEngine;
using TMPro;

public class PowerdownAlwaysJump : MonoBehaviour
{
    private int _heartValue = 10;
    private string _text = "Always Jump";
    private bool _pickupButtonPressed; // 'E'
    private PlayerController _playerController;
    private TextMeshProUGUI _textMesh;

    // mesh animation
    private Mesh _mesh;
    private Vector3[] _meshVertices;

    [SerializeField] private ParticleSystem _lightParticleSystem;
    [SerializeField] private ParticleSystem _heartParticleSystem;

    private bool _isPlayerHere = false;
    private bool _isPowerdownTaken = false;

    private AudioSource _audioSource;

    void Start()
    {
        _text += $"\n{_heartValue} ♥";
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _textMesh = gameObject.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        setText("");

        _lightParticleSystem = _lightParticleSystem.GetComponent<ParticleSystem>();
        _heartParticleSystem = _heartParticleSystem.GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.5f;
    }

    void Update()
    {
        _textMesh.ForceMeshUpdate();
        updateTextmesh();
        applyPowerdown();
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

    public void applyPowerdown()
    {
        if (_isPowerdownTaken && _heartParticleSystem.particleCount == 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!_isPlayerHere) return;

        _pickupButtonPressed = Input.GetKey(KeyCode.E);

        if (_pickupButtonPressed && !_isPowerdownTaken)
        {
            _playerController.IsAlwaysJumpingActive = !_playerController.IsAlwaysJumpingActive;
            _playerController.AddHearts(_heartValue);
            _lightParticleSystem.Emit(1);
            _lightParticleSystem.Play();
            _heartParticleSystem.Emit(1);
            _heartParticleSystem.Play();
            _audioSource.Play();
            _isPowerdownTaken = true;
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
