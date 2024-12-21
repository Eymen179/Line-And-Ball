using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ballPosition;
    public Transform leftWallPosition, rightWallPosition, leftBackgroundPosition, rightBackgroundPosition, roofPosition;
    public Transform allWallPosition;
    void Update()//Topu y ekseninde takip eden kamera ve spesifik objeler
    {
        if (ballPosition.position.y < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, ballPosition.position.y, transform.position.z);

            allWallPosition.position = new Vector3(transform.position.x, ballPosition.position.y + transform.position.z);

        }
    }
}
