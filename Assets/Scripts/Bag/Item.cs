using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Item", menuName = "Bag/New Item")]
public class Item : ScriptableObject
{
    public string name;
    public Sprite image;
    public int num;
    [TextArea]
    public string description;
}
