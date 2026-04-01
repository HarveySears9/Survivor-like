using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        ps.Play();
        Invoke(nameof(Return), ps.main.duration);
    }

    void Return()
    {
        BloodEffectPool.Instance.Return(gameObject);
    }
}