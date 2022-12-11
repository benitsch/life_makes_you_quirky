using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    private float _horizontalMovement;
    public float Speed = 10.0f;
    public int InvertedController = 1;
    public bool IsAlwaysJumpingActive;

    private bool _isJumping = false;
    public float JumpForce = 3000f;
    private float _fallMultiplier = 2.5f;
    private float _lowJumpMultiplier = 2f;

    private Rigidbody2D _rb;
    private Vector2 _currentVelocity;

    public Transform GroundCheck;
    public LayerMask GroundLayer;

    private int _hearts;
    [SerializeField] private TextMeshProUGUI _heartsText;

    private AudioSource _audioSource;
    private AudioClip _currentBark;
    private float _barkStartTime;
    [SerializeField] private AudioClip[] _barks;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _runSound;
    [SerializeField] private AudioClip _annoyingBackgroundMusic;

    [SerializeField] private ParticleSystem _left_particleSystem;
    [SerializeField] private ParticleSystem _right_particleSystem;

    private SpriteRenderer _spriteRenderer;

    private Animator _animator;

    // volumes
    private float _runSoundVolume = 2.0f;
    private float _jumpSoundVolume = 2.0f;
    private float _barkSoundVolume = 2.0f;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        IsAlwaysJumpingActive = false;
    }

    void Start()
    {
        _hearts = PlayerPrefs.HasKey("Hearts") ? PlayerPrefs.GetInt("Hearts") : 0;
        SetHeartsText();
    }

    void Update()
    {
        GetInput();
        PlayBarkSound();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }

    private bool IsMoving()
    {
        return _horizontalMovement > 0.1f || _horizontalMovement < 0.1f;
    }
    
    private void GetInput()
    {
        _horizontalMovement = Input.GetAxis("Horizontal");
        _animator.SetFloat("Speed", Mathf.Abs(_horizontalMovement));
        FlipSprite();
        _currentVelocity = _rb.velocity;

        if (Input.GetButton("Jump") || IsAlwaysJumpingActive)
        {
            _isJumping = true;
        } else
        {
            _isJumping = false;
        }
    }

    private void FlipSprite()
    {
        if ((_horizontalMovement > 0.1f && InvertedController == 1) || (_horizontalMovement < -0.1f && InvertedController == -1))
        {
            _spriteRenderer.flipX = false;
        }
        else if ((_horizontalMovement > 0.1f && InvertedController == -1) || (_horizontalMovement < -0.1f && InvertedController == 1))
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void Movement()
    {
        if (IsMoving())
        {
            PlayDirtParticle();
            _rb.velocity = new Vector2(_horizontalMovement * Speed * InvertedController, _currentVelocity.y);
        }
    }

    private void PlayDirtParticle()
    {
        if (IsOnGround())
        {
            if ((_horizontalMovement > 0.1f && InvertedController == 1) || (_horizontalMovement < -0.1f && InvertedController == -1))
            {
                _left_particleSystem.Play();
            }
            else if((_horizontalMovement > 0.1f && InvertedController == -1) || (_horizontalMovement < -0.1f && InvertedController == 1))
            {
                _right_particleSystem.Play();
            }
            else
            {
                _left_particleSystem.Stop();
                _right_particleSystem.Stop();
            }
        }
        else
        {
            _left_particleSystem.Stop();
            _right_particleSystem.Stop();
        }
    }

    private void Jump()
    {
        if (_isJumping && IsOnGround())
        {
            _rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Force);
            PlayJumpSound();
            _animator.SetBool("IsJumpingUp", true);
            _isJumping = false;
        }
        if (IsOnGround()){
            _animator.SetBool("IsJumpingUp", false);
            _animator.SetBool("IsFalling", false);
        }

        // For smooth jump
        if (_rb.velocity.y <= 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
            if (!IsOnGround())
            {
                _animator.SetBool("IsJumpingUp", false);
                _animator.SetBool("IsFalling", true);
            }
            
        } else if (_rb.velocity.y > 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            if (!IsOnGround())
            {
                _animator.SetBool("IsJumpingUp", true);
                _animator.SetBool("IsFalling", false);
            }
            
        } else
        {
            _animator.SetBool("IsJumpingUp", false);
            _animator.SetBool("IsFalling", false);
        }
    }
    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.15f, GroundLayer);
    }

    private void PlayBarkSound()
    {
        if (Input.GetKeyDown(KeyCode.R) && !IsBarkClipPlaying())
        {
            int idx = Random.Range(0, _barks.Length);
            _currentBark = _barks[idx];
            _barkStartTime = Time.time;
            _audioSource.PlayOneShot(_currentBark, _barkSoundVolume);

        }
    }

    private void PlayJumpSound()
    {
        _audioSource.PlayOneShot(_jumpSound, _jumpSoundVolume);
    }

    public bool IsBarkClipPlaying()
    {
        if (_barkStartTime != 0 && _currentBark != null)
        {
            return !((Time.time - _barkStartTime) >= _currentBark.length);
        }

        return false;
    }

    public void PlayAnnoyingMusic()
    {   
        _audioSource.clip = _annoyingBackgroundMusic;
        _audioSource.Play();
    }

    public void AddHearts(int heartsAmount)
    {
        _hearts += heartsAmount;
        SetHeartsText();
        AddHeartsToPlayerPrefs();
    }

    public void SetHeartsText()
    {
        _heartsText.SetText($"{_hearts}");
    }

    private void AddHeartsToPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Hearts"))
        {
            PlayerPrefs.SetInt("Hearts", _hearts);
        }
        else
        {
            PlayerPrefs.SetInt("Hearts", _hearts);
        }
    }
}
