using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowRotate : JHRoot
{
    private SpriteRenderer spriteRenderer;

    public PlayerArrowRotate(JHController controller) : base(controller)
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    }

    public override void EnterState()
    {

        playerValueSystem.arrowRootTrm.gameObject.SetActive(true);

    }

    public override void Update()
    {

        if (!playerValueSystem.arrowRootTrm.gameObject.activeSelf) return;

        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;

        if(playerValueSystem.flipAble) spriteRenderer.flipX = (mouse - transform.position).x < 0;

        playerValueSystem.arrowRootTrm.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    public override void ExitState()
    {

        playerValueSystem.arrowRootTrm.gameObject.SetActive(false);
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteRenderer.flipX = (mouse - transform.position).x < 0;

    }

}
