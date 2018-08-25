using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public CellularMap m_map;
    public int m_row, m_col;
	void Start()
	{
		m_map.WorldToGrid(transform.position, out m_row, out m_col);
	}
	// Update is called once per frame
	void Update () {
		bool playerMoved = false;
		if(Input.GetButtonDown("Horizontal")){
			int newCol = m_col;
			if(Input.GetAxisRaw("Horizontal") > 0){
                newCol++;
			}
			else if(Input.GetAxisRaw("Horizontal") < 0){
                newCol--;
			}
			if(!m_map.GetTileState(ref m_row, ref newCol)){
				m_col = newCol;
				transform.position = m_map.GridToWorld(m_row, m_col);
                playerMoved = true;
			}
		}
		else if(Input.GetButtonDown("Vertical")){
			int newRow = m_row;
			if(Input.GetAxisRaw("Vertical") > 0){
                newRow--;
			}
			else if(Input.GetAxisRaw("Vertical") < 0){
                newRow++;
			}
			if(!m_map.GetTileState(ref newRow, ref m_col)){
                m_row = newRow;
				transform.position = m_map.GridToWorld(m_row, m_col);
                playerMoved = true;
			}
            
        }
		else if(Input.GetButtonDown("Skip")){
			playerMoved = true;
        }

		if(playerMoved){
			GameManager.Instance.PlayerMoved();
        }

	}
}
