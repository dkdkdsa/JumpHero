using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CutSceneText
{

    public string key;
    [TextArea] public string text;

}

public class IntroCutScene : MonoBehaviour
{

    [SerializeField] private Transform cameraTrm;
    [SerializeField] private GameObject textpanel;
    [SerializeField] private TMP_Text panelText;
    [SerializeField] private AudioSource textsource;
    [SerializeField] private List<CutSceneText> textScenes;
    [SerializeField] private GameObject gob;
    [SerializeField] private GameObject kb;
    [SerializeField] private GameObject player;
    [SerializeField] private ParticleSystem zParticle;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject nextText;

    private Dictionary<string, char[]> textDic = new Dictionary<string, char[]>();
    private SpriteRenderer gobRenderer;
    private SpriteRenderer playerRenderer;
    private Animator gobAnimator;
    private Animator playerAnimator;

    private void Awake()
    {

        panelText.text = "";

        foreach(var text in textScenes)
        {

            textDic.Add(text.key, text.text.ToCharArray());

        }

        textpanel.transform.localScale = new Vector3(0, 1, 1);
        gobAnimator = gob.GetComponent<Animator>();
        gobRenderer = gob.GetComponent<SpriteRenderer>();
        playerAnimator = player.GetComponent<Animator>();
        playerRenderer = player.GetComponent<SpriteRenderer>();

    }

    private void Start()
    {

        StartCoroutine(CutSceneCo());

    }

    private void CameraMovement(Vector2 pos)
    {

        cameraTrm.position = new Vector3(pos.x, pos.y, -10);

    }

    private IEnumerator FadeIn()
    {

        fadeImage.color = new Color(0, 0, 0, 0);

        float per = 0;

        while(per < 1)
        {

            per += Time.deltaTime;

            var color = fadeImage.color;
            color.a = per;
            fadeImage.color = color;

            yield return null;

        }

        fadeImage.color = new Color(0, 0, 0, 1);

    }

    private IEnumerator FadeOut()
    {

        fadeImage.color = new Color(0, 0, 0, 1);

        float per = 0;

        while (per < 1)
        {

            per += Time.deltaTime;

            var color = fadeImage.color;
            color.a = 1 - per;
            fadeImage.color = color;

            yield return null;

        }

        fadeImage.color = new Color(0, 0, 0, 0);

    }

    private IEnumerator CutSceneCo()
    {

        yield return StartCoroutine(FadeOut());
        yield return StartCoroutine(FirstCameraMoveCo());
        yield return StartCoroutine(GobMoveCo());
        yield return StartCoroutine(GobSteal());
        yield return StartCoroutine(GobRunCo());
        yield return StartCoroutine(WakeUp());
        yield return StartCoroutine(CastleUp());
        yield return StartCoroutine(FadeIn());

    }

    private IEnumerator FirstCameraMoveCo()
    {

        float per = 0;
        Vector2 targetVec = new Vector2(0, -3f);
        Vector2 originVec = cameraTrm.position;
        while (per < 1)
        {

            per += Time.deltaTime / 2;

            var vec = Vector2.Lerp(originVec, targetVec, per);

            CameraMovement(vec);

            yield return null;

        }

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(OpenPanel());
        yield return StartCoroutine(TextShowCo("First"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        yield return StartCoroutine(TextShowCo("Sec"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        yield return ClosePanel();

    }

    private IEnumerator GobMoveCo()
    {

        float per = 0;
        Vector2 gobPos = new Vector2(-0f, -2.1f);
        Vector2 gobOrigin = gob.transform.position;
        gobAnimator.SetTrigger("RunTrigger");

        while(per < 1)
        {

            per += Time.deltaTime / 2;
            gob.transform.position = Vector2.Lerp(gobOrigin, gobPos, per);
            yield return null;

        }

        gobAnimator.SetTrigger("RunEndTrigger");

        yield return StartCoroutine(OpenPanel());
        yield return StartCoroutine(TextShowCo("Gob"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        yield return StartCoroutine(TextShowCo("GobSec"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        yield return StartCoroutine(ClosePanel());

    }

    private IEnumerator GobSteal()
    {

        float per = 0;
        Vector2 target = new Vector2(-2.8f, -2.1f);
        Vector2 origin = gob.transform.position;

        gobAnimator.SetTrigger("RunTrigger");

        while(per < 1)
        {

            per += Time.deltaTime;
            gob.transform.position = Vector2.Lerp(origin, target, per);
            yield return null;

        }

        gobAnimator.SetTrigger("RunEndTrigger");

        yield return new WaitForSeconds(0.3f);

        kb.transform.SetParent(gob.transform);

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(OpenPanel());
        yield return StartCoroutine(TextShowCo("GobSteal"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        yield return StartCoroutine(ClosePanel());

    }

    private IEnumerator GobRunCo()
    {

        float per = 0;
        Vector2 target = new Vector2(14f, -2.1f);
        Vector2 origin = gob.transform.position;

        gobRenderer.flipX = true;

        gobAnimator.SetTrigger("RunTrigger");

        while (per < 1)
        {

            per += Time.deltaTime / 3;
            gob.transform.position = Vector2.Lerp(origin, target, per);
            yield return null;

        }

        gobAnimator.SetTrigger("RunEndTrigger");

    }

    private IEnumerator WakeUp()
    {

        zParticle.Stop();
        playerAnimator.SetTrigger("WakeUpTrigger");
        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < 4; i++)
        {

            playerRenderer.flipX = !playerRenderer.flipX;
            yield return new WaitForSeconds(0.4f);

        }

        yield return StartCoroutine(OpenPanel());
        yield return StartCoroutine(TextShowCo("WakeUp"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        yield return StartCoroutine(ClosePanel());

    }

    private IEnumerator CastleUp()
    {

        float per = 0;
        Vector2 target = new Vector2(0, 34);
        Vector2 origin = cameraTrm.position;

        gob.transform.position = new Vector2(-2, 35);

        while(per < 1)
        {

            per += Time.deltaTime / 2f;
            CameraMovement(Vector2.Lerp(origin, target, per));
            yield return null;

        }

        yield return StartCoroutine(OpenPanel());
        yield return StartCoroutine(TextShowCo("Last"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));


    }

    private IEnumerator TextShowCo(string key)
    {

        nextText.SetActive(false);

        panelText.text = "";

        var curText = textDic[key];

        for(int i = 0; i < curText.Length; i++)
        {

            panelText.text += curText[i];

            if (curText[i] != ' ')
            {

                textsource.Play();

            }

            yield return new WaitForSeconds(0.05f);

        }

        nextText.SetActive(true);

    }

    private IEnumerator OpenPanel()
    {
        panelText.text = "";
        float per = 0;

        while (per < 1)
        {

            per += Time.deltaTime * 4;

            textpanel.transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), per);
            yield return null;

        }

    }

    private IEnumerator ClosePanel()
    {
        
        nextText.SetActive(false);

        float per = 0;

        while (per < 1)
        {
            per += Time.deltaTime * 4;

            textpanel.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 1, 1), per);
            yield return null;

        }

    }

}
