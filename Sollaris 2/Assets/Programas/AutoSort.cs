using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int sortingOrderBase = 5000; 

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void LateUpdate(){
        spriteRenderer.sortingOrder = sortingOrderBase - (int)(transform.position.y * 100);
    }
}