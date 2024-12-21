using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AnimatedBallMovements : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform animBall;
    private int direction = 1;

    public float speed;

    void Update()
    {
        PositionAdjuster();
    }
    private void PositionAdjuster()//Ana menüdeki hareketli topun animasyon ayarlarýnýn yapýldýðý metot
    {
        Vector2 targetPos = CurrentTarget();
        animBall.position = Vector2.Lerp(animBall.position, targetPos, speed * Time.deltaTime);

        float distance = (targetPos - (Vector2)animBall.position).magnitude;
        if (distance < 0.1f)
            direction *= -1;
    }
    private Vector2 CurrentTarget()
    {
        if (direction == 1)
            return endPoint.transform.position;
        else
            return startPoint.transform.position;
    }
}
