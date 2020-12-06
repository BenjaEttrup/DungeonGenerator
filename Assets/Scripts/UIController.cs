using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject uiImage;
    public GameObject backButton;

    public void ShowBuyMeny()
    {
        uiImage.SetActive(true);
        backButton.SetActive(true);
    }

    public void HideBuyMeny()
    {
        uiImage.SetActive(false);
        backButton.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }
}
