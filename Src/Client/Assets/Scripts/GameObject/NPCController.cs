using UnityEngine;
using Common.Data;
using Managers;
using System.Collections;
using Models;
public class NPCController : MonoBehaviour
{
    public int npcID;
    Animator animator;
    NPCDefine npc;
    Color originColor;
    SkinnedMeshRenderer renderer;
    private bool inInteractive = false;
    NpcQuestStatus questStatus;

   

    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        npc = NPCManager.Instance.GetNPCDefine(npcID);
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = renderer.sharedMaterial.color;
        this.StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }
    void OnQuestStatusChanged(Quest quest)
    {
        this.RefreshNpcStatus();
    }

    private void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcID);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

    private void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
                yield return new WaitForSeconds(2f);
            else
                yield return new WaitForSeconds(Random.Range(5f,10f));

            this.Relax();
        }
    }

    void Relax()
    {
        animator.SetTrigger("Relax");
    }

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npc))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (Models.User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo))> 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    private void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (renderer.sharedMaterial.color != Color.white)
                renderer.sharedMaterial.color = Color.white;
        }
        else
        {
            if (renderer.sharedMaterial.color != originColor)
                renderer.sharedMaterial.color = originColor;
        }
    }

    
}
