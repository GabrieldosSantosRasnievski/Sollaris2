using UnityEngine;

public class FiltroCrafting : MonoBehaviour {
    public GameObject[] todosOsCraftings;

    public void MostrarArmas() {
        foreach (GameObject craft in todosOsCraftings) {
            if (craft != null) {
                bool ehArma = craft.CompareTag("CraftArma");
                craft.SetActive(ehArma);
            }
        }
    }
    public void MostrarArmaduras() {
        foreach (GameObject craft in todosOsCraftings) {
            if (craft != null) {
                bool ehArmadura = craft.CompareTag("CraftArmadura");
                craft.SetActive(ehArmadura);
            }
        }
    }
    public void MostrarTudo() {
        foreach (GameObject craft in todosOsCraftings) {
            if (craft != null) {
                craft.SetActive(true);
            }
        }
    }
}