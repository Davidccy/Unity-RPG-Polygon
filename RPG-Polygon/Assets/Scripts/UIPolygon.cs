using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPolygon : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private bool _playTween = false;

    [SerializeField]
    private float _tweenDuration = 0;

    [SerializeField]
    private UIPolygonSide _resSide = null;
    #endregion

    #region Internal Fields
    private List<UIPolygonSide> _sideList = new List<UIPolygonSide>();
    private int _curSideCount;
    private float _maxValue;
    private readonly int _MIN_SIDE_COUNT = 3;
    #endregion

    #region Properties
    public int CurrentSideCount {
        get {
            return _curSideCount;
        }
    }

    private int CurrentSideObjectCount {
        get {
            return _sideList.Count;
        }
    }
    #endregion

    #region APIs
    public void SetNewSideCount(int sideCount) {
        // NOTE:
        // this step generate new UIPolygonSide object
        if (sideCount < _MIN_SIDE_COUNT) {
            Debug.LogErrorFormat("Set side count which is larger equal than {0}", _MIN_SIDE_COUNT);
            return;
        }

        _curSideCount = sideCount;

        // generate new UIPolygonSide object
        for (int i = 0; i < sideCount; i++) {
            if (i + 1 <= CurrentSideObjectCount) {
                continue;
            }

            UIPolygonSide newSide = Instantiate(_resSide, this.transform);
            _sideList.Add(newSide);
        }

        // hide redundant UIPolygonSide object
        for (int i = sideCount; i < CurrentSideObjectCount; i++) {
            _sideList[i].gameObject.SetActive(false);
        }

        // set transform
        float avgDegree = 360f / sideCount;
        float oriScale = Mathf.Tan((avgDegree / 2 * Mathf.Deg2Rad));
        float newScale = Mathf.Tan(45 * Mathf.Deg2Rad);

        for (int i = 0; i < sideCount; i++) {
            UIPolygonSide sideObject = _sideList[i];
            sideObject.gameObject.SetActive(true);
            sideObject.transform.localEulerAngles = new Vector3(0, 0, -(i * avgDegree));
            sideObject.transform.localScale = new Vector3(1, oriScale / newScale, 1);
        }

        // adjust root angle
        this.transform.eulerAngles = Vector3.forward * (45 + (90 - avgDegree) / 2);
    }
        
    public void SetMaxValue(float maxValue) {
        _maxValue = maxValue;
    }

    public void SetValue(List<float> valueList) {
        // is side generated
        if (CurrentSideCount <= 0) {
            return;
        }

        // NOTE:
        // Count of value array should be same as current side count.
        if (valueList.Count != CurrentSideCount) {
            return;
        }

        for (int i = 0; i < CurrentSideCount; i++) {
            int idexIndex = i;
            float width = idexIndex == 0 ? valueList.Last() : valueList[idexIndex - 1];
            float height = valueList[idexIndex];

            float duration = _playTween ? _tweenDuration : 0;
            float ratio = 0;
            DOTween.To(() => ratio,
                (v) => { ratio = v; _sideList[idexIndex].SetAreaLength(width * ratio, height * ratio); },
                1, duration).SetUpdate(true);

            _sideList[idexIndex].SetFrameLength(_maxValue);
        }
    }
    #endregion

    #region Testing APIs
    public void SetRandom() {
        int rndSideCount = Random.Range(3, 10);
        List<float> valueList = new List<float>();
        for (int i = 0; i < rndSideCount; i++) {
            valueList.Add(Random.Range(50f, 200f));
        }

        SetNewSideCount(rndSideCount);
        SetMaxValue(200);
        SetValue(valueList);
    }
    #endregion
}
