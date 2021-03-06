using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Level Preview Images")]
    public Sprite _LevelLocked;
    public Sprite _Level1Preview;
    public Sprite _Level2Preview;
    public Sprite _Level3Preview;


    [Header("Volume Settings")]
    [SerializeField] private TextMeshProUGUI volumeTextValue = null;
    [SerializeField] private UnityEngine.UI.Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 0.6f;
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Global Level Progress Saved Data")]
    public int pnum;
    public int maxLevel;

    [Header("Different New Game Messages")]
    [SerializeField] private TextMeshProUGUI playMessage = null;

    [Header("Background Sprites")]
    public Sprite _Level0Background;
    public Sprite _Level1Background;
    public Sprite _Level2Background;
    public Sprite _Level3Background;

    [Header("Levels to Load")]
    public string _Level1Scene;
    public string _Level2Scene;
    public string _Level3Scene;

    void Start()
    {
        //PlayerPrefs.SetInt("LevelProgress", 0);
        if (PlayerPrefs.HasKey("LevelProgress"))
        {
            pnum = PlayerPrefs.GetInt("LevelProgress");
        }
        else 
        {
            PlayerPrefs.SetInt("LevelProgress", 0);
            pnum = 0;
        }
        SetBackground();
        SetMessage();
        SetLevelPreview();
    }

    public void SetMessage()
    {
        if (pnum == 0) {
            playMessage.text = "Do you want to Start a New Game in Story Mode?";
        } else {
            playMessage.text = string.Format("Do you want to Continue Story Mode? (Stage {0})", pnum + 1);
        }
    }

    public void SetBackground()
    {
        GameObject background = GameObject.Find("MenuBackground");
        Image bkgimage = background.GetComponent<Image>();

        if (pnum == 0) 
        {
            bkgimage.sprite = _Level0Background;
        }
        else if (pnum == 1) 
        {
            bkgimage.sprite = _Level1Background;
        }
        else if (pnum == 2) 
        {
            bkgimage.sprite = _Level2Background;
        }
        else if (pnum == 3) 
        {
            bkgimage.sprite = _Level3Background;
        }
        else
        {
            throw new System.Exception("Files Not Loaded Correctly");
        }
    }

    public void NewGameDialogYes()
    {
        if (pnum < maxLevel) 
        {
            LoadLevel(pnum+1);
        }
        else if (pnum == maxLevel)
        {
            PlayerPrefs.SetInt("LevelProgress", 0);
        } else {
            throw new System.Exception("Wrong Max Level Register");
        }
    }

    public string ChooseLevel(int lnum) 
    {
        if (lnum == 1) {
            return _Level1Scene;
        }
        if (lnum == 2) {
            return _Level2Scene;
        }
        if (lnum == 3) {
            return _Level3Scene;
        }
        throw new System.Exception("Files Not Loaded Correctly");
    }

    public void LoadLevel(int lnum)
    {
        SceneManager.LoadScene(ChooseLevel(lnum));
    }

    public void LoadLevel1()
    {
        LoadLevel(1);
    }

     public void LoadLevel2()
    {
        if (pnum >= 1)
        {
            LoadLevel(2);
        }
    }

     public void LoadLevel3()
    {
        if (pnum >= 2)
        {
            LoadLevel(3);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
    }

    public void SetLevelPreview()
    {
        GameObject lv1pvw = GameObject.Find("/Interface/PopoutContainer/LevelSelectPanel/Level1Image");
        Image lv1pvwImage = lv1pvw.GetComponent<Image>();

        lv1pvwImage.sprite = _Level1Preview;

        GameObject lv2pvw = GameObject.Find("/Interface/PopoutContainer/LevelSelectPanel/Level2Image");
        Image lv2pvwImage = lv2pvw.GetComponent<Image>();
        
        if (pnum < 1) {
            lv2pvwImage.sprite = _LevelLocked;
        } else {
            lv2pvwImage.sprite = _Level2Preview;
        }
        
        GameObject lv3pvw = GameObject.Find("/Interface/PopoutContainer/LevelSelectPanel/Level3Image");
        Image lv3pvwImage = lv3pvw.GetComponent<Image>();
        
        if (pnum < 2) {
            lv3pvwImage.sprite = _LevelLocked;
        } else {
            lv3pvwImage.sprite = _Level3Preview;
        }
    }
}
