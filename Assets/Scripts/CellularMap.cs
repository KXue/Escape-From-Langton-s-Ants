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
	private Tile [][] m_tiles;
	private HashSet<Vector3> previousSpawnPoints;
	private RepeatingRule [] m_repeatingRules = {new RepeatingRule()};
	// Use this for initialization
	void Start () {
		previousSpawnPoints = new HashSet<Vector3>();
		InitializeMap();
	}
	void InitializeMap(){
		do{
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

		}while(!Helper.IsMapValid(m_map, m_wrap));
		CreateTiles();
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
	public Vector3[] GetSpawnPoints(int numberOfPoints){
		Vector3[] retArray = new Vector3[numberOfPoints];
		List<Vector3> floorTiles = new List<Vector3>();
		
		HashSet<Vector3> points = new HashSet<Vector3>();

		for(int row = 0; row < m_map.Length; row++){
			for(int col = 0; col < m_map[row].Length; col++){
				if(!m_map[row][col]){
					floorTiles.Add(GridToWorld(row, col));
				}
			}
		}

		for(int i = 0; i < numberOfPoints; i++){
			Vector3 foundPoint;
			do{
				foundPoint = floorTiles[Random.Range(0, floorTiles.Count)];
			}while(points.Contains(foundPoint));
			points.Add(foundPoint);
			previousSpawnPoints.Add(foundPoint);
		}

		points.CopyTo(retArray);

		return retArray;
	}
	public Vector3 GridToWorld(int row, int col){
		float widthOffset = -m_width * (m_tileSize * 0.5f);
		float heightOffset = m_height * (m_tileSize * 0.5f);

		if(m_wrap){
			Helper.GetWrappedCoordinates(m_map, ref row, ref col);
		}

		return new Vector3(widthOffset + col * m_tileSize, 0f, heightOffset - row * m_tileSize);
	}
	public void WorldToGrid(Vector3 worldPosition, out int row, out int col){
		float widthOffset = -m_width * (m_tileSize * 0.5f);
		float heightOffset = m_height * (m_tileSize * 0.5f);

		col = Mathf.RoundToInt((worldPosition.x - widthOffset) / m_tileSize);
		row = Mathf.RoundToInt(-(worldPosition.z - heightOffset) / m_tileSize);
	}
	public bool GetTileState(int row, int col){
		return Helper.MapValue(m_map, row, col, m_wrap);
	}
	public void SetTileState(int row, int col, bool value){
		Helper.SetMapValue(m_map, ref row, ref col, m_wrap, value);
		m_tiles[row][col].IsWall = value;
	}
	void CreateTiles(){	
		GameObject hidePlane = Instantiate(m_hidePlanePrefab, new Vector3(-0.5f, 0f, 0.5f), Quaternion.identity, transform);
		hidePlane.transform.localScale = new Vector3(m_width * m_tileSize * 0.1f, 1,  m_height * m_tileSize * 0.1f);
		
		m_tiles = new Tile[m_height][];
		for(int row = 0; row < m_height; row++){
			m_tiles[row] = new Tile[m_width];
			for(int col = 0; col < m_width; col++){
				Tile newTile = Instantiate(m_tilePrefab, GridToWorld(row, col), Quaternion.identity, transform);
				newTile.IsWall = m_map[row][col];
				m_tiles[row][col] = newTile;
			}
		}
	}
	void UpdateTiles(){
		for(int row = 0; row < m_tiles.Length; row++){
			for(int col = 0; col < m_tiles[row].Length; col++){
				m_tiles[row][col].IsWall = m_map[row][col];
			}
		}
	}
}
