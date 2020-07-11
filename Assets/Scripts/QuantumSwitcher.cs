using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumSwitcher : MonoBehaviour
{
    List<Vector3> positions;
    List<Quaternion> rotations;
    int curLocation;

    PlayerController player;
    Collider colliderBounds;
    bool firstSight;

    void Start()
    {
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

        //positions.Add(transform.position);
        //rotations.Add(transform.rotation);

        foreach(Transform child in transform)
        {
            if(child.name == "Positions")
            {
                foreach(Transform location in child)
                {
                    positions.Add(location.position);
                    rotations.Add(location.rotation);
                }
                GameObject.Destroy(child.gameObject);
                break;
            }
        }

        player = FindObjectOfType<PlayerController>();
        colliderBounds = GetComponent<Collider>();
        ActivateLocation(0);
        curLocation = 0;
        firstSight = false;
    }

    void FixedUpdate()
    {
        bool isVisible = GeometryUtility.TestPlanesAABB(player.camPlanes, colliderBounds.bounds);
        if(isVisible)
        {
            firstSight = true;
        }
        else if(firstSight)
        {
            SwitchToNewLocation();
        }
    }

    void SwitchToNewLocation()
    {
        var candidates = new List<int>();
        for(int i = 0; i < positions.Count; i++)
        {
            if (i == curLocation) continue;
            ActivateLocation(i);
            if (!GeometryUtility.TestPlanesAABB(player.camPlanes, colliderBounds.bounds))
                candidates.Add(i);
        }
        if (candidates.Count > 0)
        {
            int newLocation = candidates[Random.Range(0, candidates.Count)];
            ActivateLocation(newLocation);
            curLocation = newLocation;
            firstSight = false;
        }
        else
        {
            ActivateLocation(curLocation);
        }
    }

    void ActivateLocation(int index)
    {
        transform.SetPositionAndRotation(positions[index], rotations[index]);
        Physics.SyncTransforms();
    }
}
