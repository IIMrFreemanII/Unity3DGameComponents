using UnityEngine;
using UnityEngine.Serialization;

public class HandleHit : MonoBehaviour
{
    [FormerlySerializedAs("hitPosition")] public Vector4[] hitPositions;
    private float[] initialHitSize;
    
    public MeshRenderer meshRenderer;
    public int lastIndexPos = 0;
    public float increaseSpeed = 10f;

    public float increaseMultiplier = 5f;

    private static readonly int HitPositions = Shader.PropertyToID("hitPositions");
    private static readonly int InitialHitSize = Shader.PropertyToID("initialHitSize");
    private static readonly int ArrayLength = Shader.PropertyToID("_ArrayLength");
    private static readonly int IncreaseMultiplier = Shader.PropertyToID("_IncreaseMultiplier");

    private void Start()
    {
        initialHitSize = new float[hitPositions.Length];
        
        Init();
        PassDataToShader();
        
        meshRenderer.material.SetFloat(IncreaseMultiplier, increaseMultiplier);
    }

    private void Update()
    {
        HandleHitPointsFade();
    }

    private void Init()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
    }

    public void AddHitPoint(Vector4 hitPoint)
    {
        // when rich max pos start from beginning
        int nextIndex = lastIndexPos % hitPositions.Length;
        hitPositions[nextIndex] = hitPoint;
        initialHitSize[nextIndex] = hitPoint.w;
        lastIndexPos++;

        PassDataToShader();
    }

    private void PassDataToShader()
    {
        PassHitPositionsArrayToShader();
        meshRenderer.material.SetInt(ArrayLength, lastIndexPos + 1);
    }

    private void PassHitPositionsArrayToShader()
    {
        meshRenderer.material.SetVectorArray(HitPositions, hitPositions);
        meshRenderer.material.SetFloatArray(InitialHitSize, initialHitSize);
    }

    private void HandleHitPointsFade()
    {
        for (int i = 0; i < hitPositions.Length; i++)
        {
            hitPositions[i].w = Mathf.Lerp(hitPositions[i].w, initialHitSize[i] * increaseMultiplier, Time.deltaTime * increaseSpeed);
        }
        
        PassHitPositionsArrayToShader();
    }
}
