using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Setting : MonoBehaviour {
	public SoundPlay soundPlay;

	private Image aToggle, bToggle, mToggle;
	private Slider aSlider, bSlider, mSlider;
	private InputField inputField;
	private string theme;
	private string UIColor;
	private string textColor;
	private string pathUI;
	float time = 0.0f;



	// 初始化调用
	void Start () {
		soundPlay = GameObject.Find ("Audio_Source").GetComponent<SoundPlay> ();
		soundPlay.InSettingScene ();
		InitializeUIColor ();
	}

	void Update () {
		time += Time.deltaTime;
	}

	public void Button_NameChange(){
		inputField.gameObject.SetActive (true);
	}

	public void Button_TextChange(){
		
	}

	public void Button_TextChangeEnd(){
		string str = inputField.text;
		if (str != "") {
			GameObject.Find ("Canvas/PlayerNameText").GetComponent<Text> ().text = (textColor + str + "</color>");
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("PlayerName", str);
			inputField.text = "";
			inputField.gameObject.SetActive (false);
		}
	}


	public void Button_CheckAssemble(){
		if (aToggle.enabled == true) {
			aToggle.enabled = false;
			PlayerPrefs.SetInt ("isAutomaticAdjustment", 0);
		} else {
			aToggle.enabled = true;
			PlayerPrefs.SetInt ("isAutomaticAdjustment", 1);
		}
	}

	public void Button_CheckBackGroundSound(){
		if (bToggle.enabled == true) {
			bToggle.enabled = false;
			PlayerPrefs.SetInt ("IsPlayBackGroundSound", 0);
		} else {
			bToggle.enabled = true;
			PlayerPrefs.SetInt ("IsPlayBackGroundSound", 1);
		}
	}

	public void Button_CheckMoveSound(){
		if (mToggle.enabled == true) {
			mToggle.enabled = false;
			PlayerPrefs.SetInt ("IsPlayMoveSound", 0);
		} else {
			mToggle.enabled = true;
			PlayerPrefs.SetInt ("IsPlayMoveSound", 1);
		}
	}
		
	public void Button_ChangeAssemble(){
		PlayerPrefs.SetFloat ("operationError", aSlider.value);
	}

	public void Button_ChangeBackGroundSound(){
		PlayerPrefs.SetFloat ("PlayBackGroundSound", bSlider.value);
	}

	public void Button_ChangeMoveSound(){
		PlayerPrefs.SetFloat ("PlayMoveSound", mSlider.value);
	}

	// 返回-跳转场景
	public void Button_Exit () {
		PlayerPrefs.SetFloat ("TimeChange", time);
		SceneManager.LoadScene ("MainSence");
	}

	void InitializeUIColor () {

		theme = PlayerPrefs.GetString ("theme");
		UIColor = PlayerPrefs.GetString ("UIColor");
		textColor = PlayerPrefs.GetString ("textColor");
		pathUI = "Sprites/" + theme + "/UI_" + UIColor + "/UI_" + UIColor;

		GameObject.Find ("Background/Background").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Background01");
		// Canvas下组件的初始化
		string playerEName = PlayerPrefs.GetString ("PlayerName");
		string playerCName = WWW.UnEscapeURL (playerEName);
		GameObject.Find ("Canvas/SceneNameText").GetComponent<Text> ().text = (textColor + "设置" + "</color>");
		GameObject.Find ("Canvas/PlayerNameText").GetComponent<Text> ().text = (textColor + playerCName + "</color>");
		GameObject.Find ("Canvas/ChangeNameButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("Canvas/ChangeNameButton/Text").GetComponent<Text> ().text = (textColor + "更改" + "</color>");
		inputField = GameObject.Find ("Canvas/InputField").GetComponent<InputField> ();
		if (theme == "inksnow") {
			inputField.GetComponent<Image> ().color = Color.black;
			inputField.GetComponentInChildren<Text> ().color = Color.white;
			inputField.textComponent.color = Color.white;
		} else {
			inputField.GetComponent<Image> ().color = Color.white;
			inputField.GetComponentInChildren<Text> ().color = Color.black;
			inputField.textComponent.color = Color.black;
		}
		inputField.keyboardType = TouchScreenKeyboardType.Default;
		inputField.gameObject.SetActive (false);

		GameObject.Find ("Canvas/AssembleText").GetComponent<Text> ().text = (textColor + "自动调节" + "</color>");
		GameObject.Find ("Canvas/BackGroundSoundText").GetComponent<Text> ().text = (textColor + "背景音乐" + "</color>");
		GameObject.Find ("Canvas/MoveSoundText").GetComponent<Text> ().text = (textColor + "操作音效" + "</color>");

		// 单选框和滑动条的初始化
		if (theme == "starysky") {
			GameObject.Find ("Canvas/AssembleToggle/Background").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/AssembleToggle/Background/Checkmark").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/BackGroundSoundToggle/Background").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/BackGroundSoundToggle/Background/Checkmark").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/MoveSoundToggle/Background").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/MoveSoundToggle/Background/Checkmark").GetComponent<Image> ().color = Color.black;

			GameObject.Find ("Canvas/AssembleSlider/Background").GetComponent<Image> ().color = Color.gray;
			GameObject.Find ("Canvas/AssembleSlider/Fill Area/Fill").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/AssembleSlider/Handle Slide Area/Handle").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/BackGroundSoundSlider/Background").GetComponent<Image> ().color = Color.gray;
			GameObject.Find ("Canvas/BackGroundSoundSlider/Fill Area/Fill").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/BackGroundSoundSlider/Handle Slide Area/Handle").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/MoveSoundSlider/Background").GetComponent<Image> ().color = Color.gray;
			GameObject.Find ("Canvas/MoveSoundSlider/Fill Area/Fill").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/MoveSoundSlider/Handle Slide Area/Handle").GetComponent<Image> ().color = Color.white;
		} else {
			GameObject.Find ("Canvas/AssembleToggle/Background").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/AssembleToggle/Background/Checkmark").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/BackGroundSoundToggle/Background").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/BackGroundSoundToggle/Background/Checkmark").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/MoveSoundToggle/Background").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/MoveSoundToggle/Background/Checkmark").GetComponent<Image> ().color = Color.white;

			GameObject.Find ("Canvas/AssembleSlider/Background").GetComponent<Image> ().color = Color.gray;
			GameObject.Find ("Canvas/AssembleSlider/Fill Area/Fill").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/AssembleSlider/Handle Slide Area/Handle").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/BackGroundSoundSlider/Background").GetComponent<Image> ().color = Color.gray;
			GameObject.Find ("Canvas/BackGroundSoundSlider/Fill Area/Fill").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/BackGroundSoundSlider/Handle Slide Area/Handle").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/MoveSoundSlider/Background").GetComponent<Image> ().color = Color.gray;
			GameObject.Find ("Canvas/MoveSoundSlider/Fill Area/Fill").GetComponent<Image> ().color = Color.black;
			GameObject.Find ("Canvas/MoveSoundSlider/Handle Slide Area/Handle").GetComponent<Image> ().color = Color.black;
		}
		aToggle = GameObject.Find ("Canvas/AssembleToggle/Background/Checkmark").GetComponent<Image> ();
		bToggle = GameObject.Find ("Canvas/BackGroundSoundToggle/Background/Checkmark").GetComponent<Image> ();
		mToggle = GameObject.Find ("Canvas/MoveSoundToggle/Background/Checkmark").GetComponent<Image> ();
		aToggle.enabled = PlayerPrefs.GetInt ("isAutomaticAdjustment") == 0 ? false : true;
		bToggle.enabled = PlayerPrefs.GetInt ("IsPlayBackGroundSound") == 0 ? false : true;
		mToggle.enabled = PlayerPrefs.GetInt ("IsPlayMoveSound") == 0 ? false : true;
		aSlider = GameObject.Find ("Canvas/AssembleSlider").GetComponent<Slider> ();
		bSlider = GameObject.Find ("Canvas/BackGroundSoundSlider").GetComponent<Slider> ();
		mSlider = GameObject.Find ("Canvas/MoveSoundSlider").GetComponent<Slider> ();
		aSlider.value = PlayerPrefs.GetFloat ("operationError");
		bSlider.value = PlayerPrefs.GetFloat ("PlayBackGroundSound");
		mSlider.value = PlayerPrefs.GetFloat ("PlayMoveSound");
		GameObject.Find ("Canvas/HLine1").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("Canvas/HLine2").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("Canvas/SLine1").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("Canvas/SLine2").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("Canvas/ExitButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("Canvas/ExitButton/Text").GetComponent<Text> ().text = (textColor + "       退 出" + "</color>");
	}
}
