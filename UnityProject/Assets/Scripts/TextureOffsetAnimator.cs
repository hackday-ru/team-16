using UnityEngine;
using System.Collections;

public class TextureOffsetAnimator : MonoBehaviour
{
    public Material material;
    public Vector2 offsetSpeed;

    void Update()
    {
        var value = material.mainTextureOffset + offsetSpeed / Time.deltaTime;
        value.x = value.x % 10.0f;
        value.y = value.y % 10.0f;
        material.mainTextureOffset = value;
    }
}
