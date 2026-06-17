using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public float maxThrottleForce = 25.0f;
    public float maxPitchRollForce = 15.0f;
    public float maxYawTorque = 5.0f;

    Rigidbody rb;

    float throttleInput = 0.0f;
    float rudderInput = 0.0f;
    float elevatorInput = 0.0f;
    float aileronInput = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1.5f;
        rb.angularDamping = 2.0f;
    }

    void Update()
    {
    if (ARAVRInput.GetAxis("Vertical", ARAVRInput.Controller.LTouch) != 0)
        {
            throttleInput = ARAVRInput.GetAxis("Vertical", ARAVRInput.Controller.LTouch);
        }
        else
        {
            throttleInput = 0;
        }

        // 왼쪽 스틱 좌/우 (회전)
        if (ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.LTouch) != 0)
        {
            rudderInput = ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.LTouch);
        }
        else
        {
            rudderInput = 0;
        }

        // 오른쪽 스틱 위/아래 (전진/후진)
        if (ARAVRInput.GetAxis("Vertical", ARAVRInput.Controller.RTouch) != 0)
        {
            elevatorInput = ARAVRInput.GetAxis("Vertical", ARAVRInput.Controller.RTouch);
        }
        else
        {
            elevatorInput = 0;
        }

        // 오른쪽 스틱 좌/우 (좌우 이동)
        if (ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.RTouch) != 0)
        {
            aileronInput = ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.RTouch);
        }
        else
        {
            aileronInput = 0;
        }
    }

    void FixedUpdate()
    {
        // Throttle (왼쪽 스틱 위/아래) -> 수직 상승/하강
        if (throttleInput != 0)
        {
            rb.AddForce(transform.up * throttleInput * maxThrottleForce);
        }

        // Rudder (왼쪽 스틱 좌/우) -> 제자리 회전 (Y축 회전)
        if (rudderInput != 0)
        {
            rb.AddTorque(transform.up * rudderInput * maxYawTorque);
        }

        // Elevator (오른쪽 스틱 위/아래) & Aileron (오른쪽 스틱 좌/우) -> 평면 상의 앞/뒤/좌/우 이동
        Vector3 moveDirection = (transform.forward * elevatorInput) + (transform.right * aileronInput);
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            rb.AddForce(moveDirection.normalized * maxPitchRollForce);

            Quaternion targetRotation = Quaternion.Euler(elevatorInput * 15f, transform.eulerAngles.y, -aileronInput * 15f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }
}