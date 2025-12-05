using UnityEngine;

public class Enemy : MonoBehaviour
{
    int hp;
    [SerializeField] float speed;
    Vector2 nextPos;
    int posIndex = 0;
    Map map;

    void Start()
    {
        
        map = GameObject.FindAnyObjectByType<Map>();
        GetNextPos();
    }

    void GetNextPos()
    {
        nextPos = map.GetNextPosFrom(posIndex);
        if (nextPos == Vector2.negativeInfinity)
        {
            // TODO delete Enemy and get dmg
            return;
        }
        posIndex++;
    }

    void Update()
    {
        Vector2 direction = (nextPos - (Vector2)transform.position).normalized;

        Vector2 translation = direction * speed * Time.deltaTime;
        if (translation.magnitude > Vector2.Distance(transform.position, nextPos))
        {
            transform.position = nextPos;
            GetNextPos();
        }
        else
        {
            transform.Translate(translation);
        }
    }
}
