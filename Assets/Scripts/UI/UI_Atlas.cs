using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Atlas : MonoBehaviour {

	private Canvas canvasWindow;
	private RectTransform canvasAtlas, canvasTangram, canvasPlayerAtlas;
	private Text sceneText, labelText;
	private Vector3 vEnable = new Vector3 (0, 0, 0);
	private Vector3 vUnEnable = new Vector3 (-1000, -1000, 0);
	private int pageCount = 1;
	private int numberCount = 1;
	private Image[] tangramImage = new Image[9];
	private Text[] tangramName = new Text[9];
	private Image[] tangramPlayerImage = new Image[9];
	private Text[] tangramPlayerName = new Text[9];
	private Text[] gameScore = new Text[4];
	private Text pageText, pageTextP ,numberText;
	private Button previousPageButton1, previousPageButton2, previousNumberButton;
	private Button nextPageButton1, nextPageButton2, nextNumberButton;
	private bool isPlayerAtlas;

	private string theme;
	private string UIColor;
	private string textColor;
	private string textUsableColor;
	private string pathUI;
	private string pathTangram;
	float time = 0.0f;


	// 初始化调用
	void Start () {
		isPlayerAtlas = false;
		// 设置画布的可见性
		canvasWindow = GameObject.Find ("CanvasWindow").GetComponent<Canvas> ();
		canvasAtlas = canvasWindow.transform.Find ("CanvasAtlas").GetComponent<RectTransform> ();
		canvasTangram = canvasWindow.transform.Find ("CanvasTangram").GetComponent<RectTransform> ();
		canvasPlayerAtlas = canvasWindow.transform.Find ("CanvasPlayerAtlas").GetComponent<RectTransform> ();
		canvasAtlas.localPosition = vEnable;
		canvasTangram.localPosition = vUnEnable;
		canvasPlayerAtlas.localPosition = vUnEnable;

		InitializeUIColor ();
	}

	// 每一帧调用

	void Update () {
		time += Time.deltaTime;
	}
	// 系统图鉴刷新
	private void RefreshAtlas () {
		int number = PlayerPrefs.GetInt ("AtlasNumber") - 1;
		string targetAtlasEName;
		string targetAtlasCName;
		for (int i = 1; i <= 8; i++) {
			tangramName [i].text = (textColor + "未解锁" + "</color>");
			tangramImage [i].sprite = Resources.Load<Sprite> (pathUI + "_TangramUncertain");
		}
		number -= (8 * (pageCount - 1));
		if (number >= 8)
			number = 8;
		for (int j = 1; j <= number; j++) {
			targetAtlasEName = PlayerPrefs.GetString ((j + (pageCount - 1) * 8).ToString ());
			targetAtlasCName = PlayerPrefs.GetString (targetAtlasEName);
			targetAtlasCName = WWW.UnEscapeURL (targetAtlasCName);
			tangramName [j].text = (textColor + targetAtlasCName + "</color>");
			tangramImage [j].sprite = Resources.Load<Sprite> (pathTangram + targetAtlasEName + "_T");
		}
	}

	// 自定义图鉴刷新
	private void RefreshPlayerAtlas () {
		int number = PlayerPrefs.GetInt ("PlayerAtlasNumber");
		string targetAtlasEName;
		string targetAtlasCName;
		for (int i = 1; i <= 8; i++) {
			tangramPlayerName [i].text = (textColor + "未保存" + "</color>");
			tangramPlayerImage [i].sprite = Resources.Load<Sprite> (pathUI + "_TangramUncertain");
		}
		number -= (8 * (pageCount - 1));
		if (number >= 8)
			number = 8;
		if (number != 0) {
			for (int j = 1; j <= number; j++) {
				targetAtlasEName = PlayerPrefs.GetString ((j + (pageCount - 1) * 8).ToString ());
				targetAtlasCName = PlayerPrefs.GetString (targetAtlasEName);
				targetAtlasCName = WWW.UnEscapeURL (targetAtlasCName);
				tangramPlayerName [j].text = (textUsableColor + targetAtlasCName + "</color>");
				tangramPlayerImage [j].sprite = Resources.Load<Sprite> (pathTangram + targetAtlasEName + "_T");
			}
		}
	}

	// 下一页
	public void Button_NextPage () {
		if (pageCount < 10) {
			pageCount++;
			if (!isPlayerAtlas) {
				previousPageButton1.interactable = true;
				pageText.text = (textColor + pageCount + "</color>");
				RefreshAtlas ();
				if (pageCount == 10)
					nextPageButton1.interactable = false;
			} else {
				previousPageButton2.interactable = true;
				pageTextP.text = (textColor + pageCount + "</color>");
				RefreshPlayerAtlas ();
				if (pageCount == 10)
					nextPageButton2.interactable = false;
			}
		}
	}

	// 上一页
	public void Button_PreviousPage () {
		if (pageCount > 1) {
			pageCount--;
			if (!isPlayerAtlas) {
				nextPageButton1.interactable = true;
				pageText.text = (textColor + pageCount + "</color>");
				 RefreshAtlas ();
				if (pageCount == 1)
					previousPageButton1.interactable = false;
			} else {
				nextPageButton2.interactable = true;
				pageTextP.text = (textColor + pageCount + "</color>");
				RefreshPlayerAtlas ();
				if (pageCount == 1)
					previousPageButton2.interactable = false;
			}
		}
	}

	// 切换图鉴
	public void Button_ChangeAtlas () {
		pageCount = 1;
		if (!isPlayerAtlas) {
			canvasAtlas.localPosition = vUnEnable;
			canvasPlayerAtlas.localPosition = vEnable;
			canvasTangram.localPosition = vUnEnable;
			sceneText.text = (textColor + "玩家图鉴浏览" + "</color>");
			labelText.text = (textColor + "       系统图鉴" + "</color>");
			isPlayerAtlas = true;
		} else {
			canvasPlayerAtlas.localPosition = vUnEnable;
			canvasAtlas.localPosition = vEnable;
			canvasTangram.localPosition = vUnEnable;
			sceneText.text = (textColor + "图鉴浏览" + "</color>");
			labelText.text = (textColor + "       玩家图鉴" + "</color>");
			isPlayerAtlas = false;
		}
	}

	// 查看详情
	public void Button_TangramTure () {
		int number = PlayerPrefs.GetInt ("AtlasNumber");
		string targetAtlasEName, targetAtlasCName;
		int score;
		int hour, minute, second, tenthsecond;
		float timeSpend = 0.0f;
		nextNumberButton.interactable = true;
		previousNumberButton.interactable = true;
		// 获取点击图片的标签，并转换成相应的图鉴编号
		string buttonTag = " ";
		buttonTag = EventSystem.current.currentSelectedGameObject.tag;
		if (buttonTag.Length <= 2) {
			numberCount = Convert.ToInt32 (buttonTag) + ((pageCount - 1) * 8);
			numberText.text = (textColor + numberCount + "</color>");
			if (numberCount < number) {
				canvasAtlas.localPosition = vUnEnable;
				canvasTangram.localPosition = vEnable;
				targetAtlasEName = PlayerPrefs.GetString (numberCount.ToString ());
				targetAtlasCName = PlayerPrefs.GetString (targetAtlasEName);
				targetAtlasCName = WWW.UnEscapeURL (targetAtlasCName);
				tangramName [0].text = (textColor + targetAtlasCName + "</color>");
				tangramImage [0].sprite = Resources.Load<Sprite> (pathTangram + targetAtlasEName + "_T");
				gameScore [0].text = (textColor + PlayerPrefs.GetInt (targetAtlasEName + "Move") + "</color>");
				gameScore [1].text = (textColor + PlayerPrefs.GetInt (targetAtlasEName + "Rolate") + "</color>");

				timeSpend = PlayerPrefs.GetFloat (targetAtlasEName + "Time");
				hour = (int)timeSpend / 3600;  
				minute = ((int)timeSpend - hour * 3600) / 60;  
				second = (int)timeSpend - hour * 3600 - minute * 60;  
				tenthsecond = (int)((timeSpend - (int)timeSpend) * 10);  
				if (hour != 0) {
					gameScore [2].text = (textColor + string.Format ("{0:D2}:{1:D2}:{2:D2}.{3:D1}", hour, minute, second, tenthsecond) + "</color>");
				} else {
					gameScore [2].text = (textColor + string.Format ("{0:D2}:{1:D2}.{2:D1}", minute, second, tenthsecond) + "</color>");
				}
				score = PlayerPrefs.GetInt (targetAtlasEName + "Score");
				if (score >= 800000)
					gameScore [3].text = (textColor + "A：" + score + "</color>");
				else if (score >= 600000 && score < 800000)
					gameScore [3].text = (textColor + "B：" + score + "</color>");
				else if (score >= 400000 && score < 600000)
					gameScore [3].text = (textColor + "C：" + score + "</color>");
				else
					gameScore [3].text = (textColor + "D：" + score + "</color>");
				if (numberCount == number - 1)
					nextNumberButton.interactable = false;
				if (numberCount == 1)
					previousNumberButton.interactable = false;
			}
		}
	}

	// 下一个详情
	public void Button_NextNumber () {
		int number = PlayerPrefs.GetInt ("AtlasNumber");
		if (numberCount < number - 1) {
			numberCount++;
			Button_TangramTure ();
		}
	}

	// 上一个详情
	public void Button_PreviousNumber () {
		int number = PlayerPrefs.GetInt ("AtlasNumber");
		if (numberCount > 1 && number > 1) {
			numberCount--;
			Button_TangramTure ();
		}
	}

	// 返回
	public void Button_Return () {
		if (canvasAtlas.localPosition == vEnable || canvasPlayerAtlas.localPosition == vEnable) {
			time += PlayerPrefs.GetFloat ("TimeChange");
			PlayerPrefs.SetFloat ("TimeChange", time);
			SceneManager.LoadScene ("MainSence");
		} else {
			canvasAtlas.localPosition = vEnable;
			canvasTangram.localPosition = vUnEnable;
		}
	}

	//退出-跳转场景
	public void Button_Exit () {
		time += PlayerPrefs.GetFloat ("TimeChange");
		PlayerPrefs.SetFloat ("TimeChange", time);
		SceneManager.LoadScene ("MainSence");
	}

	void OnApplicationQuit(){
		float time = Time.deltaTime;
		PlayerPrefs.SetFloat ("TimeChange", time);
		System.DateTime gameExitTime = System.DateTime.Now;
		PlayerPrefs.SetInt ("GamgExitTimeDay", (int)gameExitTime.DayOfYear);
		PlayerPrefs.SetInt ("GamgExitTimeHour", (int)gameExitTime.Hour);
		PlayerPrefs.SetInt ("GamgExitTimeMinute", (int)gameExitTime.Minute);
		PlayerPrefs.SetInt ("GamgExitTimeSecond", (int)gameExitTime.Second);
		Application.Quit ();
	}

	void InitializeUIColor () {
		theme = PlayerPrefs.GetString ("theme");
		UIColor = PlayerPrefs.GetString ("UIColor");
		textColor = PlayerPrefs.GetString ("textColor");
		textUsableColor = PlayerPrefs.GetString ("textUsableColor");
		pathUI = "Sprites/" + theme + "/UI_" + UIColor + "/UI_" + UIColor;
		pathTangram = "Sprites/" + theme + "/Tangrams_" + UIColor + "/";

		GameObject.Find ("Background/Background").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Background01");
		// Canvas下组件的初始化
		sceneText = GameObject.Find ("Canvas/SceneName").GetComponent<Text> ();
		sceneText.text = (textColor + "图鉴浏览" + "</color>");
		GameObject.Find ("Canvas/ExitButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("Canvas/ExitButton/Text").GetComponent<Text> ().text = (textColor + "       退 出" + "</color>");
		GameObject.Find ("Canvas/ChangeButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonChange02");
		labelText = GameObject.Find ("Canvas/ChangeButton/Text").GetComponent<Text> ();
		labelText.text = (textColor + "       自定义图鉴" + "</color>");

		// CanvasAtlas下组件的初始化
		for (int i = 1; i <= 8; i++) {
			tangramImage [i] = GameObject.Find ("CanvasWindow/CanvasAtlas/Panel/TangramImage" + i.ToString ()).GetComponent<Image> ();
			tangramName [i] = GameObject.Find ("CanvasWindow/CanvasAtlas/Panel/TangramName" + i.ToString ()).GetComponent<Text> ();
		}
		pageCount = 1;
		RefreshAtlas ();
		previousPageButton1 = GameObject.Find ("CanvasWindow/CanvasAtlas/PreviousPageButton").GetComponent<Button> ();
		previousPageButton1.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		previousPageButton1.interactable = false;
		GameObject.Find ("CanvasWindow/CanvasAtlas/PreviousPageButton/Text").GetComponent<Text> ().text = (textColor + "       上一页" + "</color>");
		nextPageButton1 = GameObject.Find ("CanvasWindow/CanvasAtlas/NextPageButton").GetComponent<Button> ();
		nextPageButton1.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("CanvasWindow/CanvasAtlas/NextPageButton/Text").GetComponent<Text> ().text = (textColor + "       下一页" + "</color>");
		pageText = GameObject.Find ("CanvasWindow/CanvasAtlas/PageNowText").GetComponent<Text> ();
		pageText.text = (textColor + "1" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasAtlas/PageAllText").GetComponent<Text> ().text = (textColor + "/10" + "</color>");

		// CanvasTangram下组件的初始化
		nextNumberButton = GameObject.Find ("CanvasWindow/CanvasTangram/NextNumberButton").GetComponent<Button> ();
		nextNumberButton.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		nextNumberButton.interactable = false;
		previousNumberButton = GameObject.Find ("CanvasWindow/CanvasTangram/PreviousNumberButton").GetComponent<Button> ();
		previousNumberButton.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		previousNumberButton.interactable = false;
		GameObject.Find ("CanvasWindow/CanvasTangram/HLine1").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("CanvasWindow/CanvasTangram/HLine2").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("CanvasWindow/CanvasTangram/SLine1").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("CanvasWindow/CanvasTangram/SLine2").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		tangramName [0] = GameObject.Find ("CanvasWindow/CanvasTangram/TangramName").GetComponent<Text> ();
		tangramImage[0] = GameObject.Find ("CanvasWindow/CanvasTangram/TangramImage").GetComponent<Image> ();
		GameObject.Find ("CanvasWindow/CanvasTangram/Text1").GetComponent<Text> ().text = (textColor + "最佳记录" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasTangram/Text2").GetComponent<Text> ().text = (textColor + "移动次数：" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasTangram/Text3").GetComponent<Text> ().text = (textColor + "旋转次数：" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasTangram/Text4").GetComponent<Text> ().text = (textColor + "消耗时间：" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasTangram/Text5").GetComponent<Text> ().text = (textColor + "最佳评价：" + "</color>");
		gameScore [0] = GameObject.Find ("CanvasWindow/CanvasTangram/MoveText").GetComponent<Text> ();
		gameScore [1] = GameObject.Find ("CanvasWindow/CanvasTangram/RotateText").GetComponent<Text> ();
		gameScore [2] = GameObject.Find ("CanvasWindow/CanvasTangram/TimeText").GetComponent<Text> ();
		gameScore [3] = GameObject.Find ("CanvasWindow/CanvasTangram/ScoreText").GetComponent<Text> ();
		GameObject.Find ("CanvasWindow/CanvasTangram/NumberNowText").GetComponent<Text> ().text = (textColor + "/80" + "</color>");
		numberText = GameObject.Find ("CanvasWindow/CanvasTangram/NumberNowText").GetComponent<Text> ();
		numberText.text = (textColor + "1" + "</color>");

		// CanvasPlayerAtlas下组件的初始化
		for (int k = 1; k <= 8; k++) {
			tangramPlayerImage [k] = GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/Panel/TangramImage" + k.ToString ()).GetComponent<Image> ();
			tangramPlayerName [k] = GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/Panel/TangramName" + k.ToString ()).GetComponent<Text> ();
		}
		RefreshPlayerAtlas ();
		previousPageButton2 = GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/PreviousPageButton").GetComponent <Button> ();
		previousPageButton2.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/PreviousPageButton/Text").GetComponent<Text> ().text = (textColor + "       上一页" + "</color>");
		previousPageButton2.interactable = false;
		nextPageButton2 = GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/NextPageButton").GetComponent <Button> ();
		nextPageButton2.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/NextPageButton/Text").GetComponent<Text> ().text = (textColor + "       下一页" + "</color>");
		pageTextP = GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/PageNowText").GetComponent<Text> ();
		pageTextP.text = (textColor + "1" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasPlayerAtlas/PageAllText").GetComponent<Text> ().text = (textColor + "/10" + "</color>");
	}
}