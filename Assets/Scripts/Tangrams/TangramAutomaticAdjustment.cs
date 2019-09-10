using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangramAutomaticAdjustment : MonoBehaviour {
	public GameData gamedata;

	// 声明所有七巧板特征点（0~3为定点，4为旋转中心）的二维数组
	private Vector3 [,] vt = new Vector3 [8,5];
	// 声明每个七巧板的检测距离（旋转中心到其他七巧板的顶点）和操作误差
	private float [] distance = new float [8];
	private float operationError;
//	private int targetNumber;
	private Vector3[,,] targetPoint;
	private float error = 0;

	private GameObject tangram6;

	// 初始化调用
	void Start () {
		gamedata = GameObject.Find ("Data_Source").GetComponent<GameData> ();
		operationError = PlayerPrefs.GetFloat ("operationError", 0.3f);
//		targetNumber = PlayerPrefs.GetInt (PlayerPrefs.GetString ("Target") + "Num", 1);
		targetPoint = gamedata.targetTangram;
		tangram6 = GameObject.Find ("Tangram_cf_06(Clone)");
		// 初始化所有七巧板的所有特征点为屏幕左下角区域，0没用
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 5; j++)
				vt [i, j] = gamedata.tangramPosition [0];
		}

		// 初始化每个七巧板的检测距离（旋转中心到其他七巧板的顶点）
		distance [0] = 0;
		// Mathf.Sqrt (0.7 * 0.7 + 1.3 * 1.3) + 操作误差;
		distance [1] = 2.1f + operationError;
		distance [2] = 1.4f + operationError;
		distance [3] = 1.4f + operationError;
		distance [4] = 3.0f + operationError;
		distance [5] = 3.0f + operationError;
		distance [6] = 2.1f + operationError;
		distance [7] = 1.4f + operationError;

	}
		
	// 刷新特征点函数的封装
	public void TangramRefreshPoint (GameObject tg){
		int tni = (int)tg.transform.name [12] - 48;
		if (tg.transform.position == gamedata.tangramPosition [tni]) {
			for (int j = 0; j < 4; j++)
				vt [tni, j] = gamedata.tangramPosition [0];
		} else {
//			Vector3 v3 = tg.transform.position;
//			Quaternion rotation = tg.transform.rotation;
			if (tni > 0 && tni < 8) {
				PositionVT (tg);
			}
		}
	}

	// 自动调整七巧板
	public void TangramAutAdj (GameObject tg) {
		bool isAutAdj = false;
		int tni = (int)tg.transform.name [12] - 48;
		for (int i = 1; i < 8; i++) {
			if (i != tni) {
				// 遍历所有点判断两个七巧板是否需要自动调整
				if (PPDistance (vt [tni, 4], vt [i, 4]) < distance [tni] + distance [i]) {
					// 先判断是否需要点-点自动调整
					isAutAdj = PPAutomaticAdjustment (tg, i);
					if (isAutAdj)
						return;
					
					// 判断是否需要点-边自动调整
					isAutAdj = PLAutomaticAdjustment (tg, i);
					if(isAutAdj)
						return;

					// 判断是否需要边-边自动调整
					isAutAdj = PPAutomaticAdjustment (tg, i);
					if(isAutAdj)
						return;
				}
			}
		}
	}
		
	// 选中并操作一个七巧板后，刷新其特征点
	void PositionVT (GameObject tg){
		int tni = (int)tg.transform.name [12] - 48;
		Vector3 v3 = tg.transform.position;
		Quaternion rotation = tg.transform.rotation;
		if (tni > 0) {
			vt [tni, 4] = v3;
			vt [tni, 0] = vt [tni, 4] + rotation * gamedata.offset [tni, 0];
			vt [tni, 1] = vt [tni, 4] + rotation * gamedata.offset [tni, 1];
			vt [tni, 2] = vt [tni, 4] + rotation * gamedata.offset [tni, 2];
			if (tni > 5)
				vt [tni, 3] = vt [tni, 4] + rotation * gamedata.offset [tni, 3];
		}
	}

	// 点点相靠近时的自动调整
	bool PPAutomaticAdjustment (GameObject tg, int i) {
		int tni = (int)tg.transform.name [12] - 48;
		for (int j = 0; j < 4; j++) {
			if (PPDistance (vt [tni, 4], vt [i, j]) < distance [tni]) {
				for (int k = 0; k < 4; k++) {
					// 如果两个点的距离小于操作误差值，则移动七巧板贴合两个点并返回true
					if (PPDistance (vt [tni, k], vt [i, j]) < operationError) {
						Vector3 offsetPP = vt [i, j] - vt [tni, k];
						tg.transform.position = tg.transform.position + offsetPP;
						TangramRefreshPoint (tg);
						return true;
					}
				}
			}
		}
		return false;
	}

	// 边点相靠近时的自动调整
	bool PLAutomaticAdjustment (GameObject tg, int i) {
		int tni = (int)tg.transform.name [12] - 48;
		int pointCount = 0;
		if (i < 6)
			pointCount = 3;
		else
			pointCount = 4;
		for (int j = 0; j < pointCount; j++) {
			for (int k = j + 1; k < pointCount; k++) {
				for (int p = 0; p < 4; p++) {
					// 如果点P到的直线jk的距离小于操作误差值，则判断点是否在线段内
					if (PLDistance (vt [tni, p], vt [i, j], vt [i, k]) < operationError) {
						if (PPPIn (vt [tni, p].x, vt [i, j].x, vt [i, k].x)
						     && PPPIn (vt [tni, p].y, vt [i, j].y, vt [i, k].y)) {
							Vector3 offsetPL = PLVector (vt [tni, p], vt [i, j], vt [i, k]);
							tg.transform.position = tg.transform.position + offsetPL;
							TangramRefreshPoint (tg);
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// 边边相靠近时的自动调整，由于目前的角度，边边调整完全包含于点边调整，故函数不单独列出
	bool LLAutomaticAdjustment (GameObject tg,int i) {
		return true;
	}

	// 匹配图片，有下列两种特殊情况：
	// 1)由于七巧板有两对板块相同（2和3,4和5），所以要考虑板块交换的情况，如下：
	// 目标图案的板块顺序为：1,2,3,4,5,6,7
	// 实际拼图的板块顺序为：1,2,3,4,5,6,7
	//                或：1,3,2,4,5,6,7
	//                或：1,2,3,5,4,6,7
	//                或：1,3,2,5,4,6,7
	// 2)另外由于七巧板7为正方形，具有旋转90度相等性，所以四个顶点的顺时针顺序可能如下:
	// 目标顺序：0,1,2,3；      实际顺序：0,1,2,3; 1,2,3,0; 2,3,0,1; 3,0,1,2
	public bool MatchingTangram (int targetNumber) {
		float error1, error2;
		// 第一步匹配允许误差，根据不同的图案制定不同的误差以提高精确度
		error1 = targetPoint [targetNumber, 0, 0].x;
		// 第二步匹配允许误差
		error2 = targetPoint [targetNumber, 0, 0].y;
		// 板块排序数组MatchingOrder和每种排序的误差之和errorMatching
		int[,] MatchingOrder = new int[4, 8] {
			{ 0, 1, 2, 3, 4, 5, 6, 7 },
			{ 0, 1, 3, 2, 4, 5, 6, 7 },
			{ 0, 1, 2, 3, 5, 4, 6, 7 }, 
			{ 0, 1, 3, 2, 5, 4, 6, 7 }
		};
		float[] errorMatching = new float[4]{ 0, 0, 0, 0 };
		error = 0;
		// 以正方形板块的中心为基准，计算当前图案和目标图案的整体偏移向量offest
		Vector3 offest = targetPoint [targetNumber, 7, 4] - vt [7, 4];
		// 将当前图案根据偏移向量offest平移后得到新的图案的特征点数组vtNew[ ,]
		Vector3[,] vtNew = new Vector3 [8, 5];
		for (int i = 1; i < 8; i++) {
			for (int j = 0; j < 5; j++)
				vtNew [i, j] = vt [i, j] + offest;
		}
		// 第一步：计算vtNew[ ,]和目标图案特征点数组targetPoint [, , ,]的中心误差之和
		for (int k = 0; k < 4; k++) {
			for (int i = 1; i < 8; i++) {
				errorMatching [k] += PPDistance (vtNew [MatchingOrder [k, i], 4], targetPoint [targetNumber, i, 4]);
			}
		}
		error = Min (errorMatching, 4);
		Debug.Log ("一阶段匹配误差为：" + error);
		// 第二步：如果第一步的误差在允许范围内，则计算两幅图案的七巧板定点误差之和是否在允许范围内
		// 计算顶点误差时，要考虑两种特殊情况，并且区分三个顶点和四个顶点的七巧板板块

		if (error < error1) {
			if (tangram6.transform.localScale.x * targetPoint [targetNumber, 6, 5].x >= 0) {
				// 先计算特殊的正方形板块(编号7)的四个顶点误差之和的4个值并存入数组
				// 对4取余可得到4个序列
				float[] error7 = new float[4]{ 0, 0, 0, 0 };
				for (int g = 0; g < 4; g++) {
					for (int h = 0; h < 4; h++)
						error7 [g] += PPDistance (vtNew [7, (h + g) % 4], targetPoint [targetNumber, 7, h]);
				}
				// 再计算其余6个板块的顶点误差，先循环四种排序方式
				for (int k = 0; k < 4; k++) {
					// 6个板块
					for (int i = 1; i < 7; i++) {
						// 4个顶点
						for (int j = 0; j < 3; j++)
							errorMatching [k] += PPDistance (vtNew [MatchingOrder [k, i], j], targetPoint [targetNumber, i, j]);
						if (i > 5)
							errorMatching [k] += PPDistance (vtNew [MatchingOrder [k, i], 3], targetPoint [targetNumber, i, 3]);
					}
				}
				error = Min (errorMatching, 4) + Min (error7, 4);
				if (error < error2) {
					return true;
				} else
					return false;
			} else
				return false;
		} else
			return false;
	}

	public float Get_error () {
		return error;
	}

	// 返回两点距离
	float PPDistance (Vector3 v3a, Vector3 v3b) {
		float dis = Mathf.Sqrt ((v3a.x - v3b.x) * (v3a.x - v3b.x) + (v3a.y - v3b.y) * (v3a.y - v3b.y));
		return dis;
	}

	// 返回点到直线的距离,va为点，vb、vc为直线上的两点,根据直线方程Ax+By+C=0解出
	float PLDistance (Vector3 v3a, Vector3 v3b, Vector3 v3c) {
		float A = v3c.y - v3b.y;
		float B = v3b.x - v3c.x;
		float C = v3c.x * v3b.y - v3b.x * v3c.y;
		float denominator = Mathf.Sqrt (A * A + B * B);
		float dis = Mathf.Abs (((A * v3a.x + B * v3a.y) + C) / denominator);
		return dis;
	}

	// 返回点a到直线bc的距离的方向（即三角形的高线）
	Vector3 PLVector (Vector3 v3a, Vector3 v3b, Vector3 v3c) {
		float a = PPDistance (v3b, v3c);
		float b = PPDistance (v3a, v3c);
		float c = PPDistance (v3a, v3b);
		float t = (a * a + c * c - b * b) / (2 * a * a);
		// Vector3 h = t * (v3c - v3a) + (1 - t) (v3b - v3a);
		Vector3 B = v3c - v3a;
		Vector3 C = v3b - v3a;
		Vector3 h1 = new Vector3 (t * B.x, t * B.y, t * B.z);
		Vector3 h2 = new Vector3 ((1 - t) * C.x, (1 - t) * C.y, (1 - t) * C.z);
		// 由于存在误差，所以不能完全使点边重合
//		float error = 1.0f;
//		Vector3 h = new Vector3 (error * (h1 + h2).x, error * (h1 + h2).y, error * (h1 + h2).z);
		return h1 + h2;
	}

	// 判断a的值是否在b和c之间，结合点到直线bc距离函数来判断点是否靠近线段bc
	bool PPPIn (float a, float b, float c) {
		// 如果b小于c则交换两者的值,让b始终为两者较大值
		if (b < c) {
			b = b + c;
			c = b - c;
			b = b - c;
		}
		// 考虑到线段bc平行于坐标轴的情况，加减误差值
		if (a < b + operationError && a > c - operationError)
			return true;
		else
			return false;
	}

	// 返回数组里面的最小值
	float Min(float []error,int count){
		float min = error [0];
		for (int i = 1; i < count; i++) {
			if (min > error [i])
				min = error [i];
		}
		return min;
	}
}


