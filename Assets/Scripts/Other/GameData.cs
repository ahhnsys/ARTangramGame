using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
	
//	public bool isUpdateGameData = true;
//	public bool isUpdatePlayerData = true;
	// 全局变量设置
	public Vector3[] tangramPosition = new Vector3 [8];
	public Vector3[,] offset = new Vector3 [8, 4];
	// 第一位数是目标图形编号，第二位是七巧板编号，第三位是特征点
	public Vector3[,,] targetTangram = new Vector3 [10, 8, 6];
	public Quaternion[] rotate = new Quaternion[8];

	// 初始化调用
	void Awake () {
		DontDestroyOnLoad (this.gameObject);

//		PlayerPrefs.DeleteKey ("IsFirstTime");
		bool isFirstTime = PlayerPrefs.HasKey ("IsFirstTime");
//		Debug.Log (isFirstTime);
		if (!isFirstTime) {
			// 系统设置
			PlayerPrefs.SetInt ("IsPlayBackGroundSound", 1);		// 背景音乐及大小
			PlayerPrefs.SetFloat ("PlayBackGroundSound", 0.3f); 
			PlayerPrefs.SetInt ("IsPlayMoveSound", 1);				// 操作音效及大小
			PlayerPrefs.SetFloat ("PlayMoveSound", 0.6f);
			PlayerPrefs.SetInt ("isAutomaticAdjustment", 1);		// 是否自动调整和操作误差灵敏度
			PlayerPrefs.SetFloat ("operationError", 0.3f);

			// 游戏设置和玩家信息，保存中文字符串需要先转码
			string str = "无名";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("PlayerName", str);
			PlayerPrefs.SetInt ("QBNumber", 100);

			// 当前皮肤及七巧板颜色、UI颜色、字体颜色等，默认使用“月夜星空”皮肤
			PlayerPrefs.SetString ("theme", "starysky");
			PlayerPrefs.SetString ("tangramColor", "cf");
			PlayerPrefs.SetString ("UIColor", "ss");
			PlayerPrefs.SetString ("textColor", "<color=#FFFFFFFF>");
			PlayerPrefs.SetString ("textUsableColor", "<color=#00FF2AFF>");

			// 皮肤相关参数
			PlayerPrefs.SetInt ("starysky", 1);					// 是否解锁
			PlayerPrefs.SetString ("10001", "starysky");		// 编号对应名字
			PlayerPrefs.SetInt ("staryskyNumber", 10001);		// 名字对应编号
			str = "月夜星空";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("staryskyCName", str);
			str="静谧的月色和静谧的星空融为一体，听着若隐的虫鸣，感觉整个人都安静下来了。\r\n\r\n获得方式：默认获得";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("staryskyTips", str);

			PlayerPrefs.SetInt ("inksnow", 0);
			PlayerPrefs.SetString ("10002", "inksnow");
			PlayerPrefs.SetInt ("inksnowNumber", 10002);
			str = "墨染白雪";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("inksnowCName", str);
			str="白茫茫的天地之中点点墨痕勾勒，天地寂静，万物无声。\r\n\r\n获得方式：默认获得";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("inksnowTips", str);

			// 游戏主界面参数，七巧板获得时间控制
			PlayerPrefs.SetInt ("TimeInterval", 600);
			PlayerPrefs.SetInt ("SpeedBonus", 0);
			PlayerPrefs.SetInt ("TangramCount1", 5);
			PlayerPrefs.SetInt ("TangramCount2", 5);
			PlayerPrefs.SetInt ("TangramCount3", 5);
			PlayerPrefs.SetInt ("TangramCount4", 5);
			PlayerPrefs.SetInt ("TangramCount5", 5);
			PlayerPrefs.SetInt ("TangramCount6", 5);
			PlayerPrefs.SetInt ("TangramCount7", 5);

			PlayerPrefs.SetFloat ("TimeSpend", 0.0f);
			PlayerPrefs.SetFloat ("TimeChange", 0.0f);
			PlayerPrefs.SetInt ("GameExitTimeSecond", 0);
			PlayerPrefs.SetInt ("GameExitTimeMinute", 0);
			PlayerPrefs.SetInt ("GameExitTimeHour", 0);
			PlayerPrefs.SetInt ("GameExitTimeDay", 0);
		
			// 游戏图鉴模式进度和玩家保存图鉴数量
			PlayerPrefs.SetInt ("AtlasNumber", 1);
			PlayerPrefs.SetInt ("PlayerAtlasNumber", 0);
			// 游戏目标
			str = "正方形";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("Target", "Square");
			PlayerPrefs.SetString ("Square", str);		// 英文名对应中文名
			PlayerPrefs.SetString ("1", "Square");		// 编号对应英文名
			PlayerPrefs.SetInt ("SquareNum", 1);		// 名字对应编号
			PlayerPrefs.SetInt ("SquareMove", 0);
			PlayerPrefs.SetInt ("SquareRolate", 0);
			PlayerPrefs.SetFloat ("SquareTime", 0);
			PlayerPrefs.SetInt ("SquareScore", 1);
			str = "数字1";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("Target", "NumberOne");
			PlayerPrefs.SetString ("NumberOne", str);
			PlayerPrefs.SetString ("2", "NumberOne");
			PlayerPrefs.SetInt ("NumberOneNum", 2);
			PlayerPrefs.SetInt ("NumberOneMove", 0);
			PlayerPrefs.SetInt ("NumberOneRolate", 0);
			PlayerPrefs.SetFloat ("NumberOneTime", 0);
			PlayerPrefs.SetInt ("NumberOneScore", 1);
			str = "数字2";
			str = WWW.EscapeURL (str);
			PlayerPrefs.SetString ("Target", "NumberTwo");
			PlayerPrefs.SetString ("NumberTwo", str);
			PlayerPrefs.SetString ("3", "NumberTwo");
			PlayerPrefs.SetInt ("NumberTwoNum", 3);
			PlayerPrefs.SetInt ("NumberTwoMove", 0);
			PlayerPrefs.SetInt ("NumberTwoRolate", 0);
			PlayerPrefs.SetFloat ("NumberTwoTime", 0);
			PlayerPrefs.SetInt ("NumberTwoScore", 1);

			Debug.Log ("数据导入成功");
		}
	}

	void Start () {

		PlayerPrefs.SetInt ("IsFirstTime", 1);

		// 七巧板初始位置
		Vector3 initialOffset = new Vector3 (-7.88f, -4.38f, 0);
		tangramPosition [0] = new Vector3 (-15f, -15f, 0);
		tangramPosition [1] = new Vector3 (-0.679f, 0.157f, 0) + initialOffset;
		tangramPosition [2] = new Vector3 (0.484f, 0.319f, 0) + initialOffset;
		tangramPosition [3] = new Vector3 (-0.349f, -0.497f, 0) + initialOffset;
		tangramPosition [4] = new Vector3 (-0.009f, -1.169f, 0) + initialOffset;
		tangramPosition [5] = new Vector3 (0.665f, -0.505f, 0) + initialOffset;
		tangramPosition [6] = new Vector3 (-0.752f, -0.752f, 0) + initialOffset;
		tangramPosition [7] = new Vector3 (0f, 0f, 0) + initialOffset;

		// 每个七巧板的顶点偏移量（顶点0~3）,scale=1.5
		offset [1, 0] = new Vector3 (-0.97f, -2f, 0);
		offset [1, 1] = new Vector3 (-0.978f, 0.92f, 0);
		offset [1, 2] = new Vector3 (2.00f, 0.97f, 0);
		offset [2, 0] = new Vector3 (1.483f, 0.487f, 0);
		offset [2, 1] = new Vector3 (0.009f, -0.949f, 0);
		offset [2, 2] = new Vector3 (-1.45f, 0.49f, 0);
		offset [3, 0] = new Vector3 (-0.45f, 1.5f, 0);
		offset [3, 1] = new Vector3 (1.05f, -0.021f, 0);
		offset [3, 2] = new Vector3 (-0.456f, -1.46f, 0);
		offset [4, 0] = new Vector3 (-2.968f, -0.949f, 0);
		offset [4, 1] = new Vector3 (0.009f, 1.985f, 0);
		offset [4, 2] = new Vector3 (2.976f, -0.96f, 0);
		offset [5, 0] = new Vector3 (0.946f, -2.95f, 0);
		offset [5, 1] = new Vector3 (-2.02f, 0.013f, 0);
		offset [5, 2] = new Vector3 (0.95f, 2.959f, 0);
		offset [6, 0] = new Vector3 (-0.724f, -2.135f, 0);
		offset [6, 1] = new Vector3 (-0.729f, 0.743f, 0);
		offset [6, 2] = new Vector3 (0.743f, 2.166f, 0);
		offset [6, 3] = new Vector3 (0.727f, -0.72f, 0);
		offset [7, 0] = new Vector3 (-0.02f, -1.472f, 0);
		offset [7, 1] = new Vector3 (-1.485f, -0.0136f, 0);
		offset [7, 2] = new Vector3 (-0.02f, 1.446f, 0);
		offset [7, 3] = new Vector3 (1.436f, -0.013f, 0);

		// 旋转四元数数组赋值
		for (int i = 0; i < 8; i++) {
			rotate [i] = Quaternion.AngleAxis (i * 45 - 180, new Vector3 (0, 0, 1));
		}

		// 用于匹配的目标图案特征点坐标的三维数组：
		// 第一维是图片编号；
		// 第二维是板块编号，特别的，第一块[0,0]为匹配时的允许误差，且x值为第一阶段误差，y值为第二阶段；
		// 第三维是特征点编号（下标1-3为顶点，4为旋转中心，5的z值为旋转的四元数数组rotate[i]的下标，特别的第6块的x值为翻转指数）
		// 正方形
		targetTangram [1, 0, 0] = new Vector3 (0.7f, 3f, 0);
		targetTangram [1, 1, 4] = new Vector3 (0.35116f, 3.35415f, 0);
		targetTangram [1, 1, 5] = new Vector3 (0, 0, 4);
		targetTangram [1, 2, 4] = new Vector3 (3.80116f, 3.83415f, 0);
		targetTangram [1, 2, 5] = new Vector3 (0, 0, 4);
		targetTangram [1, 3, 4] = new Vector3 (1.26316f, 1.37815f, 0);
		targetTangram [1, 3, 5] = new Vector3 (0, 0, 4);
		targetTangram [1, 4, 4] = new Vector3 (2.30416f, -0.6278f, 0);
		targetTangram [1, 4, 5] = new Vector3 (0, 0, 4);
		targetTangram [1, 5, 4] = new Vector3 (4.33416f, 1.36215f, 0);
		targetTangram [1, 5, 5] = new Vector3 (0, 0, 4);
		targetTangram [1, 6, 4] = new Vector3 (0.11015f, 0.61115f, 0);
		targetTangram [1, 6, 5] = new Vector3 (1, 0, 4);
		targetTangram [1, 7, 4] = new Vector3 (2.32716f, 2.81115f, 0);
		targetTangram [1, 7, 5] = new Vector3 (0, 0, 4);

		// 数字1
		targetTangram [2, 0, 0] = new Vector3 (0.5f, 15f, 0);
		targetTangram [2, 1, 4] = new Vector3 (1.97582f, 5.27786f, 0);
		targetTangram [2, 1, 5] = new Vector3 (0, 0, 2);
		targetTangram [2, 2, 4] = new Vector3 (2.46781f, -1.1261f, 0);
		targetTangram [2, 2, 5] = new Vector3 (0, 0, 2);
		targetTangram [2, 3, 4] = new Vector3 (1.04181f, 3.26587f, 0);
		targetTangram [2, 3, 5] = new Vector3 (0, 0, 0);
		targetTangram [2, 4, 4] = new Vector3 (1.50981f, -3.1201f, 0);
		targetTangram [2, 4, 5] = new Vector3 (0, 0, 4);
		targetTangram [2, 5, 4] = new Vector3 (0.93781f, 0.33687f, 0);
		targetTangram [2, 5, 5] = new Vector3 (0, 0, 0);
		targetTangram [2, 6, 4] = new Vector3 (2.21882f, 2.55786f, 0);
		targetTangram [2, 6, 5] = new Vector3 (-1, 0, 4);
		targetTangram [2, 7, 4] = new Vector3 (-0.0041f, 4.80186f, 0);
		targetTangram [2, 7, 5] = new Vector3 (0, 0, 4);

		// 数字2
		targetTangram [3, 0, 0] = new Vector3 (0.7f, 2.7f, 0);
		targetTangram [3, 1, 4] = new Vector3 (1.90595f, -3.0984f, 0);
		targetTangram [3, 1, 5] = new Vector3 (0, 0, 3);
		targetTangram [3, 2, 4] = new Vector3 (4.67074f, -3.1493f, 0);
		targetTangram [3, 2, 5] = new Vector3 (0, 0, 3);
		targetTangram [3, 3, 4] = new Vector3 (3.24380f, -2.1988f, 0);
		targetTangram [3, 3, 5] = new Vector3 (0, 0, 5);
		targetTangram [3, 4, 4] = new Vector3 (1.2335f, -1.0570f, 0);
		targetTangram [3, 4, 5] = new Vector3 (0, 0, 5);
		targetTangram [3, 5, 4] = new Vector3 (2.60034f, 1.77353f, 0);
		targetTangram [3, 5, 5] = new Vector3 (0, 0, 7);
		targetTangram [3, 6, 4] = new Vector3 (0.50441f, 4.98121f, 0);
		targetTangram [3, 6, 5] = new Vector3 (1, 0, 3);
		targetTangram [3, 7, 4] = new Vector3 (2.58138f, 4.54142f, 0);
		targetTangram [3, 7, 5] = new Vector3 (0, 0, 4);

		// 各顶点坐标
		for (int j = 1; j <= 3; j++) {
			for (int i = 1; i < 8; i++) {
				targetTangram [j, i, 0] = targetTangram [j, i, 4] + rotate [(int)targetTangram [j, i, 5].z] * offset [i, 0];
				targetTangram [j, i, 1] = targetTangram [j, i, 4] + rotate [(int)targetTangram [j, i, 5].z] * offset [i, 1];
				targetTangram [j, i, 2] = targetTangram [j, i, 4] + rotate [(int)targetTangram [j, i, 5].z] * offset [i, 2];
				targetTangram [j, i, 3] = tangramPosition [0];
				if (i > 5)
					targetTangram [j, i, 3] = targetTangram [j, i, 4] + rotate [(int)targetTangram [j, i, 5].z] * offset [i, 3];
			}
		}
	}
}



//		每个七巧板的顶点偏移量（顶点0~3）,scale=1
//		offset [1, 0] = new Vector3 (-0.615f, -1.265f, 0);
//		offset [1, 1] = new Vector3 (-0.623f, 0.671f, 0);
//		offset [1, 2] = new Vector3 (1.321f, 0.671f, 0);
//		offset [2, 0] = new Vector3 (0.999f, 0.331f, 0);
//		offset [2, 1] = new Vector3 (0.023f, -0.619f, 0);
//		offset [2, 2] = new Vector3 (-0.962f, 0.331f, 0);
//		offset [3, 0] = new Vector3 (-0.285f, 0.993f, 0);
//		offset [3, 1] = new Vector3 (0.675f, 0.02f, 0);
//		offset [3, 2] = new Vector3 (-0.285f, -0.958f, 0);
//		offset [4, 0] = new Vector3 (-1.951f, -0.612f, 0);
//		offset [4, 1] = new Vector3 (0.017f, 1.329f, 0);
//		offset [4, 2] = new Vector3 (1.995f, -0.610f, 0);
//		offset [5, 0] = new Vector3 (0.648f, -1.949f, 0);
//		offset [5, 1] = new Vector3 (-1.306f, 0.021f, 0);
//		offset [5, 2] = new Vector3 (0.648f, 1.991f, 0);
//		offset [6, 0] = new Vector3 (-0.466f, -1.409f, 0);
//		offset [6, 1] = new Vector3 (-0.471f, 0.507f, 0);
//		offset [6, 2] = new Vector3 (0.49f, 1.457f, 0);
//		offset [6, 3] = new Vector3 (0.506f, 0.453f, 0);
//		offset [7, 0] = new Vector3 (-0.005f, -0.964f, 0);
//		offset [7, 1] = new Vector3 (-0.98f, 0.006f, 0);
//		offset [7, 2] = new Vector3 (-0.005f, 0.976f, 0);
//		offset [7, 3] = new Vector3 (0.976f, 0.006f, 0);

// 数字1
//		targetTangram [2, 0, 0] = new Vector3 (0.5f, 15f, 0);
//		targetTangram [2, 1, 4] = new Vector3 (2.38931f, 4.98931f, 0);
//		targetTangram [2, 1, 5] = new Vector3 (0, 0, 2);
//		targetTangram [2, 2, 4] = new Vector3 (2.86431f, -1.3386f, 0);
//		targetTangram [2, 2, 5] = new Vector3 (0, 0, 2);
//		targetTangram [2, 3, 4] = new Vector3 (1.43831f, 3.05331f, 0);
//		targetTangram [2, 3, 5] = new Vector3 (0, 0, 0);
//		targetTangram [2, 4, 4] = new Vector3 (1.90631f, -3.3326f, 0);
//		targetTangram [2, 4, 5] = new Vector3 (0, 0, 4);
//		targetTangram [2, 5, 4] = new Vector3 (1.33431f, 0.12431f, 0);
//		targetTangram [2, 5, 5] = new Vector3 (0, 0, 0);
//		targetTangram [2, 6, 4] = new Vector3 (2.63031f, 2.24631f, 0);
//		targetTangram [2, 6, 5] = new Vector3 (0, 0, 4);
//		targetTangram [2, 7, 4] = new Vector3 (0.40831f, 4.54631f, 0);
//		targetTangram [2, 7, 5] = new Vector3 (0, 0, 4);