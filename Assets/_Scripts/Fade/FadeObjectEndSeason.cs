using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeasonObject {
    public GameObject gameObject;
    public bool isActive;
    public bool isFading = false;
}

public class FadeObjectEndSeason : MonoBehaviour
{
    [SerializeField]
    private List<SeasonObject> seasonObjectList;
    [SerializeField]
    private int indexSeason = 0;

    [SerializeField]
    private float FadedAlpha = 0.33f;
    [SerializeField]
    private FadeMode FadingMode;
    [SerializeField]
    private int FadeFPS = 30;
    [SerializeField]
    private float FadeSpeed = 1f;

    private Dictionary<SeasonObject, Coroutine> RunningCoroutines = new Dictionary<SeasonObject, Coroutine>();

    private void Awake() {
        for (int i = 0; i < seasonObjectList.Count; ++i) {
            seasonObjectList[i].gameObject.SetActive(seasonObjectList[i].isActive);
            if (!seasonObjectList[i].isActive)
                _SetMaterialAlpha(i, 0);
        }
    }

    void _SetMaterialAlpha(int i, int value) {
        FadeObject FadeObject = seasonObjectList[i].gameObject.GetComponent<FadeObject>();
        if (FadeObject.Materials != null && FadeObject.Materials.Count > 0 &&
        FadeObject.Materials[0].HasProperty("_Color")) {
            while (FadeObject.Materials[0].color.a > FadedAlpha)
            {
                for (int j = 0; j < FadeObject.Materials.Count; j++)
                {
                    if (FadeObject.Materials[j].HasProperty("_Color"))
                    {
                        FadeObject.Materials[j].color = new Color(
                            FadeObject.Materials[j].color.r,
                            FadeObject.Materials[j].color.g,
                            FadeObject.Materials[j].color.b,
                            value
                        );
                    }
                }
            }
        }
    }

    public void changeEnvironment() {
        RunningCoroutines.Add(
            seasonObjectList[indexSeason],
            StartCoroutine(FadeObjectOut(seasonObjectList[indexSeason]))
        );

        ++indexSeason;
        if (indexSeason >= seasonObjectList.Count)
            indexSeason = 0;

        RunningCoroutines.Add(
            seasonObjectList[indexSeason],
            StartCoroutine(FadeObjectIn(seasonObjectList[indexSeason]))
        );
    }

    private IEnumerator FadeObjectOut(SeasonObject season)
    {
        FadeObject FadeObject = season.gameObject.GetComponent<FadeObject>();
        float waitTime = 1f / FadeFPS;
        WaitForSeconds Wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        for (int i = 0; i < FadeObject.Materials.Count; i++)
        {
            FadeObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); // affects both "Transparent" and "Fade" options
            FadeObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); // affects both "Transparent" and "Fade" options
            FadeObject.Materials[i].EnableKeyword("_ALPHABLEND_ON");
            /*
            FadeObject.Materials[i].SetInt("_ZWrite", 0); // disable Z writing
        
            if (FadingMode == FadeMode.Fade)
            {
                
            }
            else
            {
                FadeObject.Materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            */
            FadeObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        if (FadeObject.Materials[0].HasProperty("_Color"))
        {
            while (FadeObject.Materials[0].color.a > FadedAlpha)
            {
                for (int i = 0; i < FadeObject.Materials.Count; i++)
                {
                    if (FadeObject.Materials[i].HasProperty("_Color"))
                    {
                        FadeObject.Materials[i].color = new Color(
                            FadeObject.Materials[i].color.r,
                            FadeObject.Materials[i].color.g,
                            FadeObject.Materials[i].color.b,
                            Mathf.Lerp(FadeObject.InitialAlpha, FadedAlpha, waitTime * ticks * FadeSpeed)
                        );
                    }
                }

                ticks++;
                yield return Wait;
            }
        }

        for (int i = 0; i < FadeObject.Materials.Count; i++)
        {
            FadeObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); // affects both "Transparent" and "Fade" options
            FadeObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); // affects both "Transparent" and "Fade" options
            FadeObject.Materials[i].DisableKeyword("_ALPHABLEND_ON");
            /*
            FadeObject.Materials[i].SetInt("_ZWrite", 0); // disable Z writing
        
            if (FadingMode == FadeMode.Fade)
            {
                
            }
            else
            {
                FadeObject.Materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            */
        }

        if (RunningCoroutines.ContainsKey(season))
        {
            StopCoroutine(RunningCoroutines[season]);
            RunningCoroutines.Remove(season);
            season.gameObject.SetActive(false);
            season.isActive = false;
            season.isFading = false;
        }
    }

    private IEnumerator FadeObjectIn(SeasonObject season)
    {
        FadeObject FadeObject = season.gameObject.GetComponent<FadeObject>();
        float waitTime = 1f / FadeFPS;
        WaitForSeconds Wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        for (int i = 0; i < FadeObject.Materials.Count; i++)
        {
            /*
            if (FadingMode == FadeMode.Fade)
            {
                FadeObject.Materials[i].DisableKeyword("_ALPHABLEND_ON");
            }
            else
            {
                FadeObject.Materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            FadeObject.Materials[i].SetInt("_ZWrite", 1); // re-enable Z Writing
            */
            FadeObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            FadeObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            FadeObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }

        if (FadeObject.Materials[0].HasProperty("_Color"))
        {
            while (FadeObject.Materials[0].color.a < 1)
            {
                for (int i = 0; i < FadeObject.Materials.Count; i++)
                {
                    if (FadeObject.Materials[i].HasProperty("_Color"))
                    {
                        FadeObject.Materials[i].color = new Color(
                            FadeObject.Materials[i].color.r,
                            FadeObject.Materials[i].color.g,
                            FadeObject.Materials[i].color.b,
                            Mathf.Lerp(FadedAlpha, 1, waitTime * ticks * FadeSpeed)
                        );
                    }
                }

                ticks++;
                season.gameObject.SetActive(true);
                season.isActive = true;
                yield return Wait;
            }
        }

        if (RunningCoroutines.ContainsKey(season))
        {
            StopCoroutine(RunningCoroutines[season]);
            RunningCoroutines.Remove(season);
            season.isFading = false;
        }
    }
    public enum FadeMode
    {
        Transparent,
        Fade
    }
}
