using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsShower : MonoBehaviour
{
    [SerializeField] private RoomEditor _roomEditor;
    [SerializeField] private RoomInfoDisplayer _roomInfoDisplayer;
    [SerializeField] private Navigator _navigator;
    [SerializeField] private List<RoomItem> _previewItems;
    [SerializeField] private GameObject _previewElements;
    [SerializeField] private GameObject _noElements;
    [SerializeField] private GameObject _createRoomCanvas;
    [SerializeField] private GameObject _allRoomsCanvas;
    [SerializeField] private GameObject _roomInfoCanvas;
    [SerializeField] private GameObject _addButton;
    [SerializeField] private GameObject _seeAllButton;
    [SerializeField] private GameObject _emptyRoom;

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
        _roomInfoCanvas.SetActive(true);
        _roomInfoDisplayer.SetInfo(index);
    }

    public void EditInfo(int index)
    {
        _navigator.HideAll();
        _createRoomCanvas.SetActive(true);
        _roomEditor.EditRoom(index);
        _roomEditor.EditEnded += OnRoomEditEnded;
    }

    public void UpdatePreviewItems()
    {
        var rooms = SaveSystem.LoadData<RoomSaveData>();
        foreach (var item in _previewItems)
        {
            item.gameObject.SetActive(false);
        }
        if (rooms.Rooms.Count > 0)
        {
            _previewElements.SetActive(true);
            _noElements.SetActive(false);
            if (rooms.Rooms.Count < 2)
            {
                _previewItems[0].SetInfo(0);
                _previewItems[0].gameObject.SetActive(true);
                _emptyRoom.SetActive(true);
                _addButton.SetActive(false);
                _seeAllButton.SetActive(false);
            }
            if (rooms.Rooms.Count >= 2)
            {
                _previewItems[0].SetInfo(0);
                _previewItems[0].gameObject.SetActive(true);
                _emptyRoom.SetActive(false);
                _previewItems[1].SetInfo(1);
                _previewItems[1].gameObject.SetActive(true);
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
        _allRoomsCanvas.SetActive(true);
    }

    public void OnCreateButtonClick()
    {
        _navigator.HideAll();
        _createRoomCanvas.SetActive(true);
        _roomEditor.CreateNewRoom();
        _roomEditor.EditEnded += OnRoomEditEnded;
    }

    private void OnRoomEditEnded()
    {
        _roomEditor.EditEnded -= OnRoomEditEnded;
        _navigator.ShowMenu();
        UpdatePreviewItems();
    }
}
