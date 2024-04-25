using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoomSaveData : SaveData
{
    public List<Room> Rooms { get; set; }
    public int RoomIndex { get; set; }

    public RoomSaveData(List<Room> rooms, int roomIndex)
    {
        Rooms = rooms;
        RoomIndex = roomIndex;
    }
    
}
