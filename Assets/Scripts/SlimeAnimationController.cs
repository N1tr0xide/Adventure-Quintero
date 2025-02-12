using UnityEngine;

[RequireComponent(typeof(SlimeAI))]
public class SlimeAnimationController : MonoBehaviour
{
    private SlimeAI _ai;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ai = GetComponent<SlimeAI>();
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.flipX = _ai.XSpeed switch
        {
            > 0 => false,
            < 0 => true,
            _ => _spriteRenderer.flipX
        };
    }
}
