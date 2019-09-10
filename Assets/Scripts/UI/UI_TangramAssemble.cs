using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_TangramAssemble : MonoBehaviour {
	public TangramAutomaticAdjustment TangramAA;
	public SoundPlay soundPlay;

	private Image mask, maskBackGround;
	private Canvas canvasWindow;
	private RectTransform matchingSuccess, matchingFail, canvasAtlas;
	private Text moveCountText, moveText;
	private Text rotateCountText, rotateText;
	private Text timeCountText, timeText;
	private int moveCount = 0;
	private int rotateCount = 0;
	private bool isNewScore = false;
	private Text scoreText, isNewScoreText;
	private Image timeImage;
	private Text tipsText;
	private Image[] tangramImage = new Image[9];
	private Text[] tangramName = new Text[9];
	private int pageCount = 1;
	private Text pageText;
	private Button previousPageButton, nextPageButton;
	Vector3 vEnable = new Vector3 (0, 0, 0);
	private Vector3 vUnEnable = new Vector3 (-1000, -1000, 0);
	private bool isStart = false;
	private int[] tangramCount = new int[8];

	private string theme;
	private string UIColor;
	private string textColor;
	private string textUsableColor;
	private string pathUI;
	private string target;
	private string pathTangram;

	int hour;
	int minute;
	int second;
	int tenthsecond;
	// 已经花费的时间  
	float timeSpend = 0.0f;
	float time = 0.0f; 
	int gameScore;

	// 初始化调用
	void Start () {
		soundPlay = GameObject.Find ("Audio_Source").GetComponent<SoundPlay> ();
		soundPlay.InAssembleScene ();
		// 设置遮罩和弹出窗口的可见性
		mask = GameObject.Find ("Canvas/Mask/mask").GetComponent<Image> ();
		mask.enabled = false;
		canvasWindow = GameObject.Find ("CanvasWindow").GetComponent<Canvas> ();
		canvasWindow.enabled = false;
		matchingSuccess = canvasWindow.transform.Find ("MatchingSuccess").GetComponent<RectTransform> ();
		matchingFail = canvasWindow.transform.Find ("MatchingFail").GetComponent<RectTransform> ();
		canvasAtlas = canvasWindow.transform.Find ("CanvasAtlas").GetComponent<RectTransform> ();
		matchingSuccess.localPosition = vUnEnable;
		matchingFail.localPosition = vUnEnable;
		canvasAtlas.localPosition = vUnEnable;
		TangramAA = GameObject.Find ("Main Camera").GetComponent<TangramAutomaticAdjustment> ();

		InitializeUIColor ();
	}
	
	// 每一帧调用

	void Update () {
		time += Time.deltaTime;
		if (isStart) {
			timeSpend += Time.deltaTime;  
			hour = (int)timeSpend / 3600;
			minute = ((int)timeSpend - hour * 3600) / 60;
			second = (int)timeSpend - hour * 3600 - minute * 60;
			tenthsecond = (int)((timeSpend - (int)timeSpend) * 10);
			if (hour != 0) {
				timeCountText.text = (textColor + "     拼图时间：" + string.Format ("{0:D2}:{1:D2}:{2:D2}.{3:D1}", hour, minute, second, tenthsecond) + "</color>");
				timeText.text = (textColor + string.Format ("{0:D2}:{1:D2}:{2:D2}.{3:D1}", hour, minute, second, tenthsecond) + "</color>");
			} else {
				timeCountText.text = (textColor + "     拼图时间：" + string.Format ("{0:D2}:{1:D2}.{2:D1}", minute, second, tenthsecond) + "</color>");
				timeText.text = (textColor + string.Format  ("{0:D2}:{1:D2}.{2:D1}", minute, second, tenthsecond) + "</color>");
			}
		}
	}

	public void MoveCount (int count) {
		moveCount = count;
		moveCountText.text = (textColor + "     移动次数：" + count + "</color>");
		moveText.text = (textColor + count + "次" + "</color>");
	}
	public void RotateCount (int count) {
		rotateCount = count;
		rotateCountText.text = (textColor + "     旋转次数：" + count + "</color>");
		rotateText.text = (textColor + count + "次" + "</color>");
	}

	// 暂停游戏
	private void PauseGame () {
		isStart = false;
		timeImage.sprite = Resources.Load<Sprite> (pathUI + "_ButtonPlay");
	}
	// 开始游戏
	public void StartGame () {
		isStart = true;
		timeImage.sprite = Resources.Load<Sprite> (pathUI + "_ButtonPause");
	}

	// 计算得分，并判断是否保存游戏数据
	private void SaveGameData () {
		int savedNumber, nowNumber, saveScore;
		int a, b, c;
		if (moveCount < 37)
			a = 37 - moveCount;
		else
			a = 1;
		if (rotateCount < 50)
			b = 50 - rotateCount;
		else
			b = 1;
		if (hour * 3600 + minute * 60 + second < 310)
			c = 310 - (hour * 3600 + minute * 60 + second);
		else
			c = 1;
		gameScore = (int)((a * a * b * b * c) / 675.0f);
//		Debug.Log (a + "," + b + "," + c + "," + "," + gameScore);
		nowNumber = PlayerPrefs.GetInt (target + "Num");
		savedNumber = PlayerPrefs.GetInt ("AtlasNumber");
		// 如果是拼凑旧图案，需要判断是否是最佳记录
		if (nowNumber < savedNumber) {
			saveScore = PlayerPrefs.GetInt (target + "Score");
			if (gameScore > saveScore) {
				isNewScore = true;
				PlayerPrefs.SetInt (target + "Score", gameScore);
				PlayerPrefs.SetInt (target + "Move", moveCount);
				PlayerPrefs.SetInt (target + "Rolate", rotateCount);
				PlayerPrefs.SetFloat (target + "Time", timeSpend);
			}
		} else {
			isNewScore = true;
			DeduckTangram ();
			int num = nowNumber + 1;
			PlayerPrefs.SetInt ("AtlasNumber", num);
			PlayerPrefs.SetInt (target + "Score", gameScore);
			PlayerPrefs.SetInt (target + "Move", moveCount);
			PlayerPrefs.SetInt (target + "Rolate", rotateCount);
			PlayerPrefs.SetFloat (target + "Time", timeSpend);
		}
	}

	private string GameScore(int score){
		if (score >= 800000)
			return "太厉害了：";
		else if (score >= 600000 && score < 800000)
			return "你真棒：";
		else if (score >= 400000 && score < 600000)
			return "不错哟：";
		else
			return "还需要加油哦：";
	}
	// 扣除七巧板
	private void DeduckTangram(){
		tangramCount [1] = PlayerPrefs.GetInt ("TangramCount1");
		tangramCount [2] = PlayerPrefs.GetInt ("TangramCount2");
		tangramCount [3] = PlayerPrefs.GetInt ("TangramCount3");
		tangramCount [4] = PlayerPrefs.GetInt ("TangramCount4");
		tangramCount [5] = PlayerPrefs.GetInt ("TangramCount5");
		tangramCount [6] = PlayerPrefs.GetInt ("TangramCount6");
		tangramCount [7] = PlayerPrefs.GetInt ("TangramCount7");
		for (int i = 1; i <= 7; i++){
			tangramCount [i]--;
			PlayerPrefs.SetInt ("TangramCount" + i.ToString (), tangramCount [i]);
		}
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
			tangramName [j].text = (textUsableColor + targetAtlasCName + "</color>");
			tangramImage [j].sprite = Resources.Load<Sprite> (pathTangram + targetAtlasEName + "_T");
		}
		// 将解锁完的图鉴的下一个标记为可解锁
		if (number < 8) {
			tangramName [number + 1].text = (textUsableColor + "可解锁" + "</color>");
			tangramImage [number + 1].sprite = Resources.Load<Sprite> (pathUI + "_TangramUncertain");
		}
	}

	// 暂停或开始游戏
	public void Button_Pause () {
		if (isStart) {
			PauseGame ();
		} else {
			StartGame ();
		}
	}
		
	// 匹配图片
	public void Button_MatchingImage () {
		float error = 0;
		bool isMatching;
		isNewScore = false;
		isNewScoreText.GetComponent<RectTransform> ().localPosition = vUnEnable;
		isMatching = TangramAA.MatchingTangram (PlayerPrefs.GetInt (target + "Num"));
		error = TangramAA.Get_error ();
		if (isMatching) {
			SaveGameData ();
			if (isNewScore)
				isNewScoreText.GetComponent<RectTransform> ().localPosition = new Vector3 (-176f, -6.2f, 0);
			scoreText.text = textColor + GameScore (gameScore) + gameScore + "</color>";
			PauseGame ();
			GameObject.Find ("Main Camera").GetComponent<TangramActions> ().enabled = false;
			mask.enabled = true;
			canvasWindow.enabled = true;
			matchingSuccess.localPosition = vEnable;
		} else {
			PauseGame ();
			GameObject.Find ("Main Camera").GetComponent<TangramActions> ().enabled = false;
			mask.enabled = true;
			canvasWindow.enabled = true;
			if (error > 10 && minute == 0)
				tipsText.text = (textColor + "刚开始呢，就要放弃了吗？" + "</color>");
			else if (error > 10)
				tipsText.text = (textColor + "再努力一下或许就能成功了呢！" + "</color>");
			else
				tipsText.text = (textColor + "还差一点点就好了呢，加油！" + "</color>");
			matchingFail.localPosition = vEnable;
		}
	}

	public void Button_TangramChange () {
		RefreshAtlas ();
		GameObject.Find ("Main Camera").GetComponent<TangramActions> ().enabled = false;
		maskBackGround.enabled = true;
		canvasWindow.enabled = true;
		canvasAtlas.localPosition = vEnable;
	}

	// 点击选择
	public void Button_TangramSelect () {
		int number = PlayerPrefs.GetInt ("AtlasNumber");
		int numberCount;
		// 获取点击图片的标签，并转换成相应的图鉴编号
		string buttonTag = " ";
		buttonTag = EventSystem.current.currentSelectedGameObject.tag;
		if (buttonTag.Length <= 2) {
			numberCount = Convert.ToInt32 (buttonTag) + ((pageCount - 1) * 8);
			if (numberCount <= number) {
				PlayerPrefs.SetString ("Target", PlayerPrefs.GetString (numberCount.ToString ()));
				SceneManager.LoadScene ("TangramAssemble");
			}
		}
	}

	// 下一页
	public void Button_NextPage ()
	{
		if (pageCount < 10) {
			pageCount++;
			previousPageButton.interactable = true;
			pageText.text = (textColor + pageCount + "</color>");
			RefreshAtlas ();
			if (pageCount == 10)
				nextPageButton.interactable = false;
		}
	}

	// 上一页
	public void Button_PreviousPage () {
		if (pageCount > 1) {
			pageCount--;
			nextPageButton.interactable = true;
			pageText.text = (textColor + pageCount + "</color>");
			RefreshAtlas ();
			if (pageCount == 1)
				previousPageButton.interactable = false;
		}
	}

	// 返回-跳转场景
	public void Button_Exit () {
		time = time + PlayerPrefs.GetFloat ("TimeChange");
		PlayerPrefs.SetFloat ("TimeChange", time);
		soundPlay.outAssembleScene ();
		SceneManager.LoadScene ("MainSence");
	}

	// 再试一次
	public void Button_TryAgain () {
		time = time + PlayerPrefs.GetFloat ("TimeChange");
		PlayerPrefs.SetFloat ("TimeChange", time);
		SceneManager.LoadScene ("TangramAssemble");
	}

	// 继续
	public void Button_TryContinue () {
		GameObject.Find ("Main Camera").GetComponent<TangramActions> ().enabled = true;
		mask.enabled = false;
		matchingFail.localPosition = vUnEnable;
	}

	public void Button_Return () {
		GameObject.Find ("Main Camera").GetComponent<TangramActions> ().enabled = true;
		maskBackGround.enabled = false;
		canvasAtlas.localPosition = vUnEnable;
	}

	void OnApplicationQuit(){
		PlayerPrefs.SetFloat ("TimeChange", 0.0f);
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
		GameObject.Find ("Background/Dividinglineh").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("Background/Dividinglines").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		// Canvas下组件的初始化
		target = PlayerPrefs.GetString ("Target");
		string targetAtlasCName = PlayerPrefs.GetString (target, " ");
		targetAtlasCName = WWW.UnEscapeURL (targetAtlasCName);
		GameObject.Find ("Canvas/Mask/NameBut").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec04");
		GameObject.Find ("Canvas/Mask/NameBut/Text").GetComponent<Text> ().text = textColor + "     " + targetAtlasCName + "</color>";
		GameObject.Find ("Canvas/Mask/TargetImage").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathTangram + target);
		//等同于GameObject.Find ("Canvas/MoveCountBut/Text").GetComponent<Text> ().text = ("     移动次数：0");
		GameObject.Find ("Canvas/Mask/MoveCountBut").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec04");
		moveCountText = GameObject.Find ("Canvas/Mask/MoveCountBut/Text").GetComponent<Text> ();
		moveCountText.text = (textColor + "     移动次数：0" + "</color>");
		GameObject.Find ("Canvas/Mask/RotateCountBut").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec04");
		rotateCountText = GameObject.Find ("Canvas/Mask/RotateCountBut/Text").GetComponent<Text> ();
		rotateCountText.text = (textColor + "     旋转次数：0" + "</color>");
		timeImage = GameObject.Find ("Canvas/Mask/TimeCountBut").GetComponent<Image> ();
		timeImage.sprite = Resources.Load<Sprite> (pathUI + "_ButtonPlay");
		timeCountText = GameObject.Find ("Canvas/Mask/TimeCountBut/Text").GetComponent<Text> ();
		timeCountText.text = (textColor + "     拼图时间：00:00.0" + "</color>");
		GameObject.Find ("Canvas/Mask/MatchingButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonMatching");
		GameObject.Find ("Canvas/Mask/MatchingButton/Text").GetComponent<Text> ().text = (textColor + "       匹配图案" + "</color>");
		GameObject.Find ("Canvas/Mask/ChangeButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonChange02");
		GameObject.Find ("Canvas/Mask/ChangeButton/Text").GetComponent<Text> ().text = (textColor + "       更换图案" + "</color>");
		GameObject.Find ("Canvas/Mask/ExitButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("Canvas/Mask/ExitButton/Text").GetComponent<Text> ().text = (textColor + "       退 出" + "</color>");
		maskBackGround = GameObject.Find ("Canvas/Mask/maskBackGround").GetComponent<Image> ();
		maskBackGround.sprite = Resources.Load<Sprite> (pathUI + "_Background01");
		maskBackGround.enabled = false;
		// CancasWindow下组件的初始化
		GameObject.Find ("CanvasWindow/MatchingSuccess").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_Window01");
		moveText = GameObject.Find ("CanvasWindow/MatchingSuccess/MoveText").GetComponent<Text> ();
		rotateText = GameObject.Find ("CanvasWindow/MatchingSuccess/RotateText").GetComponent<Text> ();
		timeText = GameObject.Find ("CanvasWindow/MatchingSuccess/TimeText").GetComponent<Text> ();
		scoreText = GameObject.Find ("CanvasWindow/MatchingSuccess/ScoreText").GetComponent<Text> ();
		isNewScoreText= GameObject.Find ("CanvasWindow/MatchingSuccess/IsNewScoreText").GetComponent<Text> ();
		isNewScoreText.text = (textColor + "新\r\n纪\r\n录" + "</color>");
		isNewScoreText.GetComponent<RectTransform> ().localPosition = vUnEnable;
		rotateText.text = (textColor + "0次" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingSuccess/MatchingText").GetComponent<Text> ().text = (textColor + "匹配成功" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingSuccess/Text (1)").GetComponent<Text> ().text = (textColor + "移动次数：" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingSuccess/Text (2)").GetComponent<Text> ().text = (textColor + "旋转次数：" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingSuccess/Text (3)").GetComponent<Text> ().text = (textColor + "消耗时间：" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingSuccess/Text (4)").GetComponent<Text> ().text = (textColor + "最终评价：" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingSuccess/TryAgainButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/MatchingSuccess/TryAgainButton/Text").GetComponent<Text> ().text = (textColor + "再次挑战" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingSuccess/ReturnButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/MatchingSuccess/ReturnButton/Text").GetComponent<Text> ().text = (textColor + "退出" + "</color>");

		GameObject.Find ("CanvasWindow/MatchingFail").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_Window01");
		GameObject.Find ("CanvasWindow/MatchingFail/MatchingText").GetComponent<Text> ().text = (textColor + "匹配失败：" + "</color>");
		tipsText = GameObject.Find ("CanvasWindow/MatchingFail/TipsText").GetComponent<Text> ();
		GameObject.Find ("CanvasWindow/MatchingFail/TryAgainButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/MatchingFail/TryAgainButton/Text").GetComponent<Text> ().text = (textColor + "继续拼凑" + "</color>");
		GameObject.Find ("CanvasWindow/MatchingFail/ReturnButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/MatchingFail/ReturnButton/Text").GetComponent<Text> ().text = (textColor + "不拼了" + "</color>");

		// CanvasAtlas下组件的初始化
		for (int i = 1; i <= 8; i++) {
			tangramImage [i] = GameObject.Find ("CanvasWindow/CanvasAtlas/Panel/TangramImage" + i.ToString ()).GetComponent<Image> ();
			tangramName [i] = GameObject.Find ("CanvasWindow/CanvasAtlas/Panel/TangramName" + i.ToString ()).GetComponent<Text> ();
		}
		pageCount = 1;
		RefreshAtlas ();
		previousPageButton = GameObject.Find ("CanvasWindow/CanvasAtlas/PreviousPageButton").GetComponent<Button> ();
		previousPageButton.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		previousPageButton.interactable = false;
		GameObject.Find ("CanvasWindow/CanvasAtlas/PreviousPageButton/Text").GetComponent<Text> ().text = (textColor + "       上一页" + "</color>");
		nextPageButton = GameObject.Find ("CanvasWindow/CanvasAtlas/NextPageButton").GetComponent<Button> ();
		nextPageButton.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("CanvasWindow/CanvasAtlas/NextPageButton/Text").GetComponent<Text> ().text = (textColor + "       下一页" + "</color>");
		pageText = GameObject.Find ("CanvasWindow/CanvasAtlas/PageNowText").GetComponent<Text> ();
		pageText.text = (textColor + "1" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasAtlas/PageAllText").GetComponent<Text> ().text = (textColor + "/10" + "</color>");
		GameObject.Find ("CanvasWindow/CanvasAtlas/ReturnButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("CanvasWindow/CanvasAtlas/ReturnButton/Text").GetComponent<Text> ().text = (textColor + "       返 回" + "</color>");
	}
}
