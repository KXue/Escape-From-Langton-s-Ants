using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RepeatingRule{
	public int repeatCount = 1;
	public MapRule m_rule = new CaveRule();
	public bool[][] ApplyRuleRepeatedly(bool [][] map){
		bool [][] returnMap = new bool[0][];
		for(int i = 0; i < repeatCount; i++){
			returnMap = m_rule.ApplyRule(map);
			map = returnMap;
		}
		return returnMap;
	}
}
public abstract class MapRule{
	public bool m_wrap;
	public bool[][] ApplyRule(bool[][] prevIteration){
		bool[][] nextIteration = new bool[prevIteration.Length][];

		for(int row = 0; row < prevIteration.Length; row++){
			nextIteration[row] = new bool[prevIteration[row].Length];
			for(int col = 0; col < prevIteration[row].Length; col++){
				nextIteration[row][col] = TestCell(prevIteration, row, col);
			}
		}
		return nextIteration;
	}
	protected abstract bool TestCell(bool[][] map, int row, int col);
}
public class CaveRule : MapRule
{
	protected override bool TestCell(bool[][] map, int row, int col){
		int immediateNeighbours = Helper.MooreNeighborWallCount(map, row, col, 1, m_wrap);
		int extendedNeighbours = Helper.MooreNeighborWallCount(map, row, col, 2, m_wrap);
		return immediateNeighbours >= 5 || extendedNeighbours <= 2;
	}
}

public class SmoothRule: MapRule{
	protected override bool TestCell(bool[][] map, int row, int col){
		int immediateNeighbours = Helper.MooreNeighborWallCount(map, row, col, 1, m_wrap);
		return immediateNeighbours >= 5;
	}
}
