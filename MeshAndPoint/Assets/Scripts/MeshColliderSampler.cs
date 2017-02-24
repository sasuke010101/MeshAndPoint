using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


#if UNITY_EDITOR
using UnityEditor ;
#endif
public class MeshColliderSampler : MonoBehaviour {

//	[NoToLuaAttribute]
	public List<Transform> areaVertexList = new List<Transform>();
//	[NoToLuaAttribute]
	public bool ShowGizmos = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


//	void OnClick( )
//	{
//		GameDebug.LogError("MeshColliderSampler clicked: "+this.name);
//	}


	public void ReCreatMesh()
	{
		Debug.LogError("CreatMesh "+this.name);
		Creat( ) ;		
	}



//	private UIRoot uiroot = null;
//	UIRoot Root{
//		get {
//			if (uiroot == null) {
//				GameObject root = GameObject.FindGameObjectWithTag("GuiCamera");
//				uiroot =  root.GetComponent<UIRoot>();
//			}
//			return uiroot;
//		}
//	}
	#if UNITY_EDITOR
	void OnDrawGizmos( )
	{
		if ( !ShowGizmos ) return ;
		int count = areaVertexList.Count;
		if(count>0){
			Gizmos.color = Color.yellow ;
			for(int i =0;i<count - 1;i++){
				Gizmos.DrawLine(/*Root.transform.localScale.x * */areaVertexList[i].localPosition,areaVertexList[i+1].localPosition/* * Root.transform.localScale.y*/);
			}
			Gizmos.DrawLine(/*Root.transform.localScale.x * */areaVertexList[count - 1].localPosition,areaVertexList[0].localPosition/* * Root.transform.localScale.y*/);
		}	
	}

	[ContextMenu("CreatMesh")]
	private void CreatMesh()
	{
		Creat( ) ;		
	}

	#endif
	
	void Creat( )
	{
		
		Mesh mesh = new Mesh ( ) ;

		MeshCollider mCol = null;
		int count = areaVertexList.Count;

		Vector3 [] vertices = null;
		if(count>0){
			vertices = new Vector3[2 * count];
			Vector3 vect;			
			for(int i = 0;i<count;i++){
				vect = areaVertexList[i].localPosition;
				vertices[i] = vect;
			}

			for(int i = 0;i<count;i++){
				vertices[count + i] = vertices[i] + new Vector3 ( 0f , 0f , 1f ) * 1f;
			}
		}
		else{
			mCol = gameObject.GetComponent <MeshCollider> ( ) ;
			if ( mCol != null )
			{
				Destroy(mCol);
			}
			return;
		}
		//得到三角形的数量   
		int triangles_count = count - 2;   
		
		//三角形顶点ID数组   

		
		int [] triangles = new int[] {
			2,
			1,
			0,
			1,
			2,
			3,
			6,
			0,
			4,
			6,
			2,
			0,
			6,
			3,
			2,
			6,
			7,
			3,
			7,
			1,
			3,
			7,
			5,
			1,
			5,
			0,
			1,
			5,
			4,
			0,
			
		} ;


//		triangles[9] =3;   
//		triangles[10] =0;   
//		triangles[11] =4;   
		mesh.vertices = vertices ;
		
		mesh.triangles = triangles ;
		
		mCol = gameObject.GetComponent <MeshCollider> ( ) ;
		if ( mCol == null )
		{
			mCol = gameObject.AddComponent <MeshCollider> ( ) ;
		}
		mCol.convex = true ;
		mCol.sharedMesh = mesh;


		#if UNITY_EDITOR
			AssetDatabase.CreateAsset(mesh, "Assets/Mesh/"+this.name+".mesh");
		#endif
	}

	

}
