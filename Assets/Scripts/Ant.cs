using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour{
	public int m_row, m_col;
	public CardinalDirections m_direction;
	public void Move(CellularMap map){
        //move forward
        int[] direction = Helper.DIRECTIONS[(int)m_direction];
		m_row += direction[0];
		m_col += direction[1];
		transform.position = map.GridToWorld(m_row, m_col);
	    //rotate
        int turnDirection = map.GetTileState(ref m_row, ref m_col) ? 1 : -1;
		m_direction = (CardinalDirections)(Helper.BetterMod((int)m_direction + turnDirection, (int)CardinalDirections.END));
		transform.rotation = Quaternion.Euler(0, (int)m_direction * -90, 0);	
	}
}
