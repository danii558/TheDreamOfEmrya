using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private PlayerInputButtons inputSystem;

    [Header("Setting Attacks Controller")]
    [SerializeField] private Transform attacksCenter;

    public void Controller()
    {
        //if (inputSystem.CheckButton(inputSystem.buttonEntries[0].name)) { Attacks(); }
    }

    private void Attacks()
    { CheakAttacksObject(Physics.BoxCastAll(attacksCenter.position, transform.localScale, transform.up, transform.rotation, 0)); }

    private void CheakAttacksObject(RaycastHit[] raycastHits)
    {
        foreach (var _hitRay in raycastHits)
        {
            Debug.Log(_hitRay.collider.name);
        }
    }
}
