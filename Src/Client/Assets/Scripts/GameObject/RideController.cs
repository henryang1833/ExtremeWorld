using SkillBridge.Message;
using UnityEngine;

public class RideController : MonoBehaviour
{
    public Transform mountPoint;
    public Vector3 offset;
    private Animator anim;
    private EntityController rider;

    void Start()
    {
        this.anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (this.mountPoint == null || this.rider == null) return;
        this.rider.SetRidePosition(this.mountPoint.position + this.mountPoint.TransformDirection(this.offset));
    }

    internal void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }

    internal void SetRider(EntityController rider)
    {
        this.rider = rider;
    }
}
