using UnityEngine;
using System.Collections ;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor ;
#endif
public class PointSampler : MonoBehaviour {

	public List<Transform> areaVertexList = new List<Transform>();
	private List<Vector3> vertexList = new List<Vector3>();

	public List<Vector3> pointList = new List<Vector3>();

//	private UIRoot uiroot = null;
	
	public int SamplerCount = 0;
//	[NoToLuaAttribute]
	public bool ShowPoint = false;

//	[NoToLuaAttribute]
	public bool worldSpace = true;
	// Use this for initialization
	void Start () {
	}


	public Vector3 GetPointByIndex(int index){
		if(SamplerCount>index)
			return pointList[index];
		return Vector3.zero;
	}
//	UIRoot Root{
//		get {
//			if (uiroot == null) {
//				GameObject root = GameObject.FindGameObjectWithTag("GuiCamera");
//				uiroot =  root.GetComponent<UIRoot>();
//			}
//			return uiroot;
//		}
//	}
	// Update is called once per frame
	void Update () {
	
	}
	public static bool IsPtInPoly(float ALon, float ALat, List<Vector3> APoints)
	{
		int iSum = 0, iCount;
		float dLon1, dLon2, dLat1, dLat2, dLon;
		if (APoints.Count < 3)
			return false;
		iCount = APoints.Count;
		for (int i = 0; i < iCount; i++)
		{
			if (i == iCount - 1)
			{
				dLon1 = APoints[i].x;
				dLat1 = APoints[i].y;
				dLon2 = APoints[0].x;
				dLat2 = APoints[0].y;
			}
			else
			{
				dLon1 = APoints[i].x;
				dLat1 = APoints[i].y;
				dLon2 = APoints[i + 1].x;
				dLat2 = APoints[i + 1].y;
			}
			//以下语句判断A点是否在边的两端点的水平平行线之间，在则可能有交点，开始判断交点是否在左射线上
			if (((ALat >= dLat1) && (ALat < dLat2)) || ((ALat >= dLat2) && (ALat < dLat1)))
			{
				if (Mathf.Abs(dLat1 - dLat2) > 0)
				{
					//得到 A点向左射线与边的交点的x坐标：
					dLon = dLon1 - ((dLon1 - dLon2) * (dLat1 - ALat)) / (dLat1 - dLat2);
					
					// 如果交点在A点左侧（说明是做射线与 边的交点），则射线与边的全部交点数加一：
					if (dLon < ALon)
						iSum++;
				}
			}
		}

		if (iSum % 2 != 0)
			return true;
		return false;
	}
	#if UNITY_EDITOR
	void OnDrawGizmos( )
	{
		int count = areaVertexList.Count;
		if(count>0){
			Gizmos.color = Color.red ;
			for(int i =0;i<count - 1;i++){
				Gizmos.DrawLine(/*Root.transform.localScale.x * */areaVertexList[i].localPosition,areaVertexList[i+1].localPosition/* * Root.transform.localScale.y*/);
			}
			Gizmos.DrawLine(/*Root.transform.localScale.x * */areaVertexList[count - 1].localPosition,areaVertexList[0].localPosition /** Root.transform.localScale.y*/);
		}		
		if(ShowPoint){
			count = pointList.Count;

			if(count>0){
				Gizmos.color = Color.blue ;
				for(int i =0;i<count;i++){
					Gizmos.DrawSphere(new Vector3( pointList[i].x,  pointList[i].y,0), /*Root.transform.localScale.x**/0.03f);
				}
			}	
		}
	}
	
	private float minX = 0f;
	private float maxX = 0f;

	private float minY = 0f;
	private float maxY = 0f;
	[ContextMenu("Sample")]
	private void Sample()
	{
		this.vertexList = new List<Vector3>();
		this.pointList = new List<Vector3>();
		int count = areaVertexList.Count;
		if(count>0){
			Vector3 vect;

			vect = worldSpace?areaVertexList[0].position:areaVertexList[0].localPosition;
			vertexList.Add(vect);
			minX = vect.x;
			maxX = minX;
			minY = vect.y;
			maxY = minY;
			for(int i = 1;i<count;i++){
				vect = worldSpace?areaVertexList[i].position:areaVertexList[i].localPosition;
				vertexList.Add(vect);
				if(vect.x<minX){
					minX = vect.x;
				}
				if(vect.x>maxX){
					maxX = vect.x;
				}
				if(vect.y<minY){
					minY = vect.y;
				}
				if(vect.y>maxY){
					maxY = vect.y;
				}
			}
			Debug.LogError("bbbb"+minX+"_"+maxX+"_"+minY+"_"+maxY);		
			CreatePoints();
		}

	}
	private Vector3 GenarateByRandom(){
		Vector3 point = Vector3.zero;
		Random.seed += Time.frameCount ;
		point.x = Random.Range(minX, maxX) ;
		Random.seed += Time.frameCount;
		point.y = Random.Range(minY, maxY) ;
		//GameDebug.LogError("point "+point);	
		return point;
	}

	private void CreatePoints()
	{
		int count = 0;
		Vector3 point = GenarateByRandom();
		while (count<SamplerCount)
		{
			if(IsPtInPoly(point.x, point.y, vertexList)){
					
				count++;
				pointList.Add(point);
						
			}
			point = GenarateByRandom();
		}

	}
	#endif
}	