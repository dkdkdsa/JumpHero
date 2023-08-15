using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateChanger : JHRoot
{

    private SoundManager soundManager;
    private MargeSencer r, l;
    private Animator animator;
    private GroundObject groundObject;
    private Rigidbody2D rigid;
    private float delTime = 0.1f;

    public JumpStateChanger(JHController controller) : base(controller)
    {

        l = controller.transform.Find("WallSencer").Find("L").GetComponent<MargeSencer>();
        r = controller.transform.Find("WallSencer").Find("R").GetComponent<MargeSencer>();
        animator = controller.GetComponent<Animator>();
        groundObject = controller.GetComponentInChildren<GroundObject>();
        rigid = controller.GetComponent<Rigidbody2D>();
        soundManager = Object.FindObjectOfType<SoundManager>();
    }

    public override void EnterState()
    {

        rigid.gravityScale = 2f;
        animator.SetTrigger("Jump");
        animator.ResetTrigger("JumpEnd");
        delTime = 0.1f;

    }

    public override void Update()
    {

        delTime -= Time.deltaTime;

        if(delTime < 0 && (groundObject.isGround || r.isSencing || l.isSencing))
        {

            if (groundObject.isGround)
            {

                FAED.Pop("DownFX", transform.position + new Vector3(0, 0.2f), Quaternion.identity).GetComponent<FXObject>().Play();
                soundManager.PlaySound("Down");

            }

            controller.ChageState(JHState.Idle);

        }

    }

    public override void ExitState()
    {

        animator.SetTrigger("JumpEnd");
        delTime = 0.1f;

    }

}
