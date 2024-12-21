using UnityEngine;

public class WrapAroundScreen : MonoBehaviour
{
    private Transform ballPos;
    private Vector2 screenLeft, screenRight;

    void Start()
    {
        // Ekran sýnýrlarýný belirliyoruz
        screenLeft = Camera.main.ViewportToWorldPoint(new Vector2(-3.6f, 0));
        screenRight = Camera.main.ViewportToWorldPoint(new Vector2(3.6f, 0));

        ballPos = GetComponent<Transform>();
    }

    void Update()
    {

        // Eðer sol sýnýrýn dýþýna çýktýysa, sað tarafa geçir
        if (ballPos.position.x < -3.6f)
        {
            ballPos.position = new Vector2(3.6f, ballPos.position.y);
        }
        // Eðer sað sýnýrýn dýþýna çýktýysa, sol tarafa geçir
        else if (ballPos.position.x > 3.6f)
        {
            ballPos.position = new Vector2(-3.6f, ballPos.position.y);
        }


    }
}
