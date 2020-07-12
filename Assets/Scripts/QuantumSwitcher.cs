using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumSwitcher : MonoBehaviour
{
    List<Vector3> positions;
    List<Quaternion> rotations;
    int curLocation;

    Camera mainCam;
    PlayerController player;
    Collider colliderBounds;
    bool firstSight;

    public static QuantumSwitcher[] allQuantumObjects;

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

        if (allQuantumObjects == null) allQuantumObjects = FindObjectsOfType<QuantumSwitcher>();

        mainCam = Camera.main;
        player = FindObjectOfType<PlayerController>();
        colliderBounds = GetComponentInChildren<Collider>();
        ActivateLocation(0);
        curLocation = 0;
        firstSight = false;
    }

    void FixedUpdate()
    {
        Physics.Raycast(mainCam.transform.position, colliderBounds.bounds.center - mainCam.transform.position, out var rayHit);
        bool directLOS = rayHit.collider == colliderBounds;
        //Debug.Log(rayHit.collider.name);
        Debug.DrawRay(mainCam.transform.position, (colliderBounds.bounds.center - mainCam.transform.position).normalized * rayHit.distance, directLOS ? Color.green : Color.red);
        if(directLOS)
        {
            bool withinFrustum = GeometryUtility.TestPlanesAABB(player.camPlanes, colliderBounds.bounds);
            if (withinFrustum)
            {
                firstSight = true;
            }
            else if (firstSight)
            {
                SwitchToNewLocation();
            }
        }

    }

    void SwitchToNewLocation()
    {
        var candidates = new List<int>();
        for(int i = 0; i < positions.Count; i++)
        {
            if (i == curLocation) continue;
            ActivateLocation(i);
            bool spotOccupied = false;
            foreach(QuantumSwitcher qs in allQuantumObjects)
                if (qs != null && this != qs && colliderBounds.bounds.Intersects(qs.colliderBounds.bounds))
                    spotOccupied = true;
            if (!spotOccupied && !GeometryUtility.TestPlanesAABB(player.camPlanes, colliderBounds.bounds))
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
