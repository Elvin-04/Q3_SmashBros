using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerAttack : MonoBehaviour
{
    public float speed;
    public float _pourcent;
    public int _lifeMax = 5;
    public bool _joystickTuched;
    public Color _colorPlayer;
    public int _life;
    public bool _isPause;
    public string _name;
    public bool _dead;
    public Animator _animatorPlayer;
    public TextMeshProUGUI _nameText;
    public bool _pseudoEntree;
    public TMP_InputField _pseudochangeField;
    public TextMeshProUGUI _pseudoText;
    public GameObject _map;

    private float _propulsionForce;
    private Rigidbody2D _rb;
    private bool _attacking;
    private AnimatorClipInfo[] _anim;
    private float _pourcentInfliged;
    private Vector2 _force;
    private Vector2 _attackDirection;
    private bool _sideAttack;
    private bool _upAttack;
    private bool _downAttack;
    private PlayerInput _playerInput;
    private Gamepad pad;
    private GameObject _ejectEffect;
    //public enum Attack
    //{
    //    None,
    //    AttackSideRight,
    //    AttackSideLeft
    //};

    [Header("Info Shake")]
    public GameObject _objectToShake;
    public AnimationCurve shakeCurve;
    public float duration = 0.15f;

    [Header("Sounds")]
    public List<AudioClip> punchSounds;
    private AudioSource _audioSource;

    private void Start()
    {
        _sideAttack = true;
        _upAttack = true;
        _downAttack = true;
        _life = _lifeMax;
        _rb = GetComponent<Rigidbody2D>();
        _pourcentInfliged = 0;
        _attacking = false;
        _isPause = false;
        _dead = false;
        _audioSource = GetComponent<AudioSource>();
        _pseudoEntree = false;

        PlayerManager.instance.AddPlayer(gameObject);

        _playerInput = GetComponent<PlayerInput>();


        if (_playerInput != null)
        {
            pad = _playerInput.devices[0] as Gamepad;
        }


    }

    void Update()
    {
        if (_pseudoEntree) 
        {
            Vector3 positionTexte = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            gameObject.GetComponentInChildren<TextMeshProUGUI>().transform.position = positionTexte + new Vector3(0, 70, 0); // Ajustez cette valeur pour le positionnement vertical
        }
        else
        {
            _pseudochangeField.image.color = _pseudoText.color = GetComponent<PlayerMovements>().normaColor;
            _pseudoText.gameObject.SetActive(false);
            Time.timeScale = 0;
            _isPause = true;
        }
    }

    public void ChangingPseudo()
    {
        _pseudoText.gameObject.SetActive(true);
        string pseudo = _pseudochangeField.text;
        _pseudochangeField.gameObject.SetActive(false);
        _pseudoText.text = pseudo;
        _name = pseudo;
        _pseudoText.color = GetComponent<PlayerMovements>().normaColor;
        _pseudoEntree = true;
        _isPause = false;
        PlayerManager.instance.CheckAllPseudo();
    }

    public void AddPourcent(float pourcent)
    {
        _pourcent += pourcent;
    }

    public void ResetStat()
    {
        _pourcent = 0;
        _life = _lifeMax;
    }

    public void Propulse(float propulsionForce, Vector2 attackDirection)
    {
        GetComponent<PlayerMovements>().canMove = false;
        GetComponent<PlayerMovements>()._ejection = true;

        RumblePulse(0.8f, 0.8f, 0.1f);
        StartCoroutine(Shaking());
        int choosedSound = Random.Range(0, punchSounds.Count);
        _audioSource.clip = punchSounds[choosedSound];
        _audioSource.Play();

        StartCoroutine(WaitForSecontToMoove(0.5f));
        _force = attackDirection * propulsionForce * (_pourcent / 8);
        _rb.AddForce(_force, ForceMode2D.Impulse);
    }

    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        if (pad != null)
        {
            pad.SetMotorSpeeds(lowFrequency, highFrequency);
            Invoke("SetMotorSpeedToZero", duration);
        }
    }

    private void SetMotorSpeedToZero()
    {
        pad.SetMotorSpeeds(0f, 0f);
    }

    IEnumerator Shaking()
    {
        Vector2 startPosition = _objectToShake.transform.localPosition;
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strenght = shakeCurve.Evaluate(elapsedTime / duration);
            _objectToShake.transform.localPosition = startPosition + Random.insideUnitCircle * strenght;
            yield return null;
        }

        _objectToShake.transform.localPosition = startPosition;
    }

    public void BaseAttack(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            if (!_joystickTuched && _rb.velocity.x <= 0.5f && Input.GetAxis("Vertical") == 0f && !_isPause && !GetComponent<PlayerMovements>().onTheWall)
            {
                if (!_joystickTuched && _rb.velocity.x <= 0.5f && Input.GetAxis("Vertical") == 0f && !_isPause && GetComponent<PlayerMovements>().canMove && !GetComponent<PlayerMovements>().onTheWall)
                {
                    _animatorPlayer.Play("BaseAttack");
                    _attacking = true;
                    _pourcentInfliged = Random.Range(6, 8);
                    _propulsionForce = 1f;
                    _attackDirection = new Vector2(_rb.transform.forward.z, 2);
                }
            }
        }
    }

    public void SideAttack(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            if (_sideAttack && !_isPause && !GetComponent<PlayerMovements>().onTheWall)
            {
                if (_sideAttack && !_isPause && GetComponent<PlayerMovements>().canMove && !GetComponent<PlayerMovements>().onTheWall)
                {
                    _sideAttack = false;
                    _animatorPlayer.Play("SideAttack");
                    _attacking = true;
                    _pourcentInfliged = Random.Range(20, 22);
                    _propulsionForce = 1.5f;
                    _attackDirection = new Vector2(_rb.transform.forward.z, 1);
                    StartCoroutine(WaitForSecontSideAttack(0.6f));
                    StartCoroutine(WaitForSecontSideAttack(0.6f));
                }
            }
        }
    }

    public void UpAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_upAttack && !_isPause && GetComponent<PlayerMovements>().canMove && !GetComponent<PlayerMovements>().onTheWall)
            {
                _upAttack = false;
                _animatorPlayer.Play("UpAttack");
                _attacking = true;
                _pourcentInfliged = Random.Range(9, 11);
                _propulsionForce = 1.0f;
                _attackDirection = new Vector2(Random.Range(-0.5f, 0.5f), 2);
                StartCoroutine(WaitForSecontUpAttack(1f));
            }
        }
    }

    public void DownAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_downAttack && !_isPause && !GetComponent<PlayerMovements>().onTheWall)
            {
                if (_downAttack && !_isPause && GetComponent<PlayerMovements>().canMove && !GetComponent<PlayerMovements>().onTheWall)
                {
                    _downAttack = false;
                    _animatorPlayer.Play("DownAttack");
                    _attacking = true;
                    _pourcentInfliged = Random.Range(14, 16);
                    _propulsionForce = 1.5f;
                    _attackDirection = new Vector2(Random.Range(-0.5f, 0.5f), 2);
                    StartCoroutine(WaitForSecontDownAttack(1f));
                }
            }
        }
    }

    private IEnumerator WaitForSecontToMoove(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        GetComponent<PlayerMovements>().canMove = true;
    }

    private IEnumerator WaitForSecontSideAttack(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        _sideAttack = true;
    }

    private IEnumerator WaitForSecontUpAttack(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        _upAttack = true;
        GetComponent<PlayerMovements>().canMove = true;
    }

    private IEnumerator WaitForSecontDownAttack(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        _downAttack = true;
    }

    //private IEnumerator WaitToDestroyGameObject(GameObject objectToDestroy, float time) 
    //{
    //    yield return new WaitForSeconds(time);
    //    Destroy(objectToDestroy);
    //}

    public void Pause()
    {
        PausManager.instance.PausResumaGame();
    }

    public void SetEjectEffect(GameObject fire)
    {
        _ejectEffect = fire;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _attacking = false;
        }
        else
        {
            if ((collision.TryGetComponent(out PlayerAttack _playerTuched) || collision.TryGetComponent(out PlayerMovements _playerMovementTuched)) && _attacking && GetComponent<PlayerMovements>().canMove)
            {
                _playerTuched.AddPourcent(_pourcentInfliged);
                _playerTuched.Propulse(_propulsionForce, _attackDirection);
                _attacking = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "LimitMap")
        {
            GameObject effect;
            AudioManager.instance.PlayerDeath();
            _life -= 1;
            _pourcent = 0;
            GetComponent<PlayerMovements>().canMove = true;
            _rb.velocity = Vector3.zero;

            Vector3 direction = transform.position - _map.transform.position;

            if (transform.position.y >= 0)
            {
                effect = Instantiate(_ejectEffect, transform.position, Quaternion.Euler(0, 0, -Mathf.Atan(direction.normalized.x / direction.normalized.y) * Mathf.Rad2Deg + 180));
            }
            else
            {
                effect = Instantiate(_ejectEffect, transform.position, Quaternion.Euler(0, 0, -Mathf.Atan(direction.normalized.x / direction.normalized.y) * Mathf.Rad2Deg));
            }

            transform.position = PlayerManager.instance.transform.position;
        }

        if (_life == 0)
        {
            gameObject.transform.position = new Vector2(100000f, 0f);
            _dead = true;
            PlayerManager.instance._playerAlive--;
            PlayerManager.instance.RemovePlayer(gameObject);
        }
    }
}
