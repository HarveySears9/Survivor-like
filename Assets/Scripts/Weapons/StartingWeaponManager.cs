using UnityEngine;
using System.Collections;

public class StartingWeaponManager : MonoBehaviour
{
    public FireBreath fireBreath;
    public SpinningBlades spinningBlades;
    public SwordSlash swordSlash;
    public ArcBolt arcBolt;
    public MeteorDrop meteorDrop;
    public Hammer hammer;
    public PoisonStaff poisonStaff;

    private IEnumerator Start()
    {
        yield return null;

        EquipStartingWeapon();
    }
    private void EquipStartingWeapon()
    {
        SaveFile.Data data = PlayerDataManager.Instance.data;

        switch (data.startingWeapon)
        {
            case 0:
                Debug.Log("Equipping Fire Breath");

                if (fireBreath == null)
                {
                    Debug.LogError("FireBreath reference is NULL!");
                    return;
                }

                fireBreath.LevelUp();
                break;

            case 1:
                spinningBlades.LevelUp();
                break;

            case 2:
                swordSlash.LevelUp();
                break;

            case 3:
                arcBolt.LevelUp();
                break;

            case 4:
                meteorDrop.LevelUp();
                break;

            case 5:
                hammer.LevelUp();
                break;

            case 6:
                poisonStaff.LevelUp();
                break;

            default:
                Debug.LogWarning("Invalid starting weapon. Defaulting to Fire Breath.");

                fireBreath.LevelUp();
                break;
        }
    }
}