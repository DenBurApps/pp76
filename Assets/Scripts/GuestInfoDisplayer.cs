using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuestInfoDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private TMP_Text _contactsText;
    [SerializeField] private TMP_Text _checkText;
    [SerializeField] private TMP_Text _departText;
    [SerializeField] private GameObject _deleteAlert;
    [SerializeField] private Navigator _navigator;
    [SerializeField] private GuestsShower _guestsShower;
    private int _currentIndex;

    public void SetInfo(int index)
    {
        _currentIndex = index;
        Guest guest = SaveSystem.LoadData<GuestsSaveData>().Guests[_currentIndex];
        _nameText.text = guest.Name + " " + guest.Surname;
        _numberText.text = "¹" + guest.RoomNumber;
        _contactsText.text = guest.Contacts;
        _checkText.text = guest.CheckDate;
        _departText.text = guest.DepartDate;     
    }

    public void OnDeleteButtonClick()
    {
        _deleteAlert.SetActive(true);
    }

    public void OnEditButtonClick()
    {
        _guestsShower.EditInfo(_currentIndex);
    }

    public void HideDeleteAlert()
    {
        _deleteAlert.SetActive(false);
    }

    public void ConfirmDelete()
    {
        HideDeleteAlert();
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        guests.Guests.RemoveAt(_currentIndex);
        SaveSystem.SaveData(guests);
        _guestsShower.UpdatePreviewItems();
        _navigator.ShowMenu();
    }
}
