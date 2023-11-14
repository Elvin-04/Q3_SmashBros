using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float speed;
    public float _pourcent;
    public float _inputDeadZone = 0.3f;
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

    //public enum Attack
    //{
    //    None,
    //    AttackSideRight,
    //    AttackSideLeft
    //};

    private void Start()
    {
        _life = _lifeMax;
        _rb = GetComponent<Rigidbody2D>();
        _animatorPlayer = GetComponent<Animator>();
        _pourcentInfliged = 0;
        _attacking = false;
        _colorPlayer = GetComponent<SpriteRenderer>().color = Random.ColorHSV();

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
        StartCoroutine(WaitForSecont(0.5f));
        _force = attackDirection * propulsionForce * (_pourcent / 8);
        _rb.AddForce(_force, ForceMode2D.Impulse);
    }

    public void BaseAttack()
    {
        if (!_joystickTuched && _rb.velocity.x <= 0.5f && Input.GetAxis("Vertical") == 0f)
        {
            _animatorPlayer.SetTrigger("BaseAttack");
            _attacking = true;
            _pourcentInfliged = Random.Range(6, 8);
            _propulsionForce = 1.0f;
            _attackDirection = new Vector2(_rb.transform.forward.z, 1);
        }
    }

    public void SideAttack()
    {
        _animatorPlayer.SetTrigger("SideAttack");
        _attacking = true;
        _pourcentInfliged = Random.Range(20, 22);
        _propulsionForce = 1.5f;
        _attackDirection = new Vector2(_rb.transform.forward.z, 1);
    }

    public void UpAttack()
    {
        _animatorPlayer.SetTrigger("UpAttack");
        _attacking = true;
        _pourcentInfliged = Random.Range(9, 11);
        _propulsionForce = 1.0f;
        _attackDirection = new Vector2(Random.Range(-0.5f,0.5f), 2);
    }

    public void DownAttack()
    {
        _animatorPlayer.SetTrigger("DownAttack");
        _attacking = true;
        _pourcentInfliged = Random.Range(14, 16);
        _propulsionForce = 1.5f;
        _attackDirection = new Vector2(Random.Range(-0.5f, 0.5f), 2);
    }

    private IEnumerator WaitForSecont(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        GetComponent<PlayerMovements>().canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _anim = _animatorPlayer.GetCurrentAnimatorClipInfo(0);
        if ((collision.TryGetComponent(out PlayerAttack _playerTuched) || collision.TryGetComponent(out PlayerMovements _playerMovementTuched)) && _attacking && GetComponent<PlayerMovements>().canMove)
        {
            _playerTuched.AddPourcent(_pourcentInfliged);
            _playerTuched.Propulse(_propulsionForce, _attackDirection);
        }

        _attacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "LimitMap")
        {
            _life -= 1;
            _pourcent = 0;
            _rb.velocity = Vector3.zero;
            GetComponent<PlayerMovements>().jumpCount = 1;
            transform.position = PlayerManager.instance.transform.position;
        }

        if (_life == 0)
        {
            gameObject.SetActive(false);
            PlayerManager.instance._playerAlive--;
        }
    }
}
