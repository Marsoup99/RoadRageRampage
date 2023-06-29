using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShop : MonoBehaviour
{
    public CharacterController cc;
    public float speed = 15;
    private CarMovement player;
    void Reset()
    {
        cc = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
            cc.Move(Vector3.forward * speed * Time.deltaTime);
        else 
        {
            if(player.transform.position.z > transform.position.z)
                cc.Move(Vector3.forward * (player.speed + 1) * Time.deltaTime);
            else cc.Move(Vector3.forward * (player.speed - 1) * Time.deltaTime);
        }
        
    }

    public void ShopClosed()
    {
        player = null;
        speed = 0;
        Destroy(this.gameObject, 5);
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {   
            GameManager.Instance.OpenShop();

            player = PlayerCtrl.Instance.carMovement;
            player.Brake();
            if(player.currentLaneSide >= 1)
            {
                player.ChangeLane(LANE.left);
                if(player.currentLaneSide == 1) player.ChangeLane(LANE.left);
            }
                
            else if(player.currentLaneSide <= -1)
            {
                player.ChangeLane(LANE.right);
                if(player.currentLaneSide == -1) player.ChangeLane(LANE.right);
            }
        }
    }
}
