using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{

    // :: 따라갈 카메라 혹은 오브젝트
    [SerializeField] private Transform iCamera = null;

    // :: 쳐다보기
    void Update()
    {
        if (this.iCamera == null) this.iCamera = Camera.main.transform;

        this.transform.LookAt(
            this.transform.position + this.iCamera.rotation * Vector3.forward,
            this.iCamera.rotation * Vector3.up);
    }
}
