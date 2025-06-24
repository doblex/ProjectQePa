using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtension
{
    /// <summary>
    /// Sets the specified material on the MeshRenderer of the GameObject.
    /// If the material is already present, it will be removed; otherwise, it will be added.
    /// <Param name="renderer">The MeshRenderer to modify.</Param>
    /// <Param name="material">The Material to add or remove.</Param>
    /// </summary>
    public static void SetMaterial(Renderer renderer, Material material)
    {
        List<Material> newMaterials = renderer.sharedMaterials.ToList();

        if (!newMaterials.Contains(material))
        {
            newMaterials.Add(material);
        }

        renderer.sharedMaterials = newMaterials.ToArray();
    }

    public static void UnsetMaterial(Renderer renderer, Material material)
    {
        List<Material> newMaterials = renderer.sharedMaterials.ToList();

        if (newMaterials.Contains(material))
        {
            newMaterials.Remove(material);
        }

        renderer.sharedMaterials = newMaterials.ToArray();
    }
}
