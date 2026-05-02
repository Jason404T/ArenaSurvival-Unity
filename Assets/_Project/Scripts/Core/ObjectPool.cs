using System.Collections.Generic;
using UnityEngine;

// This script manages a reusable pool of GameObjects.
// Instead of creating and destroying objects repeatedly,
// it keeps inactive objects ready to be reused.
public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]

    // Prefab that this pool will create and reuse.
    [SerializeField] private GameObject prefab;

    // Number of objects created at the start.
    [SerializeField] private int initialSize = 30;

    // If true, the pool can create extra objects when all are already in use.
    [SerializeField] private bool canExpand = true;

    // List that stores all pooled objects.
    private readonly List<GameObject> pooledObjects = new List<GameObject>();

    private void Awake()
    {
        // Create the initial pool when the scene starts.
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    public GameObject GetObject()
    {
        // Search for an inactive object in the pool.
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }

        // If all objects are active and expansion is allowed,
        // create a new object and return it.
        if (canExpand)
        {
            return CreateNewObject();
        }

        // If the pool cannot expand and no object is available,
        // return null.
        return null;
    }

    private GameObject CreateNewObject()
    {
        // Safety check to avoid creating null objects.
        if (prefab == null)
        {
            Debug.LogWarning("ObjectPool is missing prefab reference.");
            return null;
        }

        // Create the object as a child of this pool object.
        GameObject newObject = Instantiate(prefab, transform);

        // Disable it immediately so it waits in the pool.
        newObject.SetActive(false);

        // Store it in the pool list.
        pooledObjects.Add(newObject);

        return newObject;
    }
}