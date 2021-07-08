using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Invector.vCharacterController
{
    public class BulletHit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {

            if (other.GetComponentInParent<MyAI>() != null)
            {
                other.GetComponentInParent<MyAI>().Damaged(1);
            }

            Destroy(gameObject);
        }
    }
}
