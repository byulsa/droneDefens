using UnityEngine;

public class SelectTurret : MonoBehaviour
{
    private int currentIndex = 0;

    private void Update()
    {
        if (GameManager.Instance.isPlacingTurret)
            return;

        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch)
            || Input.GetKeyDown(KeyCode.Q))
        {
            NextTurret();
        }

        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch)
            || Input.GetKeyDown(KeyCode.E))
        {
            PreviousTurret();
        }
    }

    void NextTurret()
    {
        if (GameManager.Instance.ownedTurrets.Count == 0)
            return;

        currentIndex++;

        if (currentIndex >= GameManager.Instance.ownedTurrets.Count)
        {
            currentIndex = 0;
        }

        GameManager.Instance.selectedTurret =GameManager.Instance.ownedTurrets[currentIndex].turretData;

        Debug.Log("Selected : "+ GameManager.Instance.selectedTurret.turretName);
    }

    void PreviousTurret()
    {
        if (GameManager.Instance.ownedTurrets.Count == 0)
            return;

        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = GameManager.Instance.ownedTurrets.Count - 1;
        }

        GameManager.Instance.selectedTurret =GameManager.Instance.ownedTurrets[currentIndex].turretData;

        Debug.Log("Selected : " + GameManager.Instance.selectedTurret.turretName);
    }
}