using UnityEngine;

public class ItemArremessado : MonoBehaviour
{
//vou comecar a usar isso para me ajudar
    public float velocidade = 10f;
    public float tempoDeVoo = 0.5f;
    public float giroObjeto = 720f;
    private Vector2 direcao;
    private bool voando = true;
    private Rigidbody2D rb;
    private string nomeDoItem;
    private Sprite iconeDoItem;
//bora comecar
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    public void Inicializar(Vector2 direcaoArremesso, Sprite iconeItem, string nome){
        direcao = direcaoArremesso.normalized;
        iconeDoItem = iconeItem;
        nomeDoItem = nome;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.sprite = iconeItem;
        }
        InteragirObjeto interacao = GetComponent<InteragirObjeto>();
        if (interacao != null){
            interacao.enabled = false;
        }
        Invoke(nameof(PararNoChao), tempoDeVoo);
    }
    void Update(){
        if(voando){
            transform.Rotate(0, 0, giroObjeto * Time.deltaTime);
            transform.Translate(direcao * velocidade * Time.deltaTime, Space.World);
        }
    }
    void PararNoChao(){
        if(!voando){
            return;
        }
        voando = false;
        transform.rotation = Quaternion.identity;
        InteragirObjeto interacao = GetComponent<InteragirObjeto>();

        if (interacao != null)
        {
            interacao.nomeItem = nomeDoItem;
            interacao.iconeItem = iconeDoItem;
            interacao.enabled = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(voando){
            CancelInvoke(nameof(PararNoChao));
            PararNoChao();
        }
    }
}
