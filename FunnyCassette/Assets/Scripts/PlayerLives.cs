using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private GameObject[] _lives;
    
    private void Awake()
    {
        GameManager.OnGameLivesChanged += OnGameLivesChanged;
    }

    private void OnGameLivesChanged(int lives)
    {
        for (int i = 0; i < _lives.Length; i++)
        {
            _lives[i].SetActive(i < lives);
        }
    }
}