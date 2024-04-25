using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestsShower : MonoBehaviour
{
    [SerializeField] private GuestsEditor _guestsEditor;
    [SerializeField] private Navigator _navigator;
    [SerializeField] private List<GuestItem> _previewItems;
    [SerializeField] private GameObject _previewElements;
    [SerializeField] private GameObject _noElements;
    [SerializeField] private GameObject _createGuestCanvas;
    [SerializeField] private GameObject _guestInfoCanvas;
    [SerializeField] private GameObject _allGuestsCanvas;
    [SerializeField] private GuestInfoDisplayer _guestInfoDisplayer;
    [SerializeField] private GameObject _addButton;
    [SerializeField] private GameObject _seeAllButton;
    [SerializeField] private GameObject _emptyGuest;

    private void Awake()
    {
        foreach (var item in _previewItems)
        {
            item.Show += ShowInfo;
        }
    }

    private void Start()
    {
        UpdatePreviewItems();
    }

    public void ShowInfo(int index)
    {
        _navigator.HideAll();
        _guestInfoCanvas.SetActive(true);
        _guestInfoDisplayer.SetInfo(index);
    }

    public void EditInfo(int index)
    {
        _navigator.HideAll();
        _createGuestCanvas.SetActive(true);
        _guestsEditor.EditGuest(index);
        _guestsEditor.EditEnded += OnGuestEditEnded;
    }

    public void UpdatePreviewItems()
    {
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        foreach (var item in _previewItems)
        {
            item.gameObject.SetActive(false);
        }
        if (guests.Guests.Count > 0)
        {
            _previewElements.SetActive(true);
            _noElements.SetActive(false);
            if (guests.Guests.Count < 2)
            {
                _previewItems[0].SetInfo(0);
                _previewItems[0].gameObject.SetActive(true);
                _emptyGuest.SetActive(true);
                _addButton.SetActive(false);
                _seeAllButton.SetActive(false);
            }
            if (guests.Guests.Count >= 2)
            {
                _previewItems[0].SetInfo(0);
                _previewItems[0].gameObject.SetActive(true);
                _previewItems[1].SetInfo(1);
                _previewItems[1].gameObject.SetActive(true);
                _emptyGuest.SetActive(false);
                _addButton.SetActive(false);
                _seeAllButton.SetActive(true);
            }
        }
        else
        {
            _previewElements.SetActive(false);
            _noElements.SetActive(true);
            _addButton.SetActive(true);
            _seeAllButton.SetActive(false);
        }
    }

    public void OpenAllHotels()
    {
        _navigator.HideAll();
        _allGuestsCanvas.SetActive(true);
    }

    public void OnCreateGuestButtonClick()
    {
        _navigator.HideAll();
        _createGuestCanvas.SetActive(true);
        _guestsEditor.CreateNewGuest();
        _guestsEditor.EditEnded += OnGuestEditEnded;
    }

    private void OnGuestEditEnded()
    {
        _guestsEditor.EditEnded -= OnGuestEditEnded;
        _navigator.ShowMenu();
        UpdatePreviewItems();
    }

}
