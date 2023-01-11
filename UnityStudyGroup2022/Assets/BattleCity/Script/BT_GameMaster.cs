using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BT_GameMaster : MonoBehaviour
{
    [System.Serializable]
    public struct TankData
    {
        public GameObject m_PlayerTank;
        public float m_RepopTime;
    };
    public List<TankData> m_PlayerTanks;

    public float m_MaxRepopTime;
    public int m_PlayerCounter = 4;
    public float m_CameraPoint = 1.5f;
    public GameObject m_TankObject;
    public Vector2 m_RePopAria;
    public List<GameObject> m_TankCamera;
    void Start()
    {
        StartPop();
    }

    void Update()
    {
        PlayerLostRepop();
    }

    public void StartPop()
    {
        for (int No = 0; No < m_PlayerCounter; No++)
        {
            PlayerNewRePop(No);
        }
    }
    public void PlayerNewRePop(int PlayerNo)
    {
        if (m_PlayerTanks.Count <= PlayerNo)
        {
            m_PlayerTanks.Add(PlayerRePop(PlayerNo));
            PlayerJoySet(PlayerNo);
        }
    }
    public void PlayerJoySet(int PlayerNo)
    {
        BT_Parameta TankParameta = m_PlayerTanks[PlayerNo].m_PlayerTank.GetComponent<BT_Parameta>();
        int KeyNo = PlayerNo + 1;
        TankParameta.m_LRKeyName = "Player" + KeyNo.ToString() + "LR";
        TankParameta.m_UDKeyName = "Player" + KeyNo.ToString() + "UD";
        TankParameta.m_FireKeyName = "Player" + KeyNo.ToString() + "Fire";
        TankParameta.m_TurretKeyName = "Player" + KeyNo.ToString() + "LR2";
    }
    public void PlayerLostRepop()
    {
        for (int PlayerNo = 0; PlayerNo < m_PlayerTanks.Count; PlayerNo++)
        {
            if (m_PlayerTanks[PlayerNo].m_PlayerTank == null)
            {
                if (m_PlayerTanks[PlayerNo].m_RepopTime <= 0.0f)
                {
                    m_PlayerTanks[PlayerNo] = PlayerRePop(PlayerNo);
                    PlayerJoySet(PlayerNo);
                }
                else
                {
                    TankData DummyTankData = m_PlayerTanks[PlayerNo];
                    DummyTankData.m_RepopTime -= 1.0f * Time.deltaTime;
                    m_PlayerTanks[PlayerNo] = DummyTankData;
                }
            }
        }
    }
    public TankData PlayerRePop(int PlayerNo)
    {
        GameObject Dummy = Instantiate(
            m_TankObject,
            new Vector3(
                Random.Range(-m_RePopAria.x, m_RePopAria.x),
                1,
                Random.Range(-m_RePopAria.y, m_RePopAria.y)),
            this.transform.rotation);
        Transform DummyTransform = Dummy.GetComponent<BT_Turret>().m_Turret;
        Dummy.GetComponent<BT_Turret>().m_Camera = m_TankCamera[PlayerNo].transform;
        m_TankCamera[PlayerNo].transform.position = DummyTransform.position;
        m_TankCamera[PlayerNo].transform.rotation = DummyTransform.rotation;
        m_TankCamera[PlayerNo].transform.Translate(new Vector3(0, 0, -m_CameraPoint));
        m_TankCamera[PlayerNo].transform.parent = DummyTransform;

        TankData RepopTankData = new TankData();
        RepopTankData.m_PlayerTank = Dummy;
        RepopTankData.m_RepopTime = m_MaxRepopTime;

        return RepopTankData;
    }
}
