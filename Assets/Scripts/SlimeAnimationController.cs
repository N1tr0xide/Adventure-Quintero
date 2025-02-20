using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SlimeAI))]
public class SlimeAnimationController : MonoBehaviour
{
    private SlimeAI _ai;
    private bool _isBeingDamaged;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ai = GetComponent<SlimeAI>();
        _ai.OnDamaged += AiOnDamaged;
    }

    private void OnDisable()
    {
        _ai.OnDamaged -= AiOnDamaged;
    }

    private void AiOnDamaged()
    {
        StartCoroutine(_ChangeSpriteColorTemp(.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if(_isBeingDamaged) return;
        
        _spriteRenderer.flipX = _ai.XSpeed switch
        {
            > 0 => false,
            < 0 => true,
            _ => _spriteRenderer.flipX
        };
    }
    
    /// Change the color of a sprite for a duration.
    private IEnumerator _ChangeSpriteColorTemp(float duration)
    {
        _isBeingDamaged = true;
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(duration);
        _spriteRenderer.color = Color.white;
        _isBeingDamaged = false;
    }
}
