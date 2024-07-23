using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SkillBridge.Message;
class GameObjectTool
{
    public static Vector3 LogicToWorld(NVector3 vector)
    {
        return new Vector3(vector.X / 100f, vector.Z / 100f, vector.Y / 100f);
    }

    public static Vector3 LogicToWorld(Vector3Int vector)
    {
        return new Vector3(vector.x / 100f, vector.z / 100f, vector.y / 100f);
    }

    public static Vector3Int WorldToLogic(Vector3 vector)
    {
        return new Vector3Int()
        {
            x = Mathf.RoundToInt(vector.x * 100),
            y = Mathf.RoundToInt(vector.z * 100),
            z = Mathf.RoundToInt(vector.y * 100)
        };
    }
}

