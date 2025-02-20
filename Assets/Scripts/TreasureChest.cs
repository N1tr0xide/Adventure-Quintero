using System;
using System.Collections;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private SpriteRenderer _sr;
    private GameObject _treasure;
    private bool _isOpen;
    [SerializeField] private Sprite _closedChest, _openChest;
    [SerializeField] private int _value;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _treasure = transform.GetChild(0).gameObject;
        _sr.sprite = _closedChest;
        _treasure.SetActive(false);
    }

    public int OpenChest()
    {
        if (_isOpen) return 0;
        _sr.sprite = _openChest;
        _isOpen = true;
        StartCoroutine(_ShowLootForSec(1.5f));
        return _value;
    }

    private IEnumerator _ShowLootForSec(float delay)
    {
        _treasure.SetActive(true);
        yield return new WaitForSeconds(delay);
        _treasure.SetActive(false);
    }
}
