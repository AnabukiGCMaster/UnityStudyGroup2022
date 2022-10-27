using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    [Header("’e")]
    public GameObject m_Bullet;
    [Header("eŒû")]
    public Transform m_Muzzle;

    void Update()
    {
        //”­–Cˆ—
        Fire();
    }
    /// <summary>
    /// ”­–Cˆ—
    /// </summary>
    public void Fire()
    {
        //ƒ}ƒEƒX¶ƒNƒŠƒbƒN
        if(Input.GetMouseButtonDown(0))
        {
            //’e‚ğoŒ»‚³‚¹AeŒû‚ÌÀ•W‚ÆŒü‚«‚É‡‚í‚¹‚é
            GameObject Dummy = Instantiate(m_Bullet, m_Muzzle.position, m_Muzzle.rotation);
            //’e‚É•¨—‚ª‚È‚¢ê‡A’e‚É•¨—‚ğ‘ã“ü
            if (!Dummy.GetComponent<Rigidbody>())
                Dummy.AddComponent<Rigidbody>();
            //’e‚Ì³–Ê‚ÖŒü‚©‚Á‚ÄA‰Î—Í10000‚ÅËo‚·‚é
            Dummy.GetComponent<Rigidbody>().AddForce(Dummy.transform.forward * 10000.0f);
            //’e‚Í5•bŒã‚É©“®Á–Å‚·‚é(—\–ñ)
            Destroy(Dummy,5.0f);
        }
    }
}
