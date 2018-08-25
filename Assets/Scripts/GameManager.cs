using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public CellularMap m_map;
	public Player m_playerPrefab;
	public Ant m_antPrefab;
	public Exit m_exitPrefab;
	private static GameManager m_instance;
	private Ant[] m_ants;
	private Player m_player;
	private Exit m_exit;
	public static GameManager Instance{
		get{
			return m_instance;
		}
	}
	void Awake () {
		if(m_instance != null && m_instance != this){
			Destroy(this.gameObject);
		}else{
			m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
	}
	public void PlayerMoved(){
		//flip ant's current tile
		HashSet<Vector2Int> indices = new HashSet<Vector2Int>();
		foreach(Ant ant in m_ants){
			Vector2Int index = new Vector2Int(ant.m_row, ant.m_col);
			if(!indices.Contains(index)){
                indices.Add(index);
				m_map.SetTileState(ref ant.m_row, ref ant.m_col, !m_map.GetTileState(ref ant.m_row, ref ant.m_col));
            }
		}
		//move ants
		foreach(Ant ant in m_ants){
			ant.Move(m_map);
			//check player intersect
			if(ant.m_row == m_player.m_row && ant.m_col == m_player.m_col){
				GameOver(false);
			}
		}
		//check player exit intersect
		int exitRow, exitCol;
		m_map.WorldToGrid(m_exit.transform.position, out exitRow, out exitCol);
		if(m_player.m_row == exitRow && m_player.m_col == exitCol){
            GameOver(true);
        }
	}
	void GameOver(bool didWin){
		m_player.enabled = false;
		
	}
	void Start()
	{
		SpawnCharacters();
	}
	void SpawnCharacters(){
		Vector3[] spawnPoints = m_map.GetSpawnPoints(10);
		Vector3 playerSpawnPoint, exitSpawnPoint;

		Helper.FindFurthestPoints(spawnPoints, out playerSpawnPoint, out exitSpawnPoint);

		m_player = Instantiate(m_playerPrefab, playerSpawnPoint, Quaternion.identity);
		m_player.m_map = m_map;

		m_exit = Instantiate(m_exitPrefab, exitSpawnPoint, Quaternion.identity);

		List<Ant> ants = new List<Ant>();
		foreach(Vector3 spawn in spawnPoints){
			if(spawn != playerSpawnPoint && spawn != exitSpawnPoint){
				CardinalDirections randomDirection = (CardinalDirections)Random.Range(0, (int)CardinalDirections.END);
				Debug.Log(spawn);
				Debug.Log(randomDirection);
				Ant newAnt = Instantiate(m_antPrefab, spawn, Quaternion.Euler(0, (int)randomDirection * -90, 0));
				m_map.WorldToGrid(spawn, out newAnt.m_row, out newAnt.m_col);
				newAnt.m_direction = randomDirection;
				ants.Add(newAnt);
			}
		}
		m_ants = new Ant[ants.Count];
		ants.CopyTo(m_ants);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
