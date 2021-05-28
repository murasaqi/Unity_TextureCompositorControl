using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureCompositorControlClip))]//拡張するクラスを指定
public class TextureCompositorControlClipEditor : Editor {

    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    public override void OnInspectorGUI(){
        
        TextureCompositorControlClip textureCompositorControlClip = target as TextureCompositorControlClip;

        if (GUILayout.Button("Init wiggler value")){
            textureCompositorControlClip.InitValues();
        }  
        base.OnInspectorGUI ();

    }

}