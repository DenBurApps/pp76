using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoDisplayer : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private TMP_Text _capacityText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private TMP_Text _guestsText;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _convText;
    [SerializeField] private GameObject _deleteAlert;
    [SerializeField] private Navigator _navigator;
    [SerializeField] private RoomsShower _roomsShower;
    private int _currentIndex;

    public void SetInfo(int index)
    {
        _currentIndex = index;
        Room room = SaveSystem.LoadData<RoomSaveData>().Rooms[_currentIndex];
        if (room.ImagePath != "")
        {
            if (!ImagePicker.SetImage(room.ImagePath, _image))
            {
                _image.sprite = _defaultSprite;
            }
        }
        else
        {
            _image.sprite = _defaultSprite;
        }
        _capacityText.text = room.Capacity + " guests";
        _costText.text = room.Price + " $";
        _numberText.text = room.Number.ToString();
        _typeText.text = room.Category;
        int assignedGuests = 0;
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        foreach (var item in guests.Guests)
        {
            if (item.RoomNumber == room.Number)
            {
                assignedGuests++;
            }
        }
        _guestsText.text = assignedGuests.ToString();
        _description.text = room.Description;
        _convText.text = room.Conveniences;
    }

    public void OnDeleteButtonClick()
    {
        _deleteAlert.SetActive(true);
    }

    public void OnEditButtonClick()
    {
        _roomsShower.EditInfo(_currentIndex);
    }

    public void HideDeleteAlert()
    {
        _deleteAlert.SetActive(false);
    }

    public void ConfirmDelete()
    {
        HideDeleteAlert();
        var rooms = SaveSystem.LoadData<RoomSaveData>();
        rooms.Rooms.RemoveAt(_currentIndex);
        SaveSystem.SaveData(rooms);
        _roomsShower.UpdatePreviewItems();
        _navigator.ShowMenu();
    }

}
