using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    private PlayerController _playerController;
    private readonly List<GameObject> _heartsUI = new(6);
    [SerializeField] private GameObject _healthPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();

        for (int i = 0; i < _healthPanel.transform.childCount; i++)
        {
            _heartsUI.Add(_healthPanel.transform.GetChild(i).gameObject);
        }
        
        PlayerOnDamaged();
        _playerController.OnDamaged += PlayerOnDamaged;
    }

    private void PlayerOnDamaged()
    {
        for (int i = 0; i < _heartsUI.Count; i++)
        {
            bool active = i < _playerController.Health && _playerController.Health != 0;
            _heartsUI[i].SetActive(active);
        }
    }
}
