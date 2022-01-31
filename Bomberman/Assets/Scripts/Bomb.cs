using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    public enum ObstacleType
    {
        Crate,
        Wall,
        None
    }

    private float explosionTime = 1f;
    private float destroyTime = .24f;
    private float propagationTime = .01f;

    private List<GameObject> Explosions;
    private bool doneSouth, doneNorth, doneEast, doneWest;

    public static void Create(GameObject bomb, Vector3 position)
    {
        Instantiate(bomb, position, Quaternion.identity);
    }

    void Awake()
    {
        Explosions = new List<GameObject>();
    }

    void Start()
    {
        Vector3Int gridPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        
        StartCoroutine(Explode(gridPos, Vector3Int.up, explosionTime));
        StartCoroutine(Explode(gridPos, Vector3Int.down, explosionTime));
        StartCoroutine(Explode(gridPos, Vector3Int.left, explosionTime));
        StartCoroutine(Explode(gridPos, Vector3Int.right, explosionTime));
        StartCoroutine(ActivateBomb());
    }

    private IEnumerator ActivateBomb()
    {
        yield return new WaitForSeconds(explosionTime);

        GetComponent<SpriteRenderer>().enabled = false;
        Vector3Int gridPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        SetExplosionTile(gridPos, Explosion.ExplosionType.Middle, Vector3Int.up);

        SoundManager.Play("Explosion");
    }

    private IEnumerator DestroyBomb()
    {
        BombManager.Instance.Explode();

        if (doneNorth && doneSouth && doneEast && doneWest)
        {
            yield return new WaitForSeconds(destroyTime);
            foreach (var explosion in Explosions)
            {
                Destroy(explosion);
            }
            Destroy(gameObject);
        }
    }

    private void SetExplosionTile(Vector3Int gridPos, Explosion.ExplosionType type, Vector3Int direction)
    {
        Quaternion rotation = Quaternion.identity;
        if (direction == Vector3Int.up) rotation = Quaternion.AngleAxis(270, Vector3.forward);
        else if (direction == Vector3Int.down) rotation = Quaternion.AngleAxis(90, Vector3.forward);
        else if (direction == Vector3Int.left) rotation = Quaternion.AngleAxis(0, Vector3.forward);
        else if (direction == Vector3Int.right) rotation = Quaternion.AngleAxis(180, Vector3.forward);

        Vector3 pos = BombManager.Instance.ObstaclesMap.GetCellCenterWorld(gridPos);
        GameObject explosion = null;
        switch (type)
        {
            case Explosion.ExplosionType.Middle:
                explosion = Instantiate(BombManager.Instance.ExplosionMiddleObject, pos, rotation, GameObject.Find("Explosions").transform);
                break;

            case Explosion.ExplosionType.Line:
                explosion = Instantiate(BombManager.Instance.ExplosionLineObject, pos, rotation, GameObject.Find("Explosions").transform);
                break;

            case Explosion.ExplosionType.Side:
                explosion = Instantiate(BombManager.Instance.ExplosionSideObject, pos, rotation, GameObject.Find("Explosions").transform);
                break;
        }
        
        Explosions.Add(explosion);
    }

    private ObstacleType GetObstacleType(Vector3Int gridPos)
    {
        RuleTile tile = BombManager.Instance.ObstaclesMap.GetTile(gridPos) as RuleTile;
        if (tile != null)
        {
            GameObject gridObject = tile.m_DefaultGameObject;
            if (gridObject == null) return ObstacleType.None;
            switch (gridObject.tag)
            {
                case "Crate":
                    return ObstacleType.Crate;

                case "Wall":
                    return ObstacleType.Wall;
            }
        }
        return ObstacleType.None;
    }

    private void SetDone(Vector3Int direction)
    {
        if (direction == Vector3Int.up) doneNorth = true;
        else if (direction == Vector3Int.down) doneSouth = true;
        else if (direction == Vector3Int.left) doneWest = true;
        else if (direction == Vector3Int.right) doneEast = true;
    }

    private IEnumerator Explode(Vector3Int gridPos, Vector3Int direction, float time)
    {
        yield return new WaitForSeconds(time);

        // Test if there's a wall or a crate on the gridPos
        gridPos += direction;
        ObstacleType type = GetObstacleType(gridPos);
        switch (type)
        {
            case ObstacleType.Wall:
                SetDone(direction);
                break;

            case ObstacleType.Crate:
                BombManager.Instance.ObstaclesMap.SetTile(gridPos, null);
                SetExplosionTile(gridPos, Explosion.ExplosionType.Side, direction);
                SetDone(direction);

                SoundManager.Play("BoxBreak");
                break;

            case ObstacleType.None:
                if (GetObstacleType(gridPos + direction) == ObstacleType.Wall)
                    SetExplosionTile(gridPos, Explosion.ExplosionType.Side, direction);
                else
                    SetExplosionTile(gridPos, Explosion.ExplosionType.Line, direction);
                yield return StartCoroutine(Explode(gridPos, direction, propagationTime));
                break;
        }

        yield return StartCoroutine(DestroyBomb());
    }
}
