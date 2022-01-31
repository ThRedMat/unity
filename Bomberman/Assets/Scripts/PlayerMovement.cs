using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Player")] 
    [SerializeField] private float Speed;
    [SerializeField] private bool Dead;
    public LayerMask PLayerMask;
    
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        if (Instance == null) Instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Dead) return;

        // Bombs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int cellPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            BombManager.Instance.CreateBomb(cellPos);
        }

        // Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(moveX, moveY).normalized;

        // Set Animator
        if (move == Vector2.zero) animator.speed = 0;
        else
        {
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
            animator.speed = 1;
        }

        rb.MovePosition((Vector2)transform.position + (move * (Speed * 10) * Time.deltaTime));
    }

    public void Die()
    {
        if (Dead) return;
        Dead = true;
        animator.speed = 1;
        animator.SetBool("Dead", Dead);
        SoundManager.Play("Death");

        StartCoroutine(ResetAfterTime());
    }

    private IEnumerator ResetAfterTime()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
