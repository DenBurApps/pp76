using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuestItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _surnameText;
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private TMP_Text _checkText;
    [SerializeField] private TMP_Text _departText;
    [SerializeField] private Button _showInfoButton;
    private int _guestIndex = 0;
    public Action<int> Show;

    public void SetInfo(int index)
    {
        _guestIndex = index;
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        Guest guest = guests.Guests[index];
        _nameText.text = guest.Name;
        _surnameText.text = guest.Surname;
        _numberText.text = "¹" + guest.RoomNumber;
        _checkText.text = guest.CheckDate;
        _departText.text = guest.DepartDate;
    }

    public int GetIndex()
    {
        return _guestIndex;
    }

    private void OnEnable()
    {
        _showInfoButton.onClick.AddListener(OnShowButtonClick);
    }

    private void OnDisable()
    {
        _showInfoButton.onClick.RemoveListener(OnShowButtonClick);
    }

    private void OnShowButtonClick()
    {
        Show?.Invoke(_guestIndex);
    }
}
