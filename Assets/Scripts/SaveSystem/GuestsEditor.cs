using Bitsplash.DatePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuestsEditor : MonoBehaviour
{
    [SerializeField] private List<GameObject> _steps;
    [SerializeField] private List<TMP_InputField> _firstInputs;
    [SerializeField] private List<DatePickerDropDown> _firstDatas;
    [SerializeField] private List<Button> _completeButtons;
    [SerializeField] private List<TMP_InputField> _secondInputs;
    public Action EditEnded;
    private int _currentIndex = 0;
    private bool _hasData;

    private void Awake()
    {
        ResetInfo();
    }

    private void OnDisable()
    {
        ResetInfo();
    }

    public void ResetInfo()
    {
        foreach (var item in _firstInputs)
        {
            item.text = "";
        }
        foreach (var item in _firstDatas)
        {
            item.Label.text = "-";
        }
        foreach (var item in _secondInputs)
        {
            item.text = "";
        }
        _hasData = false;
    }

    public void CreateNewGuest()
    {
        _currentIndex = 0;
        _hasData = false;
        StartFirstStep();
    }

    public void EditGuest(int index)
    {
        _currentIndex = index;
        _hasData = true;
        var guest = SaveSystem.LoadData<GuestsSaveData>().Guests[index];
        _firstInputs[0].text = guest.Name;
        _firstInputs[1].text = guest.Surname;
        _firstDatas[0].Label.text = guest.CheckDate;
        _firstDatas[1].Label.text = guest.DepartDate;
        _secondInputs[0].text = guest.Contacts;
        _secondInputs[1].text = guest.RoomNumber.ToString();
        StartFirstStep();
    }

    public void StartFirstStep()
    {
        foreach (var item in _steps)
        {
            item.SetActive(false);
        }
        _steps[0].SetActive(true);
        foreach (var item in _firstInputs)
        {
            item.onEndEdit.AddListener(CheckFirstStep);
        }
        CheckFirstStep("");
    }

    public void StartSecondStep()
    {
        foreach (var item in _steps)
        {
            item.SetActive(false);
        }
        _steps[1].SetActive(true);
        foreach (var item in _secondInputs)
        {
            item.onEndEdit.AddListener(CheckSecondStep);
        }
        CheckSecondStep("");
    }

    public void CheckFirstStep(string text)
    {
        bool hasInfo = true;
        foreach (var item in _firstInputs)
        {
            if (item.text == "")
            {
                hasInfo = false;
                break;
            }
        }
        if (hasInfo)
        {
            _completeButtons[0].interactable = true;
        }
        else
        {
            _completeButtons[0].interactable = false;
        }
    }

    public void CheckSecondStep(string text)
    {
        bool hasInfo = true;
        foreach (var item in _secondInputs)
        {
            if (item.text == "")
            {
                hasInfo = false;
                break;
            }
        }
        if (hasInfo)
        {
            _completeButtons[1].interactable = true;
        }
        else
        {
            _completeButtons[1].interactable = false;
        }
    }

    public void OnEndFirstStep()
    {
        foreach (var item in _firstInputs)
        {
            item.onEndEdit.RemoveListener(CheckFirstStep);
        }
        StartSecondStep();
    }

    public void OnEndSecondStep()
    {
        foreach (var item in _secondInputs)
        {
            item.onEndEdit.RemoveListener(CheckSecondStep);
        }
        SaveGuestData();
    }

    public void SaveGuestData()
    {
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        Guest guest = new Guest();
        guest.Name = _firstInputs[0].text;
        guest.Surname = _firstInputs[1].text;
        guest.CheckDate = _firstDatas[0].Label.text;
        guest.DepartDate = _firstDatas[1].Label.text;
        guest.Contacts = _secondInputs[0].text;
        guest.RoomNumber = int.Parse(_secondInputs[1].text);
        if (_hasData)
        {
            guest.Index = guests.Guests[_currentIndex].Index;
            guests.Guests[_currentIndex] = guest;
        }
        else
        {
            guest.Index = guests.GuestIndex;
            guests.GuestIndex += 1;
            guests.Guests.Add(guest);
        }
        SaveSystem.SaveData(guests);
        EditEnded?.Invoke();
    }
}
