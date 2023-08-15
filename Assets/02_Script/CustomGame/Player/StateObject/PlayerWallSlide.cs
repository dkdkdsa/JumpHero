using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : JHRoot
{

    private SoundManager soundManager;
    private Animator animator;
    private MargeSencer r, l;
    private Rigidbody2D rigid;
    private GroundObject groundObject;
    private SpriteRenderer spriteRenderer;
    private Coroutine co;
    private bool isChange;

    public PlayerWallSlide(JHController controller) : base(controller)
    {

        animator = controller.GetComponent<Animator>();
        spriteRenderer = controller.GetComponent<SpriteRenderer>();
        l = controller.transform.Find("WallSencer").Find("L").GetComponent<MargeSencer>();
        r = controller.transform.Find("WallSencer").Find("R").GetComponent<MargeSencer>();
        groundObject = transform.GetComponentInChildren<GroundObject>();
        rigid = controller.GetComponent<Rigidbody2D>();
        soundManager = Object.FindObjectOfType<SoundManager>();

    }

    public override void EnterState()
    {

        isChange = false;
        animator.ResetTrigger("SlideEnd");

    }

    public override void Update()
    {

        if((r.isSencing ||  l.isSencing) && !isChange && !groundObject.isGround)
        {

            isChange = true;
            playerValueSystem.flipAble = false;
            animator.SetTrigger("Slide");
            rigid.velocity = new Vector2(0, 0);
            rigid.gravityScale = 0.2f;

            spriteRenderer.flipX = !l.isSencing;

            co = controller.StartCoroutine(SlideCo());
            soundManager.PlaySound("Slide");

        }
        else if(((!r.isSencing && !l.isSencing) || groundObject.isGround) && isChange)
        {
            
            isChange = false;
            playerValueSystem.flipAble = true;
            animator.SetTrigger("SlideEnd");
            rigid.gravityScale = 2f;
            soundManager.PauseSound("Slide");

        }

    }

    public override void ExitState()
    {

        animator.SetTrigger("SlideEnd");
        playerValueSystem.flipAble = true;
        rigid.gravityScale = 2f;
        soundManager.PauseSound("Slide");

    }

    private IEnumerator SlideCo()
    {

        while ((r.isSencing || l.isSencing) && !groundObject.isGround)
        {

            if (!l.isSencing)
            {

                var obj = FAED.Pop("SlideFX", transform.position + new Vector3(-0.5f, 0.2f), Quaternion.identity).GetComponent<FXObject>();
                obj.Play();
                obj.GetComponent<SpriteRenderer>().flipX = true;

            }
            else
            {

                var obj = FAED.Pop("SlideFX", transform.position + new Vector3(0.5f, 0.2f), Quaternion.identity).GetComponent<FXObject>();
                obj.Play();
                obj.GetComponent<SpriteRenderer>().flipX = false;

            }

            yield return new WaitForSeconds(0.4f);

        }

    }

}
