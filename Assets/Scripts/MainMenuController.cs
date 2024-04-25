using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomsText;
    [SerializeField] private TMP_Text _guestsText;

    private void OnEnable()
    {
        UpdateStatistic();
    }

    public void UpdateStatistic()
    {
        var rooms = SaveSystem.LoadData<RoomSaveData>();
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        _roomsText.text = rooms.Rooms.Count.ToString();
        _guestsText.text = guests.Guests.Count.ToString();
    }
}
