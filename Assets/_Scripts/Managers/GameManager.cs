using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : StaticInstance<GameManager> {
    [SerializeField]
    private int _maxChangementSeasonCounterTime;
    [SerializeField]
    private int _minChangementSeasonCounterTime;
    [SerializeField]
    private float _currentCounterTime;
    [SerializeField]
    private int? _maxCounterTime = null;
    private FadeObjectEndSeason _fadeObjectEndSeason;
    [SerializeField]
    private List<GameObject> _springFood = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _summerFood = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _fallFood = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _winterFood = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _junkFood = new List<GameObject>();
    private Dictionary<string, List<GameObject>> _seasonFood;

    [SerializeField]
    private List<GameObject> _goodFood = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _badFood = new List<GameObject>();
    [SerializeField]
    private int _currentIndex = -1;

    private void Start() {
        _fadeObjectEndSeason = GetComponent<FadeObjectEndSeason>();
        _seasonFood = new Dictionary<string, List<GameObject>>{
            {"spring", _springFood},
            {"summer", _summerFood},
            {"fall", _fallFood},
            {"winter", _winterFood}
        };
    }

    private void Update() {
        bool _hasCounterReached = hasCounterReached();
        if (_maxCounterTime == null || _hasCounterReached) {
            _currentCounterTime = 0;
            _set_MaxCounterTime();
        }
        if (_hasCounterReached)
            _fadeObjectEndSeason.changeEnvironment();
        _addCounter();
        if (_currentIndex != _fadeObjectEndSeason.indexSeason) {
            _currentIndex = _fadeObjectEndSeason.indexSeason;
            _setGoodFood();
            _setBadFood();
        }
    }

    private void _setGoodFood() {
        _goodFood = _seasonFood.ElementAt(_currentIndex).Value;
    }

    private void _setBadFood() {
        _badFood = new List<GameObject>();
        _badFood.AddRange(_junkFood);
        for (int i = 0; i < _seasonFood.Count; ++i)
            if (i != _currentIndex)
                _badFood.AddRange(_seasonFood.ElementAt(i).Value);
    }

    public bool hasCounterReached() {
        if (_currentCounterTime > _maxCounterTime)
            return true;
        return false;
    }

    private void _addCounter() {
        _currentCounterTime += Time.deltaTime;
    }

    private void _set_MaxCounterTime() {
        _maxCounterTime = UnityEngine.Random.Range(
            _minChangementSeasonCounterTime,
            _maxChangementSeasonCounterTime
        );
    }
}

