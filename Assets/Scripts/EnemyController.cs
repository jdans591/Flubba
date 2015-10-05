﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class EnemyController : MonoBehaviour
{
    float jumpHeight = 3f;
    float timeToJumpApex = .4f;
    float moveSpeed = 4;
	float dir = 1;

    float gravity;
    Vector3 velocity;

    private bool canMove;
    private float delay;

    PlayerPhysics controller;
	
    /**Initialisation */
    void Start() {
        //The controller is what handles our movement in the game world
        controller = GetComponent<PlayerPhysics>();

        //Variable setup
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		canMove = false;
		delay = 3;
	}

    /**
	 * Update is called every frame in order to update the player with the user's input and calculate the next movment to be handled by the PlayerPhysics class.
	 * Move() takes a Vector2 argument for the amount to be moved and carries it out on the player object. While an object is colliding with a surface, the controller.collisions field
	 * will provide information to the type of collision which can be used to test for jump validity, wall jumping etc.
	 */
    void Update() {
		//Delay for 3 seconds before the player can move.
		if (!canMove) {
			if (Time.timeSinceLevelLoad > delay) {
				canMove = true;
				
			}
			else {
				return;
			}
		}
        // This if statement check whether the game is paused or not.
        if (Time.timeScale != 0f) {
            //Vertical collision detection. If the player touches the ground or ceiling set vertical velocity to zero.
            if (TouchingGround() || TouchingCeiling()) {
                velocity.y = 0;
            }

			//Initial movement
			if(velocity.x == 0) {
				velocity.x = moveSpeed * dir;
			}

			// Reverse direction if colliding with a wall
			if (TouchingWall()) {
				velocity.x *= -1;
            }

            //Gravity is applied
            velocity.y += gravity * Time.deltaTime;

            //The controller is given a veloity to move the player by
            controller.Move(velocity * Time.deltaTime);
        }
    }

		void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			Debug.Log("Bang");
		}
	}

    //#################
    // Helper funcitons
    //#################

    //Helper function to check if the object is touching the ground.
    bool TouchingGround() {
        return (controller.collisions.below);
    }

    //Helper function to check if the object is touching either the left or the right side walls.
    bool TouchingWall() {
        return (controller.collisions.left || controller.collisions.right);
    }

    //Helper function to check if the object is touching the right hand side wall.
    bool TouchingRightWall() {
        return (controller.collisions.right);
    }

    //Helper function to check if the object is touching the left hand side wall.
    bool TouchingLeftWall() {
        return (controller.collisions.left);
    }

    //Helper function to check if the object is touching the ceiling.
    bool TouchingCeiling() {
        return (controller.collisions.above);
    }
}
