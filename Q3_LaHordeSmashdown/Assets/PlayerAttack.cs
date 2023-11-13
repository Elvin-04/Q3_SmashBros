using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float speed;
    public GameObject _rightArm;
    public GameObject _leftArm;
    public GameObject _rightWallDetection;
    public GameObject _leftWallDetection;
    public float _pourcent;
    public float _inputDeadZone = 0.3f;
    public int _lifeMax = 5;
    public bool _joystickTuched;

    private float _propulsionForce;
    private Rigidbody2D _rb;
    private Animator _animatorPlayer;
    private bool _attacking;
    private AnimatorClipInfo[] _anim;
    private float _pourcentInfliged;
    private Vector2 _force;
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
        _rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), _leftArm.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _leftArm.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_rightWallDetection.GetComponent<Collider2D>(), _leftWallDetection.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_rightWallDetection.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _leftWallDetection.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), PlayerManager.instance._map.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), PlayerManager.instance._map.GetComponent<Collider2D>());
        _animatorPlayer = GetComponent<Animator>();
        _pourcentInfliged = 0;
        _attacking = false;
        foreach (var player in PlayerManager.instance._playerList)
        {
            Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._rightArm.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._leftArm.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._leftArm.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._rightArm.GetComponent<Collider2D>());

            Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._rightWallDetection.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_rightArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._leftWallDetection.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._leftWallDetection.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(_leftArm.GetComponent<Collider2D>(), player.GetComponent<PlayerAttack>()._rightWallDetection.GetComponent<Collider2D>());
        }
        PlayerManager.instance._playerList.Add(gameObject);
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
        GetComponent<PlayerMovements>().canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        _anim = _animatorPlayer.GetCurrentAnimatorClipInfo(0);
        if ((collision.TryGetComponent(out PlayerAttack _playerTuched) || collision.TryGetComponent(out PlayerMovements _playerMovementTuched)) && _attacking && GetComponent<PlayerMovements>().canMove)
        {
            Debug.Log("ouvhb");
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
            transform.position = PlayerManager.instance.transform.position;
        }
    }
}
