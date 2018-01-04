﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class RaceManager : MonoBehaviour {
	public List<GameObject> players = new List<GameObject>();
	private static string url = "https://api.myjson.com/bins/y73cf";

	public int raceWidth = 25;
	public int yStartPosition;
	public int zStartPosition;

	private static float colliderFactor=0.6f;


	// Use this for initialization
	void Start () {
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}


	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null)
		{
			string jsonData = PlayerController.Read ("start");
			var data = JSON.Parse(jsonData);
			var players = data ["players"];
			foreach(KeyValuePair<string,SimpleJSON.JSONNode> kvp in players) {
				CreatePlayer (kvp);
			}
			PositionPlayers ();
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	private void PositionPlayers() {
		int trackWidth = raceWidth / players.Count;
		int firstPosition = 1;
		int offsetTrack = trackWidth / 2;
		foreach (GameObject player in players) {
			int playerPosition = (firstPosition * trackWidth) - offsetTrack;
			player.transform.position = new Vector3 (playerPosition, player.transform.position.y + yStartPosition, player.transform.position.z + zStartPosition);
			Debug.Log (player.transform.position.x+","+player.transform.position.y+","+player.transform.position.z);
			firstPosition++;
		}
	}

	static void FitToChildren(BoxCollider rootGameObject) {
		bool hasBounds = false;
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

		for (int i = 0; i < rootGameObject.transform.childCount; ++i) {
			Renderer childRenderer = rootGameObject.transform.GetChild(i).GetComponent<Renderer>();
			if (childRenderer != null) {
				if (hasBounds) {
					bounds.Encapsulate(childRenderer.bounds);
				}
				else {
					bounds = childRenderer.bounds;
					hasBounds = true;
				}
			}
		}

		BoxCollider collider = (BoxCollider)rootGameObject.GetComponent<Collider>();
		collider.center = bounds.center - rootGameObject.transform.position;
		collider.size = bounds.size * colliderFactor;

	}

	private void CreatePlayer(KeyValuePair<string,SimpleJSON.JSONNode> kvp) {
		string prefabName = "animals/" + kvp.Value ["type"].Value + "/FBX FILES/" + kvp.Value ["type"].Value;

		GameObject instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;

		players.Add (instance);

		Player player = instance.AddComponent<Player> ();
		player.playerType = kvp.Value ["type"].Value;
		player.uuid = kvp.Value ["uuid"].Value;
		player.mainPlayer = kvp.Value ["current_player"].AsBool;
		player.playerName = kvp.Value ["name"].Value;


		BoxCollider boxCol = instance.AddComponent<BoxCollider>();
		FitToChildren (boxCol);

		Rigidbody currentRb = instance.AddComponent<Rigidbody>();
		currentRb.detectCollisions = true;

		if (player.mainPlayer) {
			instance.AddComponent<PlayerController> ();
		}
	}


}
