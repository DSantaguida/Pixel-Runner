using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : Trap
{
  // Start is called before the first frame update
  public float RotateSpeed = 5f;
  public float Radius;
  public GameObject chainLink;

  private List<GameObject> chain = new List<GameObject>();
  public List<float> radii = new List<float>();

  private Vector2 _centre;
  private float _angle;

  private float increment = 0.2f;

  void Start()
  {
    _centre = transform.position;
    SpawnLinks();
  }

  void Update()
  {
    _angle += RotateSpeed * Time.deltaTime;
    var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle));
    transform.position = _centre + offset * Radius;

    foreach (GameObject link in chain)
    {
      link.transform.position = _centre + offset * radii[chain.IndexOf(link)];
    }
  }

  private void SpawnLinks()
  {
    float distance = Radius;
    Vector2 prevPos = _centre;
    chain.Add(Instantiate(chainLink, _centre, Quaternion.identity));
    radii.Add(0);
    chain[0].transform.parent = transform;
    while (distance > 0)
    {
      chain.Add(Instantiate(chainLink, prevPos + new Vector2(0, increment), Quaternion.identity));
      chain[chain.Count - 1].transform.parent = transform;
      prevPos = chain[chain.Count - 1].transform.position;
      radii.Add(prevPos.y - _centre.y);
      distance -= increment;
    }
  }
}
