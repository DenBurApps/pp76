using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.iOS;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private RectTransform _settingsPanel;
    [SerializeField] private GameObject _locker;
    [SerializeField] private GameObject _privacy;
    [SerializeField] private GameObject _terms;
    [SerializeField] private string _email;

    public void ShowSettings()
    {
        _locker.SetActive(true);
        _settingsCanvas.SetActive(true);
        _settingsPanel.DOAnchorPosX(0, 0.3f).SetLink(gameObject).OnComplete(() =>
        {
            _locker.SetActive(false);
        });

    }
    public void HideSettings()
    {
        _locker.SetActive(true);
        _settingsPanel.DOAnchorPosX(-850f, 0.3f).SetLink(gameObject).OnComplete(() => 
        {
            _settingsCanvas.SetActive(false);
            _locker.SetActive(false);
        });
    }

    public void ShowPrivacy()
    {
        _privacy.SetActive(true);
    }

    public void HidePrivacy()
    {
        _privacy.SetActive(false);
    }

    public void ShowTerms()
    {
        _terms.SetActive(true);
    }

    public void HideTerms()
    {
        _terms.SetActive(false);
    }

    public void RateUs()
    {
        Device.RequestStoreReview();
    }

    public void ContactUs()
    {
        Application.OpenURL("mailto:" + _email + "?subject=Mail to developer");
    }
}
