using System;
using UnityEngine;

public class GameManager : StaticInstance<GameManager> {
    [SerializeField]
    private int maxChangementSeasonCounterTime;
    [SerializeField]
    private int minChangementSeasonCounterTime;
    [SerializeField]
    private float currentCounterTime;
    [SerializeField]
    private int? maxCounterTime = null;
    private FadeObjectEndSeason fadeObjectEndSeason;

    private void Start() {
        fadeObjectEndSeason = GetComponent<FadeObjectEndSeason>();
    }

    private void Update() {
        bool _hasCounterReached = hasCounterReached();
        if (maxCounterTime == null || _hasCounterReached) {
            currentCounterTime = 0;
            _setMaxCounterTime();
        }
        if (_hasCounterReached)
            fadeObjectEndSeason.changeEnvironment();
        _addCounter();
    }

    public bool hasCounterReached() {
        if (currentCounterTime > maxCounterTime)
            return true;
        return false;
    }

    private void _addCounter() {
        currentCounterTime += Time.deltaTime;
    }

    private void _setMaxCounterTime() {
        maxCounterTime = UnityEngine.Random.Range(
            minChangementSeasonCounterTime,
            maxChangementSeasonCounterTime
        );
    }
}

