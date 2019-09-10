using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Skin : MonoBehaviour {

	private Text numberText, skinNameText, tipsText;
	private Image skinImage;
	private Button tureButton, previousNumberButton, nextNumberButton;
	private Text buttonText;
	private string skinName;
	private int skinNumber = 1;

	private string theme;
	private string UIColor;
	private string textColor;
	private string pathUI;
	private string pathSkin;
	float time = 0.0f;

	// 初始化调用
	void Start () {
		


		InitializeUIColor ();
	}
	
	// 每一帧调用

	void Update () {
		time += Time.deltaTime;
	}

	// 界面刷新
	private void Refresh(){
		skinImage.sprite = Resources.Load<Sprite> (pathSkin + skinName);
		string str = WWW.UnEscapeURL (PlayerPrefs.GetString (skinName + "CName"));
		skinNameText.text = (textColor + str + "</color>");
		str = WWW.UnEscapeURL (PlayerPrefs.GetString (skinName + "Tips"));
		tipsText.text = (textColor + str + "</color>");
		numberText.text = (textColor + skinNumber % 10000 + "</color>");
	}

	// 下一个皮肤
	public void Button_NextSkin () {
		tureButton.interactable = true;
		previousNumberButton.interactable = true;
		skinNumber++;
		string str = skinNumber.ToString ();
		skinName = PlayerPrefs.GetString (str);
		Refresh ();
		if (skinNumber == PlayerPrefs.GetInt (theme + "Number")) {
			buttonText.text = (textColor + "当前偏好" + "</color>");
			tureButton.interactable = false;
		}else
			buttonText.text = (textColor + "设为偏好" + "</color>");
		// 如果是最后一个皮肤，按钮禁用
		if (skinNumber == 10002)
			nextNumberButton.interactable = false;
	}

	// 上一个皮肤
	public void Button_PreviousSkin () {
		tureButton.interactable = true;
		nextNumberButton.interactable = true;
		skinNumber--;
		string str = skinNumber.ToString ();
		skinName = PlayerPrefs.GetString (str);
		Refresh ();
		if (skinNumber == PlayerPrefs.GetInt (theme + "Number")) {
			buttonText.text = (textColor + "当前偏好" + "</color>");
			tureButton.interactable = false;
		} else
			buttonText.text = (textColor + "设为偏好" + "</color>");
		// 如果是第一个皮肤，按钮禁用
		if (skinNumber == 10001)
			previousNumberButton.interactable = false;
	}

	// 设为偏好
	public void Button_SetSkin () {
		SetSkin (skinNumber);
		SceneManager.LoadScene ("Skin");
	}

	//退出-跳转场景
	public void Button_Exit () {
		PlayerPrefs.SetFloat ("TimeChange", time);
		SceneManager.LoadScene ("MainSence");
	}

	void InitializeUIColor () {
		theme = PlayerPrefs.GetString ("theme");
		UIColor = PlayerPrefs.GetString ("UIColor");
		textColor = PlayerPrefs.GetString ("textColor");
		pathUI = "Sprites/" + theme + "/UI_" + UIColor + "/UI_" + UIColor;
		pathSkin = "Sprites/Skin/";
		skinNumber = PlayerPrefs.GetInt (theme + "Number");
		skinName = theme;

		GameObject.Find ("Background/Background").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Background01");
		// Canvas下组件的初始化
		GameObject.Find ("Canvas/SceneNameText").GetComponent<Text> ().text = (textColor + "皮肤" + "</color>");
		GameObject.Find ("Canvas/ExitButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("Canvas/ExitButton/Text").GetComponent<Text> ().text = (textColor + "       退 出" + "</color>");

		// CanvasSkin下组件的初始化，初始化为当前选择的皮肤
		nextNumberButton = GameObject.Find ("CanvasWindow/NextNumberButton").GetComponent<Button> ();
		nextNumberButton.GetComponent<Image>().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		previousNumberButton = GameObject.Find ("CanvasWindow/PreviousNumberButton").GetComponent<Button> ();
		previousNumberButton.GetComponent<Image>().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		nextNumberButton.interactable = true;
		previousNumberButton.interactable = true;
		if (skinNumber == 10002)
			nextNumberButton.interactable = false;
		if (skinNumber == 10001)
			previousNumberButton.interactable = false;
		GameObject.Find ("CanvasWindow/HLine1").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("CanvasWindow/HLine2").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("CanvasWindow/SLine1").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("CanvasWindow/SLine2").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		skinImage = GameObject.Find ("CanvasWindow/SkinImage").GetComponent<Image> ();
		skinImage.sprite = Resources.Load<Sprite> (pathSkin + theme);
		string str = WWW.UnEscapeURL (PlayerPrefs.GetString (theme + "CName"));
		skinNameText = GameObject.Find ("CanvasWindow/SkinNameText").GetComponent<Text> ();
		skinNameText.text = (textColor + str + "</color>");
		str = WWW.UnEscapeURL (PlayerPrefs.GetString (theme + "Tips"));
		tipsText = GameObject.Find ("CanvasWindow/TipsText").GetComponent<Text> ();
		tipsText.text = (textColor + str + "</color>");
		numberText = GameObject.Find ("CanvasWindow/NumberNowText").GetComponent<Text> ();
		numberText.text = (textColor + skinNumber % 10000 + "</color>");
		GameObject.Find ("CanvasWindow/NumberAllText").GetComponent<Text> ().text = (textColor + "/2" + "</color>");
		tureButton = GameObject.Find ("CanvasWindow/TureButton").GetComponent<Button> ();
		tureButton.GetComponent<Image>().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		tureButton.interactable = false;
		buttonText = GameObject.Find ("CanvasWindow/TureButton/Text").GetComponent<Text> ();
		buttonText.text = (textColor + "当前偏好" + "</color>");
	}

	private void SetSkin (int number) {
		if (number == 10001) {
			PlayerPrefs.SetString ("theme", "starysky");
			PlayerPrefs.SetString ("tangramColor", "cf");
			PlayerPrefs.SetString ("UIColor", "ss");
			PlayerPrefs.SetString ("textColor", "<color=#FFFFFFFF>");
			PlayerPrefs.SetString ("textUsableColor", "<color=#00FF2AFF>");
		} else if (number == 10002) {
			PlayerPrefs.SetString ("theme", "inksnow");
			PlayerPrefs.SetString ("tangramColor", "cf");
			PlayerPrefs.SetString ("UIColor", "is");
			PlayerPrefs.SetString ("textColor", "<color=#000000FF>");
			PlayerPrefs.SetString ("textUsableColor", "<color=#00FF2AFF>");
		}
	}

}
