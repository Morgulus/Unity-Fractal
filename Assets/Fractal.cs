using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public int maxDepth;
    public float childScale,
                 spawnProbability,
                 maxRotationSpeed;
    public float[] maxTwists;
    private int depth;
    private float rotationSpeed;

    public Mesh[] meshes;
    public Material material;

    private static Vector3[] childrenDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };
    private static Quaternion[] childrenOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f,0f,-90f),
        Quaternion.Euler(0f,0f,90f),
        Quaternion.Euler(90f,0f,0f),
        Quaternion.Euler(-90f,0f,0f),
    };
    private Material[,] materials;
    // Use this for initialization
    
    void Start () {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(maxTwists[Random.Range(0, maxTwists.Length)], -maxTwists[Random.Range(0, maxTwists.Length)]), 0f, 0f);
        if(materials == null)
        {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0,3)];
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
	}
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
    void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 3];
        for (int i = 0; i <= maxDepth; i++)
        {

            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
            materials[i, 2] = new Material(material);
            materials[i, 2].color = Color.Lerp(Color.white, Color.blue, t);
        }
        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
        materials[maxDepth, 2].color = Color.green;
    }
    void Initialize(Fractal parent, int childIndex)
    {

        meshes = parent.meshes;
        materials = parent.materials;
        depth = parent.depth + 1;
        maxDepth = parent.maxDepth;
        transform.parent = parent.transform;
        childScale = parent.childScale;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwists = parent.maxTwists;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childrenDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childrenOrientations[childIndex];
    }
  
    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childrenDirections.Length; i++)
        {
            if (Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("FractalChild").AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }
}
