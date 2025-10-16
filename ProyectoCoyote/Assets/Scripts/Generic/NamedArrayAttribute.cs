using System;
using UnityEngine;

public class NamedArrayAttribute : PropertyAttribute
{
    //Codigo para mostrar Palabras especificas en la lista de Stats

    public readonly string[] names;
    public NamedArrayAttribute() { this.names = System.Enum.GetNames(typeof(EnemyAI.BasicActions)); ; }
}

