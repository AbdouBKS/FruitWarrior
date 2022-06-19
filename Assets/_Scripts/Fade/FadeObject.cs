using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    public List<Renderer> Renderers;
    public Vector3 Position;
    public List<Material> Materials = new List<Material>();
    [HideInInspector]
    public float InitialAlpha;

    private void Awake() {
        foreach (Renderer render in GetComponentsInChildren<Renderer>())
            Renderers.Add(render);
        Position = transform.position;
        print(Renderers.Count);
        if (Renderers.Count == 0)
        {
            Renderers.AddRange(GetComponentsInChildren<Renderer>());
        }
        for (int i = 0; i < Renderers.Count; i++)
        {
            Materials.AddRange(Renderers[i].materials);
        }

        InitialAlpha = Materials[0].color.a;
    }

    public bool Equals(FadeObject other)
    {
        return Position.Equals(other.Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}
