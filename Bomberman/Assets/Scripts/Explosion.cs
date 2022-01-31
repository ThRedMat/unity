using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public enum ExplosionType
    {
        Middle,
        Line,
        Side
    }

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;

        BombManager.OnExplode += OnExplode;
    }

    void OnDisable()
    {
        BombManager.OnExplode -= OnExplode;
    }

    void OnExplode()
    {
        animator.speed = 1;

        // Detect Player
        if (Physics2D.IsTouching(gameObject.GetComponent<Collider2D>(), PlayerMovement.Instance.gameObject.GetComponent<Collider2D>()))
        {
            PlayerMovement.Instance.Die();
        }
    }
}
