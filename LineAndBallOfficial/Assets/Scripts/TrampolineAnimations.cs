using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineAnimations : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    private Transform trampoline;
    private int direction = 1;

    public float speed;

    private bool canUpdate = false;
    public float workingTime;
    private void Start()
    {
        trampoline = GetComponent<Transform>();
    }
    private void Update()
    {
        if (canUpdate)
        {
            PositionAdjuster();

            StartCoroutine(DisableUpdate(workingTime));
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            canUpdate = true;
        }
    }
    private void PositionAdjuster()//Ana menüdeki hareketli topun animasyon ayarlarýnýn yapýldýðý metot
    {
        Vector2 targetPos = CurrentTarget();
        trampoline.position = Vector2.Lerp(trampoline.position, targetPos, speed * Time.deltaTime);

        float distance = (targetPos - (Vector2)trampoline.position).magnitude;
        if (distance < 0.1f)
            direction *= -1;
    }
    private Vector2 CurrentTarget()
    {
        if(direction == 1)
            return endPoint.transform.position;
        else 
            return startPoint.transform.position;
    }

    private IEnumerator DisableUpdate(float sec)//Animasyon sýnýrlý bir süre oynatýlýr.
    {
        // 1 saniye bekle
        yield return new WaitForSeconds(sec);
        canUpdate = false;
    }
}
