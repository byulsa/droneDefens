using UnityEngine;

public class TurretPlacement : MonoBehaviour
{
    public GhostTurret ghostTurret;
    bool isPlacing = false;
    public LayerMask placementLayer;
    [SerializeField]
    private LayerMask obstacleLayer;
    public Camera mainCamera;
    [SerializeField]
    [Header("Core Build Block Settings")]
    private Transform coreTransform;

    [SerializeField]
    private float coreBuildBlockRadius = 5f;
    [Header("Turret Spacing Settings")]
    [SerializeField]
    private float turretMinDistance = 2f;
    [SerializeField]
    private LayerMask turretLayer;

    void Start()
    {
        if (ghostTurret == null)
        {
            Debug.LogError("Ghost Turret reference is missing!");
            enabled = false;
            return;
        }

        ghostTurret.gameObject.SetActive(false);
    }
    void Update()
    {
        GameManager.Instance.isPlacingTurret = isPlacing;
        PlacementInput();

        if (ghostTurret != null)
        {
            Debug.LogWarning("Updating Ghost Turret Position");
            UpdateGhost();
        }
    }

    void PlacementInput()
    {
        // 설치 시작
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.F))
        {
            StartPlacement();
        }

        // 설치 완료
        if ((ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch) || Input.GetKeyUp(KeyCode.F)) && isPlacing)
        {
            PlaceTurret();
        }

        // 취소
        if ((ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.C)) && isPlacing)
        {
            CancelPlacement();
        }
    }

    void StartPlacement()
    {
        if (GameManager.Instance.selectedTurret == null)
            return;

        ghostTurret.gameObject.SetActive(true);

        ghostTurret.SetRange(GameManager.Instance.selectedTurret.attackRange);

        isPlacing = true;
    }

    void UpdateGhost()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //Ray ray = new Ray(ARAVRInput.RHandPosition,ARAVRInput.RHandDirection);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementLayer))
        {
            ghostTurret.transform.position = hit.point;

            bool canPlace = CheckCoreDistance() && CheckTurretDistance() && CheckObstacle();

            ghostTurret.SetValid(canPlace);
        }
    }

    void PlaceTurret()
    {
        if (!CheckCoreDistance() || !CheckTurretDistance() || !CheckObstacle())
            return;

        if (!GameManager.Instance.CanUseMemory(
                GameManager.Instance.selectedTurret.memoryCost))
            return;

        GameObject turretObj =
            Instantiate(
                GameManager.Instance.selectedTurret.turretPrefab,
                ghostTurret.transform.position,
                Quaternion.identity);

        Turret turret = turretObj.GetComponent<Turret>();

        if (turret != null)
        {
            GameManager.Instance.RegisterTurret(turret);
        }

        // 메모리 사용량 증가
        GameManager.Instance.AddMemoryUsage(GameManager.Instance.selectedTurret.memoryCost);

        // 인벤토리에서 하나 제거
        GameManager.Instance.ConsumeTurret(GameManager.Instance.selectedTurret);

        ghostTurret.gameObject.SetActive(false);
        isPlacing = false;
    }
    void CancelPlacement()
    {
        ghostTurret.gameObject.SetActive(false);

        isPlacing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(coreTransform.position, coreBuildBlockRadius);
    }
    bool CheckCoreDistance()
    {
        float distance = Vector3.Distance(ghostTurret.transform.position, coreTransform.position);

        return distance > coreBuildBlockRadius;
    }
    bool CheckTurretDistance()
    {
        Collider[] colliders =
            Physics.OverlapSphere(
                ghostTurret.transform.position,
                turretMinDistance,
                turretLayer);

        return colliders.Length == 0;
    }
    bool CheckObstacle()
    {
        Collider[] colliders =
            Physics.OverlapSphere(
                ghostTurret.transform.position,
                2f,
                obstacleLayer);

        return colliders.Length == 0;
    }
}