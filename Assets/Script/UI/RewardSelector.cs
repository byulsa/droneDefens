using UnityEngine;

public class RewardSelector : MonoBehaviour
{
    public Camera mainCamera;

    private TurretCard currentCard;
    public Turretinformation turretinformation;
    [SerializeField]
    private TurretRewardManager rewardManager;

    void Update()
    {
        Hovering();
    }

    public void Hovering()
    {
        // Ray ray = new Ray(
        //     ARAVRInput.RHandPosition,
        //     ARAVRInput.RHandDirection);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TurretCard card = hit.collider.GetComponent<TurretCard>();

            if (card != null)
            {
                // 카드가 바뀌었다면
                if (currentCard != card)
                {
                    if (currentCard != null)
                    {
                        currentCard.parentBundle.ExitHover();
                    }

                    currentCard = card;

                    currentCard.parentBundle.Hovering();

                    // 정보창 갱신
                    turretinformation.PanelUpdate(card.turretData);
                }

                if (ARAVRInput.GetDown(ARAVRInput.Button.One))
                {
                    Debug.Log("선택됨");
                    rewardManager.SelectBundle(currentCard.parentBundle);
                }

                return;
            }
        }

        // 아무것도 안 바라보면 Hover 해제
        if (currentCard != null)
        {
            currentCard.parentBundle.ExitHover();
            currentCard = null;
        }
    }
}