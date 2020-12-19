using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ZB_HealthDisplay : MonoBehaviour
{
    [SerializeField] private ZB_Health health;
    [SerializeField] private GameObject healthBarParent = null;
    [SerializeField] private Image healthBarImage;

    private void Awake()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated; 
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true); 
    }

    private void OnMouseExit()
    {
        healthBarParent.SetActive(false); 
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float) currentHealth / maxHealth; 
    }
}