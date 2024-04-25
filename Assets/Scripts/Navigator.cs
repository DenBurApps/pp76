using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _canvases;

    private void Awake()
    {
        ShowMenu();
    }

    public void HideAll()
    {
        foreach (var item in _canvases)
        {
            item.SetActive(false);
        }
    }

    public void ShowMenu()
    {
        HideAll();
        _canvases[0].SetActive(true);
    }

    public void ShowBlackjack()
    {
        HideAll();
        _canvases[1].SetActive(true);
    }

    public void ShowRoulette()
    {
        HideAll();
        _canvases[2].SetActive(true);
    }

    public void ShowSettings()
    {
        HideAll();
        _canvases[3].SetActive(true);
    }
}
