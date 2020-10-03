using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Bone[] bones = new Bone[10];

    public float scoreMultiplier = .25f;
    public float injuryThreshhold = 250;
    public GameObject bloodEffect;

    [SerializeField] private float score;

    private Vector3 previousForce = Vector3.zero;

    private void FixedUpdate()
    {
        for (var i = 0; i < bones.Length; i++)
        {
            var forceDiff = bones[i].joint.currentForce - previousForce;

            var forceDiffMag = forceDiff.magnitude;

            if (forceDiffMag > injuryThreshhold)
            {
                score += forceDiffMag * scoreMultiplier;
                if (bloodEffect != null)
                {
                    var tempBlood = Instantiate(bloodEffect, bones[i].joint.transform.position,
                        bones[i].joint.transform.rotation);
                    Destroy(tempBlood, 1);
                }
            }

            previousForce = bones[i].joint.currentForce;
        }
    }
}

[Serializable]
public struct Bone
{
    public string name;
    public CharacterJoint joint;
}