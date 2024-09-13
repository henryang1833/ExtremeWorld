using UnityEngine;
using Common.Data;
using Services;
[ExecuteInEditMode]
public class Teleporte : MonoBehaviour
{
    public int ID;
    Mesh mesh = null;
    void Start()
    {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 pos = this.transform.position + Vector3.up * this.transform.localScale.y * .5f;
        Gizmos.color = Color.red;
        if (this.mesh != null)
            Gizmos.DrawWireMesh(this.mesh, pos, this.transform.rotation, this.transform.localScale);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, pos, this.transform.rotation, 1f, EventType.Repaint);
        UnityEditor.Handles.Label(pos, "Teleporte:" + this.ID);
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
        if (playerInputController != null && playerInputController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td == null)
            {
                Debug.LogErrorFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}] , But TeleporterDifine not existed", playerInputController.character.Info.Name, this.ID);
                return;
            }
            Debug.LogFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}:{2}]", playerInputController.character.Info.Name, this.ID, td.Name);
            if(td.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                    MapService.Instance.SendMapTeleport(this.ID);
                else
                    Debug.LogErrorFormat("Teleporter ID:{0} LinkID {1} error!", td.ID, td.LinkTo);
            }
        }
        
    }
}
