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
        MiniMapManager.Instance.miniMap = this;  
        this.UpdateMap();
    }

    public void UpdateMap()
    {       
        this.mapName.text = User.Instance.CurrentMapData.Name;
        this.miniMap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
        this.miniMap.SetNativeSize();
        this.miniMap.transform.localPosition = Vector3.zero;
        this.minimapBoundingBox = MiniMapManager.Instance.miniMapBoundingBox;
        this.playerTransform = null;
    }

    
    void Update()
    {
        if(playerTransform == null)
        {
            this.playerTransform = MiniMapManager.Instance.PlayerTransform;
            return;
        }
        if (minimapBoundingBox == null)
            Common.Log.Error("minimapBoundingBox == null");
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;


        if (playerTransform == null)
            Common.Log.Error("playerTransform == null");
        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        if (this.miniMap == null)
            Common.Log.Error("this.miniMap == null");
        if (this.miniMap.rectTransform == null)
            Common.Log.Error("this.miniMap.rectTransform == null");
        if (this.arrow == null)
            Common.Log.Error("this.arrow == null");
        this.miniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.miniMap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);//要取负号
    }
}
