using UnityEngine;

public class UIPolygonSide : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private RectTransform _rtFilledArea = null;

    [SerializeField]
    private RectTransform _rtFrame = null;
    #endregion

    #region Mono Behaviour Hooks
    private void Awake() {
        _rtFilledArea.localEulerAngles = new Vector3(0, 0, 45);
        _rtFrame.localEulerAngles = new Vector3(0, 0, 45);
    }
    #endregion

    #region APIs
    public void SetAreaLength(float width, float height) {
        _rtFilledArea.sizeDelta = new Vector2(width, height);
    }

    public void SetFrameLength(float length) {
        _rtFrame.sizeDelta = new Vector2(length, length);
    }
    #endregion
}
