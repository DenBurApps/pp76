using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private TMP_Text _categoryText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private GameObject _imageParent;
    [SerializeField] private Image _roomImage;
    [SerializeField] private TMP_Text _capacityText;
    [SerializeField] private Button _showInfoButton;
    private int _roomIndex = 0;
    public Action<int> Show;

    public void SetInfo(int index)
    {
        _roomIndex = index;
        var hotel = SaveSystem.LoadData<RoomSaveData>();
        Room room = hotel.Rooms[index];
        _nameText.text = room.Name;
        _numberText.text = "¹" + room.Number;
        _categoryText.text = room.Category;
        _priceText.text = "$" + room.Price + "/night";
        _capacityText.text = room.Capacity + " guests";
        if(room.ImagePath != "")
        {
            if (ImagePicker.SetImage(room.ImagePath, _roomImage))
            {
                _imageParent.gameObject.SetActive(true);
            }
            else
            {
                _imageParent.gameObject.SetActive(false);
            }
        }
        else
        {
            _imageParent.gameObject.SetActive(false);
        }
    }

    public int GetIndex()
    {
        return _roomIndex;
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
        Show?.Invoke(_roomIndex);
    }
}
