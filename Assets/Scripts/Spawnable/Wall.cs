using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public WallType wallType;
    public BoxCollider2D boxCollider;

    public Sprite undamaged;

    private enum WallHealth
    {
        NoDamage,
        MinorDamage,
        MediumDamage,
        MajorDamage,
        Destroyed
    }

    public enum WallType
    {
        Fence,
        Wooden,
        Stone,
        Iron,
        Gold
    }

    public List<Sprite> wallSprites; // sprites for representing wall's 
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        Debug.Assert(spriteRenderer != null);
        Debug.Assert(maxHealth > 0);
        
        health = maxHealth;
    }

    // Returns a sprite representing the given wall health enum
    private Sprite GetWallSprite(WallHealth wallHealth)
    {
        return wallSprites[(int)wallHealth];
    }

    // Method will set a sprite for the wall that reflects its current health
    private void UpdateWallSprite()
    {
        float healthAsPercentage = ((float)health / maxHealth);

        if (healthAsPercentage >= .80f)
            spriteRenderer.sprite = GetWallSprite(WallHealth.NoDamage);
        else if (healthAsPercentage >= .60f)
            spriteRenderer.sprite = GetWallSprite(WallHealth.MinorDamage);
        else if (healthAsPercentage >= .40f)
            spriteRenderer.sprite = GetWallSprite(WallHealth.MediumDamage);
        else if (healthAsPercentage >= .20f)
            spriteRenderer.sprite = GetWallSprite(WallHealth.MajorDamage);
        else if (healthAsPercentage <= 0.0f)
            spriteRenderer.sprite = GetWallSprite(WallHealth.Destroyed);
    }

    // Adds health to the wall
    public void TakeHeal(int amount)
    {
        if (health <= 0)
            return;

        amount = (amount < 0) ? 0 : amount;
        health += amount;
        UpdateWallSprite();
    }

    public void SetHealth(int amount)
    {
        health = amount;
        boxCollider.enabled = (health > 0);
        UpdateWallSprite();
    }
    
    // Inflicts damage against the wall
    public void TakeDamage(int damage)
    {
        if (health <= 0)
            return;
        
        damage = (damage < 0) ? 0 : damage;
        health -= damage;
        if (health == 0.0f)
            boxCollider.enabled = false;
        UpdateWallSprite();
    }
}
