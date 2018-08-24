using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CellularMap : MonoBehaviour {
	public bool m_wrap;
	public int m_width;
	public int m_height;
	public float m_tileSize;
	public float m_wallPercent;
	public Tile m_tilePrefab;
	public GameObject m_hidePlanePrefab;
	private bool [][] m_map;
	private RepeatingRule [] m_repeatingRules = {new RepeatingRule()};
	// Use this for initialization
	void Start () {
		CreateRandomMap();

		RepeatingRule firstRule = new RepeatingRule();
		firstRule.repeatCount = 1;
		firstRule.m_rule = new CaveRule();
		firstRule.m_rule.m_wrap = m_wrap;
		m_map = firstRule.ApplyRuleRepeatedly(m_map);

		RepeatingRule secondRule = new RepeatingRule();
		secondRule.repeatCount = 4;
		secondRule.m_rule = new CaveRule();
		secondRule.m_rule.m_wrap = m_wrap;
		m_map = secondRule.ApplyRuleRepeatedly(m_map);

		Helper.PrintMap(m_map);
		Debug.Log(Helper.IsMapValid(m_map, m_wrap));

		CreateMapVisuals();
	}
	void CreateRandomMap(){
		m_map = new bool[m_height][];
		for(int row = 0; row < m_height; row++){
			m_map[row] = new bool[m_width];
			for(int col = 0; col < m_width; col++){
				m_map[row][col] = Random.Range(0, 100) < m_wallPercent;
			}
		}
	}
	void CreateMapVisuals(){
		float widthOffset = -m_width * (m_tileSize * 0.5f);
		float heightOffset = m_height * (m_tileSize * 0.5f);
		
		GameObject hidePlane = Instantiate(m_hidePlanePrefab);
		hidePlane.transform.localScale = new Vector3(m_width * m_tileSize * 0.1f, 1,  m_height * m_tileSize * 0.1f);

		for(int row = 0; row < m_height; row++){
			for(int col = 0; col < m_width; col++){
				Tile newTile = Instantiate(m_tilePrefab, new Vector3(widthOffset + col * m_tileSize, 0f, heightOffset - row * m_tileSize), Quaternion.identity);
				newTile.IsWall = m_map[row][col];
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
