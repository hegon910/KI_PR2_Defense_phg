using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerContorl : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private InputActionReference _aimAction;
    [SerializeField] private InputActionReference _lookAction;
    [SerializeField] private InputActionReference _reloadAction;

    [Header("Shoot Options")]
    [SerializeField] private float shootDistance = 100f;
    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private GameObject sparkVFX;
    [SerializeField] private int maxAmmo = 5;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float shootCooldown = 1f;

    [Header("Camera & Animation")]
    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private CanvasGroup aimCanvas;
    [SerializeField] private RawImage aimView;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Camera scopedCamera;

    [Header("Rotation Clamp")]
    [SerializeField] private float clamAngleX = 45f;
    [SerializeField] private float clampAngleY = 70f;
    [SerializeField] public float sensitivity = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource gunShotAudio;
    [SerializeField] private AudioClip gunShotClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioSource reloadAudio;

    [SerializeField] private PlayerUIManager uiManager;


    private bool isAiming;
    private float rotationX;
    private float rotationY;
    private float targetAlpha = 0;
    public int currentAmmo;
    private bool canShoot = true;
    private bool isReloading = false;
    public bool controlsEnabled = false;


    private void Awake()
    {
        rotationX = cameraRoot.localEulerAngles.y;
        rotationY = cameraRoot.localEulerAngles.x;

    }

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        _shootAction.action.performed += OnShoot;
        _aimAction.action.started += OnAimStarted;
        _aimAction.action.canceled += OnAimCancelded;
        _reloadAction.action.performed += OnReload;

        _shootAction.action.Enable();
        _aimAction.action.Enable();
        _lookAction.action.Enable();
        _reloadAction.action.Enable();
    }

    private void OnDisable()
    {
        _shootAction.action.performed -= OnShoot;
        _aimAction.action.started -= OnAimStarted;
        _aimAction.action.canceled -= OnAimCancelded;
        _reloadAction.action.performed -= OnReload;

        _shootAction.action.Disable();
        _aimAction.action.Disable();
        _lookAction.action.Disable();
        _reloadAction.action.Disable();
    }

    private void Update()
    {
        if (!controlsEnabled) return;

        Vector2 lookDelta = _lookAction.action.ReadValue<Vector2>() * sensitivity;
        rotationX += lookDelta.x;
        rotationY -= lookDelta.y;

        rotationX = Mathf.Clamp(rotationX, -clamAngleX, clamAngleX);
        rotationY = Mathf.Clamp(rotationY, -clampAngleY, clampAngleY);

        cameraRoot.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);

        if (Mathf.Abs(aimCanvas.alpha - targetAlpha) > 0.01f)
        {
            aimCanvas.alpha = Mathf.MoveTowards(
                aimCanvas.alpha, targetAlpha, Time.deltaTime * fadeDuration
            );
        }

        if (uiManager != null && uiManager.gameObject.activeInHierarchy)
        {
            uiManager.UpdateAmmo(currentAmmo, maxAmmo);
            uiManager.UpdateHealth(GlobalHealthManager.Instance.GetHealthRatio());
            uiManager.SetReloading(isReloading);
            uiManager.UpdateScore(GlobalHealthManager.Instance.Score);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!controlsEnabled) return;
        Debug.Log("OnShoot 호출됨");
        if (!isAiming || currentAmmo <= 0||!canShoot||isReloading) return;

        StartCoroutine(ShootCooldown());
        currentAmmo--;
        _animator.SetTrigger("Attack");
        gunShotAudio?.PlayOneShot(gunShotClip);
        StartCoroutine(ShakeRawImage());

        Vector3 shootDir = rayOrigin.forward;
        Ray ray = new Ray(rayOrigin.position, shootDir);
        Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, 5f);

        RaycastHit hit;
        Vector3 spawnPos;
        Quaternion spawnRot;

        if (Physics.Raycast(ray, out hit, shootDistance, hitLayerMask))
        {
            spawnPos = hit.point;
            spawnRot = Quaternion.LookRotation(hit.normal);

            var identity = hit.collider.GetComponentInParent<EnemyIdentity>();
            if (identity != null)
            {
                identity.HandleHit();
            }
        }
        else
        {
            spawnPos = ray.origin + shootDir * shootDistance;
            spawnRot = Quaternion.LookRotation(-shootDir);
        }

        // 이 위치에서 무조건 VFX 생성
        GameObject spark = Instantiate(sparkVFX, spawnPos, spawnRot);
        var ps = spark.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
        Destroy(spark, 2f);
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private IEnumerator ShakeRawImage()
    {
        Vector3 originalPos = aimView.rectTransform.localPosition;
        float duration = 0.15f;
        float strength = 10f; // UI 단위니까 충분히 큰 값 필요
        float elapsed = 0f;

        while (elapsed < duration)
        {
            aimView.rectTransform.localPosition = originalPos + (Vector3)UnityEngine.Random.insideUnitCircle * strength;
            elapsed += Time.deltaTime;
            yield return null;
        }

        aimView.rectTransform.localPosition = originalPos;
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if (!controlsEnabled) return;
        if (isReloading || currentAmmo == maxAmmo) return;
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (reloadAudio != null && reloadClip != null)
        {
            
            reloadAudio.PlayOneShot(reloadClip);
            yield return new WaitForSeconds(reloadClip.length + 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(3f); // 기본 재장전 시간
        }

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("재장전 완료");
    }

    private void OnAimStarted(InputAction.CallbackContext context)
    {
        if (!controlsEnabled) return;
        isAiming = true;
        _animator.SetBool("Aiming", true);

        _aimCamera.gameObject.SetActive(true);
        aimCanvas.alpha = 1;
        targetAlpha = 1f;
    }

    private void OnAimCancelded(InputAction.CallbackContext context)
    {
        if (!controlsEnabled) return;
        isAiming = false;

        _aimCamera.gameObject.SetActive(false);

        _animator.SetBool("Aiming", false);
        targetAlpha = 0f;

    }

    public bool IsAiming()
    {
        return isAiming;
    }

    public void EnableControls()
    {
        controlsEnabled = true;
    }

    public void DisableControls()
    {
        controlsEnabled = false;
    }

    public void SetSenseitivity(float value)
    {
        sensitivity = value;
    }
}
