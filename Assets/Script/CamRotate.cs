using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // ���� ����
    Vector3 angle;
    // ���콺 ����
    public float sensitivity = 200;

    void Start()
    {
        angle = Camera.main.transform.eulerAngles;
        angle.x *= -1;
    }

    void Update()
    {
        float x = Input.GetAxis("Mouse Y");
        float y = Input.GetAxis("Mouse X");
        angle.x += x * sensitivity * Time.deltaTime;
        angle.y += y * sensitivity * Time.deltaTime;

        angle.x = Mathf.Clamp(angle.x, -90, 90);
        transform.eulerAngles = new Vector3(-angle.x, angle.y, transform.eulerAngles.z);
    }
}
