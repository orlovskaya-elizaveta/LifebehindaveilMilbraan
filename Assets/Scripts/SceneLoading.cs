using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {
    [Header("Загружаемая сцена")]
    public int sceneID;
    [Header("Остальный объекты")]
    public Image LoadingImage;
    public Text LoadingText;

	// Use this for initialization
	void Start () {
        StartCoroutine(AsyncLoad());
	}

    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            LoadingImage.fillAmount = progress;
            LoadingText.text = string.Format("{0:0}%", progress * 100);
            yield return null;
        }
    }
}
