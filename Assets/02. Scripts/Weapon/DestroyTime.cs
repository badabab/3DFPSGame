using UnityEngine;

public enum DeleteType
{
    Destory,
    Inactive,
}
public class DestroyTime : MonoBehaviour
{
    public DeleteType DeleteType;
    public float DeleteTime = 1.5f;
    public float BloodTime = 1f;
    private float _timer = 0;

    private void Update()
    {
        _timer += Time.deltaTime;
        

        if (_timer >= DeleteTime)
        {
            if (DeleteType == DeleteType.Destory)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Init();
                gameObject.SetActive(false);
            }
        }
        if (_timer >= BloodTime)
        {
            if (DeleteType == DeleteType.Inactive)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Init()
    {
        _timer = 0;
    }
    private void OnDisable()    //비활성화 될 때
    {
        Init();
    }
}