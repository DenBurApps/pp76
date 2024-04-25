using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GuestsSaveData : SaveData
{
    public List<Guest> Guests { get; set; }
    public int GuestIndex { get; set; }

    public GuestsSaveData(List<Guest> guests, int guestIndex)
    {
        Guests = guests;
        GuestIndex = guestIndex;
    }
}
