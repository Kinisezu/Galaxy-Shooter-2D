using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _waveTextRequirement;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    [SerializeField]
    private Slider _thrusterSlider;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + " / 15";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _thrusterSlider.value = 50f;

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateAmmo (int currentAmmo)
    {
        _ammoText.text = "Ammo: " + currentAmmo + " / 15";
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateWave(int waveNumber)
    {
        _waveText.gameObject.SetActive(true);
        _waveText.text = "Wave: " + waveNumber;
        _waveTextRequirement.gameObject.SetActive(true);
        _waveTextRequirement.text = "Earn " + 50 * waveNumber + " points to reach next Wave";
        StartCoroutine(WaveTextInactive());
    }

    public void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);

        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }

    IEnumerator WaveTextInactive()
    {
        if (_waveText.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(3);
            _waveText.gameObject.SetActive(false);
            _waveTextRequirement.gameObject.SetActive(false);
        }
    }

    public void ThrusterGaugeAdjust(float _thrusterValue)
    {
        _thrusterSlider.value = _thrusterValue;
    }
}
