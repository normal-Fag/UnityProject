using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarController : MonoBehaviour
{
    public GameObject canvasUI;
    public Slider slider;

    private float health;
    private float maxHealth;
    private Enemy enemy;
    private RectTransform rectTransform;
    private Vector3 scale;

    void Start()
    {
        rectTransform = canvasUI.GetComponent<RectTransform>();
        scale = rectTransform.localScale;
        enemy = GetComponent<Enemy>();
        maxHealth = enemy.health;
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void Update()
    {
        health = enemy.health;
        slider.value = CalculateHealth();

        if (health < maxHealth)
        {
            canvasUI.SetActive(true);
        }

        FacingUI();
    }

    private float CalculateHealth()
    {
        return health / maxHealth;
    }

    private void FacingUI()
    {
        scale.x = enemy.facingDirection;
        rectTransform.localScale = scale;
    }
}
