using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float speed;
    public float _pourcent;
    public int _lifeMax = 5;
    public bool _joystickTuched;
    public Color _colorPlayer;
    public int _life;

    private float _propulsionForce;
    private Rigidbody2D _rb;
    private Animator _animatorPlayer;
    private bool _attacking;
    private AnimatorClipInfo[] _anim;
    private float _pourcentInfliged;
    private Vector2 _force;
    private Vector2 _attackDirection;
    private bool _sideAttack;
    private bool _upAttack;
    private bool _downAttack;

    //public enum Attack
    //{
    //    None,
    //    AttackSideRight,
    //    AttackSideLeft
    //};

    private void Start()
    {
        _sideAttack = true;
        _upAttack = true;
        _downAttack = true;
        _life = _lifeMax;
        _rb = GetComponent<Rigidbody2D>();
        _animatorPlayer = GetComponent<Animator>();
        _pourcentInfliged = 0;
        _attacking = false;

        PlayerManager.instance.AddPlayer(gameObject);
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
        StartCoroutine(WaitForSecontToMoove(0.5f));
        _force = attackDirection * propulsionForce * (_pourcent / 8);
        _rb.AddForce(_force, ForceMode2D.Impulse);
    }

    public void BaseAttack()
    {
        if (!_joystickTuched && _rb.velocity.x <= 0.5f && Input.GetAxis("Vertical") == 0f)
        {
            _animatorPlayer.Play("BaseAttack");
            _attacking = true;
            _pourcentInfliged = Random.Range(6, 8);
            _propulsionForce = 1f;
            _attackDirection = new Vector2(_rb.transform.forward.z, 2);
        }
    }

    public void SideAttack()
    {
        if (_sideAttack)
        {
            _sideAttack = false;
            _animatorPlayer.Play("SideAttack");
            _attacking = true;
            _pourcentInfliged = Random.Range(20, 22);
            _propulsionForce = 1.5f;
            _attackDirection = new Vector2(_rb.transform.forward.z, 1);
            StartCoroutine(WaitForSecontSideAttack(2f));
        }
    }

    public void UpAttack()
    {
        if (_upAttack)
        {
            _upAttack = false;
            _animatorPlayer.Play("UpAttack");
            _attacking = true;
            _pourcentInfliged = Random.Range(9, 11);
            _propulsionForce = 1.0f;
            _attackDirection = new Vector2(Random.Range(-0.5f,0.5f), 2);
            StartCoroutine(WaitForSecontUpAttack(1f));
        }
    }

    public void DownAttack()
    {
        if (_downAttack)
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
    }

    private IEnumerator WaitForSecontDownAttack(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        _downAttack = true;
    }

    public void Pause()
    {
        PausManager.instance.PausResumaGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _attacking = false;
        }
        else
        {
            if ((collision.TryGetComponent(out PlayerAttack _playerTuched) || collision.TryGetComponent(out PlayerMovements _playerMovementTuched)) && _attacking && GetComponent<PlayerMovements>().canMove)
            {
                if (_playerTuched.gameObject.GetComponent<PlayerMovements>().canMove)
                {
                    _playerTuched.AddPourcent(_pourcentInfliged);
                    _playerTuched.Propulse(_propulsionForce, _attackDirection);
                    _attacking = false;
                }
            }
        }
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
                if (_playerTuched.gameObject.GetComponent<PlayerMovements>().canMove)
                {
                    _playerTuched.AddPourcent(_pourcentInfliged);
                    _playerTuched.Propulse(_propulsionForce, _attackDirection);
                    _attacking = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "LimitMap")
        {
            _life -= 1;
            _pourcent = 0;
            _rb.velocity = Vector3.zero;
            transform.position = PlayerManager.instance.transform.position;
        }

        if (_life == 0)
        {
            gameObject.transform.position = new Vector2(10000f, 0f);
            PlayerManager.instance._playerAlive--;
        }
    }
}
