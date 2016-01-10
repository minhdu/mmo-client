using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Copyright Mike Hergaarden (M2H)
/// </summary>
public class NetworkCulling : Photon.MonoBehaviour
{
	static NetworkCulling lastNetCullScript;
	
	enum GroupType
	{
		reasonablyClose,
		veryClose
	} 
	
	/*
		 * 
		 * To use this class, attach it to your player object.
		 * In it's OnPhotonSerializeView you need to call SetGroup, see:
		 * ----------------------------------------
		 * void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		 * if (stream.isWriting)
		 * {
		 *    networkCulling.SetGroup(serializePV);	
		 * -----------------------------------------
		 * 
		 * Example, 10 updates
		 * 
		 * Always
		 * Reasonably Close	49 slots more crude:	200-248 = levelwidth / 7  	(115x115)	
		 * 	Very Close			100 slots fine:		100-199 = levelwidth / 10  	(60x60)
		 * 
		 * 	Minimum(Always)	=  	%4		=	+3		3 updates/sec 
		 * Reasonably Close= 	%2		=	+2		5 updates/sec
		 * Very Close 		= 	ALL		=	-		10 updates/sec
		 * 
		 * 0  	Always
		 * 1			Very Close
		 * 2 		Reasonably Close
		 * 3			Very Close
		 * 4 	Always
		 * 5			Very Close
		 * 6 		Reasonably Close
		 * 7			Very Close
		 * 8 	Always
		 * 9			Very Close
	     */
	
	const int DEFAULT_GROUP = 0; //Default group (Minimum/Always updates are sent here)
	
	//Size per blocks: Tweak this for your game
	const int veryCloseBlockSize = 60;  		//Default: 10x10 grid (100 groups: 100-199). This makes a 600 units area
	const int reasonableCloseBlockSize = 115; 	//Default: 7*7 grid (49 groups: 200-249). This makes a 805 units area
	const int recalculateDistance = 10; //If the camera position moves over this threshold, the local groups will be recalculated
	
	//Blocks per row:(matters for max. group assignment
	// only tweak this if you don't want to use groups 100-249)
	const int veryCloseTileCount = 10; 		// 10*10 	= 100 groups -> 100 to 199
	const int reasonableCloseTileCount = 7; // 7*7 		= 49  groups -> 200 to 249

	private int updateNR = 0; //To calculate what we should be sending to
	
	static bool enableFarUpdates = true;
	static bool enableNearUpdates = true;
	
	private Transform trans; //cached transform for performance
	
	
	public enum CullingOption { NoCulling100, NormalCulling50, HeavyCulling10 }
	
	static CullingOption cullingOption;
	
	
	public static void SetCullingOption (CullingOption newCulling)
	{
		cullingOption = newCulling;
		lastNetCullScript = null; //Force recalc
		
	}
	
	void Awake ()
	{
		trans = transform;
	}
		
	void Start(){
		if (photonView.isMine){
			RecalculateGroups();
		}
	}
	
	
	
	#region sending
	/// <summary>
	/// Call this from inside the OnSerializeWrite
	/// </summary>
	/// <returns>The group.</returns>
	/// <param name="view">View.</param>
	public void SetGroup (PhotonView view)
	{
		
		if (updateNR % 4 == 0) {
			//AlwaysUpdate
			view.group = DEFAULT_GROUP;
		} else {
			int posX = (int)(trans.position.x);
			int posZ = (int)(trans.position.z);
			
			if (updateNR % 2 == 0) {
				//Reasoanbly Close / Extra
				view.group = GetGroupFor (posX, posZ, GroupType.reasonablyClose);
			} else {
				//Very Close / Same area
				view.group = GetGroupFor (posX, posZ, GroupType.veryClose);
			}
		}
		
		updateNR++;
	}
	
	int GetGroupFor (int x, int z, GroupType groupType)
	{
		//Further
		int blockSize = reasonableCloseBlockSize;
		int perRow = reasonableCloseTileCount;
		
		//VeryClose
		if (groupType == GroupType.veryClose) {
			blockSize = veryCloseBlockSize;
			perRow = veryCloseTileCount;
		}
		int levelChunkSize = (blockSize * perRow);
		
		if (x < 0)
			x = x + levelChunkSize;
		if (z < 0)
			z = z + levelChunkSize;
		
		return (groupType == GroupType.veryClose ? 100 : 200) //PV GROUPS ASSIGNMENT
			+ ((x / blockSize) % perRow) + perRow * ((z / blockSize) % perRow);
	}
	
	#endregion
	
	#region client, receiving groups
	
	
	
	static List<int> currentListenList = new List<int> ();
	private Vector2 lastPos = Vector2.zero;
	
	Vector2 GetPosition ()
	{
		return new Vector2 (trans.position.x, trans.position.z);
	}
	
	void Update ()
	{	
		if (photonView.isMine) {
			Vector2 xyPos = GetPosition ();
			int dist = (int)Vector2.Distance (lastPos, xyPos);
			
			if (lastNetCullScript != this || Mathf.Abs (dist) > recalculateDistance) {
				lastNetCullScript = this;
				RecalculateGroups ();
				lastPos = xyPos;
			}	   
			
		}
		
	}
	
	void RecalculateGroups ()
	{
		
		Vector2 posVector = GetPosition ();
		int posX = (int)(posVector.x);
		int posZ = (int)(posVector.y);
		
		//Very Close Groups
		int offsetDiff = veryCloseBlockSize / 2; 
		
		List<int> newListenList = new List<int> ();
	
		int group = 0;
		if (enableNearUpdates)
		{
			group = GetGroupFor(posX + offsetDiff, posZ + offsetDiff, GroupType.veryClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
			group = GetGroupFor(posX + offsetDiff, posZ - offsetDiff, GroupType.veryClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
			group = GetGroupFor(posX - offsetDiff, posZ + offsetDiff, GroupType.veryClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
			group = GetGroupFor(posX - offsetDiff, posZ - offsetDiff, GroupType.veryClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
		}
		
		//Reasonably close
		offsetDiff = reasonableCloseBlockSize / 2;
		if (enableFarUpdates)
		{
			group = GetGroupFor(posX + offsetDiff, posZ + offsetDiff, GroupType.reasonablyClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
			group = GetGroupFor(posX + offsetDiff, posZ - offsetDiff, GroupType.reasonablyClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
			group = GetGroupFor(posX - offsetDiff, posZ + offsetDiff, GroupType.reasonablyClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
			group = GetGroupFor(posX - offsetDiff, posZ - offsetDiff, GroupType.reasonablyClose);
			if (!newListenList.Contains(group))
				newListenList.Add(group);
		}	
		
		List<int> removeList = new List<int> ();
		foreach (int g in currentListenList) {
			if (!newListenList.Contains (g)) {
				if (g < 100 || g >= 250) {
					Debug.LogError ("Error culling removing incorect group " + g + ". Expectng 100-249");
				}
				removeList.Add (g);
			}
		}
		
		List<int> enableList = new List<int> ();
		foreach (int b in newListenList) {
			if (!currentListenList.Contains (b)) {
				enableList.Add (b);
			}
		}
		
		#region debug
		/*
		if(enableList.Count>0 || removeList.Count>0){
			string debug = posX+", "+posZ+" ADD: ";
			foreach(int b in enableList){
				debug += " "+b;
			}
			debug += " REMOVE:  ";
			foreach(int b in removeList){
				debug += " "+b;
			}
			Debug.Log (debug);
		}
		*/
		#endregion
		
		PhotonNetwork.SetReceivingEnabled(enableList.ToArray(),removeList.ToArray()  );
		
		currentListenList = newListenList;
	}
	
	
	
	
	#if UNITY_EDITOR
	//NOTE: this will only draw correctly for x and z > 0 (between 0 and CHUCKSIZE, where CHUNKSIZE is 
	void OnDrawGizmos ()
		{
			if(!photonView.isMine)
				return;

				string debugString = "";
				foreach (int b in currentListenList) {
						if (b >= 200) {
								int numb = b - 200;
								Gizmos.color = new Color (0, 0, 1, 0.5f);
				
								int x = (numb % reasonableCloseTileCount) * reasonableCloseBlockSize;
								int z = (numb / reasonableCloseTileCount) * reasonableCloseBlockSize;
								int LEVEL_WIDTH = 0;// reasonableClose * reasonableCloseBlockSize;

								Gizmos.DrawCube (new Vector3 (x + reasonableCloseBlockSize / 2, 0, z + reasonableCloseBlockSize / 2), Vector3.one * (reasonableCloseBlockSize ));
				

						} else 	if (b >= 100 && b < 200) {
								int numb = b - 100;
								Gizmos.color = new Color (1, 0, 0, 0.5f);

								int x = (numb % veryCloseTileCount) * veryCloseBlockSize;
								int z = (numb / veryCloseTileCount) * veryCloseBlockSize;
								int LEVEL_WIDTH = 0;//veryClose * veryCloseBlockSize;

								Gizmos.DrawCube (new Vector3 (x + veryCloseBlockSize / 2, 1, z + veryCloseBlockSize / 2), Vector3.one * (veryCloseBlockSize ));
						}
						debugString += " " + b;
				}
				Gizmos.color = new Color (0, 1, 0, 0.5f);
				Gizmos.DrawCube (new Vector3 (GetPosition ().x, trans.position.y, GetPosition ().y), Vector3.one * 1);

		}
	#endif
	
	
	#endregion client, receiving groups
	
	
	
}
