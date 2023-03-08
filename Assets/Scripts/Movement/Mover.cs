using SKGG.Attributes;
using SKGG.Physics;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Movement
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Attractable), typeof(AttributeContainer))]
    public class Mover : MonoBehaviour
    {
        private AttributeContainer attributeContainer;
        private CharacterAttributes attributes { get => (CharacterAttributes)attributeContainer.attributes; }

        [HideInInspector] public Vector2 targetVelocity;
        public Vector2 relativeVelocity
        {
            get => attractable.WorldToRelativeOffset(rigidbody.velocity);
            set => rigidbody.velocity = attractable.RelativeToWorldOffset(value);
        }

        private new Rigidbody2D rigidbody;
        private Attractable attractable;

        private void Awake()
        {
            attributeContainer = GetComponent<AttributeContainer>();
            rigidbody = GetComponent<Rigidbody2D>();
            attractable = GetComponent<Attractable>();
        }

        void FixedUpdate()
        {
            Vector2 velocityDiff = targetVelocity - relativeVelocity;
            //non-flying characters can't change their y velocity normally
            if (!attributes.canFly)
                velocityDiff.y = 0;

            if (velocityDiff.magnitude < attributes.acceleration * Time.fixedDeltaTime)
            {
                relativeVelocity += velocityDiff;
            }
            else
            {
                relativeVelocity += attributes.acceleration * Time.fixedDeltaTime * velocityDiff.normalized; //apparently this ordering improves performance according to VS.
            }
        }

        [ServerRpc]
        public void Knockback(Vector2 knockback)
        {
            relativeVelocity = knockback;
        }

        [ServerRpc]
        public void Jump(float jumpPower)
        {
            relativeVelocity = new Vector2(relativeVelocity.x, jumpPower);
        }
    }
}
