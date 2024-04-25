using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Guest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string CheckDate { get; set; }
    public string DepartDate { get; set; }
    public string Contacts { get; set; }
    public int RoomNumber { get; set; }
    public int Index { get; set; }
}
