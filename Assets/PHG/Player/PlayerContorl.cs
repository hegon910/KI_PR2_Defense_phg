using Cinemachine;
using System;
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

    [Header("Shoot Options")]
    [SerializeField] private float shootDistance = 100f;
    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private GameObject sparkVFX;
    [SerializeField] private int maxAmmo = 5;
    [SerializeField] private Transform rayOrigin;

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
    [SerializeField] private float sensitivity = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource gunShotAudio;
    [SerializeField] private AudioClip gunShotClip;


    private bool isAiming;
    private float rotationX;
    private float rotationY;
    private float targetAlpha = 0;
    public int currentAmmo;


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

        _shootAction.action.Enable();
        _aimAction.action.Enable();
        _lookAction.action.Enable();
    }

    private void OnDisable()
    {
        _shootAction.action.performed -= OnShoot;
        _aimAction.action.started -= OnAimStarted;
        _aimAction.action.canceled -= OnAimCancelded;

        _shootAction.action.Disable();
        _aimAction.action.Disable();
        _lookAction.action.Disable();
    }

    private void Update()
    {
        if(isAiming)
        {
            Vector2 lookDelta = _lookAction.action.ReadValue<Vector2>() * sensitivity;
            rotationX += lookDelta.x;
            rotationY -= lookDelta.y;

            rotationX = Mathf.Clamp(rotationX, -clamAngleX, clamAngleX);
            rotationY = Mathf.Clamp(rotationY, -clampAngleY, clampAngleY);

            cameraRoot.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }
        if (Mathf.Abs(aimCanvas.alpha - targetAlpha) > 0.01f)
        {
            aimCanvas.alpha = Mathf.MoveTowards(
                aimCanvas.alpha, targetAlpha, Time.deltaTime * fadeDuration
            );
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Debug.Log("OnShoot 호출됨");
        if (!isAiming || currentAmmo <= 0) return;

        currentAmmo--;
        //SetHealthUIGuage; //Todo: 탄약UI로 교체 필요

        _animator.SetTrigger("Attack");

        if (isAiming)
        {
            _animator.SetTrigger("Attack");
            gunShotAudio?.PlayOneShot(gunShotClip);
            StartCoroutine(ShakeRawImage()); // 또는 DOTween 방식
        }

        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, 5f);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, shootDistance, hitLayerMask))
        {
            if(sparkVFX != null)
            {
                Instantiate(sparkVFX, hit.point, Quaternion.LookRotation(hit.normal));
            }

            var identity = hit.collider.GetComponentInParent<EnemyIdentity>();
            if(identity != null)
            {
                identity.HandleHit();
            }
        }

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


    private void OnAimStarted(InputAction.CallbackContext context)
    {
        isAiming = true;
        _animator.SetBool("Aiming", true);

        _aimCamera.gameObject.SetActive(true);
        aimCanvas.alpha = 0f;
        targetAlpha = 1f;


   
          
    }

    private void OnAimCancelded(InputAction.CallbackContext context)
    {
        isAiming = false;
       
        _aimCamera.gameObject.SetActive(false);
  
        _animator.SetBool("Aiming", false);
        targetAlpha = 0f;

    }




    public void TakeDamage()
    {
        //TODO
    }

    public void GameOver()
    { }

    public void SubscribeEvent()
    { }

    public void UnsubscribeEvent()
    {

    }

    private void SetHealthUIGuage()
    {

    }



}
