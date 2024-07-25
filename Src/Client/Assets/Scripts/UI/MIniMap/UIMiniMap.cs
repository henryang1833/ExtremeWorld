using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Managers;
public class UIMiniMap : MonoBehaviour
{
    public Collider minimapBoundingBox;
    public Image miniMap;
    public Image arrow;
    public Text mapName;

    private Transform playerTransform;
    void Start()
    {
        this.InitMap();
    }

    private void InitMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        if(this.miniMap.overrideSprite == null)
            this.miniMap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
        this.miniMap.SetNativeSize();
        this.miniMap.transform.localPosition = Vector3.zero;
        this.playerTransform = User.Instance.CurrentCharacterObject.transform;
    }

    
    void Update()
    {
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        this.miniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.miniMap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);//要取负号
    }
}
