using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public CellularMap m_map;
    int row, col;
	void Start()
	{
		m_map.WorldToGrid(transform.position, out row, out col);
	}
	// Update is called once per frame
	void Update () {
		bool playerMoved = false;
		if(Input.GetButtonDown("Horizontal")){
			int newCol = col;
			if(Input.GetAxisRaw("Horizontal") > 0){
                newCol++;
			}
			else if(Input.GetAxisRaw("Horizontal") < 0){
                newCol--;
			}
			if(!m_map.GetTileState(ref row, ref newCol)){
				col = newCol;
				transform.position = m_map.GridToWorld(row, col);
                playerMoved = true;
			}
		}
		else if(Input.GetButtonDown("Vertical")){
			int newRow = row;
			if(Input.GetAxisRaw("Vertical") > 0){
                newRow--;
			}
			else if(Input.GetAxisRaw("Vertical") < 0){
                newRow++;
			}
			if(!m_map.GetTileState(ref newRow, ref col)){
                row = newRow;
				transform.position = m_map.GridToWorld(row, col);
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
