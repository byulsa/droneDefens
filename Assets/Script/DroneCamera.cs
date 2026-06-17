using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    public Rigidbody droneRigidbody;

    [Header("Dynamic FOV (Speed Effect)")]
    public Camera cam;
    public float minFOV = 60f;          // 기본 속도일 때 FOV (정지 시)
    public float maxFOV = 100f;         // 최고 속도일 때 FOV (극적인 광각 왜곡)
    public float maxSpeedForFOV = 18f;  // FOV가 최대가 되는 속도 기준 (약 65km/h 상당)
    public float fovChangeSpeed = 10f;  // FOV 변화 반응 속도 (통합 시점을 위해 높임)

    void Start()
    {
        if (cam == null) cam = GetComponent<Camera>();

        // 카메라가 드론의 자식으로 등록되어 있는지 확인하는 최소한의 코드
        if (transform.parent == null || transform.parent.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Error: 이 스크립트가 붙은 카메라는 반드시 드론 Rigidbody가 있는 오브젝트의 자식이어야 합니다.");
        }
    }

    void LateUpdate()
    {
        if (droneRigidbody == null) return;

        // 드론 속도 기반 동적 FOV 계산 및 부드러운 적용 (매 프레임 위치 계산 불필요)
        float currentSpeed = droneRigidbody.linearVelocity.magnitude;
        float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeedForFOV);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, speedRatio);

        // 이미지처럼 극적인 변화와 반응성을 위해 속도를 높였습니다.
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFOV, fovChangeSpeed * Time.deltaTime);
    }
}