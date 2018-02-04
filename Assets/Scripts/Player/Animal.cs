﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	public float velocity;
	public float experience;
	public string playerType;
	public string playerName;
	public string uuid;
    public int mass;
    public int force;
	public bool mainPlayer;
    public bool isInGround;
    public string idleName;
    public string walkName;
    public string runName;

    float colliderSize = 0.18f;

    public Rigidbody currentRb;

    private void Awake()
    {
        // initialize animal
        uuid = "19849731-323d-486d-a548-8fd2f8e29811";
        playerType = "PIG";
        playerName = "Unknown";
        experience = 1;
        mass = 1;
        force = 1;
        mainPlayer = false;
        isInGround = false;
        idleName = "idle";
        walkName = "walk";
        runName = "run";
    }

    void Start()
    {
        // transforms
        transform.localScale = new Vector3(2, 2, 2);
        currentRb = gameObject.AddComponent<Rigidbody>();
        currentRb.detectCollisions = true;
        currentRb.freezeRotation = true;
        currentRb.mass = mass;

        foreach (Transform child in GetComponentsInChildren<Transform>(true)) //include inactive
        {
            if (child.gameObject.name.Contains("_"))
            {
                child.gameObject.tag = "Player";
                BoxCollider joint = child.gameObject.AddComponent<BoxCollider>();
                joint.size = new Vector3(colliderSize, colliderSize, colliderSize);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isInGround = true;
            // currentRb.freezeRotation = true;
        }   
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isInGround = false;
            // currentRb.freezeRotation = false;
        }
    }



}
