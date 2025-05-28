using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;
public class PlayerContorl : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private InputActionReference _aimAction;

    [Header("Camera & Animation")]
    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Animator _animator;


    private bool isAiming;




    private void OnEnable()
    {
        _shootAction.action.performed += OnShoot;
        _aimAction.action.started += OnAimStarted;
        _aimAction.action.canceled += OnAimCancelded;
        _shootAction.action.Enable();
        _aimAction.action.Enable();
    }

    private void OnDisable()
    {
        _shootAction.action.performed -= OnShoot;
        _aimAction.action.started -= OnAimStarted;
        _aimAction.action.canceled -= OnAimCancelded;

        _shootAction.action.Disable();
        _aimAction.action.Disable();
    }



    public void OnShoot(InputAction.CallbackContext context)
    {
        if(isAiming)
        {
            if (!isAiming) return;

            _animator.SetTrigger("Attack");                //Todo:DAMAGGGE
        }

    }

    private void OnAimStarted(InputAction.CallbackContext context)
    {
        isAiming = true;
        _aimCamera.gameObject.SetActive(true);
        _animator.SetBool("Aiming", true);
    }

    private void OnAimCancelded(InputAction.CallbackContext context)
    {
        isAiming = false;
        _aimCamera.gameObject.SetActive(false);
        _animator.SetBool("Aiming", false);

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
