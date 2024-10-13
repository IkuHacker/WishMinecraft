using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryRecipe))]
public class InventoryRecipeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // On récupère l'objet cible du script
        InventoryRecipe recipe = (InventoryRecipe)target;

        // Section OUTPUT
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("OUTPUT", new GUIStyle { fontStyle = FontStyle.Bold });
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // Affichage de la texture et des propriétés pour l'item de sortie
        EditorGUILayout.BeginVertical();
        Texture outputTexture = recipe.output != null && recipe.output.item != null ? recipe.output.item.visual.texture : null;
        GUILayout.Box(outputTexture, GUILayout.Width(150), GUILayout.Height(150));

        // Champs pour l'item de sortie (ItemData) et la quantité (count)
        EditorGUILayout.PropertyField(serializedObject.FindProperty("output.item"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("output.count"), new GUIContent("Count"), true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Section RECIPE (la grille des items)
        GUILayout.Label("RECIPE", new GUIStyle { fontStyle = FontStyle.Bold });

        // Affichage de la grille 2x2 des items
        for (int y = 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x <= 1; x++)
            {
                EditorGUILayout.BeginVertical();
                Texture itemTexture = recipe.GetItem(x, y) != null ? recipe.GetItem(x, y).visual.texture : null;
                GUILayout.Box(itemTexture, GUILayout.Width(150), GUILayout.Height(150));
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"item_{x}{y}"), GUIContent.none, true, GUILayout.Width(150));
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
