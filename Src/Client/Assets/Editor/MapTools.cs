﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Common.Data;
public class MapTools : MonoBehaviour
{
    [MenuItem("Map Tools/Export Teleporters")]
    public static void ExportTeleporters()
    {
        DataManager.Instance.Load();
        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }
         
        List<Teleporte> allTeleporters = new List<Teleporte>();
        foreach(var map in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("Scene {0} not existed!", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            Teleporte[] teleportes = GameObject.FindObjectsOfType<Teleporte>();
            foreach(var telepoter in teleportes)
            {
                if (!DataManager.Instance.Teleporters.ContainsKey(telepoter.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0} 中配置的 Teleporter:[{1}] 不存在", map.Value.Resource, telepoter.ID), "确定");
                    return;
                }

                TeleporterDefine def = DataManager.Instance.Teleporters[telepoter.ID];
                if(def.MapID != map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0} 中配置的 Teleporter:[{1}]  MapId:{2} 错误", map.Value.Resource, telepoter.ID, def.MapID), "确定");
                    return;
                }
                def.Position = GameObjectTool.WorldToLogicN(telepoter.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(telepoter.transform.forward);
            }
        }

        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成", "确定");
    }

    void Start()
    {

    }


    void Update()
    {

    }
}