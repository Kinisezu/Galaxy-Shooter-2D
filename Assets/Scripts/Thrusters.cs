using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thrusters : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private float _thrusterValue;
    private Slider _thrusterSlider;
    private Image _thrusterImage;

    private bool _isThrustActive = false;

    private UIManager _uIManager;


    // Start is called before the first frame update
    void Start()
    {
        _thrusterValue = 100f;

        _player = GetComponent<Player>();

        _thrusterSlider = GameObject.FindGameObjectWithTag("ThrusterSlider").GetComponent<Slider>();
        _thrusterImage = GameObject.Find("Fill").GetComponent<Image>();
        

        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        if (_thrusterSlider == null)
        {
            Debug.LogError("Thruster Slider is NULL");
        }

        if (_uIManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ThrusterBoost();

        if (_thrusterValue >= 100f)
        {
            _thrusterImage.color = Color.green;
            _thrusterSlider.image.color = Color.green;
        }
    }

    void ThrusterBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterValue > 0)
        {
            _isThrustActive = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || _thrusterValue <= 0)
        {
            _isThrustActive = false;
        }

        if (_isThrustActive)
        {
            _player.ThrusterSpeedActive();
            ThrusterGaugeDeplete();
        }
        else
        {
            _player.ThrusterSpeedInactive();
            ThrusterGaugeReplenish();
        }

        _uIManager.ThrusterGaugeAdjust(_thrusterValue);
    }

    public void ThrusterGaugeDeplete()
    {
        _thrusterValue -= 1f;
    }

    public void ThrusterGaugeReplenish()
    {
        if (_thrusterValue < 100 && _thrusterValue > 0)
        {
            _thrusterValue += 0.5f;
        }
        else if (_thrusterValue <= 0)
        {
            StartCoroutine(ThrusterFlash());
            StartCoroutine(ThrustersOverheat());
        }
    }

    IEnumerator ThrustersOverheat()
    {
        yield return new WaitForSeconds(2.0f);
        
        if (_thrusterValue < 100f)
        {
            StartCoroutine(ThrusterFlash());
            _thrusterValue += 0.25f;
            _isThrustActive = false;
        }
    }

    IEnumerator ThrusterFlash()
    {
        _thrusterImage.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 10, 1));
        _thrusterSlider.image.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 10, 1));
        yield return null;
    }
}
