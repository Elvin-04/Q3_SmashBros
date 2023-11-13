using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject _rightArm;
    public GameObject _leftArm;
    public float _pourcent;
    public float _inputDeadZone = 0.3f;
    public int _lifeMax = 5;

    private float _propulsionForce;
    private Rigidbody2D _rb;
    private Animator _animatorPlayer;
    private bool _attacking;
    private AnimatorClipInfo[] _anim;
    private float _pourcentInfliged;
    private Vector2 _force;
    private bool _cantMoov;
    private bool _joystickTuched;
    private Vector2 _attackDirection;
    public int _life;

    //public enum Attack
    //{
    //    None,
    //    AttackSideRight,
    //    AttackSideLeft
    //};

    private void Start()
    {
        _life = _lifeMax;
        _cantMoov = false;
        _rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), _leftArm.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _leftArm.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), PlayerManager.instance._map.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), PlayerManager.instance._map.GetComponent<Collider2D>());
        _animatorPlayer = GetComponent<Animator>();
        _pourcentInfliged = 0;
        _attacking = false;
        foreach (var player in PlayerManager.instance._playerList)
        {
            Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), player.GetComponent<Player>()._rightArm.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), player.GetComponent<Player>()._leftArm.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), player.GetComponent<Player>()._leftArm.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), player.GetComponent<Player>()._rightArm.GetComponent<Collider2D>());
        }
        foreach (var mapChild in PlayerManager.instance._mapChilder)
        {
            Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), mapChild.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), mapChild.GetComponent<Collider2D>());
        }
        PlayerManager.instance._playerList.Add(gameObject);
    }

    public void Moovement(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _joystickTuched = true;
        }

        if (value.canceled)
        {
            _joystickTuched = false;
        }

        if (!_cantMoov)
        {
            Vector2 inputMoovement = value.ReadValue<Vector2>();
            if (inputMoovement.x > _inputDeadZone)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (inputMoovement.x < -_inputDeadZone)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            _rb.velocity = inputMoovement.normalized * speed;
        }
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
        _cantMoov = true;
        StartCoroutine(WaitForSecont(1.0f));
        _force = attackDirection * propulsionForce * (_pourcent / 8);
        _rb.AddForce(_force, ForceMode2D.Impulse);
    }

    public void BaseAttack()
    {
        if (!_joystickTuched)
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
        _cantMoov = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _anim = _animatorPlayer.GetCurrentAnimatorClipInfo(0);
        if (collision.TryGetComponent(out Player _playerTuched) && _attacking && !_cantMoov)
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
            transform.position = PlayerManager.instance.transform.position;
        }
    }
}
