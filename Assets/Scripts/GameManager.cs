using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public CellularMap m_map;
	public Player m_playerPrefab;
	public Ant m_antPrefab;
	public Exit m_exitPrefab;
	public Text m_gameOverText;
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
	void GameOver(bool didWin){
		if(didWin){
			m_gameOverText.text = "You Win!";
		}else{
            m_gameOverText.text = "Death by Ants!";
		}
        m_gameOverText.enabled = true;
        m_player.enabled = false;
	}
	void Start(){
		SpawnCharacters();
	}
	void SpawnCharacters(){
		Vector3[] spawnPoints = m_map.GetSpawnPoints(10);
		Vector3 playerSpawnPoint, exitSpawnPoint;

		Helper.FindFurthestPoints(spawnPoints, out playerSpawnPoint, out exitSpawnPoint);
		// playerSpawnPoint = spawnPoints[0];
		// exitSpawnPoint = spawnPoints[1];
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
	public void ResetGame(){
		m_gameOverText.enabled = false;
		Destroy(m_player.gameObject);
		Destroy(m_exit.gameObject);
		foreach(Ant ant in m_ants){
			Destroy(ant.gameObject);
		}
		m_map.ResetGame();
		SpawnCharacters();
	}
    public void PlayerMoved()
    {
        //flip ant's current tile
        HashSet<Vector2Int> indices = new HashSet<Vector2Int>();
        foreach (Ant ant in m_ants)
        {
            Vector2Int index = new Vector2Int(ant.m_row, ant.m_col);
            if (!indices.Contains(index))
            {
                indices.Add(index);
                m_map.SetTileState(ref ant.m_row, ref ant.m_col, !m_map.GetTileState(ref ant.m_row, ref ant.m_col));
            }
        }
        //move ants
        foreach (Ant ant in m_ants)
        {
            ant.Move(m_map);
            //check player intersect
            if (ant.m_row == m_player.m_row && ant.m_col == m_player.m_col)
            {
				Debug.Log("overlap");
                GameOver(false);
            }
        }
        //check player exit intersect
        int exitRow, exitCol;
        m_map.WorldToGrid(m_exit.transform.position, out exitRow, out exitCol);
        if (m_player.m_row == exitRow && m_player.m_col == exitCol)
        {
            GameOver(true);
        }
    }
	void Update()
	{
		if(Input.GetButtonDown("Cancel")){
			Exit();
		}
	}
	public void Exit(){
		Application.Quit();
	}
}
