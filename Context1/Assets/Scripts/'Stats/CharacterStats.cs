using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int Health = 100;
    public int currenthealth { get; private set; }

    public Stat damage;
    public Stat armor;

    void Awake()
    {
        currenthealth = Health;
    }
    public void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currenthealth -= damage;
        if(currenthealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        //meant to be overwritten so each character can die in their own ways.
    }
}
