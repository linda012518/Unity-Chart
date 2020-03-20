using UnityEngine;

namespace Utils.Effect
{
    public class ShowEffect : MonoBehaviour
    {

        Material mat;

        public float scrollSpeed = 0.3f;
        float jianbianoffset = 0;

        Vector2 textureOffset = Vector2.zero;

        void Start()
        {
            mat = GetComponent<Renderer>().material;
        }

        void OnEnable()
        {
            jianbianoffset = 0;
        }

        void Update()
        {
            textureOffset.x = Time.time * scrollSpeed;
            textureOffset.x %= 1;
            mat.mainTextureOffset = textureOffset;


            jianbianoffset -= 0.02f;
            textureOffset.x = jianbianoffset;
            mat.SetTextureOffset("_Mask", textureOffset);
        }

    }
}