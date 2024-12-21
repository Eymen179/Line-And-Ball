using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformMovements : MonoBehaviour
{
    private float pointA = -1.1f; // Ýlk nokta (x ekseninde)
    private float pointB = 1.1f; // Ýkinci nokta (x ekseninde)
    public float speed; // Hareket hýzý

    private float targetX; // Hedef x koordinatý
    private Transform movingPlatformPosition; // Baþlangýç pozisyonu (Y ve Z koordinatlarýný korumak için)

    void Start()
    {
        movingPlatformPosition = GetComponent<Transform>();
        targetX = pointB; // Baþlangýçta platform pointB'ye doðru hareket edecek
    }

    void Update()
    {
        // Platformun mevcut Y ve Z koordinatlarýný koruyarak x ekseninde hedefe doðru hareket et
        Vector2 targetPosition = new Vector2(targetX, movingPlatformPosition.position.y);
        transform.position = Vector2.MoveTowards(new Vector2(movingPlatformPosition.position.x, movingPlatformPosition.position.y), targetPosition, speed * Time.deltaTime);

        // Hedef noktaya ulaþýldýðýnda hedefi deðiþtir
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            targetX = (targetX == pointB) ? pointA : pointB;
        }
        Vector2 savingPos = new Vector2(movingPlatformPosition.position.x, transform.position.y);
        movingPlatformPosition.position = savingPos;
    }
}
