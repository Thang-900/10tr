using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum positionsType
{
    ground,
    wall,
    water,
    hightground
}
public class positions 
{
    public int x { get; set; }
    public int y { get; set; }
    public positionsType type { get; set; }
    public positions(int x, int y, positionsType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
