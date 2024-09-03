using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] fallenObjects;
    [SerializeField] private Progress progressBarManager;

    [SerializeField] private GameObject tapText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject finishPopUp;

    [SerializeField] private float spawnInterval = 2.0f;
    [SerializeField] private float fadeTime = .7f;

    private float _lastTimeSpawn;
    private bool _isSpawning;

    private GameObject _spawnParent;
    [SerializeField]    private Animator animator;
    private void OnEnable()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        progressBarManager.OnBarFull.AddListener(FinishLevel);
    }

    private void OnDisable()
    {
        progressBarManager.OnBarFull.RemoveListener(FinishLevel);
    }

    private void FinishLevel()
    {
        _isSpawning = false;
        finishPopUp.SetActive(true);
    }

    private void Awake()
    {
        finishPopUp.SetActive(false);
        fadeImage.gameObject.SetActive(true);
        _spawnParent = GameObject.Find("SpawnParent");
        FadeOut();
    }

    private void Update()
    {
        if (!_isSpawning && Input.GetMouseButtonDown(0))
        {
            tapText.gameObject.SetActive(false);
            BeginSpawning();
        }
        

        if (_isSpawning)
        {
            HandleSpawning();
        }
    }

    private void BeginSpawning()
    {
        _isSpawning = true;
        _lastTimeSpawn = 0f;
    }

    private void HandleSpawning()
    {
        _lastTimeSpawn += Time.deltaTime;

        if (_lastTimeSpawn >= spawnInterval)
        {
            SpawnObject();
            _lastTimeSpawn = 0f;
        }
    }
    private void SpawnObject()
    {
        int spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

        Instantiate(fallenObjects[UnityEngine.Random.Range(0, fallenObjects.Length)], spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, _spawnParent.transform);
    }

    public void ResetLevel()
    {
        FadeIn();
    }
    public void FadeIn()
    {
        StartCoroutine(FadeInCourutine(fadeTime));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCourutine(fadeTime));
    }

    private IEnumerator FadeInCourutine(float duration)
    {
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }
        fadeImage.color = endColor;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FadeOutCourutine(float duration)
    {
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }
        fadeImage.color = endColor;
    }

    public void OnPause()
    {
        Time.timeScale = 0f;
        animator.Play("PopUpOn");

    }
    public void OnGame()
    {
        Time.timeScale = 1f;
        animator.Play("PopUpOff");
    }

    
}
