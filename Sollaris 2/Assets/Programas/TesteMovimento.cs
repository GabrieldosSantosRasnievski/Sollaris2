using UnityEngine;
using System.Collections;
public class TesteMovimento : MonoBehaviour

{
    public float velocidade = 5f;
    public float velocidadeDash = 15f;
    public float duracaoDash = 0.5f;
    public float recargaDash = 1f;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteHomem;
    public Sprite spriteMulher;
    private Collider2D playerCollider;
    private Vector2 ultimaDirecaoDash = Vector2.right;
    private bool realizandoDash = false;
    private bool consegueDash = true;
    public ParticleSystem particulaDash;
    public float emissaomaxima = 60f;

    void Start(){
        if(particulaDash != null){
            particulaDash.Stop();
        }
        AtualizarGenero();
        playerCollider = GetComponent<Collider2D>();
    }
    public void AtualizarGenero(){
        string generoEscolhido = PlayerPrefs.GetString("GeneroPlayer", "Homem");
        if(generoEscolhido == "Homem"){
            spriteRenderer.sprite = spriteHomem;
        }
        else if (generoEscolhido == "Mulher"){
            spriteRenderer.sprite = spriteMulher;
        }
    }
    void Update()
    {
        Vector2 direcaoInput = Vector2.zero;
        if(Input.GetKey(KeyCode.W)) direcaoInput.y = direcaoInput.y + 1f;
        if(Input.GetKey(KeyCode.S)) direcaoInput.y = direcaoInput.y - 1f;
        if(Input.GetKey(KeyCode.D)) direcaoInput.x = direcaoInput.x + 1f;
        if(Input.GetKey(KeyCode.A)) direcaoInput.x = direcaoInput.x - 1f;
        if(direcaoInput != Vector2.zero){
            ultimaDirecaoDash = direcaoInput.normalized;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && consegueDash){
            StartCoroutine(DarDash());
        }
        if (realizandoDash){
            transform.Translate(ultimaDirecaoDash * velocidadeDash * Time.deltaTime);
        }
        else{
            transform.Translate(direcaoInput.normalized * velocidade * Time.deltaTime);
        }
    }
        private IEnumerator DarDash(){
        consegueDash = false;
        realizandoDash = true;
        if(playerCollider != null){
            playerCollider.isTrigger = true;
        }
        if(particulaDash != null){
            particulaDash.Play();
        }
        float tempoColapso = 0f;
        ParticleSystem.EmissionModule emission = particulaDash.emission;
        while(tempoColapso < duracaoDash){
            tempoColapso = tempoColapso + Time.deltaTime;
            float progresso = 1f - (tempoColapso / duracaoDash);
            emission.rateOverTime = emissaomaxima * progresso;
            yield return null;
        }
        if(particulaDash != null){
            particulaDash.Stop();
        }
        if(playerCollider != null){
            playerCollider.isTrigger = false;
        }
        realizandoDash = false;
        yield return new WaitForSeconds(recargaDash);
        consegueDash = true;
    }
}
