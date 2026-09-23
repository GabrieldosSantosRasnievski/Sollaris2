using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class AbrirInventario : MonoBehaviour{
    public GameObject painelMenuGeral;
    public GameObject painelInventario;
    public GameObject painelCrafting;
    public List<TextMeshProUGUI> textosSlotsRapidos;
    public List<Image> imagensSlotsRapidos;
    public List<Image> fundosSlotsRapidos;
    public Color corNormal = Color.white;
    public Color corSelecionado = new Color(0.5f, 0.5f, 0.5f, 1f);
    public List<TextMeshProUGUI> textosSlotsInventario;
    public List<Image> imagensSlotsInventario;

    void Start(){
        if(painelMenuGeral != null){
            painelMenuGeral.SetActive(false);
        }
        AbrirPainelCrafting();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape)){
            AlternarMenuGeral();
        }

        if(painelMenuGeral != null && painelMenuGeral.activeSelf){
            AtualizarUI();
        }
    }

    public void AlternarMenuGeral(){
        if(painelMenuGeral != null){
            bool ativar = !painelMenuGeral.activeSelf;
            painelMenuGeral.SetActive(ativar);

            if(ativar){
                Time.timeScale = 0f;
                AbrirPainelCrafting();
            }
            else{
                Time.timeScale = 1f;
            }
        }
    }

    public void AbrirPainelInventario(){
        if(painelInventario != null){
            painelInventario.SetActive(true);
        }
        if(painelCrafting != null){
            painelCrafting.SetActive(false);
        }
    }

    public void AbrirPainelCrafting(){
        if(painelCrafting != null){
            painelCrafting.SetActive(true);
        }
        if(painelInventario != null){
            painelInventario.SetActive(false);
        }
    }

    public void AtualizarUI(){
        if(InventarioJogador.Instance == null){
            return;
        }

        int slotAtivo = InventarioJogador.Instance.slotSelecionado;

        for(int i = 0; i < textosSlotsRapidos.Count; i++){
            if(i < fundosSlotsRapidos.Count && fundosSlotsRapidos[i] != null){
                if(i == slotAtivo){
                    fundosSlotsRapidos[i].color = corSelecionado;
                }
                else{
                    fundosSlotsRapidos[i].color = corNormal;
                }
            }

            if(i < InventarioJogador.Instance.slotsRapidos.Count){
                var slot = InventarioJogador.Instance.slotsRapidos[i];

                if(!string.IsNullOrEmpty(slot.nomeItem) && slot.quantidadeItem > 0){
                    if(slot.quantidadeItem > 1){
                        textosSlotsRapidos[i].text = slot.nomeItem + "\nx" + slot.quantidadeItem;
                    }
                    else{
                        textosSlotsRapidos[i].text = slot.nomeItem;
                    }

                    if(i < imagensSlotsRapidos.Count && imagensSlotsRapidos[i] != null){
                        imagensSlotsRapidos[i].sprite = slot.iconeItem;
                        imagensSlotsRapidos[i].enabled = true;
                    }
                }
                else{
                    if(textosSlotsRapidos[i] != null){
                        textosSlotsRapidos[i].text = "";
                    }
                    if(i < imagensSlotsRapidos.Count && imagensSlotsRapidos[i] != null){
                        imagensSlotsRapidos[i].sprite = null;
                        imagensSlotsRapidos[i].enabled = false;
                    }
                }
            }
            else{
                if(textosSlotsRapidos[i] != null){
                    textosSlotsRapidos[i].text = "";
                }
                if(i < imagensSlotsRapidos.Count && imagensSlotsRapidos[i] != null){
                    imagensSlotsRapidos[i].sprite = null;
                    imagensSlotsRapidos[i].enabled = false;
                }
            }
        }

        for(int i = 0; i < textosSlotsInventario.Count; i++){
            if(i < InventarioJogador.Instance.inventario.Count){
                var slot = InventarioJogador.Instance.inventario[i];

                if(!string.IsNullOrEmpty(slot.nomeItem) && slot.quantidadeItem > 0){
                    if(slot.quantidadeItem > 1){
                        textosSlotsInventario[i].text = slot.nomeItem + "\nx" + slot.quantidadeItem;
                    }
                    else{
                        textosSlotsInventario[i].text = slot.nomeItem;
                    }

                    if(i < imagensSlotsInventario.Count && imagensSlotsInventario[i] != null){
                        imagensSlotsInventario[i].sprite = slot.iconeItem;
                        imagensSlotsInventario[i].enabled = true;
                    }
                }
                else{
                    if(textosSlotsInventario[i] != null){
                        textosSlotsInventario[i].text = "";
                    }
                    if(i < imagensSlotsInventario.Count && imagensSlotsInventario[i] != null){
                        imagensSlotsInventario[i].sprite = null;
                        imagensSlotsInventario[i].enabled = false;
                    }
                }
            }
            else{
                if(textosSlotsInventario[i] != null){
                    textosSlotsInventario[i].text = "";
                }
                if(i < imagensSlotsInventario.Count && imagensSlotsInventario[i] != null){
                    imagensSlotsInventario[i].sprite = null;
                    imagensSlotsInventario[i].enabled = false;
                }
            }
        }
    }
}