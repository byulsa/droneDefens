using System.Collections;
using UnityEngine;

public class DroneAttack : MonoBehaviour
{
    public Transform leftFirePoint;
    public Transform rightFirePoint;
    public Camera mainCamera;
    [Header("Particle Effects")]
    public ParticleSystem LeftFireEffect;
    public ParticleSystem RightFireEffect;
    [Header("Damage")]
    public int damage = 5;
    [Header("Attack")]
    public float attackCooldown = 0.5f;
    private float lastLeftAttackTime;
    private float lastRightAttackTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) || ARAVRInput.Get(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
        {
            if (Time.time >= lastLeftAttackTime + attackCooldown)
            {
                ShootLeft();
                lastLeftAttackTime = Time.time;
            }
        }

        if (Input.GetMouseButton(1) || ARAVRInput.Get(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            if (Time.time >= lastRightAttackTime + attackCooldown)
            {
                ShootRight();
                lastRightAttackTime = Time.time;
            }
        }
    }
    //ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger,ARAVRInput.Controller.LTouch)

    void ShootLeft()
    {
        LeftFireEffect.Play();
        Ray centerRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(centerRay, out RaycastHit centerHit, 100f))
        {
            targetPoint = centerHit.point;
        }
        else
        {
            targetPoint = centerRay.origin + centerRay.direction * 100f;
        }

        Vector3 direction =
            (targetPoint - leftFirePoint.position).normalized;

        if (Physics.Raycast(leftFirePoint.position, direction, out RaycastHit hit, 100f))
        {
            EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Hit Enemy: " + enemy.name);
            }

            Debug.DrawRay(
                leftFirePoint.position,
                direction * hit.distance,
                Color.red,
                1f);
        }
    }
    void ShootRight()
    {
        RightFireEffect.Play();
        Ray centerRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(centerRay, out RaycastHit centerHit, 100f))
        {
            targetPoint = centerHit.point;
        }
        else
        {
            targetPoint = centerRay.origin + centerRay.direction * 100f;
        }

        Vector3 direction = (targetPoint - rightFirePoint.position).normalized;

        if (Physics.Raycast(rightFirePoint.position, direction, out RaycastHit hit, 100f))
        {
            EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Hit Enemy: " + enemy.name);
            }

            Debug.DrawRay(rightFirePoint.position, direction * hit.distance, Color.red, 1f);
        }
    }

}
