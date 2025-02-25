using UnityEngine;

public class PlayerAttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")) collision.gameObject.GetComponent<SlimeAI>().ApplyDamage(1);
    }
}
