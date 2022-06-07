using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerManager : MonoBehaviour
{
    [SerializeField]
    GameObject stickerPrefab;
    [SerializeField]
    List<SOSticker> stickerScriptableObjects = new List<SOSticker>();
    List<Sticker> stickers = new List<Sticker>();
    int index;
    bool canContinue;

    public bool CanContinue
    {
        set { canContinue = value; }
    }

    private void Awake()
    {
        if (!stickerPrefab || stickerScriptableObjects.Count == 0) { return; }
        for(int i = 0; i < stickerScriptableObjects.Count; i++)
        {
            GameObject sticker = Instantiate(stickerPrefab);
            sticker.transform.parent = transform;
            sticker.transform.position = transform.position;
            sticker.GetComponent<Sticker>().SetStickerScriptableObject = stickerScriptableObjects[i];
        }
        
        Sticker[] newList = GetComponentsInChildren<Sticker>();
        stickers.AddRange(newList);

        for (int i = 1; i < stickers.Count; i++)
        {
            stickers[i].gameObject.SetActive(false);
        }
    }

    public void UpdateActiveStickers()
    {
        if (!canContinue || index == stickerScriptableObjects.Count) { return; }
       
        stickers[index].gameObject.SetActive(false);
        index++;
        stickers[index].gameObject.SetActive(true);
        canContinue = false;
    }
}
