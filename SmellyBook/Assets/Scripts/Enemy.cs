using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;
    public float leftX;
    public float rightX;
    public float upY;
    public float downY;
    public float speed = 1.0f;

    public int maxHealth = 5;
    private int currentHealth;

    private Vector3 pos1, pos2;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        pos1 = new Vector3(transform.position.x - leftX, transform.position.y - downY, transform.position.z);
        pos2 = new Vector3(transform.position.x + rightX, transform.position.y + upY, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed, 1.0f));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        StartCoroutine(DamageAnimation());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageAnimation() {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(this.gameObject);
    }
}
