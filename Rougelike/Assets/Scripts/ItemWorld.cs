using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;



public class ItemWorld : MonoBehaviour
{

    public static ItemWorld SpawnItemWorld(Vector2 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();

        itemWorld.SetItem(item);

        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    private Light2D light2D;
    private Rigidbody2D rigidbody2D;


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2D = GetComponent<Light2D>();
    
    }

 
    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        light2D.color = item.GetColor();
        
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }


    public static ItemWorld DropItem(Vector2 dropPosition, Item item, float m_facingDirection, bool enemy)
    {
        ItemWorld itemWorld;

        if (enemy)
        {
            itemWorld = SpawnItemWorld(dropPosition + Vector2.up * m_facingDirection * 2f, item);
        }

        else
        {
            itemWorld = SpawnItemWorld(dropPosition + Vector2.right * m_facingDirection * 2f, item);
        }
        itemWorld.GetComponent<Rigidbody2D>().AddForce(Vector2.right * m_facingDirection * 2f, ForceMode2D.Impulse);
        return itemWorld;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
         
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

}
