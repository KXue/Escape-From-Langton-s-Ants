using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	static Color WALLCOLOUR = Color.black;
	static Color FLOORCOLOUR = Color.white;
	public bool IsWall{
		get{
			return m_isWall;
		}
		set{
			if(m_isWall != value){
				m_isWall = value;
				UpdateColour();
			}
		}
	}
	Renderer m_renderer;
	bool m_isWall;
	
	// Use this for initialization
	void Awake () {
		m_renderer = GetComponent<Renderer>();
		UpdateColour();
	}
	void UpdateColour(){
		m_renderer.material.color = m_isWall ? WALLCOLOUR : FLOORCOLOUR;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
