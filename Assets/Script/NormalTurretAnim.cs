using UnityEngine;
using System.Collections;

public class NormalTurretAnim : MonoBehaviour, IRecoil
{
   public Transform turretHead;
    public float recoilSpeed = 20f;       
    public float returnSpeed = 5f;        
    public float recoilDistance = 0.5f;   

    private Vector3 originalLocalPos;
    private Coroutine recoilCoroutine;

    void Start()
    {
        // 기준점은 부모 대비 원래 위치(로컬)로 기억해둠
        originalLocalPos = turretHead.localPosition;
    }

    public void PlayRecoil()
    {
        if (recoilCoroutine != null)
        {
            StopCoroutine(recoilCoroutine);
        }
        recoilCoroutine = StartCoroutine(RecoilRoutine());
    }

    private IEnumerator RecoilRoutine()
    {
        Vector3 recoilDirection = -turretHead.forward;
        Vector3 targetWorldPos = turretHead.position + (recoilDirection * recoilDistance);
        while (Vector3.Distance(turretHead.position, targetWorldPos) > 0.01f)
        {
            turretHead.position = Vector3.MoveTowards(
                turretHead.position, 
                targetWorldPos, 
                recoilSpeed * Time.deltaTime
            );
            yield return null;
        }
        while (Vector3.Distance(turretHead.localPosition, originalLocalPos) > 0.01f)
        {
            turretHead.localPosition = Vector3.Lerp(
                turretHead.localPosition, 
                originalLocalPos, 
                returnSpeed * Time.deltaTime
            );
            yield return null;
        }

        turretHead.localPosition = originalLocalPos;
    }
}