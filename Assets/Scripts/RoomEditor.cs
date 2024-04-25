using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEditor : MonoBehaviour
{
    [SerializeField] private List<GameObject> _steps;
    [SerializeField] private List<TMP_InputField> _firstInputs;
    [SerializeField] private List<Button> _completeButtons;
    [SerializeField] private List<TMP_InputField> _secondInputs;
    [SerializeField] private List<TMP_InputField> _thirdInputs;
    [SerializeField] private List<Toggle> _toggles;
    [SerializeField] private GameObject _photoParent;
    [SerializeField] private GameObject _addPhotoParent;
    [SerializeField] private Image _photo;
    public Action EditEnded;
    private List<string> _categories = new List<string>{ "Econom", "Standart", "Family room", "De luxe", "Premium" };
    private int _currentIndex = 0;
    private bool _hasData;
    private string _imagePath;

    private void Awake()
    {
        ResetInfo();
    }

    private void OnDisable()
    {
        ResetInfo();
    }

    public void CreateNewRoom()
    {
        _currentIndex = 0;
        _hasData = false;
        StartFirstStep();
    }

    public void EditRoom(int index)
    {
        _currentIndex = index;
        _hasData = true;
        var room = SaveSystem.LoadData<RoomSaveData>().Rooms[index];
        _firstInputs[0].text = room.Name;
        _firstInputs[1].text = room.Number.ToString();
        _firstInputs[2].text = room.Price.ToString();
        _secondInputs[0].text = room.Capacity.ToString();
        int categoryIndex = _categories.IndexOf(room.Category);
        _toggles[categoryIndex].isOn = true;
        _imagePath = room.ImagePath;
        if (room.ImagePath != null)
        {
            if (ImagePicker.SetImage(room.ImagePath, _photo))
            {
                _photoParent.SetActive(true);
                _addPhotoParent.SetActive(false);
            }
            else
            {
                _photoParent.SetActive(false);
                _addPhotoParent.SetActive(true);
            }
        }
        else
        {
            _photoParent.SetActive(false);
            _addPhotoParent.SetActive(true);

        }      
        _thirdInputs[0].text = room.Conveniences;
        _thirdInputs[1].text = room.Description;
        StartFirstStep();
    }

    public void OnSetImageButtonClick()
    {
        ImagePicker.ImagePicked += OnImageSelected;
        ImagePicker.PickImage(_photo);
    }

    private void OnImageSelected(string path)
    {
        ImagePicker.ImagePicked -= OnImageSelected;
        Debug.Log("Selected " + path);
        if (path == "")
        {
            _photoParent.SetActive(false);
            _addPhotoParent.SetActive(true);
        }
        else
        {
            _photoParent.SetActive(true);
            _addPhotoParent.SetActive(false);
        }
        _imagePath = path;
    }

    public void ResetInfo()
    {
        foreach (var item in _firstInputs)
        {
            item.text = "";
        }
        foreach (var item in _secondInputs)
        {
            item.text = "";
        }
        foreach (var item in _thirdInputs)
        {
            item.text = "";
        }
        foreach (var item in _toggles)
        {
            item.isOn = false;
        }
        _photoParent.SetActive(false);
        _photo.sprite = null;
        _addPhotoParent.SetActive(true);
        _imagePath = "";
        _hasData = false;
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
        foreach (var item in _toggles)
        {
            item.onValueChanged.AddListener(CheckSecondStep);
        }
        CheckSecondStep("");
        CheckSecondStep(true);
    }

    public void StartThirdStep()
    {
        foreach (var item in _steps)
        {
            item.SetActive(false);
        }
        _steps[2].SetActive(true);
        foreach (var item in _thirdInputs)
        {
            item.onEndEdit.AddListener(CheckThirdStep);
        }
        CheckThirdStep("");
    }

    public void CheckFirstStep(string text)
    {
        bool hasInfo = true;
        foreach (var item in _firstInputs)
        {
            if(item.text == "")
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
        foreach (var item in _toggles)
        {
            item.onValueChanged.RemoveListener(CheckSecondStep);
        }
        StartThirdStep();
    }

    public void OnEndThirdStep()
    {
        foreach (var item in _thirdInputs)
        {
            item.onEndEdit.RemoveListener(CheckThirdStep);
        }
        SaveRoomData();
    }

    public void SaveRoomData()
    {
        var rooms = SaveSystem.LoadData<RoomSaveData>();
        Room room = new Room();
        room.Name = _firstInputs[0].text;
        room.Number = int.Parse(_firstInputs[1].text);
        room.Price = int.Parse(_firstInputs[2].text);
        room.Capacity = int.Parse(_secondInputs[0].text);
        int currentToggle = 0;
        for (int i = 0; i < _toggles.Count; i++)
        {
            if (_toggles[i].isOn)
            {
                currentToggle = i;
                break;
            }
        }
        room.Category = _categories[currentToggle];
        room.ImagePath = _imagePath;
        room.Conveniences = _thirdInputs[0].text;
        room.Description = _thirdInputs[1].text;
        if (_hasData)
        {         
            room.Index = rooms.Rooms[_currentIndex].Index;
            rooms.Rooms[_currentIndex] = room;
        }
        else
        {
            room.Index = rooms.RoomIndex;
            rooms.RoomIndex += 1;
            rooms.Rooms.Add(room);
        }
        SaveSystem.SaveData(rooms);
        EditEnded?.Invoke();
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
        bool hasToggle = false;
        foreach (var item in _toggles)
        {
            if (item.isOn)
            {
                hasToggle = true;
            }
        }
        if (hasInfo && hasToggle)
        {
            _completeButtons[0].interactable = true;
        }
        else
        {
            _completeButtons[0].interactable = false;
        }
    }

    public void CheckSecondStep(bool isOn)
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
        bool hasToggle = false;
        foreach (var item in _toggles)
        {
            if (item.isOn)
            {
                hasToggle = true;
            }
        }
        if (hasInfo && hasToggle)
        {
            _completeButtons[1].interactable = true;
        }
        else
        {
            _completeButtons[1].interactable = false;
        }
    }

    public void CheckThirdStep(string text)
    {
        bool hasInfo = true;
        foreach (var item in _thirdInputs)
        {
            if (item.text == "")
            {
                hasInfo = false;
                break;
            }
        }
        if (hasInfo)
        {
            _completeButtons[2].interactable = true;
        }
        else
        {
            _completeButtons[2].interactable = false;
        }
    }
}
