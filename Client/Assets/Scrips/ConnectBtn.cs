using CommonClient;
using CommonUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConnectBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
        if(btn != null)
        {
            //btn.onClick.AddListener(Click);
           
        }
    }

    void Update()
    {

    }
    private int i = 0;
    public void Click(GameObject obj)
    {   
        Debug.Log("========Click=======");
        ClientManager.Instance.B2C("=====客户端发送消息====="+(++i));
        Player p = new Player();
        p.id = (uint)i;
        p.name = "角色" + i;
        p.force = 1;
        ClientManager.Instance.B2C(p);
        BuffEvent buff = new BuffEvent();
        buff.uid = (uint)i;
        buff.templateId = Random.Range(1, 100000);
        ClientManager.Instance.B2C(buff);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btn != null)
        {
            btn.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (btn != null)
        {
            btn.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
