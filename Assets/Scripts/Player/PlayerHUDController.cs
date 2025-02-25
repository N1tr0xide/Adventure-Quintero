using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    private PlayerController _playerController;
    private readonly List<GameObject> _heartsUI = new(6);
    [SerializeField] private GameObject _healthPanel, _gameOverPanel;
    [SerializeField] private Text _gameOverText, _moneyText, _chestRemainingText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
        _gameOverPanel.SetActive(false);

        for (int i = 0; i < _healthPanel.transform.childCount; i++)
        {
            _heartsUI.Add(_healthPanel.transform.GetChild(i).gameObject);
        }
        
        PlayerOnDamaged();
        _playerController.OnDamaged += PlayerOnDamaged;
        _playerController.OnMoneyChanged += PlayerOnMoneyChanged;
        _playerController.OnChestOpened += PlayerOnChestOpened;
        _playerController.onGameOver += PlayerOnGameOver;
    }

    private void PlayerOnGameOver(bool hasWon)
    {
        _gameOverPanel.SetActive(true);
        _gameOverText.text = hasWon ? "YOU WIN!" : "YOU LOSE";
    }

    private void OnDisable()
    {
        _playerController.OnDamaged -= PlayerOnDamaged;
        _playerController.OnMoneyChanged -= PlayerOnMoneyChanged;
        _playerController.OnChestOpened -= PlayerOnChestOpened;
    }

    private void PlayerOnChestOpened(int obj)
    {
        _chestRemainingText.text = $"{obj}";
    }

    private void PlayerOnMoneyChanged(int obj)
    {
        _moneyText.text = $"x {obj}";
    }

    private void PlayerOnDamaged()
    {
        for (int i = 0; i < _heartsUI.Count; i++)
        {
            bool active = i < _playerController.Health && _playerController.Health != 0;
            _heartsUI[i].SetActive(active);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
