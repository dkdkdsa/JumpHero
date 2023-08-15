using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : JHRoot
{

    private const float MaxGaugeCount = 3f;

    private SoundManager soundManager;
    private Rigidbody2D rigid;
    private float jumpGauge;
    private bool isUp;

    public PlayerJump(JHController controller) : base(controller)
    {

        rigid = controller.GetComponent<Rigidbody2D>();
        soundManager = Object.FindObjectOfType<SoundManager>();

    }

    public override void EnterState()
    {

        playerValueSystem.gaugePanel.SetActive(false);
        playerValueSystem.gaugeBase.localScale = new Vector3(0, 1);

    }

    public override void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            playerValueSystem.gaugePanel.SetActive(true);
            isUp = true;

        }

        if (isUp)
        {

            jumpGauge += Time.deltaTime * MaxGaugeCount;
            playerValueSystem.gaugeBase.localScale = new Vector3(jumpGauge / MaxGaugeCount, 1);

        }

        if((Input.GetMouseButtonUp(0) || jumpGauge >= MaxGaugeCount) && isUp) 
        {
            
            rigid.velocity = ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized + Vector3.up) 
                * (playerValueSystem.jumpPower + (playerValueSystem.jumpPower * jumpGauge));
            rigid.gravityScale = 2;

            soundManager.PlaySound("Jump");
            FAED.Pop<FXObject>("JumpFX", transform.position - new Vector3(0, 0.5f), out var jp);
            jp.Play();

            controller.ChageState(JHState.Jump);
            isUp = false;

        }

    }

    public override void ExitState()
    {

        jumpGauge = 0;
        playerValueSystem.gaugePanel.SetActive(false);

    }

}
