
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyManager : MonoBehaviour
{

    private List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    void Update()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            enemies[i].Move();

            if (enemies[i].isAtTarget() || enemies[i].isDestroyed())
            {
                if (enemies[i].isAtTarget())
                {
                    // player.takeDamage(enemies[i].damageToObject);
                }
                Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
            }
        }
    }
}