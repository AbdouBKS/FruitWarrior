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
    private List<SeasonObject> _seasonObjectList;
    [SerializeField]
    private int _indexSeason = 0;
    public int indexSeason {
        get {
            return _indexSeason;
        }
    }

    [SerializeField]
    private float _fadedAlpha = 0.33f;
    [SerializeField]
    private FadeMode _fadingMode;
    [SerializeField]
    private int _fadeFPS = 30;
    [SerializeField]
    private float _fadeSpeed = 1f;

    private Dictionary<SeasonObject, Coroutine> _runningCoroutines = new Dictionary<SeasonObject, Coroutine>();

    private void Start() {
        for (int i = 0; i < _seasonObjectList.Count; ++i) {
            _seasonObjectList[i].gameObject.SetActive(_seasonObjectList[i].isActive);
            if (!_seasonObjectList[i].isActive)
                _SetMaterialAlpha(i, 0);
        }
    }

    void _SetMaterialAlpha(int i, int value) {
        FadeObject FadeObject = _seasonObjectList[i].gameObject.GetComponent<FadeObject>();
        if (FadeObject.Materials != null && FadeObject.Materials.Count > 0 &&
        FadeObject.Materials[0].HasProperty("_Color")) {
            while (FadeObject.Materials[0].color.a > _fadedAlpha)
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
        _runningCoroutines.Add(
            _seasonObjectList[_indexSeason],
            StartCoroutine(FadeObjectOut(_seasonObjectList[_indexSeason]))
        );

        ++_indexSeason;
        if (_indexSeason >= _seasonObjectList.Count)
            _indexSeason = 0;

        _runningCoroutines.Add(
            _seasonObjectList[_indexSeason],
            StartCoroutine(FadeObjectIn(_seasonObjectList[_indexSeason]))
        );
    }

    private IEnumerator FadeObjectOut(SeasonObject season)
    {
        FadeObject FadeObject = season.gameObject.GetComponent<FadeObject>();
        float waitTime = 1f / _fadeFPS;
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
            while (FadeObject.Materials[0].color.a > _fadedAlpha)
            {
                for (int i = 0; i < FadeObject.Materials.Count; i++)
                {
                    if (FadeObject.Materials[i].HasProperty("_Color"))
                    {
                        FadeObject.Materials[i].color = new Color(
                            FadeObject.Materials[i].color.r,
                            FadeObject.Materials[i].color.g,
                            FadeObject.Materials[i].color.b,
                            Mathf.Lerp(FadeObject.InitialAlpha, _fadedAlpha, waitTime * ticks * _fadeSpeed)
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

        if (_runningCoroutines.ContainsKey(season))
        {
            StopCoroutine(_runningCoroutines[season]);
            _runningCoroutines.Remove(season);
            season.gameObject.SetActive(false);
            season.isActive = false;
            season.isFading = false;
        }
    }

    private IEnumerator FadeObjectIn(SeasonObject season)
    {
        FadeObject FadeObject = season.gameObject.GetComponent<FadeObject>();
        float waitTime = 1f / _fadeFPS;
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
                            Mathf.Lerp(_fadedAlpha, 1, waitTime * ticks * _fadeSpeed)
                        );
                    }
                }

                ticks++;
                season.gameObject.SetActive(true);
                season.isActive = true;
                yield return Wait;
            }
        }

        if (_runningCoroutines.ContainsKey(season))
        {
            StopCoroutine(_runningCoroutines[season]);
            _runningCoroutines.Remove(season);
            season.isFading = false;
        }
    }
    public enum FadeMode
    {
        Transparent,
        Fade
    }
}
