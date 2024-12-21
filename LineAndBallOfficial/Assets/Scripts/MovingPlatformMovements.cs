using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformMovements : MonoBehaviour
{
    private float pointA = -1.1f; // �lk nokta (x ekseninde)
    private float pointB = 1.1f; // �kinci nokta (x ekseninde)
    public float speed; // Hareket h�z�

    private float targetX; // Hedef x koordinat�
    private Transform movingPlatformPosition; // Ba�lang�� pozisyonu (Y ve Z koordinatlar�n� korumak i�in)

    void Start()
    {
        movingPlatformPosition = GetComponent<Transform>();
        targetX = pointB; // Ba�lang��ta platform pointB'ye do�ru hareket edecek
    }

    void Update()
    {
        // Platformun mevcut Y ve Z koordinatlar�n� koruyarak x ekseninde hedefe do�ru hareket et
        Vector2 targetPosition = new Vector2(targetX, movingPlatformPosition.position.y);
        transform.position = Vector2.MoveTowards(new Vector2(movingPlatformPosition.position.x, movingPlatformPosition.position.y), targetPosition, speed * Time.deltaTime);

        // Hedef noktaya ula��ld���nda hedefi de�i�tir
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            targetX = (targetX == pointB) ? pointA : pointB;
        }
        Vector2 savingPos = new Vector2(movingPlatformPosition.position.x, transform.position.y);
        movingPlatformPosition.position = savingPos;
    }
}
