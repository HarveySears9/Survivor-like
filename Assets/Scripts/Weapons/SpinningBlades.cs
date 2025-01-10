using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBlades : MonoBehaviour
{
    private GameObject[] blades; // Array of blades (if you want multiple blades)
    private GameObject currentLevel;
    public float spinSpeed;     // Speed of the spinning (in degrees per second)
    public int level;
    public GameObject[] level1Blades;
    public GameObject[] level2Blades;
    public GameObject[] level3Blades;
    public GameObject[] level4Blades;
    public GameObject[] level5Blades;

    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;
    public GameObject level5;


    void Start()
    {
        level = 1;
        blades = level1Blades; // Initialize blades with level1 at the start
        currentLevel = level1;
        currentLevel.SetActive(true);
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        // Rotate the main object (spinning around the Z-axis)
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime); // Spin around Z-axis

        // Loop through each blade and rotate it
        foreach (GameObject blade in blades)
        {
            blade.transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime); // Spin each blade around Z-axis
        }
    }

    // Function to increase the level and update the blades
    public void LevelUp()
    {
        level++;  // Increase level
        currentLevel.SetActive(false);

        switch (level)
        {
            case 2:
                blades = level2Blades;
                currentLevel = level2;
                break;
            case 3:
                blades = level3Blades;
                currentLevel = level3;
                break;
            case 4:
                blades = level4Blades;
                currentLevel = level4;
                break;
            case 5:
                blades = level5Blades;
                currentLevel = level5;
                break;
            default:
                // If level is 1 or an unexpected value, set to level1 blades
                blades = level1Blades;
                currentLevel = level1;
                break;
        }

        currentLevel.SetActive(true);
    }
}
