using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int Hearts;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _barks;
    [SerializeField] private AudioClip _annoyingBackgroundMusic;

    [SerializeField] private ParticleSystem _left_particleSystem;
    [SerializeField] private ParticleSystem _right_particleSystem;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        //_particleSystem = GetComponent<ParticleSystem>();
        Hearts = 0;
        IsAlwaysJumpingActive = false;
    }

    void Start()
    {

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
        _currentVelocity = _rb.velocity;

        if (Input.GetButton("Jump") || IsAlwaysJumpingActive)
        {
            _isJumping = true;
        } else
        {
            _isJumping = false;
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

    private void Jump()
    {
        if (_isJumping && IsOnGround())
        {
            _rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Force);
            _isJumping = false;
        }
        // For smooth jump
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
        } else if (_rb.velocity.y > 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.15f, GroundLayer);
    }

    private void PlayBarkSound()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int idx = Random.Range(0, _barks.Length);
            _audioSource.PlayOneShot(_barks[idx]);
        }
    }

    public void PlayAnnoyingMusic()
    {   
        _audioSource.clip = _annoyingBackgroundMusic;
        _audioSource.Play();
    }
}
