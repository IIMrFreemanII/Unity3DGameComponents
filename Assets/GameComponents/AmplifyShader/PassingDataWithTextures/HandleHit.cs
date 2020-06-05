using UnityEngine;

namespace GameComponents.AmplifyShader.PassingDataWithTextures
{
    public class HandleHit : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        private static readonly int HitPositions = Shader.PropertyToID("_HitPositions");
        private static readonly int TextureWidth = Shader.PropertyToID("_TextureWidth");
        private static readonly int InitRadii = Shader.PropertyToID("_InitRadii");
        private static readonly int IncreaseMultiplier = Shader.PropertyToID("_IncreaseMultiplier");

        public int maxWidth = 16;
        public int lastIndexPos = 0;
        
        public float increaseSpeed = 10f;
        public float increaseMultiplier = 15f;

        public Vector4[] hitPositions;
        public Vector4[] initRadii;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            
            InitArrays();
            InitData();
            
            meshRenderer.material.SetFloat(IncreaseMultiplier, increaseMultiplier);
        }
        
        private void Update()
        {
            HandleHitPointsFade();
        }
        
        private void HandleHitPointsFade()
        {
            for (int i = 0; i < hitPositions.Length; i++)
            {
                hitPositions[i].w = Mathf.Lerp(hitPositions[i].w, initRadii[i].w * increaseMultiplier, Time.deltaTime * increaseSpeed);
            }
        
            PassTexturesToShader();
        }

        private void InitArrays()
        {
            hitPositions = new Vector4[maxWidth];
            initRadii = new Vector4[maxWidth];
            
            for (int i = 0; i < maxWidth; i++)
            {
                hitPositions[i] = Vector4.zero;
                initRadii[i] = Vector4.zero;
            }
        }

        private void InitData()
        {
            PassTexturesToShader();
            SetTextureWidth();
        }

        public void AddHitPoint(Vector4 hitPoint)
        {
            int nextIndex = lastIndexPos % maxWidth;
            hitPositions[nextIndex] = hitPoint;
            initRadii[nextIndex] = hitPoint;
            lastIndexPos++;

            PassTexturesToShader();
        }

        private void PassTexturesToShader()
        {
            SetTextureData(hitPositions, HitPositions);
            SetTextureData(initRadii, InitRadii);
        }

        private void SetTextureData(Vector4[] data, int propId)
        {
            Texture2D texture2D = CreateTexture(data);
            SetTexture(texture2D, propId);
        }

        private void SetTextureWidth()
        {
            meshRenderer.material.SetInt(TextureWidth, hitPositions.Length);
        }

        private void SetTexture(Texture2D texture2D, int propId)
        {
            meshRenderer.material.SetTexture(propId, texture2D);
        }

        private Texture2D CreateTexture(Vector4[] data)
        {
            int height = 1;
            int width = data.Length;

            TextureFormat format = TextureFormat.RGBAFloat;
            TextureWrapMode wrapMode = TextureWrapMode.Clamp;

            Texture2D texture2D = new Texture2D(width, height, format, false);
            texture2D.wrapMode = wrapMode;
            texture2D.filterMode = FilterMode.Point;

            Color[] hitPositionsColor = new Color[width];

            for (int i = 0; i < data.Length; i++)
            {
                Vector4 hit = data[i];
                hitPositionsColor[i] = new Color(hit.x, hit.y, hit.z, hit.w);
            }

            texture2D.SetPixels(hitPositionsColor);
            texture2D.Apply();

            return texture2D;
        }
    }
}