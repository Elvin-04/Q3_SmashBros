using UnityEngine;

public class WallDetection : MonoBehaviour
{
    PlayerMovements playerMovements;

    private void Start()
    {
        playerMovements = transform.parent.GetComponent<PlayerMovements>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
            playerMovements.GrabWall();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerMovements.UnGrabWall();
    }
}
