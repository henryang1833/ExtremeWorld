using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
public class MapController : MonoBehaviour {
    public Collider miniMapBoundingBox;
	
	void Start ()
    {
        MiniMapManager.Instance.UpdateMiniMap(miniMapBoundingBox);
	}
	
	
	void Update ()
    {
		
	}
}
