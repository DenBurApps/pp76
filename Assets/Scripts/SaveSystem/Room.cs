using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Room
{
    public string Name { get; set; }
    public int Price { get; set; }
    public string ImagePath { get; set; }
    public string Category { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
    public string Conveniences { get; set; }
    public string Description { get; set; }
    public int Index { get; set; }
}
