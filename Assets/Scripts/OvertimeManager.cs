using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OvertimeManager : MonoBehaviour
{
    public static OvertimeManager Instance;

    [Header("Overtime")]
    public bool isOvertime = false;
    public float overtimeTime = 0f;

    [Header("Coin Multiplier")]
    public float coinMultiplier = 1f;
    [SerializeField] private float multiplierInterval = 30f;
    [SerializeField] private float multiplierIncrease = 0.25f;

    [Header("Difficulty")]
    public int overtimeDifficulty = 0;
    [SerializeField] private float difficultyInterval = 30f;

    [Header("UI")]
    public GameObject overtimeUI;
    public TextMeshProUGUI multiplierText;
    public Slider multiplierSlider;

    private float nextMultiplierTime;
    private float nextDifficultyTime;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        if (overtimeUI != null)
        {
            overtimeUI.SetActive(false);
        }

        if (multiplierSlider != null)
        {
            multiplierSlider.minValue = 0f;
            multiplierSlider.maxValue = 1f;
            multiplierSlider.value = 0f;
        }
    }


    private void Update()
    {
        if (!isOvertime)
            return;

        overtimeTime += Time.deltaTime;

        UpdateMultiplier();
        UpdateDifficulty();
        UpdateMultiplierSlider();
    }


    public void StartOvertime()
    {
        isOvertime = true;

        overtimeTime = 0f;

        coinMultiplier = 1f;
        overtimeDifficulty = 0;

        nextMultiplierTime = multiplierInterval;
        nextDifficultyTime = difficultyInterval;

        if (overtimeUI != null)
        {
            overtimeUI.SetActive(true);
        }

        UpdateMultiplierUI();

        if (multiplierSlider != null)
        {
            multiplierSlider.value = 0f;
        }

        Debug.Log("OVERTIME STARTED!");
    }


    private void UpdateMultiplier()
    {
        if (overtimeTime >= nextMultiplierTime)
        {
            coinMultiplier += multiplierIncrease;

            nextMultiplierTime += multiplierInterval;

            UpdateMultiplierUI();

            if (multiplierSlider != null)
            {
                multiplierSlider.value = 0f;
            }

            Debug.Log("Coin Multiplier: x" + coinMultiplier);
        }
    }


    private void UpdateMultiplierSlider()
    {
        if (multiplierSlider == null)
            return;

        float progress = overtimeTime - (nextMultiplierTime - multiplierInterval);

        multiplierSlider.value = Mathf.Clamp01(progress / multiplierInterval);
    }


    private void UpdateMultiplierUI()
    {
        if (multiplierText != null)
        {
            multiplierText.text = " : x" + coinMultiplier.ToString("0.00");
        }
    }


    private void UpdateDifficulty()
    {
        if (overtimeTime >= nextDifficultyTime)
        {
            overtimeDifficulty++;

            nextDifficultyTime += difficultyInterval;

            Debug.Log("Overtime Difficulty: " + overtimeDifficulty);
        }
    }
}