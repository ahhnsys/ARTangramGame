using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangramActions : MonoBehaviour {
	public TangramAutomaticAdjustment TangramAA;
	public UI_TangramAssemble TangramUI;
	public SoundPlay soundPlay;
	public GameData gamedata;

	private bool isMouseDownL = false;					// 鼠标左键是否按下
	private bool isMouseDownR = false;					// 鼠标右键是否按下
	private bool isSpriteDown = false;					// 是否选中精灵
	private bool isClick = false;						// 是否是有效点击
	private bool isMove = false;						// 是否发生过移动
	private float mouseDownTime, clickTime;				// 鼠标按下时间，鼠标单击间隔时间
	private int moveCount = 0;							// 七巧板组件移动次数
	private int rotateCount = 0;						// 七巧板组件旋转次数
	private string hitName;								// 当前选中七巧板组件名称
	private string hitNameOld = null;					// 上个被选中的七巧组件板名称
	private RaycastHit2D hit;							// 射线
	private Vector3 lastMousePosition = Vector3.zero;
	private Vector3 v3 = Vector3.zero;
	private Vector2 v2 = Vector2.zero;
	// 保存七巧板的引用和名字(tangramgameobject and tangramname,下标0为旋转按钮)
	private GameObject [] tg = new GameObject [8];
	private static string [] tn = new string [8];

	void Awake () {
		Application.targetFrameRate = 48;
	}

	void Start () {
		Screen.SetResolution(1024, 640, false);
		string tangramColor = PlayerPrefs.GetString ("tangramColor", "0");

		TangramAA = GameObject.Find ("Main Camera").GetComponent<TangramAutomaticAdjustment> ();
		if (GameObject.Find ("Canvas").GetComponent<UI_TangramAssemble> () != null) {
			TangramUI = GameObject.Find ("Canvas").GetComponent<UI_TangramAssemble> ();
		}
		soundPlay = GameObject.Find ("Audio_Source").GetComponent<SoundPlay> ();
		gamedata = GameObject.Find ("Data_Source").GetComponent<GameData> ();
		tn [0] = "Tangram_" + tangramColor + "_00";
		tg [0] = Instantiate (Resources.Load ("Prefabs/Tangrams/Tangrams_" + tangramColor + "/" + tn [0])) as GameObject;
		tg [0].transform.position = gamedata.tangramPosition [0];
		tn [0] = tn [0] + "(Clone)";
		// 动态创建七巧板并设置坐标(tangramname and tangramposition)
		for (int i = 1; i < 8; i++) {
			tn [i] = "Tangram_" + tangramColor + "_0" + i.ToString ();
			tg [i] = Instantiate (Resources.Load ("Prefabs/Tangrams/Tangrams_" + tangramColor + "/" + tn [i])) as GameObject;
			tg [i].transform.position = gamedata.tangramPosition [i];
			tn [i] = tn [i] + "(Clone)";
			// 动态添加事件注册脚本
//			tg [i].AddComponent<TangramListener>();
		}
	}

	void Update () {
		
		// 点击鼠标左键取得、移动、旋转、翻转七巧板
		if (Input.GetMouseButtonDown (0)) {
			// 将屏幕坐标转换成世界坐标
			v3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			// 将三维坐标转换成二维坐标
			v2 = new Vector2 (v3.x, v3.y);
			// 发射射线
			hit = Physics2D.Raycast (v2, Vector2.zero);
			if (hit.collider != null && hit.collider.tag.Equals ("Tangram")) {
				TangramUI.StartGame ();
				isMouseDownL = true;
				// 获得碰撞物体名字
				hitName = hit.collider.gameObject.name;
				// 如果点击的是旋转精灵，则翻转6号七巧板
				if (hitName.Equals (tn [0])) {
					Vector3 tempScale = tg [6].transform.localScale;
					tempScale.x *= -1;
					tg [6].transform.localScale = tempScale;
					TangramAA.TangramRefreshPoint (hit.collider.gameObject);
//					tg [6].transform.Rotate (180, 0, 0);
					rotateCount += 2;
					TangramUI.RotateCount (rotateCount);
				} else {
					// 如果点击一个七巧板后直接点击另一个七巧板，则初始化上个七巧板，并渲染当前七巧板
					if (isSpriteDown && hitNameOld != hitName) {
						InitialTangramBack (hitNameOld);
					}
					isSpriteDown = true;
					isClick = true;
					mouseDownTime = Time.time;
					hitNameOld = hitName;
					RenderTangram (hitName);
				}
			} else {
				// 如果点击空白区域，初始化七巧板
				isMouseDownL = false;
				isClick = false;
				if (isSpriteDown) {
					InitialTangramBack (hitName);
					InitialTangramBack (hitNameOld);
					isSpriteDown = false;
					hitNameOld = hitName;
				}
			}
		}

		// 当按下并移动鼠标时同步移动七巧板
		if (isMouseDownL && isSpriteDown) {
			if (lastMousePosition != Vector3.zero) {
				Vector3 offset = Camera.main.ScreenToWorldPoint (Input.mousePosition) - lastMousePosition;
				if (offset != Vector3.zero) {
					isMove = true;
					hit.transform.position = hit.transform.position + offset;
					ScaleTangramChange (hit.collider.gameObject, IsInMakeArea (hit.collider.gameObject));
				}
			}
			lastMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}

		// 单击鼠标右键初始化其他七巧板，若有翻转则弹出翻转精灵
		if (Input.GetMouseButtonDown(1)) {
			v3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			v2 = new Vector2 (v3.x, v3.y);
			hit = Physics2D.Raycast (v2, Vector2.zero);
			if (isMouseDownR) {
				tg [0].transform.position = gamedata.tangramPosition [0];
				isMouseDownR = false;
			}
			if (isSpriteDown && hitNameOld != null) {
				InitialTangramBack (hitNameOld);
				isSpriteDown = false;
			}
			if (hit.collider != null && IsInMakeArea (hit.collider.gameObject) && hit.collider.tag.Equals ("Tangram")) {
				isMouseDownR = true;
				isSpriteDown = true;
				hitName = hit.collider.gameObject.name;
				if (hitName.Equals (tn [6])) {
					RenderTangram (hitName);
					tg [0].transform.position = new Vector3 (hit.transform.position.x, hit.transform.position.y, 0);
					hitNameOld = hitName;
				}
			}
		}

		// 松开鼠标左键时判断七巧版状态并执行相应操作
		if (Input.GetMouseButtonUp (0)) {
			isMouseDownL = false;
			lastMousePosition = Vector3.zero;
			// 判断七巧板的位置，决定是否将其移回，移回后刷新特征点
			if (isSpriteDown && !IsInMakeArea (hit.collider.gameObject)) {
				MoveTangramBack (hit.collider.gameObject);
				TangramAA.TangramRefreshPoint (hit.collider.gameObject);
			}
			// 判断是否移动过，决定移动次数和自动调整
			if (isMove) {
				moveCount++;
				TangramUI.MoveCount (moveCount);
				soundPlay.PlayMoveSound ();
				isMove = false;
				// 如果对七巧板进行过操作，则自动调整七巧板位置
				if ((PlayerPrefs.GetInt ("isAutomaticAdjustment", 1) == 1) && isSpriteDown && IsInMakeArea (hit.collider.gameObject)) {
					TangramAA.TangramRefreshPoint (hit.collider.gameObject);
					TangramAA.TangramAutAdj (hit.collider.gameObject);
				}
			} else {
				// 每完成一次翻转或点击其他地方时初始化旋转精灵坐标
				tg [0].transform.position = gamedata.tangramPosition [0];
				// 如果是有效单击（响应时间内的单击）七巧板则旋转七巧板
				if (isSpriteDown && isClick && IsInMakeArea (hit.collider.gameObject) && !hitName.Equals (tn [0])) {
					clickTime = Time.time - mouseDownTime;
					if (clickTime < 0.3f) {
						hit.transform.Rotate (0, 0, -45);
						rotateCount++;
						TangramUI.RotateCount (rotateCount);
						soundPlay.PlayMoveSound ();
						isClick = false;
						// 旋转后不调整位置，但刷新特征点
						if ((PlayerPrefs.GetInt ("isAutomaticAdjustment", 1) == 1) && isSpriteDown && IsInMakeArea (hit.collider.gameObject)) {
							TangramAA.TangramRefreshPoint (hit.collider.gameObject);
						}
					}
				}
			}

		}

		if (Input.GetMouseButtonUp (1)) {
			lastMousePosition = Vector3.zero;
			isClick = false;
		}
	}

	//初始化七巧板
	void InitialTangramBack (string t) {
		for (int k = 1; k < 8; k++) {
			if (t.Equals (tn [k])) {
				tg [k].GetComponent<SpriteRenderer> ().color = Color.white;
				break;
			}
		}
	}

	//渲染七巧板
	void RenderTangram (string t) {
		for (int k = 1; k < 8; k++) {
			if (t.Equals (tn [k])) {
				tg [k].GetComponent<SpriteRenderer> ().color = Color.gray;
				break;
			}
		}
	}

	// 判断七巧板的位置
	bool IsInMakeArea (GameObject t) {
		if (t.transform.position.x > -5.65f && t.transform.position.y > -3.52f && t.transform.position.x < 10.07f && t.transform.position.y < 6.31f)
			return true;
		else
			return false;
	}
		
	// 移动七巧板回初始坐标并初始化旋转角度
	void MoveTangramBack (GameObject t) {
		for (int k = 1; k < 8; k++) {
			if (t.transform.name.Equals (tn [k])) {
				t.transform.position = gamedata.tangramPosition [k];
				t.transform.rotation = Quaternion.Euler (0, 0, 0);
				break;
			}
		}
	}

	// 改变七巧板大小
	void ScaleTangramChange (GameObject t, bool isInmakeArea) {
		if (isInmakeArea) {
			t.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
		} else {
			t.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
		}
	}
		
}