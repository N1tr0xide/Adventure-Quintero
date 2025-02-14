using UnityEngine;

public class PlayerAttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            print("Enemy Hit!! " + gameObject.name);
        }
    }
}
