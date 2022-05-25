using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StickerManager : MonoBehaviour
{
    bool stickerSpawned;
    public GameObject sticker;
    public string stickerTag;
    Highlights[] highlights;
    public List<GameObject> permaStickers;
    
    public GameObject cityPrefabBW, cityPrefabCol, characterPrefabBW, characterPrefabCol;

    private void Awake()
    {
        LoadResources();
        highlights = FindObjectsOfType<Highlights>();
    }
    void LoadResources()
    {
        //cityPrefabBW = Resources.Load<GameObject>("CitySticker_BaW").gameObject;
        //cityPrefabCol = Resources.Load<GameObject>("CitySticker_Coloured").gameObject;
        characterPrefabBW = Resources.Load<GameObject>("CharacterSticker_BaW").gameObject;
        characterPrefabCol = Resources.Load<GameObject>("CharacterSticker_Coloured").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(stickerSpawned)
        {
            MoveObject(sticker);
        }
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Raycast();
        }
    }

    public void SpawnObject(string name)
    {
        stickerSpawned = true;
        switch (name)
        {
            case "City":
                sticker = Instantiate(cityPrefabBW);
                stickerTag = name;
                break;
            case "Character":
                sticker = Instantiate(characterPrefabBW);
                stickerTag = name;
                break;
            default:
                stickerSpawned = false;
                break;
        }
    }

    void MoveObject(GameObject obj)
    {
        obj.transform.position = Mouse2WorldPos();
    }

    void Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        var hit = Physics2D.GetRayIntersection(ray, 100f);

        if(!hit)
        {
            DespawnObject(sticker);
           
            return;
        }
        if (hit.collider.tag == stickerTag)
        {
            switch (stickerTag)
            {
                case "City":
                    permaStickers.Add(Instantiate(cityPrefabCol, Mouse2WorldPos(), Quaternion.identity));
                    DespawnObject(hit.collider.gameObject);
                    break;
                case "Character":
                    permaStickers.Add(Instantiate(characterPrefabCol, Mouse2WorldPos(), Quaternion.identity));
                    DespawnObject(hit.collider.gameObject);
                    break;
                default:
                    break;
            }
        }
        DespawnObject(sticker);
    }
    void DespawnObject(GameObject obj)
    {
        stickerTag = null;
        stickerSpawned = false;
        Destroy(obj);
        sticker = null;
    }
    
    Vector3 Mouse2WorldPos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);
    }
}
