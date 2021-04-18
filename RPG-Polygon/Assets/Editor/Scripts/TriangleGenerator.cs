using UnityEngine;
using UnityEditor;
using System.IO;

public class TriangleGenerator : EditorWindow {
    [MenuItem("CustomizeTools/ShowTriangleGeneratorWindow")]
    public static void ShowTriangleGeneratorWindow() {
        GetWindow<TriangleGenerator>().Show();
    }

    private Color _triangleColor;
    private Color _frameColor;
    private void OnGUI() {

        // select color
        _triangleColor = EditorGUILayout.ColorField(_triangleColor);
        _frameColor = EditorGUILayout.ColorField(_frameColor);

        if (GUILayout.Button("Generate Texture!")) {
            Generate();
        }
    }

    private int _frameThickness = 2;
    private int _length = 256;
    private void Generate() {
        // generate Isosceles Right Triangle texture and it's frame
        //
        // draw triangle like:
        // xxxxxxxxxx
        // xxxxxxxxx
        // xxxxxxxx
        // xxxxxxx
        // xxxxxx
        // xxxxx
        // xxxx
        // xxx
        // xx
        // x
        //
        // and draw frame like:
        // (if thickness = 2)
        // xxxxxxxxxx
        // xxxxxxxxx
        // xx    xx
        // xx   xx
        // xx  xx
        // xx xx
        // xxxx
        // xxx
        // xx
        // x
        //
        // width = height
        Texture2D texTriangle = new Texture2D(_length, _length, TextureFormat.ARGB32, false);
        Texture2D texFrame = new Texture2D(_length, _length, TextureFormat.ARGB32, false);

        // set pixels
        for (int w = 0; w < _length; w ++) {
            for (int h = 0; h < _length; h++) {
                Color colorTriangle = _triangleColor;
                if (w > h) {
                    colorTriangle.a = 0;
                }
                texTriangle.SetPixel(w, h, colorTriangle);

                Color colorFrame = _frameColor;
                if ((w > h) || ((w >= _frameThickness) && (h - w) >= Mathf.Sqrt(2 * Mathf.Pow(_frameThickness, 2))) && Mathf.Abs(h - _length) >= _frameThickness) {
                    colorFrame.a = 0;
                }
                texFrame.SetPixel(w, h, colorFrame);
            }
        }

        byte[] bytesTriangle = texTriangle.EncodeToPNG();
        File.WriteAllBytes("Assets/testTriangle.png", bytesTriangle);

        byte[] bytesFrame = texFrame.EncodeToPNG();
        File.WriteAllBytes("Assets/testFrame.png", bytesFrame);

        AssetDatabase.Refresh();
    }
}
