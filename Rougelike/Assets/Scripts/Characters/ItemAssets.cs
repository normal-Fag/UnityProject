using UnityEngine;

public class ItemAssets : MonoBehaviour
{
   public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;

    public Sprite hpPotionSprite;
    public Sprite hpBuffSprite;
    public Sprite rageBuffSprite;
    public Sprite CD_BackGround;
}
