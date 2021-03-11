using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public AudioSource grunt;
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        grunt.Play();
    }
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
