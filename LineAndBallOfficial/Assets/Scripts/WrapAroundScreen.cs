using UnityEngine;

public class WrapAroundScreen : MonoBehaviour
{
    private Transform ballPos;
    private Vector2 screenLeft, screenRight;

    void Start()
    {
        // Ekran s�n�rlar�n� belirliyoruz
        screenLeft = Camera.main.ViewportToWorldPoint(new Vector2(-3.6f, 0));
        screenRight = Camera.main.ViewportToWorldPoint(new Vector2(3.6f, 0));

        ballPos = GetComponent<Transform>();
    }

    void Update()
    {

        // E�er sol s�n�r�n d���na ��kt�ysa, sa� tarafa ge�ir
        if (ballPos.position.x < -3.6f)
        {
            ballPos.position = new Vector2(3.6f, ballPos.position.y);
        }
        // E�er sa� s�n�r�n d���na ��kt�ysa, sol tarafa ge�ir
        else if (ballPos.position.x > 3.6f)
        {
            ballPos.position = new Vector2(-3.6f, ballPos.position.y);
        }


    }
}
