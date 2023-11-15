using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    public Transform levelFocus;
    private Camera cam;

    private float posX;
    private float posY;
    private Vector3 pos;
    public List<Transform> players;

    [Header("Cam")]
    public float maxSize = 8;
    public float minSize = 2.5f;
    public float smoothTime = 0.5f;
    float actSize = 0.0f;
    bool started = false;

    private Vector2 velocity = Vector2.zero;

    [Header("CameraBounds")]
    public Vector3 minValues;
    public Vector3 maxValues;


    public List<Vector2> twofurther = new List<Vector2>(2) { new Vector2(0, 0), new Vector2(0, 0) };

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        posX = 0;
        posY = 0;
        players.Add(levelFocus);
    }

    private void FixedUpdate()
    {

        posX = 0;
        posY = 0;
        for (int i = 0; i < players.Count; i++)
        {
            posX += players[i].position.x;
            posY += players[i].position.y;
        }
        posX /= players.Count;
        posY /= players.Count;
        SetCameraPosition();

        twofurther = new List<Vector2>(2) { new Vector2(0,0), new Vector2(0,0) };
        List<float> values = new List<float>(2) { 0, 0 };

        for (int i = 0; i < players.Count; i++)
        {
            float newDistance = Vector2.Distance(transform.position, players[i].position);
            if (values[0] < newDistance)
            {
                twofurther[1] = twofurther[0];
                twofurther[0] = players[i].position;
                values[1] = values[0];
                values[0] = newDistance;
            }
            else if (values[1] < newDistance)
            {
                twofurther[1] = players[i].position;
                values[1] = newDistance;
            }
        }

        actSize = Vector2.Distance(twofurther[0], twofurther[1]) / 2 + 1.5f;

        if (!started)
        {
            started = true;

            if (actSize < minSize)
                actSize = minSize;
            else if (actSize > maxSize)
                actSize = maxSize;

            cam.orthographicSize = actSize;
        }

        if (actSize > minSize && actSize < maxSize)
        {
            cam.orthographicSize = actSize;
        }


    }

    private void SetCameraPosition()
    {
        pos.Set(posX, posY, -10);

        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(pos.x, minValues.x, maxValues.x),
            Mathf.Clamp(pos.y, minValues.y, maxValues.y),
            Mathf.Clamp(pos.z, minValues.z, maxValues.z));

        transform.position = Vector2.SmoothDamp(transform.position, boundPosition, ref velocity, smoothTime);
        transform.position += new Vector3(0, 0, -10);
    }

}
