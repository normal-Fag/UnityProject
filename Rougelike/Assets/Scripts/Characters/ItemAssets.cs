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
    public Sprite InfinityHpBuffSprite;
    public Sprite AttackBuffSprite;
    public Sprite SkillBuffSprite;


    public Sprite PoisonSprite;
    public Sprite ManaPotionSprite;
    public Sprite RegenManaPotionSprite;
    public Sprite ManaStoneSprite;
    public Sprite DaggerSprite;
    public Sprite InfinityBagSprite;
    public Sprite PosionBagSprite;
    public Sprite InfinityAttackBuffSprite;
    public Sprite SpareBagSprite;
    public Sprite PhoenixFeatherSprite;
    public Sprite BurstStoneSprite;
    public Sprite DropOfFurySprite;
    public Sprite SkullOfRageSprite;
    public Sprite ScrollOfKnowledgeSprite;


}
