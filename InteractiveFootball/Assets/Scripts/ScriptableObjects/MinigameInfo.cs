using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "MinigameInfo", menuName = "Scriptable Objects/MinigameInfo")]
public class MinigameInfo : ScriptableObject
{
    public string name;
    public string subtitle;
    public Sprite description;
    public Sprite logo;
    public string sceneName;
    public VideoClip previewClip;
}
