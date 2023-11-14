using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    private Camera cam;

    private float posX;
    private float posY;
    private Vector3 pos;

    [Header("Cam")]
    public float maxSize = 8;
    public float minSize = 2.5f;
    float actSize = 0.0f;
    bool started = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        posX = 0;
        posY = 0;
        SetCameraPosition();
    }

    private void FixedUpdate()
    {
        if (playerManager._playerList.Count == 2)
        {
            posX = (playerManager._playerList[0].transform.position.x + playerManager._playerList[1].transform.position.x) / 2;
            posY = (playerManager._playerList[0].transform.position.y + playerManager._playerList[1].transform.position.y) / 2;
            SetCameraPosition();


            actSize = Vector2.Distance(playerManager._playerList[0].transform.position, playerManager._playerList[1].transform.position) / 2 + 1.5f;
            if(!started)
            {
                started = true;

                if(actSize < minSize)
                    actSize = minSize;
                else if(actSize > maxSize)
                    actSize = maxSize;

                cam.orthographicSize = actSize;
            }

            if(actSize > minSize && actSize < maxSize)
            {
                cam.orthographicSize = actSize;
            }
        }


    }

    private void SetCameraPosition()
    {
        pos.Set(posX, posY, -10);
        transform.position = pos;
    }
}
