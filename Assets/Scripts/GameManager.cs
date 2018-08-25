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

	}
	void Start()
	{
		SpawnCharacters();
	}
	void SpawnCharacters(){
		Vector3[] spawnPoints = m_map.GetSpawnPoints(5);
		Vector3 playerSpawnPoint, exitSpawnPoint;

		Helper.FindFurthestPoints(spawnPoints, out playerSpawnPoint, out exitSpawnPoint);

		m_player = Instantiate(m_playerPrefab, playerSpawnPoint, Quaternion.identity);
		m_player.m_map = m_map;

		List<Ant> ants = new List<Ant>();
		foreach(Vector3 spawn in spawnPoints){
			if(spawn != playerSpawnPoint && spawn != exitSpawnPoint){
				Ant newAnt = Instantiate(m_antPrefab, spawn, Quaternion.identity);
				m_map.WorldToGrid(spawn, out newAnt.row, out newAnt.col);
				newAnt.direction = (CardinalDirections)Random.Range(0, (int)CardinalDirections.END);
				ants.Add(newAnt);
			}
		}
		ants.CopyTo(m_ants);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
