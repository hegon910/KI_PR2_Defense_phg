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

    [Header("Camera & Animation")]
    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private CanvasGroup aimCanvas;
    [SerializeField] private RawImage aimView;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Rotation Clamp")]
    [SerializeField] private float clamAngleX = 45f;
    [SerializeField] private float clampAngleY = 70f;
    [SerializeField] private float sensitivity = 1.5f;


    private bool isAiming;
    private float rotationX;
    private float rotationY;
    private float targetAlpha = 0;


    private void Awake()
    {
        rotationX = cameraRoot.localEulerAngles.y;
        rotationY = cameraRoot.localEulerAngles.x;
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
        if (isAiming)
        {
            if (!isAiming) return;
            _animator.SetTrigger("Attack");                //Todo:DAMAGGGE

        }

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
