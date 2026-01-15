using UnityEngine;

public class DragonAlterMenu : MonoBehaviour
{
    private DragonAlter currentAlter;

    public void SetAlter(DragonAlter alter) { currentAlter = alter; }

    public void Open()
    {
        Time.timeScale = 0f;
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        Time.timeScale = 1f;
        currentAlter = null;
        this.gameObject.SetActive(false);
    }

    public void AcceptDeal()
    {
        if (currentAlter == null) return;

        currentAlter.TakeDeal();
        Close();
    }
}
