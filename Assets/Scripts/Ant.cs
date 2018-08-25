using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour{
	public CellularMap m_map;
	public int row, col;
	public CardinalDirections direction;
	void Start()
	{
        m_map.WorldToGrid(transform.position, out row, out col);
	}

}
