using UnityEngine;

public class bg : MonoBehaviour
{
    public SpriteRenderer bg1;
    public SpriteRenderer bg2;
    public float speed=5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            var color1 = bg1.color;
            color1.a -= speed * Time.deltaTime;
            bg1.color = color1;
        }
    }
}
