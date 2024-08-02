using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Models;
namespace Managers
{
    class MiniMapManager:Singleton<MiniMapManager>
    {
        public UIMiniMap miniMap;
        public Collider miniMapBoundingBox;
        public Collider MiniMapBoundingBox
        {
            get
            {
                return miniMapBoundingBox;
            }
        }
        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                    return null;
                return User.Instance.CurrentCharacterObject.transform;
            }
        }

        public Sprite LoadCurrentMiniMap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMiniMap(Collider minimapBoundBox)
        {
            this.miniMapBoundingBox = minimapBoundBox;
            if (this.miniMap != null)
                this.miniMap.UpdateMap();
        }
    }
}
