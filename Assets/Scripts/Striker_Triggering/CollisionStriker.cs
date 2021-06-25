using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RamboGameStudio.CarromBoard
{
    public class CollisionStriker : Triggggger
    {
     /*   public Rigidbody2D StrikerRigidBody2D { get => strikerRigidBody2D; set => strikerRigidBody2D = value; }
        public CircleCollider2D StrikerCircleCollider2D { get => stikerCircleCollider; set => stikerCircleCollider = value; }
        public Image NewStrikerColor { get => newStrikercolor; set => newStrikercolor = value; }
        /// <summary>
        /// Sent each frame where a collider on another object is touching
        /// this object's collider (2D physics only).
        /// </summary>
        /// <param name="other">The Collision2D data associated with this collision.</param>
        void OnCollisionStay2D(Collision2D other)
        {

            Debug.Log(other.gameObject.name);
            Vector2 dir = other.contacts[0].point - (Vector2)transform.position;
            dir = -dir.normalized;
            StrikerRigidBody2D.AddForce(dir);
            NewStrikerColor.color = Color.white;
            // storeAnchorMinX = _rectTransform.anchorMin.x;
        }
        /// <summary>
        /// Sent when a collider on another object stops touching this
        /// object's collider (2D physics only).
        /// </summary>
        /// <param name="other">The Collision2D data associated with this collision.</param>
        void OnCollisionExit2D(Collision2D other)
        {
            StrikerRigidBody2D.velocity = new Vector2(0, 0);
            StrikerCircleCollider2D.isTrigger = true;

        }*/
    }
}

