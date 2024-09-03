using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    [SerializeField] private Image progressBar; // The UI Image representing the progress bar
    [SerializeField] private float totalCapacity; // Total capacity of the cup in cubic units
    [SerializeField] private float fillSpeed = 1f; // Speed of the progress bar filling animation

    private float targetFillAmount;
    private float currentFillAmount;
    private float occupiedVolume; // Track the total volume of objects in the cup

    public UnityEvent OnBarFull;

    private void CheckBarFull()
    {
        if (progressBar.fillAmount >= 0.95f)
        {
            progressBar.fillAmount = Mathf.Lerp(currentFillAmount, 1, Time.deltaTime * fillSpeed);
            currentFillAmount = 1f;
            OnBarFull?.Invoke();
        }
    }

    private void Start()
    {
        // Initialize fill amounts
        currentFillAmount = progressBar.fillAmount;
        targetFillAmount = currentFillAmount;
    }

    private void Update()
    {
        SmoothFillUpdate();
        CheckBarFull();
    }

    private void OnTriggerEnter(Collider other)
    {
        UpdateTargetFillAmount(CalculateVolume(other));
    }

    private void OnTriggerExit(Collider other)
    {
        UpdateTargetFillAmount(-CalculateVolume(other));
    }

    private void SmoothFillUpdate()
    {
        if (Mathf.Approximately(progressBar.fillAmount, targetFillAmount)) return;

        currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * fillSpeed);
        progressBar.fillAmount = currentFillAmount;
    }

    private void UpdateTargetFillAmount(float volumeChange)
    {
        occupiedVolume = Mathf.Clamp(occupiedVolume + volumeChange, 0, totalCapacity);
        targetFillAmount = occupiedVolume / totalCapacity;
    }

    private float CalculateVolume(Collider collider)
    {
        // Calculate the volume based on object's bounds
        var renderer = collider.GetComponent<Renderer>();
        return renderer != null ? renderer.bounds.size.x * renderer.bounds.size.y * renderer.bounds.size.z : 0f;
    }
}
